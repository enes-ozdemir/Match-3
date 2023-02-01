using System;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        private int _currentScore;
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