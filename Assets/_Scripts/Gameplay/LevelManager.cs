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
            SetLevel(PlayerPrefs.GetInt("Level", 0));
        }

        #endregion

        [SerializeField] private List<Level> gameLevels;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Timer timer;
        [SerializeField] private TileInputManager tileInputManager;
        [SerializeField] private GridManager gridManager;

        private Level _currentLevel;
        private int _levelNumber;

        private void Start()
        {
            timer.onTimesUp += SetGameOver;
            StartGame();
        }

        public void StartGame()
        {
            uiManager.ResetScore();
            StartLevel();
        }

        public Vector2Int GetGridSize() => new(_currentLevel.gridSizeX, _currentLevel.gridSizeY);

        private void SetGameOver()
        {
            Debug.Log("Game Over");
            uiManager.EnableEndGameUI();
            tileInputManager.onInputDisabled.Invoke();
        }

        public void NextLevel()
        {
            if (_levelNumber <= gameLevels.Count) _levelNumber = gameLevels.Count;
            else _levelNumber++;
            SetLevel(_levelNumber);
            StartLevel();
        }

        private void SetLevel(int level)
        {
            _levelNumber = level;
            PlayerPrefs.SetInt("Level", _levelNumber);
            if (_levelNumber <= 0) _levelNumber = 1;
            _currentLevel = gameLevels[_levelNumber - 1];
        }

        private void StartLevel()
        {
            Debug.Log(
                $"{_levelNumber} started. Tile size: {_currentLevel.gridSizeX},{_currentLevel.gridSizeY} Time limit:{_currentLevel.timeLimit}");

            if (_currentLevel.timeLimit > 0)
            {
                timer.StartTimer(_currentLevel.timeLimit);
            }

            uiManager.SetTimerUI(timer);
            gridManager.SetupGrid();
        }
    }
}