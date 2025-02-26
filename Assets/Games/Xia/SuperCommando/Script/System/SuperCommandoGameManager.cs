using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class SuperCommandoGameManager : MonoBehaviour
{
    public static SuperCommandoGameManager Instance { get; private set; }

    public enum GameState { Menu, Playing, Dead, Finish, Waiting };
    public GameState State { get; set; }

    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask groundLayer;

    public bool isWatchingAd { get; set; }

    [Header("CONTINUE GAME OPTION")]
    public int continueCoinCost = 100;

    // public bool canBeSave()
    // {
    //     return (GlobalValue.SavedCoins >= continueCoinCost) || (AdsManager.Instance && AdsManager.Instance.isRewardedAdReady());
    // }

    public GameObject FadeInEffect;
    [Header("Floating Text")]
    public GameObject FloatingText;
    private SuperCommandoMenuManager menuManager;

    [ReadOnly] public Vector2 currentCheckpoint = Vector2.zero;

    public bool isSpecialBullet { get; set; }
    public bool isHasKey
    {
        get
        {
            return hasKey;
        }
        set
        {
            if (value)
                KeyUI.Instance.Get();
            else
                KeyUI.Instance.Used();

            hasKey = value;
        }
    }
    bool hasKey = false;

    public SuperCommandoPlayer Player { get; private set; }

    [HideInInspector]
    public bool isNoLives = false;

    public int MissionStarCollected { get; set; }

    public bool hideGUI = false;

    void Awake()
    {
        isSpecialBullet = false;
        Instance = this;
        State = GameState.Menu;
        Player = FindObjectOfType<SuperCommandoPlayer>();
        //MissionStarCollected = 0;

        if (CharacterHolder.Instance != null && CharacterHolder.Instance.CharacterPicked != null)
        {
            Instantiate(CharacterHolder.Instance.CharacterPicked, Player.transform.position, Player.transform.rotation);
            Destroy(Player.gameObject);

            Player = FindObjectOfType<SuperCommandoPlayer>();
        }

        var startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        if (startPoint)
            Player.transform.position = startPoint.transform.position;
    }


    public int Point { get; set; }
    int savePointCheckPoint;

    public int Coin { get; set; }
    int saveCoinCheckPoint;
    int checkpointBigStar;

    public void AddPoint(int addpoint)
    {
        Point += addpoint;
    }

    public void AddBullet(int addbullet)
    {
        /*Bullet*/
        if (DefaultValue.Instance && DefaultValue.Instance.defaultBulletMax)
            Debug.LogWarning("NO LIMIT BULLET");
        else
            SuperCommandoGlobalValue.Instance.Bullets += addbullet;
    }

    public void PauseCamera(bool pause) {
        SuperCommandoCameraFollow.Instance.pauseCamera = pause;
        if (pause == false)
            SuperCommandoCameraFollow.Instance.MoveCameraToPlayerPos();
    }


    void Start()
    {
        menuManager = FindObjectOfType<SuperCommandoMenuManager>();
        currentCheckpoint = Player.transform.position;
        

        SuperCommandoSoundManager.Instance.PlaySfx(SuperCommandoSoundManager.Instance.beginSoundInMainMenu);
    }

    private void Update()
    {
        
    }
    


    public void SaveCheckPoint(Vector2 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    private void ResetCheckPoint()
    {
        if (savePointCheckPoint != 0)
        {
            Point = savePointCheckPoint;
            //Coin = saveCoinCheckPoint;
            State = GameState.Playing;

        }
        else
        {
            //Coin = GlobalValue.SavedCoins;

        }
    }

    public void ShowFloatingText(string text, Vector2 positon, Color color)
    {
        GameObject floatingText = Instantiate(FloatingText) as GameObject;
        var _position = Camera.main.WorldToScreenPoint(positon);

        floatingText.transform.SetParent(menuManager.transform, false);
        floatingText.transform.position = _position;

        var _FloatingText = floatingText.GetComponent<FloatingText>();
        _FloatingText.SetText(text, color);
    }

    public void StartGame()
    {
        State = GameState.Playing;

        var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
        foreach (var _listener in listener_)
        {
            _listener.IPlay();
        }

        SuperCommandoSoundManager.Instance.PlayGameMusic();
    }

    public void GameFinish(int delay = 0)
    {
        if (State == GameState.Finish)
            return;

        State = GameState.Finish;

        var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
        foreach (var _listener in listener_)
        {
            _listener.ISuccess();
        }


        //GlobalValue.SavedCoins = Coin;
        Invoke("FinishGame", delay);
    }

    void FinishGame()
    {
        Player.GameFinish();
        SuperCommandoMenuManager.Instance.Gamefinish();
        SuperCommandoSoundManager.Instance.PauseMusic(true);
        SuperCommandoSoundManager.Instance.PlaySfx(SuperCommandoSoundManager.Instance.soundGamefinish, 0.3f);
    }

    public void UnlockLevel()
    {
        //check to unlock new level
        if (SuperCommandoGlobalValue.Instance.levelPlaying == (SuperCommandoGlobalValue.Instance.LevelHighest))
        {
            SuperCommandoGlobalValue.Instance.LevelHighest++;
            Debug.LogWarning("Unlock new level");
        }
    }

    public void GameOver(bool forceGameOver = false)
    {
        StartCoroutine(GameOverCo(forceGameOver));
    }

    public IEnumerator GameOverCo(bool forceGameOver = false)
    {
        SuperCommandoControllerInput.Instance.Shot(false);
        if (State != GameState.Dead && State != GameState.Waiting)
        {
            SuperCommandoGlobalValue.Instance.Attempt++;
        }

        if (State == GameState.Dead)
            yield break;

        // if (State != GameState.Dead && State != GameState.Waiting && AdsManager.Instance)
        // {
        //     AdsManager.Instance.ShowNormalAd(GameManager.GameState.Dead);
        // }

        if (!forceGameOver )//&& canBeSave()
        {
            if (State == GameState.Dead || State == GameState.Waiting)
                yield break;

            State = GameState.Waiting;
            Player.Kill();

            SuperCommandoControllerInput.Instance.StopMove();
            SuperCommandoMenuManager.Instance.GUI.SetActive(false && !hideGUI);
            SuperCommandoSoundManager.Instance.PauseMusic(true);
            SuperCommandoMenuManager.Instance.OpenSaveMe(true);
        }
        else
        {
            var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
            foreach (var _listener in listener_)
            {
                _listener.IGameOver();
            }

            SuperCommandoControllerInput.Instance.StopMove();
            State = GameState.Dead;
           
            SuperCommandoMenuManager.Instance.GameOver();
            SuperCommandoSoundManager.Instance.PlaySfx(SuperCommandoSoundManager.Instance.soundGameover, 0.5f);
            SuperCommandoSoundManager.Instance.PauseMusic(true);

            //Reset Lives
            SuperCommandoGlobalValue.Instance.ResetLives(); 
        }
    }

    public void Continue()
    {
        StartCoroutine(ContinueCo());
    }

    IEnumerator ContinueCo()
    {
        var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
        foreach (var _listener in listener_)
        {
            _listener.IOnRespawn();
        }

        SuperCommandoMenuManager.Instance.OpenSaveMe(false);
        Player.RespawnAt(currentCheckpoint);

        yield return new WaitForSeconds(1.5f);
        State = GameState.Playing;
        SuperCommandoMenuManager.Instance.GUI.SetActive(true && !hideGUI);
        SuperCommandoSoundManager.Instance.PauseMusic(false);
    }

    public void ResetLevel()
    {
        SuperCommandoMenuManager.Instance.RestartGame();
    }
}
