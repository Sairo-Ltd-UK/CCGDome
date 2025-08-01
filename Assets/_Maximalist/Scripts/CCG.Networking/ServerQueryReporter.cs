// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

#if UNITY_SERVER

using Mirror;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplay;
using UnityEngine;
using System.Linq;

#endif

using Mirror;
using System.Collections.Generic;

namespace CCG.Networking
{
	public static class ServerQueryReporter
	{
		private static readonly Dictionary<NetworkConnectionToClient, string> connectionToPlayerId = new();

		private const ushort defaultMaxPlayers = 50;
		private const string defaultServerName = "MyServerExample";
		private const string defaultGameType = "MyGameType";
		private const string defaultBuildId = "MyBuildId";
		private const string defaultMap = "MyMap";

		private static ushort currentPlayerCount = 0;

#if UNITY_SERVER

		private static IServerQueryHandler queryHandler;

		public static async Task Initialize()
		{
			queryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(defaultMaxPlayers, defaultServerName, defaultGameType, defaultBuildId, defaultMap);
		}

#endif
		public static void OnPlayerJoined(int connectionID)
		{
#if UNITY_SERVER

			currentPlayerCount++;
			UpdatePlayerCount();
#endif
		}

		public static void OnPlayerLeft(NetworkConnectionToClient connectionID)
		{
#if UNITY_SERVER

			currentPlayerCount--;
			UpdatePlayerCount();
			UnregisterPlayer(connectionID);
#endif
		}

		public static void UpdatePlayerCount()
		{
#if UNITY_SERVER

			if (queryHandler != null)
				queryHandler.CurrentPlayers = currentPlayerCount;

			if (currentPlayerCount == 0)
				ServiceManager.CloseServices();
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

		internal static void CloseServices()
		{
#if UNITY_SERVER

			if (queryHandler != null)
				queryHandler.Dispose();

#endif
		}

		public static void RegisterPlayerId(NetworkConnectionToClient conn, string playerId)
		{
#if UNITY_SERVER

			connectionToPlayerId[conn] = playerId;
			_ = OnPlayerJoinedBackfill(playerId);

#endif
		}

		public static void UnregisterPlayer(NetworkConnectionToClient conn)
		{
#if UNITY_SERVER

			if (connectionToPlayerId.TryGetValue(conn, out var playerId))
			{
				_ = OnPlayerLeftBackfill(playerId);
				connectionToPlayerId.Remove(conn);
			}
#endif
		}

#if UNITY_SERVER

		public static async Task OnPlayerJoinedBackfill(string playerId)
		{
			string ticketId = MultiplayServerEventHandler.backfillTicketId;

			try
			{
				var backfillTicket = await MatchmakerService.Instance.ApproveBackfillTicketAsync(ticketId);
				var matchProps = backfillTicket.Properties.MatchProperties;

				// Check if player already exists
				if (!matchProps.Players.Any(p => p.Id == playerId))
				{
					var newPlayer = new Player(
						playerId,
						new Dictionary<string, object> { { "Team", "Default" } }
					);

					matchProps.Players.Add(newPlayer);
				}

				// Also ensure they're on the team
				var defaultTeam = matchProps.Teams.FirstOrDefault(t => t.TeamName == "Default");
				if (defaultTeam != null && !defaultTeam.PlayerIds.Contains(playerId))
				{
					defaultTeam.PlayerIds.Add(playerId);
				}

				await MatchmakerService.Instance.UpdateBackfillTicketAsync(ticketId, backfillTicket);
			}
			catch (Exception ex)
			{
				Debug.LogError($"[Backfill] Failed to update ticket: {ex.Message}");
			}
		}

		public static async Task OnPlayerLeftBackfill(string playerId)
		{
			Debug.Log("OnPlayerLeftBackfill: " + playerId);

			string ticketId = MultiplayServerEventHandler.backfillTicketId;

			try
			{
				BackfillTicket response = await MatchmakerService.Instance.ApproveBackfillTicketAsync(ticketId);
				var matchProps = response.Properties.MatchProperties;

				// Remove player from the main player list
				var playerToRemove = matchProps.Players.FirstOrDefault(p => p.Id == playerId);
				if (playerToRemove != null)
					matchProps.Players.Remove(playerToRemove);

				// Remove player from any teams
				foreach (var team in matchProps.Teams)
				{
					team.PlayerIds.RemoveAll(id => id == playerId);
				}

				await MatchmakerService.Instance.UpdateBackfillTicketAsync(ticketId, response);
			}
			catch (Exception ex)
			{
				Debug.LogError($"[Backfill] Failed to update ticket on player leave: {ex.Message}");
			}
		}

#endif
	}
}
