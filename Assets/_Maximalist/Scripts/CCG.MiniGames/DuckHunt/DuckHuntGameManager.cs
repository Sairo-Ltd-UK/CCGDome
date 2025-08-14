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
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CCG.MiniGames.Duckhunt
{
	public class DuckHuntGameManager : MiniGameBase
	{
		// Events, only currently the ui listents to these, used events to keep it decoupled
		public static event Action<bool> OnShowGameOver;
		public static event Action<float> OnTimerChange;
		public static event Action<int> OnShotsRemainingChanged;
		public static event Action<int> OnChangeDucksRemaining;
		
		[Tooltip("UsedToTrackTheScore")]
		[SerializeField] private DuckHuntScoreManager duckHuntScoreManager;
		[SerializeField] private Duck[] ducks;

        // Timer related 
        [Tooltip("Amount of time at start of each round")]
		[SerializeField] private float roundDuration = 30;

		// Ammo
		[Tooltip("Amount of Ammo at the end of each round")]
		[SerializeField] private int maxShots;
		
		// State
		private bool roundOver = false;
		private bool roundStarted = false;

        // Score
        private int score = 0;

        private int ducksRemainingInRound = 0;
        private int currentShotsRemaining;

        private float currentRoundTime = 0;
        private float timerCooldown = 1f;

        public bool HasShotsRemaining { get { return currentShotsRemaining > 0; } }

        private void Start()
        {
            for (int i = 0; i < ducks.Length; i++)
            {
                ducks[i].Index = i;
            }

            ResetDuckhuntGameManager();
        }

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

        private void FixedUpdate()
        {
			UpdateTimer();
        }

        private void UpdateTimer() //CW We want to minimise Ui updates
        {
            if (roundOver || !roundStarted)
                return;

			if (currentRoundTime < 0)
			{
				EndRound();
				return;
			}

            // Countdown until we hit 1 second
            timerCooldown -= Time.deltaTime;

            if (timerCooldown <= 0f)
            {
                // Perform the timer update
                UpdateTimer(currentRoundTime - 1f); // reduce by 1 second
                timerCooldown = 1f; // reset cooldown
            }
			
        }
        private void StartRound()
        {
            roundStarted = true;
        }

        private void ResetDuckhuntGameManager()
        {
            if (duckHuntScoreManager)
            {
                duckHuntScoreManager.ResetScore();
            }

            UpdateShotsRemaining(maxShots);
            UpdateDucksRemaining(ducks.Length);
            UpdateTimer(roundDuration);
			roundOver = false;
            OnShowGameOver?.Invoke(roundOver);
        }

        private async void EndRound()
		{
			if (!roundOver)
			{
				roundOver = true;
				roundStarted = false;
				OnShowGameOver?.Invoke(roundOver);
			}

			await Task.Delay(2000);

			// /* evaluate win/loss, prepare next */
			ResetDucks();
            ResetDuckhuntGameManager();
		}

		private void HandleDuckDeath(int scoreIncrease)
		{
			if (duckHuntScoreManager)
			{
				duckHuntScoreManager.AddScore(scoreIncrease);
			}
			
			const int ducksDecrease = 1;
			UpdateDucksRemaining(ducksRemainingInRound - ducksDecrease);

			if(roundOver == false && ducksRemainingInRound == 0)
				EndRound();
		}

		public void Fire()
		{
			const int shotsDecrease = 1;

			if (HasShotsRemaining == true)
			{
				UpdateShotsRemaining(currentShotsRemaining - shotsDecrease);
			}
		}

        public void ResetDucks()
        {
			if (ducks == null)
				return;

			for (int i = 0; i < ducks.Length; i++)
			{
				if(ducks[i])
					ducks[i].ResetDuck();
			}

            StartRound();
        }

        private void UpdateDucksRemaining(int newDucksRemaining)
		{
			ducksRemainingInRound = newDucksRemaining;
			
			if (ducksRemainingInRound <= 0)
			{
				ducksRemainingInRound = 0;
			}

			OnChangeDucksRemaining?.Invoke(ducksRemainingInRound);
		}

		private void UpdateShotsRemaining(int newShotsRemaining)
		{
			currentShotsRemaining = newShotsRemaining;

			if (currentShotsRemaining <= 0)
			{
				currentShotsRemaining = 0;
			}

			OnShotsRemainingChanged?.Invoke(currentShotsRemaining);
		}

		private void UpdateTimer(float newTime)
		{
			currentRoundTime = newTime;
	
			if (currentRoundTime <= 0)
			{
				currentRoundTime = 0;

				if (!roundOver)
					EndRound();
			}

			OnTimerChange?.Invoke(currentRoundTime);
		}

        internal void HitDuck(int index)
        {
			if(score == 0) //Start the round when the first duck is hit.
                StartRound();

            ducks[index].OnHit();

        }
    }
}

