// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System.Threading.Tasks;
using UnityEngine;
using System;

#if UNITY_SERVER
using Unity.Services.Multiplay;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
#endif

namespace CCG.Networking
{
	public static class ServerQueryReporter
	{
		private const ushort defaultMaxPlayers = 50;
		private const string defaultServerName = "MyServerExample";
		private const string defaultGameType = "MyGameType";
		private const string defaultBuildId = "MyBuildId";
		private const string defaultMap = "MyMap";

		private static ushort currentPlayerCount = 0;

#if UNITY_SERVER
		private static IServerQueryHandler queryHandler;
#endif
		public static async Task Initialize()
		{
#if UNITY_SERVER
			queryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(defaultMaxPlayers, defaultServerName, defaultGameType, defaultBuildId, defaultMap);
#endif
		}

		public static void OnPlayerJoined()
		{
#if UNITY_SERVER
			currentPlayerCount++;
			UpdatePlayerCount();

			AddPlayerToBackFill();
#endif
		}

		public static void OnPlayerLeft()
		{
#if UNITY_SERVER
			currentPlayerCount--;
			UpdatePlayerCount();
			RemovePlayerFromBackFill();
#endif
		}

		public static void UpdatePlayerCount()
		{
#if UNITY_SERVER
			if (queryHandler != null)
				queryHandler.CurrentPlayers = currentPlayerCount;
#endif
		}

		public static void UpdateServerQuery()
		{
#if UNITY_SERVER

			if (Application.isEditor)
				return;

			if (queryHandler != null)
				queryHandler.UpdateServerCheck();
#endif
		}


		private static async Task AddPlayerToBackFill()
		{
			// pull the ticket ID from your allocation handler
			string ticketId = MultiplayServerEventHandler.backfillTicketId;

			try
			{
				// build a list of Player objects from current connections
				BackfillTicket response = await MatchmakerService.Instance.ApproveBackfillTicketAsync(ticketId);
				response.Properties.MatchProperties.Players.Add(new Player(currentPlayerCount.ToString()));
				await MatchmakerService.Instance.UpdateBackfillTicketAsync(ticketId, response);

			}
			catch (Exception ex)
			{
				Debug.LogError($"[Backfill] Failed to update ticket: {ex.Message}");
			}
		}

		private static async Task RemovePlayerFromBackFill()
		{
			// pull the ticket ID from your allocation handler
			string ticketId = MultiplayServerEventHandler.backfillTicketId;

			try
			{
				// build a list of Player objects from current connections
				BackfillTicket response = await MatchmakerService.Instance.ApproveBackfillTicketAsync(ticketId);

				if (response.Properties.MatchProperties.Players.Count <= 0)
					return;

				response.Properties.MatchProperties.Players.RemoveAt(0);
				await MatchmakerService.Instance.UpdateBackfillTicketAsync(ticketId, response);
			}
			catch (Exception ex)
			{
				Debug.LogError($"[Backfill] Failed to update ticket: {ex.Message}");
			}
		}
	}
}
