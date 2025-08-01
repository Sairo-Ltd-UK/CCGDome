// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

#if UNITY_SERVER
using System;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Matchmaker.Models;
using System.Threading;
using Unity.Services.Multiplay;
using Unity.Services.Matchmaker;
#endif

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
			CreateBackfillTicketOptions createBackfillTicketOptions = new CreateBackfillTicketOptions();
			backfillTicketId = await MatchmakerService.Instance.CreateBackfillTicketAsync(createBackfillTicketOptions);

			// Start backfill approval loop
			_backfillCts?.Cancel();
			_backfillCts = new CancellationTokenSource();
			_ = BackfillApprovalLoopAsync(backfillTicketId, _backfillCts.Token);

		}

		private static void OnDeallocate(MultiplayDeallocation deallocation)
		{
			// Clean up or save game state here if necessary.
			Debug.Log("[Multiplay] Server deallocated – shutting down soon.");
			_backfillCts?.Cancel();

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

		/// <summary>
		/// Every second, approve the backfill ticket to keep it alive and pull in new players.
		/// </summary>
		private static async Task BackfillApprovalLoopAsync(string backfillTicketId, CancellationToken token)
		{
			const int intervalMs = 1000;
			try
			{
				while (!token.IsCancellationRequested)
				{
					try
					{
						// Approve the backfill ticket (same ID as allocation)
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
				// Expected on shutdown
			}
		}
#endif
	}
}
