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
using UnityEngine;

#if UNITY_SERVER
using Unity.Services.Multiplay;
#endif

namespace CCG.Networking
{
	public static class ServiceManager
	{
		public static bool IsInitialized { get; private set; } = false;
		public static bool IsShuttingDown { get; private set; } = false;
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static async void InitializeServices()
		{
			try
			{
				await UnityServices.InitializeAsync();
				IsInitialized = true;

#if UNITY_EDITOR

				NetworkManager.singleton.StartHost();

				//await AuthenticationService.InitializeAsync();
				//await MatchmakingClient.RequestMatchAsync();

#elif UNITY_SERVER

				Debug.Log("Running in headless server mode. Starting Mirror server...");

				await ServerQueryReporterService.Initialize();
				await MultiplayServerEventsService.Init();

				NetworkManager.singleton.StartServer();
				LogServerConfig();

#elif !UNITY_SERVER

				await AuthenticationService.InitializeAsync();
				await MatchmakingClient.RequestMatchAsync();
#endif

			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		public static async void CloseServices()
		{
			if (IsShuttingDown == true)
				return;

			IsShuttingDown = true;
			Debug.Log(" CloseServices");

#if UNITY_SERVER

			ServerQueryReporterService.CloseServices();
			await MultiplayServerEventsService.CloseServices();

			if(NetworkManager.singleton)
				NetworkManager.singleton.StopServer();
		
			Application.Quit();

#elif !UNITY_SERVER

			await MatchmakingClient.CloseServices();

			if(NetworkManager.singleton)
				NetworkManager.singleton.StopClient();
#endif
		}

#if UNITY_SERVER
		public static void LogServerConfig()
		{
			var serverConfig = MultiplayService.Instance.ServerConfig;
			Debug.Log($"Server ID[{serverConfig.ServerId}]");
			Debug.Log($"AllocationID[{serverConfig.AllocationId}]");
			Debug.Log($"Port[{serverConfig.Port}]");
			Debug.Log($"QueryPort[{serverConfig.QueryPort}");
			Debug.Log($"LogDirectory[{serverConfig.ServerLogDirectory}]");
		}
#endif

	}
}
