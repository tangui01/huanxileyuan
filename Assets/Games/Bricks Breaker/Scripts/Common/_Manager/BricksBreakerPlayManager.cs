using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum SceneType {
    None,
    Home,
    Play,
    Result
}

public class BricksBreakerPlayManager : MonoBehaviour
{
    public SceneType sceneType = SceneType.None;
    [HideInInspector] public bool isLoadScene = false;
    [HideInInspector] public int score;
    [HideInInspector] public int turn;
    [HideInInspector] public bool isSaveGameStart = false;
    [HideInInspector] public bool isFade = false;
    [HideInInspector] public int countPlay;
    [HideInInspector] public int countBreakeBrick;
    [HideInInspector] public int countAllClear;
    [HideInInspector] public int countLuckyBonus;
    [HideInInspector] public int countHighestCombo;
    [HideInInspector] public bool isPopupOn = false;

    [HideInInspector] public PanelBase panelBase;
    public Scene scene;
   public static BricksBreakerPlayManager Instance;

    private CtrBase _ctrBase;

    public CtrBase currentBase
    {
        get { return _ctrBase; }
        set { _ctrBase = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Play sound when combo effect
    /// </summary>
    public void CameraSound()
    {
        StartCoroutine(CameraSoundCo());
    }

    IEnumerator CameraSoundCo()
    {
        int num = Random.Range(5, 6);
        for (int i = 0; i < num; i++)
        {
            BricksBreakerSoundManager.Instance.PlayEffect(SoundList.sound_gameover_sfx_cameraflash);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
    /// <summary>
    /// 결과팝업에서 카메라 셔터 사운드 재생
    /// </summary>
    Coroutine cameraCoroutine;

    bool isCameraSound = false;

    public void CameraSoundLoop()
    {
        isCameraSound = true;
        cameraCoroutine = StartCoroutine(CameraSoundLoopCo());
    }

    IEnumerator CameraSoundLoopCo()
    {
        while (isCameraSound)
        {
            BricksBreakerSoundManager.Instance.PlayEffect(SoundList.sound_gameover_sfx_cameraflash);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}


