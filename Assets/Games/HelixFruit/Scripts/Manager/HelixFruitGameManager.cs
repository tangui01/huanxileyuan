using UnityEngine;
using Sans.Core;
using System;
using UnityEngine.SceneManagement;

namespace Sans.Manager
{
    public class HelixFruitGameManager : MonoBehaviour
    {
        [SerializeField] Ball _ball;
        [SerializeField] Rotator _rotator;

        int _currentStage;
        float _fullDistance;
        Vector3 _finishPosition;
        Transform _startTransform;
        // MenuManager _menu;
        ScoreHandler _score;

        public ScoreHandler GetScoreHandler => _score;
        private int Level;

        public static event Action<float> OnUpdateProgressValue;

        private void Awake()
        {
            _score = GetComponent<ScoreHandler>();
            Level = PlayerPrefs.GetInt("level");
            GameTimeManager.instance.TimeOverAction = TimeOver;
        }

        private void OnEnable()
        {
            Ball.OnPlayerDeath += ShowEndScreen;
            FinishSet.OnLevelComplete += HandleOnLevelComplete;
            Rotator.OnFinishPositionCalculated += HandleOnFinishPositionCalculated;
            Rotator.OnPlaying += HandleOnPlaying;
        }

        private void OnDisable()
        {
            Ball.OnPlayerDeath -= ShowEndScreen;
            FinishSet.OnLevelComplete -= HandleOnLevelComplete;
            Rotator.OnFinishPositionCalculated -= HandleOnFinishPositionCalculated;
            Rotator.OnPlaying -= HandleOnPlaying;
        }
        private void Update()
        {
            UpdateProgressValue();
        }

        private float GetDistance()
        {
            return Vector2.Distance(_startTransform.position, _finishPosition);
        }

        public void UpdateProgressValue()
        {
            float newDistance = GetDistance();
            float progressValue = Mathf.InverseLerp(_fullDistance, 0f, newDistance);
            OnUpdateProgressValue?.Invoke(progressValue);
        }

        public void ContinueGameplay()
        {
            if (!_ball) return;
            _rotator.EnableRotatorInput();
            _ball.Revive();
        }
        private void ShowEndScreen()
        {
            CommonUI.instance.BackMainPanel_OPen(true);
        }

        private void TimeOver()
        {
           CommonUI.instance.StartCouterColdDown();
        }
        private void HandleOnPlaying()
        {
            
        }
        private void HandleOnFinishPositionCalculated(Vector3 position)
        {
            _startTransform = FindObjectOfType<Ball>().transform;
            _finishPosition = position;
            _fullDistance = GetDistance();
        }
        // event handler
        private void HandleOnLevelComplete()
        {
            Level++;
            PlayerPrefs.SetInt("level",Level);
            PlayerPrefs.SetInt("score",ScoreHandler.Instance.GetCurrentScore());
            SceneLoadManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}
