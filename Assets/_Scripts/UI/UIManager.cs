using _Scripts.Gameplay;
using _Scripts.Systems;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
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

        public void ResetScore() => scoreManager.SetScore(0);

        public void EnableEndGameUI() => gameOverUI.gameObject.SetActive(true);

        private void SetScore(int score)
        {
            scoreText.text = score + " pts";
            _currentScore = score;
            gameOverUI.SetScore(_currentScore);
        }

        private void SetTimerText(int time) => timerText.text = time.ToString();

        public void SetTimerUI(Timer timer) => timer.onTimerChanged += SetTimerText;
    }
}