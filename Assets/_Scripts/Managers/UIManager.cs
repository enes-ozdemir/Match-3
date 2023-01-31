using System;
using _Scripts.Systems;
using _Scripts.UI;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GameOverUI gameOverUI;

        private int _currentScore;

        private void Awake()
        {
            scoreManager.onScoreChanged += SetScore;
            SetScore(0);
        }

        public void SetGameOverUI()
        {
            gameOverUI.gameObject.SetActive(true);
            gameOverUI.OpenGameOverUI(_currentScore);
        }

        private void SetScore(int score)
        {
            scoreText.text = score + " pts";
            _currentScore = score;
        }

        private void SetTimerText(int time) => timerText.text = time.ToString();

        public void SetTimerUI(Timer timer) => timer.onTimerChanged += SetTimerText;
    }
}