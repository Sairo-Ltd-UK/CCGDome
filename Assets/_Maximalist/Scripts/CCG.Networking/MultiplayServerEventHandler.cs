//------------------------------------------------------------------------------
// Project:     CCG Dome
// Author:      Corrin Wilson
// Company:     Maximalist Ltd
// Created:     11/07/2025

// Copyright © 2025 Maximalist Ltd. All rights reserved.
// This file is subject to the terms of the contract with the client.
//------------------------------------------------------------------------------

#if UNITY_SERVER
using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplay;
using UnityEngine;
using UnityEngine.UIElements;

#endif

using Mirror;
using System.Collections.Generic;

namespace CCG.Networking
{
	public static class MultiplayServerEventHandler
	{

#if UNITY_SERVER

		public static string backfillTicketId = "0";

		private static MultiplayEventCallbacks eventCallbacks;
		private static CancellationTokenSource _backfillCts;

		public static async Task Init()
		{
			eventCallbacks = new MultiplayEventCallbacks();
			eventCallbacks.Allocate += OnAllocate;
			eventCallbacks.Deallocate += OnDeallocate;
			eventCallbacks.Error += OnError;
			eventCallbacks.SubscriptionStateChanged += OnSubscriptionStateChanged;

			try
			{
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
			var payloadAllocation = await MultiplayService.Instance.GetPayloadAllocationFromJsonAs<MatchmakingResults>();
			backfillTicketId = payloadAllocation.BackfillTicketId;

			_backfillCts?.Cancel();
			_backfillCts = new CancellationTokenSource();
			_ = BackfillApprovalLoopAsync(backfillTicketId, _backfillCts.Token);

			await MultiplayService.Instance.ReadyServerForPlayersAsync();
		}

		private static void OnDeallocate(MultiplayDeallocation deallocation)
		{
			Debug.Log("[Multiplay] Server deallocated – shutting down soon.");
			ServiceManager.CloseServices();
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

		private static async Task BackfillApprovalLoopAsync(string backfillTicketId, CancellationToken token)
		{
			const int intervalMs = 1000;
			try
			{
				while (!token.IsCancellationRequested)
				{
					try
					{
						BackfillTicket response = await MatchmakerService.Instance.ApproveBackfillTicketAsync(backfillTicketId);
					}
					catch (Exception ex)
					{
						Debug.LogWarning($"[Matchmaker] Backfill approval failed: {ex.Message}");
					}

					await Task.Delay(intervalMs, token);
				}
			}
			catch (TaskCanceledException)
			{
				 //Expected on shutdown
			}
		}

		internal static async Task CloseServices()
		{
			_backfillCts?.Cancel();

			if(eventCallbacks == null)
				return;

			eventCallbacks.Allocate -= OnAllocate;
			eventCallbacks.Deallocate -= OnDeallocate;
			eventCallbacks.Error -= OnError;
			eventCallbacks.SubscriptionStateChanged -= OnSubscriptionStateChanged;
		}
#endif
	}
}
