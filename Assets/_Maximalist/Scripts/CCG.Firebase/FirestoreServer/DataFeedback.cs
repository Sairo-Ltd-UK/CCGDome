// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay Alexander Andrade-Hunt
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace CCG.Firebase
{
	public class DataFeedback : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private TextMeshProUGUI titleText;
		private List<int> scoreList = new List<int>();

		public void UpdateScoreText(int score)
		{
			scoreList.Add(score);

			string result = "";
			foreach (int s in scoreList)
			{
				result += $"Score: {s}\n";
			}

			if (scoreList.Count > 1)
			{
				int min = scoreList.Min();
				int max = scoreList.Max();
				int improvement = max - min;
				result += $"\nImprovement: {improvement} (Min: {min}, Max: {max})";
			}

			scoreText.text = result;
		}

		public void EnteredZone(string zone)
		{
			titleText.text = zone + " \nScores & Improvement";
		}
	}
}