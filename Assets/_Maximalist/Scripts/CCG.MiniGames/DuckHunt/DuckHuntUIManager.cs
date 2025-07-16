using System;
using CCG.MiniGames.Duckhunt;
using UnityEngine;
using UnityEngine.UI;

namespace CCG.MiniGames
{
    public class DuckHuntUIManager : MonoBehaviour
    {
        [Header("DuckHuntGameUIManager")]
        [SerializeField] private Text timerText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text shotsLeftText;
        [SerializeField] private Text ducksLeftText;
        [Space]
        [SerializeField] private GameObject winScreen;


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
            if (timerText)
            {
                timerText.text = (int)secondsCount + " sec";
            }
        }

        private void UpdateRemainingShots(int shotsLeft)
        {
            if (shotsLeft >= 0 && shotsLeftText != null)
            {
                shotsLeftText.text = shotsLeft.ToString();
            }
        }

        private void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }

        private void HandleGameFinishMenu(bool showMenu)
        {
            if (winScreen != null)
            {
                winScreen.SetActive(showMenu);
            }
        }

        private void UpdateDucksRemainingText(int ducksRemaining)
        {
            if (ducksLeftText != null)
            {
                ducksLeftText.text = ducksRemaining.ToString();
            }
        }
    }
}
