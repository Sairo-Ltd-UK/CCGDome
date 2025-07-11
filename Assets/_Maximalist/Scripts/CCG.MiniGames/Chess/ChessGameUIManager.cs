// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------


using UnityEngine;
using UnityEngine.UI;

namespace CCG.MiniGames.Chess
{
	public class ChessGameUIManager : MonoBehaviour
	{
		public static int p1PieceCount;
		public static int p2PieceCount;
		public static int p1PromotionCount;
		public static int p2PromotionCount;

		[Header("ChessGameUIManager")]
		[SerializeField] private Text timerText;
		[SerializeField] private Text currentPlayerText;
		[SerializeField] private Text countText;
		[SerializeField] private Text promotionCountText;
		[SerializeField] private Text winScreenText;
		[Space]

		[Space]
		[SerializeField] private GameObject timer;
		[SerializeField] private GameObject tacticalMenu;
		[SerializeField] private GameObject winScreen;

		private float secondsCount;
		private int minuteCount;

		private void Start()
		{
			p1PieceCount = 16;
			p2PieceCount = 16;
			p1PromotionCount = 0;
			p1PromotionCount = 0;
		}

		private void Update()
		{
			UpdateUI();
			UpdateTimer();
		}

		private void UpdateUI()
		{
			//if (!BI.WhiteTurn)
			//	currentPlayerText.text = "Black Team";
			//else
			//	currentPlayerText.text = "White Team";

			countText.text = "Black Team: " + p1PieceCount + "\nWhite Team: " + p2PieceCount;
			promotionCountText.text = "Black Team: " + p1PromotionCount + "\nWhite Team: " + p2PromotionCount;
		}

		private void UpdateTimer()
		{
			secondsCount += Time.deltaTime;
			timerText.text = minuteCount + " min: " + (int)secondsCount + " sec";

			if (secondsCount >= 60)
			{
				minuteCount++;
				secondsCount = 0;
			}
		}

		public void PlayerOneWin()
		{
			winScreen.SetActive(true);
			winScreenText.text = "This match has been won by the Black Team";
			SomoneWon();
		}

		public void PlayerTwoWin()
		{
			winScreen.SetActive(true);
			winScreenText.text = "This match has been won by the White Team";
		}

		private void SomoneWon()
		{
			ResetChessGameUI();
		}

		private void ResetChessGameUI()
		{
			minuteCount = 0;
			secondsCount = 0;
		}

	}
}