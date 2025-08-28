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
using Unity.Services.Core;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

namespace CCG.Networking
{
	public static class MatchmakingClient
	{
		private const string PROD_QUEUE_NAME = "Default";
		private const string DEV_QUEUE_NAME = "DevDefault";

#if PRODUCTION_BUILD
		private static bool isProduction = true; // <-- Change via build config
#else
		private static bool isProduction = false; // <-- Change via build config

#endif

		private static bool isShuttingDown = false;

		private const int maxAttempts = 60;

		public static async Task RequestMatchAsync()
		{
			try
			{

				string queueName = isProduction ? PROD_QUEUE_NAME : DEV_QUEUE_NAME;


				var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(
					new List<Player> { new Player(Unity.Services.Authentication.AuthenticationService.Instance.PlayerId) },
					new CreateTicketOptions(queueName)
				);

				string ticketId = ticketResponse.Id;
				Debug.Log($"[Matchmaker] Ticket created: {ticketId}");

	
				for (int attempt = 0; attempt < maxAttempts; attempt++)
				{
					await Task.Delay(1000);

					TicketStatusResponse statusResponse = await MatchmakerService.Instance.GetTicketAsync(ticketId);

					if (isShuttingDown == true)
						return;

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
								Debug.LogError($"Matchmaking {assignment.Status}");
								Debug.LogError($"Matchmaking {assignment.Message}");
								return;

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

		internal static async Task CloseServices()
		{
			isShuttingDown = true;
		}
	}
}
