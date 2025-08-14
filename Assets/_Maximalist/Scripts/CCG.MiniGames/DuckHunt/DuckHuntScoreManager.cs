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
	public class DuckHuntScoreManager : MonoBehaviour
	{
		public static event Action<int> OnScoreChanged;
		
		public int currentScore;

		public void ResetScore()
		{
			UpdateScore(0);
		}
		
		public void AddScore(int points)
		{
			UpdateScore(currentScore + points);
		}

		private void UpdateScore(int points)
		{
			currentScore = points;
			OnScoreChanged?.Invoke(currentScore);
		}
	}
}