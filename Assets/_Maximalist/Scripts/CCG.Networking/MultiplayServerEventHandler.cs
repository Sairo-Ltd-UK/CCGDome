using System;
using UnityEngine;

#if UNITY_SERVER
using Unity.Services.Multiplay;
#endif

namespace CCG.Networking
{
    public class MultiplayServerEventHandler : MonoBehaviour
    {
#if UNITY_SERVER

        private MultiplayEventCallbacks eventCallbacks;
        private IServerEvents serverEvents;

        private async void Awake()
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
                serverEvents = await MultiplayService.Instance.SubscribeToServerEventsAsync(eventCallbacks);
                Debug.Log("[Multiplay] Successfully subscribed to server events");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Multiplay] Failed to subscribe to server events: {ex.Message}");
            }
        }

        private void OnAllocate(MultiplayAllocation allocation)
        {
            Debug.Log($"[Multiplay] Server allocated with ID: {allocation.AllocationId}");
            // You can parse payload or start gameplay logic here if needed.
        }

        private void OnDeallocate(MultiplayDeallocation deallocation)
        {
            Debug.Log("[Multiplay] Server deallocated – shutting down soon.");
            // Clean up or save game state here if necessary.
            Application.Quit();
        }

        private void OnError(MultiplayError error)
        {
            Debug.LogError($"[Multiplay] Error received: {error.Reason}");

            if (!string.IsNullOrEmpty(error.Detail))
            {
                Debug.LogError($"[Multiplay] Error detail: {error.Detail}");
            }
        }

        private void OnSubscriptionStateChanged(MultiplayServerSubscriptionState state)
        {
            Debug.Log($"[Multiplay] Subscription state changed: {state}");
        }

        private void OnApplicationQuit()
        {
            // Optionally unsubscribe to free resources (not required, but good practice)
            serverEvents.UnsubscribeAsync();
        }
    }

#endif
}
