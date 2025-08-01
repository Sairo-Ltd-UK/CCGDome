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
			OnPlayerJoinedBackfill(connectionID);

#endif
		}

		public static void OnPlayerLeft(int connectionID)
		{
#if UNITY_SERVER

			currentPlayerCount--;
			UpdatePlayerCount();
			OnPlayerLeftBackfill(connectionID);

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

#if UNITY_SERVER

		public static async Task OnPlayerJoinedBackfill(int playerId)
		{
			string ticketId = MultiplayServerEventHandler.backfillTicketId;

			try
			{
				BackfillTicket response = await MatchmakerService.Instance.ApproveBackfillTicketAsync(ticketId);

				response.Properties.MatchProperties.Players.Add(new Player(playerId.ToString(), null, null));
				await MatchmakerService.Instance.UpdateBackfillTicketAsync(ticketId, response);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		public static async Task OnPlayerLeftBackfill(int playerId)
		{
			string ticketId = MultiplayServerEventHandler.backfillTicketId;

			try
			{
				BackfillTicket response = await MatchmakerService.Instance.ApproveBackfillTicketAsync(ticketId);

				var playerToRemove = response.Properties.MatchProperties.Players.FirstOrDefault(p => p.Id.Equals(playerId));
				response.Properties.MatchProperties.Players.Remove(playerToRemove);

				await MatchmakerService.Instance.UpdateBackfillTicketAsync(ticketId, response);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

#endif
	}
}
