using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using WGM;

public class CtrUI : CtrBase
{

    static CtrUI _instance;

    public static CtrUI instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CtrUI>();
            }

            return _instance;
        }
    }
    public TextMeshProUGUI textBallCount;
    public TextMeshProUGUI textTurn;

    public TextMeshProUGUI textScore;

    public Sprite[] spriteCombo;

    public ComboEffectText _ComboEffectText;

    public void SetTurn(int num)
    {
        textTurn.text = num.ToString();
    }

    public void AllClear()
    {
        _ComboEffectText.AllClear();

        //Counting GameData
        BricksBreakerPlayManager.Instance.countAllClear++;
    }

    bool isLucky = false;

    public void LuckyBonus()
    {
        if (isLucky) return;
        isLucky = true;
        _ComboEffectText.Lucky();

        //Counting GameData
        BricksBreakerPlayManager.Instance.countLuckyBonus++;
    }

    /// <summary>
    /// Reset for next turn
    /// </summary>
    public void NextTurnReady()
    {
        isLucky = false;
    }


    private void Awake()
    {
        _ComboEffectText.UIReset();
        SetResolutionScreen();
    }

    public Camera mainCamera;


    int width;
    float screenRatio;

    public void SetResolutionScreen()
    {
        float screenRatio = (1.0f * Screen.width) / (1.0f * Screen.height);


#if UNITY_ANDROID
        /*
        //10:16
        width = 1200;
        heigh = 1920;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>10:16</color> : " + screenRatio);

        //10:16
        width = 1600;
        heigh = 2560;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>10:16</color> : " + screenRatio);

        //10:16
        width = 800;
        heigh = 1280;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>10:16</color> : " + screenRatio);


        //9:16
        width = 640;
        heigh = 1136;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:16</color> : " + screenRatio);

        //9:16
        width = 720;
        heigh = 1280;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:16</color> : " + screenRatio);

        //9:16
        width = 750;
        heigh = 1334;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:16</color> : " + screenRatio);

        //9:16
        width = 1440;
        heigh = 2560;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:16</color> : " + screenRatio);

        //9:16
        width = 1080;
        heigh = 1920;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:16</color> : " + screenRatio);

        //9:18
        width = 1440;
        heigh = 2880;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:18</color> : " + screenRatio);

        //9:18.5
        width = 1440;
        heigh = 2960;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:18.5</color> : " + screenRatio);

        //9:19
        width = 1440;
        heigh = 3040;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:19</color> : " + screenRatio);

        //9:19.5
        width = 1440;
        heigh = 3120;
        screenRatio = (1.0f * width) / (1.0f * heigh);
        Debug.Log("<color=red>9:19:.5</color> : " + screenRatio);
    */

        if (screenRatio < 0.47f) {
            //9:19.5
            //Debug.Log("<color=red>9:19.5</color> : " + screenRatio);
            mainCamera.orthographicSize = 7.8f;
        } else if (screenRatio > 0.47f && screenRatio < 0.48f) {
            //9:19
            //Debug.Log("<color=red>9:19</color> : " + screenRatio);
            mainCamera.orthographicSize = 7.6f;
        } else if (screenRatio > 0.48f && screenRatio < 0.495f) {
           //Debug.Log("<color=red>9:18.5</color> : " + screenRatio);
            //9:18.5
            mainCamera.orthographicSize = 7.4f;
        } else if (screenRatio > 0.495f && screenRatio < 0.55f) {
            //Debug.Log("<color=red>9:18</color> : " + screenRatio);
            //9:18
            mainCamera.orthographicSize = 7.2f;
        } else {
            //Debug.Log("<color=red>9:16</color> : " + screenRatio);
            //9:16
            mainCamera.orthographicSize = 6.4f;
        }
#else


        if (screenRatio > 0.6f && screenRatio < 0.7f)
        {
            //Debug.Log("3:2 iPhones models 4 and earlier");
        }
        else if (screenRatio > 0.5f && screenRatio < 0.6f)
        {
            //Debug.Log("16:9 iPhones models 5, SE, 8+");
        }
        else if (screenRatio > 0.4f && screenRatio < 0.5f)
        {
            //Debug.Log("19.5:9 iPhones - models X, Xs, Xr, Xsmax");
            mainCamera.orthographicSize = 7.8f;
        }
        else
        {
            //Debug.Log("Find Not iPhones Size");
        }
#endif
    }


    public void SetBallCount(int ballCount)
    {
        textBallCount.text = string.Format("x{0}", ballCount);
    }
    
    public void Click_ReturnBall()
    {
        if (Player.instance.isReturnBall) return;
        Player.instance.isReturnBall = true;

        BricksBreakerSoundManager.Instance.PlayEffect(SoundList.sound_play_common_sfx_ballcollect);
        Player.instance.ReturnBall();
    }







    bool isScoreAnim = false;

    public void AddScore(int num)
    {
        BricksBreakerPlayManager.Instance.score += num;

        StartCoroutine(ScoreAnimCo(num));
    }

    IEnumerator ScoreAnimCo(int num)
    {
        isScoreAnim = true;
        int bScore = BricksBreakerPlayManager.Instance.score - num;
        int score = BricksBreakerPlayManager.Instance.score;

        DOTween.To(() => bScore, x => score = x, score, 0.5f).SetEase(Ease.OutCubic)
            .OnComplete(() => { isScoreAnim = false; });

        while (isScoreAnim)
        {
            textScore.text = Utility.ChangeThousandsSeparator(score);
            yield return null;
        }
    }

    private void Update()
    {
         if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore)&&CtrGame.instance.IsLock)
         {
             Click_ReturnBall();
         }
    }
}



