using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements;

public class SuperCommandoMenuManager : MonoBehaviour
{
    public static SuperCommandoMenuManager Instance;

    public GameObject Startmenu;
    public GameObject GUI;
    public GameObject Gameover;
    public GameObject GameFinish;
    public GameObject GamePause;
    public GameObject Controller;
    public GameObject SaveMe;
    public GameObject Loading;

    [Header("Sound and Music")]
    public Image soundImage;
    public Image musicImage;
    public Sprite soundImageOn, soundImageOff, musicImageOn, musicImageOff;

    public GameObject passLevelButton;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
     

        Startmenu.SetActive(true);
        GUI.SetActive(false);
        Gameover.SetActive(false);
        GameFinish.SetActive(false);
        GamePause.SetActive(false);
        SaveMe.SetActive(false);
        Loading.SetActive(false);
        StartCoroutine(StartGame(2));

        soundImage.sprite = SuperCommandoGlobalValue.Instance.isSound ? soundImageOn : soundImageOff;
        musicImage.sprite = SuperCommandoGlobalValue.Instance.isMusic ? musicImageOn : musicImageOff;
        if (!SuperCommandoGlobalValue.Instance.isSound)
            SuperCommandoSoundManager.Instance.SoundVolume = 0;
        if (!SuperCommandoGlobalValue.Instance.isMusic)
            SuperCommandoSoundManager.Instance.MusicVolume = 0;
    }

    #region Music and Sound
    public void TurnSound()
    {
        SuperCommandoGlobalValue.Instance.isSound = !SuperCommandoGlobalValue.Instance.isSound;
        soundImage.sprite = SuperCommandoGlobalValue.Instance.isSound ? soundImageOn : soundImageOff;

        SuperCommandoSoundManager.Instance.SoundVolume = SuperCommandoGlobalValue.Instance.isSound ? 1 : 0;
    }

    public void TurnMusic()
    {
        SuperCommandoGlobalValue.Instance.isMusic = !SuperCommandoGlobalValue.Instance.isMusic;
        musicImage.sprite = SuperCommandoGlobalValue.Instance.isMusic ? musicImageOn : musicImageOff;

        SuperCommandoSoundManager.Instance.MusicVolume = SuperCommandoGlobalValue.Instance.isMusic ? SuperCommandoSoundManager.Instance.musicsGameVolume : 0;
    }
    #endregion

    public void NextLevel()
    {
        // SoundManager.PlaySfx(SoundManager.Instance.soundClick);

        SuperCommandoGameManager.Instance.UnlockLevel();
        
        SuperCommandoGlobalValue.Instance.levelPlaying ++;
        
        Loading.SetActive(true);
        LoadABManger.Instance.UnloadAB(SceneManager.GetActiveScene().name);
        if (SuperCommandoGlobalValue.Instance.levelPlaying <= SuperCommandoGlobalValue.Instance.totalLevel)
            LoadABManger.Instance.LoadAB("SuperCommando " + SuperCommandoGlobalValue.Instance.levelPlaying); 
        else
            LoadABManger.Instance.LoadAB("SuperCommando " + (SuperCommandoGlobalValue.Instance.levelPlaying % SuperCommandoGlobalValue.Instance.totalLevel)); 
            
    }

    [Header("LOADING PROGRESS")]
    public Slider slider;
    public Text progressText;
    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (slider != null)
                slider.value = progress;
            if (progressText != null)
                progressText.text = (int)progress * 100f + "%";
            //			Debug.LogError (progress);
            yield return null;
        }
    }

    public void TurnController(bool turnOn)
    {
        Controller.SetActive(turnOn);
    }
    public void TurnGUI(bool turnOn)
    {
        GUI.SetActive(turnOn && !SuperCommandoGameManager.Instance.hideGUI);
    }

    public void OpenSaveMe(bool open)
    {
        if (open)
            StartCoroutine(OpenSaveMe());
        else
            SaveMe.SetActive(false);
    }

    IEnumerator OpenSaveMe()
    {
        yield return new WaitForSeconds(1);
        SaveMe.SetActive(true);
    }


    public void RestartGame()
    {
        StartCoroutine(ResetGame());
    }

    IEnumerator  ResetGame()
    {
        CommonUI.instance.BackMainPanel_OPen();
        yield return new WaitForSeconds(0.75f);
        
        SuperCommandoSoundManager.Instance.PlaySfx(SuperCommandoSoundManager.Instance.soundClick);
        //if (!GlobalValue.RemoveAds && DefaultValue.Instance && DefaultValue.Instance.restartLevelAd && Advertisement.IsReady())
        //{
        //    watchVideoType = WatchVideoType.Restart;
        //    var options = new ShowOptions { resultCallback = HandleShowResult };
        //    if (!Advertisement.isShowing)
        //        Advertisement.Show(options);
        //}
        //else
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Loading.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().name));
    }

    public void HomeScene()
    {
        //if (GameManager.Instance.State != GameManager.GameState.Finish)
        //{
        //    if (LevelMapType.Instance && !LevelMapType.Instance.playerNoLimitLife)
        //        GlobalValue.SaveLives -= 1;
        //}
        SuperCommandoSoundManager.Instance.PlaySfx(SuperCommandoSoundManager.Instance.soundClick);
        Time.timeScale = 1;
        //SceneManager.LoadSceneAsync("MainMenu");
        Loading.SetActive(true);
        StartCoroutine(LoadAsynchronously("MainMenu"));

    }

    public void Gamefinish()
    {
        StartCoroutine(GamefinishCo(2));
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCo(2));
    }

    public void Pause()
    {
        SuperCommandoSoundManager.Instance.PlaySfx(SuperCommandoSoundManager.Instance.soundClick);
        if (GamePause.activeSelf)
        {
            FindObjectOfType<NewSuperCommando>().isPaused = false;
            
            GamePause.SetActive(false);
            GUI.SetActive(true && !SuperCommandoGameManager.Instance.hideGUI);
            SuperCommandoSoundManager.Instance.PauseMusic(false);
        }
        else
        {
            FindObjectOfType<NewSuperCommando>().isPaused = true;
            GamePause.SetActive(true);
            GUI.SetActive(false);
            SuperCommandoSoundManager.Instance.PauseMusic(true);
        }

        SuperCommandoControllerInput.Instance.StopMove();
    }

    public enum WatchVideoType { Checkpoint, Restart, Next }
    public WatchVideoType watchVideoType;

    IEnumerator StartGame(float time)
    {
        yield return new WaitForSeconds(time - 0.5f);
        Startmenu.GetComponent<Animator>().SetTrigger("play");

        yield return new WaitForSeconds(0.5f);
        Startmenu.SetActive(false);
        GUI.SetActive(true && !SuperCommandoGameManager.Instance.hideGUI);

        SuperCommandoGameManager.Instance.StartGame();
    }

    IEnumerator GamefinishCo(float time)
    {
        GUI.SetActive(false);
        yield return new WaitForSeconds(time);
        // GameFinish.SetActive(true);
        Invoke("NextLevel",1);
        SuperCommandoSoundManager.Instance.MusicVolume = 1;
        SuperCommandoSoundManager.Instance.PlayMusic(SuperCommandoSoundManager.Instance.musicFinishPanel, false);
    }

    IEnumerator GameOverCo(float time)
    {
        GUI.SetActive(false);

        yield return new WaitForSeconds(time);

        //show ads
        Gameover.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        SuperCommandoGameManager.Instance.ResetLevel();
      
    }

    public void ForceFinishLevel()
    {
        SuperCommandoGameManager.Instance.GameFinish();
    }
}
