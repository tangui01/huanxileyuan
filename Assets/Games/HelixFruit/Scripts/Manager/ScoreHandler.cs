using Sans.Core;
using System;
using UnityEngine;

namespace Sans.Manager
{
    public class ScoreHandler : MonoBehaviour
    {
        public static ScoreHandler Instance;
        private void Awake()
        {
            if (Instance==null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public int _score;

        public static event Action<int> OnScoreUpdated;

        private void OnEnable()
        {
            _score=PlayerPrefs.GetInt("score",0);
            GroundSet.OnPieceDestroyed += AddScore;
        }

        private void OnDisable()
        {
            GroundSet.OnPieceDestroyed -= AddScore;
        }

        private void Start()
        {
            OnScoreUpdated?.Invoke(_score);
        }

        public int GetCurrentScore() {
            return _score;
        }
        public int GetnSetBestScore() {
            int bestScore = PlayerPrefs.GetInt("HighScore", 0);

            if (_score > bestScore) {
                PlayerPrefs.SetInt("HighScore", _score);
                bestScore = _score;
            }

            return bestScore;
        }

        public void AddScore(int score) {
            _score += score;
            OnScoreUpdated?.Invoke(_score);
        }
    }
}