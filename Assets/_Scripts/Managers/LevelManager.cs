using System.Collections.Generic;
using _Scripts.SO;
using _Scripts.Systems;
using UnityEngine;

namespace _Scripts.Managers
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
            SetLevel(0);
            StartLevel();
        }

        private void SetGameOver()
        {
            uiManager.SetGameOverUI();
            tileInputManager.onInputDisabled.Invoke();
        }

        public void SetLevel(int level) => _currentLevel = gameLevels[level];

        public void StartLevel()
        {
            gridManager.ClearGrid();
            if (_currentLevel.timeLimit > 0)
            {
                timer.StartTimer(_currentLevel.timeLimit);
            }
            uiManager.SetTimerUI(timer);
            gridManager.FillGrid(true);
        }
    }
}