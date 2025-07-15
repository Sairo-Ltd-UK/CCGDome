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
	public class ServerBootstrapper : MonoBehaviour
	{
		void Start()
		{
#if UNITY_SERVER
			Debug.Log("Running in headless server mode. Starting Mirror server...");
			NetworkManager.singleton.StartServer();
#else
			Debug.Log("Not a server build. ServerBootstrapper will do nothing.");
#endif
		}
	}
}
