// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.MiniGames.Duckhunt
{
	public class DuckHuntScoreManager : MonoBehaviour
	{
		public int currentScore;

		public void AddScore(int points)
		{
			currentScore += points;
			// update UI or network sync
		}
	}
}