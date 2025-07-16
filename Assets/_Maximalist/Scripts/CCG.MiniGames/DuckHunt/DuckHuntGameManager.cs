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
	    // Events, only currently the ui listents to these, used events to keep it decoupled
	    public static event Action<bool> OnShowGameOver;
	    public static event Action<float> OnTimerChange;
	    public static event Action<int> OnShotsRemainingChanged;
	    public static event Action<int> OnChangeDucksRemaining;
	    
	    
	    
		public static DuckHuntGameManager Instance { get; private set; }
		
		[Tooltip("UsedToTrackTheScore")]
		[SerializeField] DuckHuntScoreManager duckHuntScoreManager;
		
		
		// Timer related 
		[Tooltip("Amount of time at start of each round")]
		[SerializeField]private float startRoundTime;
		private float currentRoundTime;
		
		// Duck
		[Tooltip("Amount of duck to spawn per round")]
		[SerializeField]private int ducksPerRound;
		private int ducksRemainingInRound;
		
		// Score
		private int score = 0;

		// Ammo
		[Tooltip("Amount of Ammo at the end of each round")]
		[SerializeField] private int maxShots;
		private int currentShotsRemaining;

		
		// State
		private bool roundOver;

		private void OnEnable()
		{
			Duck.OnDuckDied += HandleDuckDeath;
		}

		private void OnDisable()
		{
			Duck.OnDuckDied -= HandleDuckDeath;
		}

		private void OnValidate()
		{
			if (duckHuntScoreManager == null)
			{
				Debug.LogWarning("DuckHuntScoreManager is not assigned!", this);
			}
		}

		private void Update()
		{
			if (!roundOver)
			{
				UpdateTimer(currentRoundTime -Time.deltaTime);
			}
		}

		private void Start()
		{
			StartRound();
		}

		private void StartRound()
		{
			roundOver = false;
			if (duckHuntScoreManager)
			{
				duckHuntScoreManager.ResetScore();
			}
			UpdateShotsRemaining(maxShots);
			UpdateDucksRemaining(ducksPerRound);
			UpdateTimer(startRoundTime);
			OnShowGameOver?.Invoke(roundOver);
		}

		private void EndRound()
		{
			if (!roundOver)
			{
				roundOver = true;
				OnShowGameOver?.Invoke(roundOver);
			}
			// /* evaluate win/loss, prepare next */
		}

		private void HandleDuckDeath()
		{
			if (duckHuntScoreManager)
			{
				const int scoreIncrease = 1;
				duckHuntScoreManager.AddScore(scoreIncrease);
			}
			
			const int ducksDecrease = 1;
			UpdateDucksRemaining(ducksRemainingInRound - ducksDecrease);
		}

		private void Fire()
		{
			const int shotsDecrease = 1;
			if (currentShotsRemaining > 0)
			{
				UpdateShotsRemaining(currentShotsRemaining - shotsDecrease);
			}
		}
		
		private void UpdateDucksRemaining(int newDucksRemaining)
		{
			ducksRemainingInRound = newDucksRemaining;
			const int zeroDucks = 0;
			if (ducksRemainingInRound <= zeroDucks)
			{
				ducksRemainingInRound = zeroDucks;
				if (!roundOver) EndRound();
			}
			OnChangeDucksRemaining?.Invoke(ducksRemainingInRound);
		}

		private void UpdateShotsRemaining(int newShotsRemaining)
		{
			currentShotsRemaining = newShotsRemaining;
			const int zeroShots = 0;
			if (currentShotsRemaining <= zeroShots)
			{
				currentShotsRemaining = zeroShots;
				if (!roundOver) EndRound();
			}
			OnShotsRemainingChanged?.Invoke(currentShotsRemaining);
		}

		private void UpdateTimer(float newTime)
		{
			currentRoundTime = newTime;
			const float zeroTime = 0f;
			if (currentRoundTime <= zeroTime)
			{
				currentRoundTime = zeroTime;
				if (!roundOver) EndRound();
			}

			OnTimerChange?.Invoke(currentRoundTime);
		}
    }
}

