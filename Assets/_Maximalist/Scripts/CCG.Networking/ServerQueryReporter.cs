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

#if UNITY_SERVER
using Unity.Services.Multiplay;
#endif

namespace CCG.Networking
{
	public static class ServerQueryReporter
	{
		private const ushort defaultMaxPlayers = 10;
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
			currentPlayerCount++;
			UpdatePlayerCount();
		}

		public static void OnPlayerLeft()
		{ 
			currentPlayerCount--;
			UpdatePlayerCount();
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
	}
}
