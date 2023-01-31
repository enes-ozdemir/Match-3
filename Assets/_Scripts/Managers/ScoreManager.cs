using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private int _currentScore = 0;
        public Action<int> onScoreChanged;

        public void AddScore(int tileAmount)
        {
            _currentScore += tileAmount * 10;
            onScoreChanged.Invoke(_currentScore);
        }

        public void SetScore(int score)
        {
            _currentScore = score;
            onScoreChanged.Invoke(_currentScore);
        }
    }
}