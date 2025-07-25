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
using Unity.Services.Core;
using UnityEngine;

namespace CCG.Networking
{
	public static class ServiceManager
	{
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static async void InitializeServices()
		{
			try
			{
				await UnityServices.InitializeAsync();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}
	}
}
