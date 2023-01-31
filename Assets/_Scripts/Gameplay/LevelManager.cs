using System.Collections.Generic;
using _Scripts.SO;
using _Scripts.Systems;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        #region "Singleton"

        public static LevelManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        [SerializeField] private List<Level> gameLevels;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Timer timer;
        [SerializeField] private TileInputManager tileInputManager;
        [SerializeField] private GridManager gridManager;
        
        private Level _currentLevel;

        private void Start()
        {
            timer.onTimesUp += SetGameOver;
            StartGame();
        }

        public void StartGame()
        {
            uiManager.ResetScore();
            //todo change this
            SetLevel(0);
            StartLevel();
            gridManager.SetupGrid();
        }

        public Vector2Int GetGridSize() => new(_currentLevel.gridSizeX,_currentLevel.gridSizeY);

        private void SetGameOver()
        {
            Debug.Log("Game Over");
            uiManager.EnableEndGameUI();
            tileInputManager.onInputDisabled.Invoke();
        }

        private void SetLevel(int level) => _currentLevel = gameLevels[level];

        private void StartLevel()
        {
            if (_currentLevel.timeLimit > 0)
            {
                timer.StartTimer(_currentLevel.timeLimit);
            }

            uiManager.SetTimerUI(timer);
        }
    }
}