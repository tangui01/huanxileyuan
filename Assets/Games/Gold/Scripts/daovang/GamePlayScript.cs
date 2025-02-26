using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WGM;

public class GamePlayScript : MonoBehaviour {
    public static GamePlayScript instance;
	private int time;
    private float countDown;
    public Animator animPanelPlay;
    public GameObject scoreTextFly, boomFly;
    public Transform canvas;
    public string itemSeclected;
    private GameObject itemDestroy, fireObject;
    
    public AudioClip explosive, lowValue, normalValue, highValue, pull, lose, win,level_bg;

    bool victory, fail;
    // Use this for initialization
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AudioManager.Instance.playerBGm(level_bg);
    }

    public void PlaySound(int i)
    {
        switch (i)
        {
            case 1:
                AudioManager.Instance.playerEffect2(lowValue);
                break;
            case 2:
                AudioManager.Instance.playerEffect2(normalValue);
                break;
            case 3:
                AudioManager.Instance.playerEffect2(highValue);
                break;
            case 4:
                AudioManager.Instance.playerEffect1(explosive);
                break;
            case 5:
                if (!AudioManager.Instance.Effect2ISPalyer())
                {
                    AudioManager.Instance.playerEffect2(pull);  
                }
                break;
            case 7:
                AudioManager.Instance.playerEffect1(lose);
                break;
            case 8:
                AudioManager.Instance.playerEffect1(win);
                break;
        }
    }

    public void StopAllSound()
    {
        AudioManager.Instance.StopEffect1Player();
        AudioManager.Instance.StopEffect2Player();
    }

    public void Power()
    {
        GoldMinerGameManager.instance.powerCurrent = true;
        OngGiaScript.instance.Happy(true);
    }
    
    public void CreateScoreFly(int score)
    {
        Vector3 vector3 = scoreTextFly.transform.position;
        Instantiate(scoreTextFly, vector3, Quaternion.identity).transform.SetParent(canvas, false);
        TextScoreScript.score = score;
    }

    public void ScoreZoomEffect()
    {
        animPanelPlay.SetBool("Zoom", true);
        StartCoroutine(ScoreZoomOut());
    }
    IEnumerator ScoreZoomOut()
    {
        yield return new WaitForSeconds(1f);
        animPanelPlay.SetBool("Zoom", false);
    }
    public void CreateBoomFly()
    {
        Vector3 vector3 = boomFly.transform.position;
        Instantiate(boomFly, vector3, Quaternion.identity).transform.SetParent(canvas, false);
    }
}
