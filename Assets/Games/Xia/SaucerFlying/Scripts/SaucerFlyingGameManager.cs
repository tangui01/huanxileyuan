using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace SaucerFlying
{
    public enum GameState
    {
        Prepare,
        Playing,
        Paused,
        PreGameOver,
        GameOver
    }

    public enum ScoreMode
    {
        Distance,
        Obstacle
    }

    public enum AccelerationMode
    {
        Distance,
        Obstacle
    }

    public class SaucerFlyingGameManager : MonoBehaviour
    {
        public static SaucerFlyingGameManager Instance { get; private set; }

        public static event System.Action<GameState, GameState> GameStateChanged;

        private static bool isRestart;

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;

                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
            }
        }

        public static int GameCount
        {
            get { return _gameCount; }
            private set { _gameCount = value; }
        }

        private static int _gameCount = 0;

        [Header("Set the target frame rate for this game")]
        [Tooltip("Use 60 for games requiring smooth quick motion, set -1 to use platform default frame rate")]
        public int targetFrameRate = 30;

        [Header("Current game state")]
        [SerializeField]
        private GameState _gameState = GameState.Prepare;

        // List of public variable for gameplay tweaking
        [Header("Gameplay Config")]
        [Header("Score")]
        [SerializeField]
        private ScoreMode _scoreMode;
        public ScoreMode ScoreMode
        {
            get { return _scoreMode; }
        }

        [SerializeField]
        private float _distanceGetScore = 1f;
        public float DistanceGetScore
        {
            get { return _distanceGetScore; }
        }

        [SerializeField]
        private int _plusScore = 5;
        public int PlusScore
        {
            get { return _plusScore; }
        }

        [Header("Acceleration")]
        [SerializeField]
        private AccelerationMode _accelMode;
        public AccelerationMode AccelMode
        {
            get { return _accelMode; }
        }

        public float accelIndex = 0.1f;

        [Header("Player")]
        [SerializeField]
        private Vector3 startPlayerPosition;

        public float minMoveSpeed = 3;

        public float maxMoveSpeed = 10;

        [Range(0f, 1f)]
        public float coinFrequency = 0.1f;

        // List of public variables referencing other objects
        [Header("Object References")]
        public SaucerFlyingPlayerController playerController;

        void OnEnable()
        {
            SaucerFlyingPlayerController.PlayerDied += PlayerController_PlayerDied;
            CharacterScroller.ChangeCharacter += CreateNewCharacter;
        }

        void OnDisable()
        {
            SaucerFlyingPlayerController.PlayerDied -= PlayerController_PlayerDied;
            CharacterScroller.ChangeCharacter -= CreateNewCharacter;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
            CreateNewCharacter(UnityEngine.Random.Range(0,9));
            FindObjectOfType<SaucerFlyingUIManager>().StartGame();
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            // Initial setup
            Application.targetFrameRate = targetFrameRate;
            SaucerFlyingScoreManager.Instance.Reset();

            PrepareGame();
        }

        // Listens to the event when player dies and call GameOver
        void PlayerController_PlayerDied()
        {
            GameOver();
        }

        // Make initial setup and preparations before the game can be played
        public void PrepareGame()
        {
            GameState = GameState.Prepare;

            // Automatically start the game if this is a restart.
            if (isRestart)
            {
                isRestart = false;
                StartGame();
            }
        }

        // A new game official starts
        public void StartGame()
        {
            StartCoroutine(DelayStartGame());
        }

        IEnumerator DelayStartGame()
        {
            yield return new WaitForEndOfFrame();
            GameState = GameState.Playing;
            if (SaucerFlyingSoundManager.Instance.background != null)
            {
                SaucerFlyingSoundManager.Instance.PlayMusic(SaucerFlyingSoundManager.Instance.background);
            }
        }

        // Called when the player died
        public void GameOver()
        {
            if (SaucerFlyingSoundManager.Instance.background != null)
            {
                SaucerFlyingSoundManager.Instance.StopMusic();
            }

            SaucerFlyingSoundManager.Instance.PlaySound(SaucerFlyingSoundManager.Instance.gameOver);
            GameState = GameState.GameOver;
            GameCount++;

            // Add other game over actions here if necessary
        }

        // Start a new game
        public void RestartGame(float delay = 0)
        {
            isRestart = true;
            StartCoroutine(CRRestartGame(delay));
        }

        IEnumerator CRRestartGame(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void HidePlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(false);
        }

        public void ShowPlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(true);
        }

        void CreateNewCharacter(int curChar)
        {
            if (playerController != null)
            {
                DestroyImmediate(playerController.gameObject);
                playerController = null;
            }
            StartCoroutine(CR_DelayCreateNewCharacter(curChar));
        }

        IEnumerator CR_DelayCreateNewCharacter(int curChar)
        {
            yield return new WaitForEndOfFrame();
            try
            {
                GameObject player = Instantiate(SaucerFlyingCharacterManager.Instance.characters[curChar]);
                player.transform.position = startPlayerPosition;
                playerController = player.GetComponent<SaucerFlyingPlayerController>();
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
}