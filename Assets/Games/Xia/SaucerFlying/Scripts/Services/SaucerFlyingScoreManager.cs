using UnityEngine;
using System;
using System.Collections;

namespace SaucerFlying
{
    public class SaucerFlyingScoreManager : MonoBehaviour
    {
        public static SaucerFlyingScoreManager Instance { get; private set; }

        public int Score { get; private set; }

        public int HighScore { get; private set; }

        public bool HasNewHighScore { get; private set; }

        public static event Action<int> ScoreUpdated = delegate {};
        public static event Action<int> HighscoreUpdated = delegate {};

        private const string HIGHSCORE = "HIGHSCORE";
        // key name to store high score in PlayerPrefs

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            Reset();
        }

        public void Reset()
        {
            // Initialize score
            Score = 0;

            // Initialize highscore
            HighScore = 0;
            HasNewHighScore = false;
        }

        public void AddScore(int amount)
        {
            Score += amount;
            ScoreUpdated(Score);
            
        }

        public void UpdateHighScore(int newHighScore)
        {
            // Update highscore if player has made a new one
            if (newHighScore > HighScore)
            {
                HighScore = newHighScore;
                HighscoreUpdated(HighScore);
            }
        }
    }
}
