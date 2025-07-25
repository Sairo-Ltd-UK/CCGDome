// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     14/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using Mirror;

namespace CCG.Networking
{
	public static class ServerBootstrapper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void StartServerIfHeadless()
		{
#if UNITY_SERVER
			Debug.Log("Running in headless server mode. Starting Mirror server...");

			if (Application.isEditor)
			{
				Debug.Log("Application is editor, returning to allow debugging.");
				return;
			}

			NetworkManager.singleton.StartServer();
#else
			Debug.Log("Not a server build. ServerBootstrapper will do nothing.");
#endif
		}
	}
}
