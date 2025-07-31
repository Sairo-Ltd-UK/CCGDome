// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using Mirror;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

namespace CCG.Networking
{
	public static class MatchmakingClient
	{
		private const string QUEUE_NAME_KEY = "Default";
		private static bool isInitialized = false;

		public static async Task InitializeAsync()
		{
			if (isInitialized)
				return;

			await UnityServices.InitializeAsync();
			if (!AuthenticationService.Instance.IsSignedIn)
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

			Debug.Log("[Matchmaking] Initialized");
			isInitialized = true;
		}

		public static async Task RequestMatchAsync()
		{
			try
			{
				await InitializeAsync();

				var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(
					new List<Player> { new Player(AuthenticationService.Instance.PlayerId) },
					new CreateTicketOptions(QUEUE_NAME_KEY)
				);

				string ticketId = ticketResponse.Id;
				Debug.Log($"[Matchmaker] Ticket created: {ticketId}");

				const int maxAttempts = 30;
				for (int attempt = 0; attempt < maxAttempts; attempt++)
				{
					await Task.Delay(1000);

					TicketStatusResponse statusResponse = await MatchmakerService.Instance.GetTicketAsync(ticketId);

					if (statusResponse.Value is MultiplayAssignment assignment)
					{
						switch (assignment.Status)
						{
							case MultiplayAssignment.StatusOptions.Found:
								string ip = assignment.Ip;
								int port = assignment.Port ?? 0;
								Debug.Log($"[Matchmaker] Found match at {ip}:{port}");

								NetworkManager.singleton.networkAddress = ip;
								NetworkManager.singleton.StartClient();
								return;

							case MultiplayAssignment.StatusOptions.InProgress:
								Debug.Log("[Matchmaker] Still in progress...");
								break;

							case MultiplayAssignment.StatusOptions.Failed:
							case MultiplayAssignment.StatusOptions.Timeout:
								throw new Exception($"Matchmaking {assignment.Status}");
						}
					}
					else
					{
						Debug.LogError($"Unexpected ticket value type: {statusResponse.Value?.GetType()}");
					}
				}

				throw new TimeoutException("[Matchmaker] Matchmaking timed out");
			}
			catch (Exception ex)
			{
				Debug.LogError($"[Matchmaking] Error: {ex.Message}");
			}
		}
	}
}
