using _Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameOverUIScoreText;

        public void OpenGameOverUI(int score) => gameOverUIScoreText.text = $"Score : {score}";

        public void RetryGame()
        {
            LevelManager.Instance.StartGame();
            gameObject.SetActive(false);
        }
    }
}