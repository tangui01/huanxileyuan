using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WGM;

/****************************************************
    文件：Coin_inPanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：投币面版
*****************************************************/
public class Coin_inPanel : MonoBehaviour
{
    public Text timetext;
    public float countdown;
    private float timer;
    private Animator Ani;
    [SerializeField] private AudioClip SuccessSound;
    [SerializeField] private AudioClip FailSound;
    /// <summary>
    /// 续币成功
    /// </summary>
    public static Action Renew_money_Success;
   
    private bool isgameover = false;
    private void Awake()
    {
        Ani = GetComponent<Animator>();
    }

    public void StartColdDown(bool _isgameover)
    {
        countdown = 20;
        DealCommand.SendGameResultToLedScreen(0);
        if (LocalizationManager.Instance.GetCurrentLanguage()==Language.Chinese)
        {
            if (DealCommand.GetPrize(1))
            {
                CommonUI.instance.AddTips("你获得了"+LibWGM.machine.Cl_prize+"个礼品");
                DriverManager.WinOnePrize(LibWGM.machine.Cl_prize);
                LibWGM.machine.Cl_prize = 0;
                Debug.Log(LibWGM.machine.Cl_prize);
            }
           
        }
        else
        {
            if (DealCommand.GetPrize(1))
            {
                CommonUI.instance.AddTips("You get "+LibWGM.machine.Cl_prize+"gifts");
                DriverManager.WinOnePrize(LibWGM.machine.Cl_prize);
                LibWGM.machine.Cl_prize = 0;
            }
        }
        CommonUI.instance.BackMain.gameObject.SetActive(false);
        timetext.text = countdown + "S";
        Ani.SetTrigger("Update");
        isgameover=_isgameover;
        delaytime=1;
    }

    public void ExitColdDown()
    {
        Ani.SetTrigger("Exit");
    }

    /// <summary>
    /// 计时结束后
    /// </summary>
    public void ColdDownover()
    {
        if (CommonUI.instance.CoinCountPanel.ISStartGame())
        {
            Success();
        }
        else
        {
            Fail();
        }
    }
    /// <summary>
    /// 是否续币成功
    /// </summary>
    public void IsRenewmoney()
    {
        if (CommonUI.instance.CoinCountPanel.ISStartGame())
        {
            Success();
        }
        else
        {
            countdown -= 1;
        }
    }

    private void Success()
    {
        CurrentCoinCountPanel.instance.reduceCoinCount();
        GameStateManager.Instance.SwitchState(GameState.Play);
        GameTimeManager.instance.StartColdDown(LibWGM.machine.GameTime);
        CommonUI.instance.ExitCouterColdDown();
        Renew_money_Success?.Invoke();
        AudioManager.Instance.playerEffect3(SuccessSound);
        delaytime = 0;
        gameObject.SetActive(false);
    }

    private void Fail()
    {
        GameStateManager.Instance.SwitchState(GameState.Idle);
        SceneLoadManager.instance.BackMainGameByCoin_in();
        AudioManager.Instance.playerEffect3(FailSound);
        delaytime = 0;
        gameObject.SetActive(false);
    }

    private float delaytime=0f;

    private void Update()
    {
        if (timer < 1)
        {
            timer += Time.unscaledDeltaTime;
        }
        else
        {
            timer = 0;
            delaytime += 1;
            OnColdDown();
        }
        //续币
        if (DealCommand.GetKeyDown(1, AppKeyCode.UpScore)&&delaytime>=2f)
        {
            IsRenewmoney();
        }
        else
        {
            delaytime+=Time.unscaledDeltaTime;
        }
    }

    void OnColdDown()
    {
        if (countdown > 0)
        {
            countdown -= 1;
            timetext.text = countdown + "S";
        }
        else
        {
            ExitColdDown();
        }
    }
}