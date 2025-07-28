// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------
using System;
using Mirror;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

namespace CCG.Networking
{
	public static class ServiceManager
	{
        public static bool IsInitialized { get; private set; } = false;

        private const ushort defaultMaxPlayers = 10;
        private const string defaultServerName = "MyServerExample";
        private const string defaultGameType = "MyGameType";
        private const string defaultBuildId = "MyBuildId";
        private const string defaultMap = "MyMap";
        public static ushort currentPlayerCount;
        private static IServerQueryHandler serverQueryHandler;

        public static void ChangeQueryResponseValues(ushort maxPlayers, string serverName, string gameType, string buildId)
        {
            serverQueryHandler.MaxPlayers = maxPlayers;
            serverQueryHandler.ServerName = serverName;
            serverQueryHandler.GameType = gameType;
            serverQueryHandler.BuildId = buildId;
        }

        public static async void PlayerCountChanged()
        {
            serverQueryHandler.CurrentPlayers = currentPlayerCount;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static async void InitializeServices()
		{
            try
            {

                await UnityServices.InitializeAsync();
                IsInitialized = true;

#if UNITY_SERVER

                if (Application.isEditor)
                {
                    Debug.Log("Application is editor, returning to allow debugging.");
                    return;
                }

                Debug.Log("Running in headless server mode. Starting Mirror server...");

                NetworkManager.singleton.StartServer();
                await MultiplayService.Instance.ReadyServerForPlayersAsync();

                LogServerConfig();
                serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(defaultMaxPlayers, defaultServerName, defaultGameType, defaultBuildId, defaultMap);

 
#else
			Debug.Log("Not a server build. ServerBootstrapper will do nothing.");
#endif
            }
            catch (Exception e)
			{
				Debug.LogException(e);
			}
        }

        public static void UpdateServerQuery()
        {
#if UNITY_SERVER
            if (Application.isEditor)
                return;

            if (IsInitialized == false)
                return;

            serverQueryHandler.UpdateServerCheck();
#endif
        }

        public static void LogServerConfig()
        {
            var serverConfig = MultiplayService.Instance.ServerConfig;
            Debug.Log($"Server ID[{serverConfig.ServerId}]");
            Debug.Log($"AllocationID[{serverConfig.AllocationId}]");
            Debug.Log($"Port[{serverConfig.Port}]");
            Debug.Log($"QueryPort[{serverConfig.QueryPort}");
            Debug.Log($"LogDirectory[{serverConfig.ServerLogDirectory}]");
        }

    }
}
