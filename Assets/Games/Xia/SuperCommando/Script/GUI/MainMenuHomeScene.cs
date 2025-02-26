using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class MainMenuHomeScene : MonoBehaviour {
	public static MainMenuHomeScene Instance;

	public GameObject StartMenu;
	public GameObject WorldsChoose;
	public GameObject LoadingScreen;
    public GameObject Settings;
	SuperCommandoSoundManager soundManager;
    [Header("Sound and Music")]
    public Image soundImage;
    public Image musicImage;
    public Sprite soundImageOn, soundImageOff, musicImageOn, musicImageOff;
    private SuperCommandoGlobalValue superCommandoGlobalValue;
    void Awake(){
		Instance = this;
		soundManager = FindObjectOfType<SuperCommandoSoundManager> ();
        superCommandoGlobalValue = FindObjectOfType<SuperCommandoGlobalValue> ();
    }
    
	void Start () {
        // if(AdsManager.Instance)
        //     AdsManager.Instance.ShowAdmobBanner(true);

        if (!superCommandoGlobalValue.isSetDefaultValue)
        {
            superCommandoGlobalValue.isSetDefaultValue = true;
            if (DefaultValue.Instance)
            {
                superCommandoGlobalValue.Bullets = DefaultValue.Instance.defaultBulletMax ? int.MaxValue : DefaultValue.Instance.defaultBullet;
               superCommandoGlobalValue.SaveLives = DefaultValue.Instance.defaultLives;
            }
        }

        StartMenu.SetActive(false);
        WorldsChoose.SetActive (false);
		LoadingScreen.SetActive (false);
        Settings.SetActive(false);
        soundManager.PlayMusic(SuperCommandoSoundManager.Instance.musicsMenu);
        if (superCommandoGlobalValue.isFirstOpenMainMenu)
        {
            superCommandoGlobalValue.isFirstOpenMainMenu = false;
            soundManager.ResetMusic();
        }

        soundManager.PlayMusic(soundManager.musicsMenu);
        StartMenu.SetActive(true);
        

        soundImage.sprite = superCommandoGlobalValue.isSound ? soundImageOn : soundImageOff;
        musicImage.sprite = superCommandoGlobalValue.isMusic ? musicImageOn : musicImageOff;
        if (!superCommandoGlobalValue.isSound)
            soundManager.SoundVolume = 0;
        if (!superCommandoGlobalValue.isMusic)
            soundManager.MusicVolume = 0;

        soundManager.PlayGameMusic();
        
    }

    #region Music and Sound
    public void TurnSound()
    {
        superCommandoGlobalValue.isSound = !superCommandoGlobalValue.isSound;
        soundImage.sprite = superCommandoGlobalValue.isSound ? soundImageOn : soundImageOff;

        soundManager.SoundVolume = superCommandoGlobalValue.isSound ? 1: 0;
    }

    public void TurnMusic()
    {
        superCommandoGlobalValue.isMusic = !superCommandoGlobalValue.isMusic;
        musicImage.sprite = superCommandoGlobalValue.isMusic ? musicImageOn : musicImageOff;

        soundManager.MusicVolume = superCommandoGlobalValue.isMusic ? SuperCommandoSoundManager.Instance.musicsGameVolume : 0;
    }
    #endregion

    public void TurnExitPanel(bool open)
    {
        soundManager.Click();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenFacebook()
    {
#if !UNITY_WEBGL
        GameMode.Instance.OpenFacebook();
#else
        openPage(facebookLink);
#endif
        soundManager.PlaySfx(soundManager.soundClick);
    }

    public void RemoveAds()
    {
#if UNITY_PURCHASING
        if (Purchaser.Instance)
        {
            Purchaser.Instance.BuyRemoveAds();
        }
#endif
    }

    public void OpenSettings(bool open)
    {
        soundManager.Click();
        Settings.SetActive(open);
        StartMenu.SetActive(!open);
    }

	public void OpenWorldChoose(){
        StartMenu.SetActive(false);
        WorldsChoose.SetActive (true);

        soundManager.PlaySfx (soundManager.soundClick);
    }

	public void OpenStartMenu(){
        StartMenu.SetActive(true);
        WorldsChoose.SetActive (false);

        soundManager.PlaySfx (soundManager.soundClick);
    }

    public void LoadScene(string name)
    {
        WorldsChoose.SetActive(false);
        //SceneManager.LoadSceneAsync(name);
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(name));
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
                progressText.text = (int) progress * 100f + "%";
            yield return null;
        }
    }

    public void Exit(){
		Application.Quit ();
	}
}
