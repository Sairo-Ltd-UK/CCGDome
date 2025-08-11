// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using Mirror;

namespace CCG.Networking
{
    public class ClientOnlyObjects : NetworkBehaviour
    {
        [SerializeField] private GameObject[] clientOnlyObjects;

        public override void OnStartServer()
        {
            base.OnStartServer();

            if(clientOnlyObjects == null || clientOnlyObjects.Length == 0)
            {
                Debug.LogWarning("[COCSI]: No client-only objects assigned.");
                return;
            }

            for (int i = 0; i < clientOnlyObjects.Length; i++)
            {
                if (clientOnlyObjects[i])
                    StaticClientOnlyController.HandleServerObject(clientOnlyObjects[i]);
            }
        }
    }

    public static class StaticClientOnlyController
    {
        public static void HandleServerObject(GameObject objectToDisable)
        {
            if (objectToDisable == null)
            {
                Debug.LogWarning("[SCOC]: Attempted to disable a null object.");
                return;
            }

            objectToDisable.SetActive(false);
        }
    }
}
