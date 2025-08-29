// ----------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CCG.MiniGames.Duckhunt
{
	public class DuckHuntUIManager : NetworkBehaviour
	{
		[Header("DuckHuntGameUIManager")]
		[SerializeField] private TextMeshProUGUI timerText;
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private TextMeshProUGUI shotsLeftText;
		[SerializeField] private TextMeshProUGUI ducksLeftText;
		[Space]
		[SerializeField] private GameObject winScreen;
		[Space]
		[SerializeField] private Button resetDuckhuntButton;

		private void OnEnable()
		{
			DuckHuntScoreManager.OnScoreChanged += UpdateScore;
			DuckHuntGameManager.OnTimerChange += UpdateTimer;
			DuckHuntGameManager.OnShotsRemainingChanged += UpdateRemainingShots;
			DuckHuntGameManager.OnShowGameOver += HandleGameFinishMenu;
			DuckHuntGameManager.OnChangeDucksRemaining += UpdateDucksRemainingText;

		}

		private void OnDisable()
		{
			DuckHuntScoreManager.OnScoreChanged -= UpdateScore;
			DuckHuntGameManager.OnTimerChange -= UpdateTimer;
			DuckHuntGameManager.OnShotsRemainingChanged -= UpdateRemainingShots;
			DuckHuntGameManager.OnShowGameOver -= HandleGameFinishMenu;
			DuckHuntGameManager.OnChangeDucksRemaining -= UpdateDucksRemainingText;
		}

		private void UpdateTimer(float secondsCount)
		{
			if (timerText != null)
			{
				timerText.text = (int)secondsCount + " sec";
			}
		}

		private void UpdateRemainingShots(int shotsLeft)
		{
			Debug.Log("[DHUI] UpdateRemainingShots");

			if (shotsLeftText != null)
			{
				shotsLeftText.text = $"Shots Left: {shotsLeft}";
			}
		}

		private void UpdateScore(int score)
		{
			Debug.Log("[DHUI] UpdateScore");

			if (scoreText != null)
			{
				scoreText.text = $"Score: {score}";
			}
		}

		private void HandleGameFinishMenu(bool showMenu)
		{
			Debug.Log("[DHUI] HandleGameFinishMenu");

			if (winScreen != null)
			{
				winScreen.SetActive(showMenu);
			}
		}

		private void UpdateDucksRemainingText(int ducksRemaining)
		{
			Debug.Log("[DHUI] UpdateDucksRemainingText");

			if (ducksLeftText != null)
			{
				ducksLeftText.text = $"Ducks Remaning: {ducksRemaining}";
			}
		}


		[ContextMenu("OnResetButtonPressed")]
		private void OnResetButtonPressed()
		{
			CmdRequestReset();
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestReset()
		{
			RpcDoReset();

			if (isServerOnly)
				DoReset();
		}

		[ClientRpc]
		private void RpcDoReset()
		{
			DoReset();
		}

		private void DoReset()
		{
			DuckHuntGameManager.Reset();
		}
	}
}
