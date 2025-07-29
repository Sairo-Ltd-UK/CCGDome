// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_SERVER
using Unity.Services.Multiplay;
#endif

namespace CCG.Networking
{
	public static class MultiplayServerEventHandler
	{
#if UNITY_SERVER

		private static MultiplayEventCallbacks eventCallbacks;

		public static async Task Init()
		{
			// We must first prepare our callbacks like so:
			eventCallbacks = new MultiplayEventCallbacks();
			eventCallbacks.Allocate += OnAllocate;
			eventCallbacks.Deallocate += OnDeallocate;
			eventCallbacks.Error += OnError;
			eventCallbacks.SubscriptionStateChanged += OnSubscriptionStateChanged;

			try
			{
				// Subscribe to the Multiplay server events
				await MultiplayService.Instance.SubscribeToServerEventsAsync(eventCallbacks);
				Debug.Log("[Multiplay] Successfully subscribed to server events");
			}
			catch (Exception ex)
			{
				Debug.LogError($"[Multiplay] Failed to subscribe to server events: {ex.Message}");
			}
		}

		private static async void OnAllocate(MultiplayAllocation allocation)
		{
			// You can parse payload or start gameplay logic here if needed.
			Debug.Log($"[Multiplay] Server allocated with ID: {allocation.AllocationId}");
			await MultiplayService.Instance.ReadyServerForPlayersAsync();
		}

		private static void OnDeallocate(MultiplayDeallocation deallocation)
		{
			// Clean up or save game state here if necessary.
			Debug.Log("[Multiplay] Server deallocated – shutting down soon.");
			Application.Quit();
		}

		private static void OnError(MultiplayError error)
		{
			Debug.LogError($"[Multiplay] Error received: {error.Reason}");

			if (!string.IsNullOrEmpty(error.Detail))
			{
				Debug.LogError($"[Multiplay] Error detail: {error.Detail}");
			}
		}

		private static void OnSubscriptionStateChanged(MultiplayServerSubscriptionState state)
		{
			Debug.Log($"[Multiplay] Subscription state changed: {state}");
		}

#endif
	}
}
