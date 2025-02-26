using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WGM;
using Random = UnityEngine.Random;

public class DualControlGameManager : MonoBehaviour
{
    public static DualControlGameManager instance; 
    int CurrentLevel = 0;

    public GameObject GameOverUI, GameWinUI;
    public bool finished;
    public Text currentText, nextText;

    public AudioClip BackgroundSound, GameoverSound, ClickSound,Level_UpSound;

    [Header("Reaction")]
    public GameObject[] Emoji;

    [Header("Control")]
    public GameObject Dpad;
    public GameObject Joystick;
    public GameObject Steering;

    ParticleSystem PS1, PS2;

    [HideInInspector]
    public int Cars;

    public Text ScoreText;
    private static float Score = 0;
    public GameObject[] Level;
    public void Awake()
    {
        AudioManager.Instance.playerBGm(BackgroundSound);
        ScoreText.text =(int)Score+"";
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        CurrentLevel = Random.Range(0,Level.Length);//关卡下标
        Instantiate(Level[CurrentLevel]);
    }
    

    public void AddScore(int score = 0)
    {
        Score += score; 
    }
    void Start()
    {
        finished = false;
        currentText.text = "MISSION " + (CurrentLevel+1);
        
        // int controlActive = PlayerPrefs.GetInt("Control", 0);//操作方式
        // if (controlActive == 1)
        // {
        //     Dpad.SetActive(true);
        // }
        // else if (controlActive == 2)
        // {
        //     Joystick.SetActive(true);
        // }

        PS1 = GameObject.FindGameObjectWithTag("Blast").GetComponent<ParticleSystem>();
    }
    [HideInInspector]
    public bool isPaly = false;
    void Update()
    {
        if (Cars == 2 && !finished) {
            GameWin();
        }

        if (isPaly)
        {
            Score += Time.deltaTime*10; 
            ScoreText.text =(int)Score+"";
        }

    }

    IEnumerator End() {
        isPaly = false;
        yield return new WaitForSeconds(1f);
        CommonUI.instance.BackMainPanel_OPen();
        yield return new WaitForSeconds(1f);
        Score = 0;
        Restart();
    }
    
    public void GameEnd() {
        finished = true;
        AudioManager.Instance.playerEffect1(GameoverSound);
        AudioManager.Instance.StopBGm();
        RunEmoji();
        StartCoroutine(End());
    }

    public void GameWin()
    {
        isPaly = false;
        finished = true;
        // Adcontrol.instance.ShowInterstitial();
        PS1.Play();
        AudioManager.Instance.playerEffect1(Level_UpSound);
        CurrentLevel = Random.Range(0,Level.Length);
        StartCoroutine(Win());
       
    }


    IEnumerator Win() {
        yield return new WaitForSeconds(2f);
        Restart();
    }



    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    //显示表情UI
    void RunEmoji() {
        int RandNum = Random.Range(0, 3);
        Emoji[RandNum].SetActive(true);
        Dpad.SetActive(false);
    }
    
}
