// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay A Hunt
//  Company:     Maximalist Ltd
//  Created:     11/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.Firebase
{
	/// <summary>
	/// Optional MonoBehaviour to initialise Firebase from the scene.
	/// Either auto-initialises on Start or waits for a manual trigger.
	/// </summary>
	public class FirebaseBootstrap : MonoBehaviour
	{
		[Header("Initialisation Settings")]
		[Tooltip("If true, calls FirebaseManager.InitialiseAsync() in Start().")]
		[SerializeField] private bool initialiseOnStart = true;

		private async void Start()
		{
			if (initialiseOnStart)
				await InitialiseNow();
		}

		/// <summary>
		/// Triggers Firebase initialisation if it hasn't run yet.
		/// Safe to call from UI buttons or other scripts.
		/// </summary>
		public async void TriggerInitialise()
		{
			await InitialiseNow();
		}

		private async System.Threading.Tasks.Task InitialiseNow()
		{
			if (FirebaseManager.IsInitialised)
			{
				Debug.Log("[FirebaseBootstrap] Firebase already initialised.");
				return;
			}

			Debug.Log("[FirebaseBootstrap] Initialising Firebase...");
			await FirebaseManager.InitialiseAsync();
		}
	}
}