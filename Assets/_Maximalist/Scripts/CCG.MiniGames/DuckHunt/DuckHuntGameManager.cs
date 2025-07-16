// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CCG.MiniGames.Duckhunt
{
    public class DuckHuntGameManager : MiniGameBase
    {
		public static DuckHuntGameManager Instance { get; private set; }

		public float roundTime;
		public int ducksPerRound;

		public int score;
		
		private void OnEnable()
		{
			Duck.OnDuckDied += HandleDuckDeath;
		}

		private void OnDisable()
		{
			Duck.OnDuckDied -= HandleDuckDeath;
		}

		private void StartRound() { /* spawn ducks, reset timer */ }
		private void EndRound() { /* evaluate win/loss, prepare next */ }

		private void HandleDuckDeath()
		{
			score++;
		}
	}
}

