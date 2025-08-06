// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using CCG.Core;

namespace CCG.Networking
{
    public static class AuthenticationService
    {
        private static bool isInitialized = false;

        public static async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            await UnityServices.InitializeAsync();

            if (!Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
            {
                await Unity.Services.Authentication.AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"[Auth] Signed in as {Unity.Services.Authentication.AuthenticationService.Instance.PlayerId}");
            }
            else
            {
                Debug.Log($"[Auth] Already signed in as {Unity.Services.Authentication.AuthenticationService.Instance.PlayerId}");
            }

            StaticLocalPlayerData.localPlayerID = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;
            isInitialized = true;
        }
    }
}