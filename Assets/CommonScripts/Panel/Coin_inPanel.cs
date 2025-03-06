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
    [SerializeField] private AudioClip SwitchSound;
    [SerializeField] private AudioClip ClickSound;
    private int index=0;
    private int indexmax=1;
    private int indexmin=0;
    [SerializeField]private Animator surebtnAni;
    [SerializeField]private Animator cancelBtnAni;
    private const string BtnSelectbool = "Select";
    /// <summary>
    /// 续币成功
    /// </summary>
    public static Action Renew_money_Success;
   
    private bool isgameover = false;
    //是否已经判断
    private bool isjudge=false;
    private void Awake()
    {
        Ani = GetComponent<Animator>();
    }

    public void StartColdDown(bool _isgameover)
    {
        countdown = 20;
        CommonUI.instance.BackMain.gameObject.SetActive(false);
        timetext.text = countdown + "S";
        Ani.SetTrigger("Update");
        isgameover=_isgameover;
        delaytime=1;
        isjudge=false;
        SwitchBtnViscal();
        if (AutoGameManger.Instance.AutoGame()&&LibWGM.machine.AutoTime<countdown&&CurrentCoinCountPanel.instance.ISStartGame())
        {
            CommonUI.instance.AutoPanel.StartColddwon();
        }
    }

    private void ExitColdDown()
    {
        Ani.SetTrigger("Exit");
    }
    /// <summary>
    /// 计时结束后
    /// </summary>
    public void ColdDownover()
    {
        SwitchBtnViscal();
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
            countdown -=1;
            countdown = Mathf.Max(0, countdown);
            timetext.text = countdown + "S";
        }
    }

    private void Success()
    {
        CurrentCoinCountPanel.instance.reduceCoinCount();
        GameStateManager.Instance.SwitchState(GameState.Play);
        CommonUI.instance.AutoPanel.Stop();
        GameTimeManager.instance.StartColdDown(LibWGM.machine.GameTime);
        CommonUI.instance.ExitCouterColdDown();
        Renew_money_Success?.Invoke();
        AudioManager.Instance.playerEffect3(SuccessSound);
        delaytime = 0;
        isjudge=true;
        gameObject.SetActive(false);
    }
    private void Fail()
    {
        GameStateManager.Instance.SwitchState(GameState.Idle);
        SceneLoadManager.instance.BackMainGameByCoin_in();
        AudioManager.Instance.playerEffect3(FailSound);
        delaytime = 0;
        isjudge = true;
        gameObject.SetActive(false);
    }
    private float delaytime=0f;
    private float AutoTime=0f;
    private void Update()
    {
        if (AutoGameManger.Instance.AutoGame()&&!isjudge)
        {
            AutoTime+=Time.unscaledDeltaTime;
            if (AutoTime>=LibWGM.machine.AutoTime)
            {
                AutoTime = 0;
                SwitchBtnClickEvent();
            }
        }
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
        SelectBtnInput();
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
            ColdDownover();
        }
    }
    private void SwitchBtnClickEvent()
    {
        switch (index)
        {
            case 0:
                IsRenewmoney();
                break;
            case 1:
                Fail();
                break;
        } 
    }
    private void SelectBtnInput()
    {
        if (DealCommand.GetKeyDown(1,AppKeyCode.TicketOut))
        {
            index--;
            AudioManager.Instance.playerEffect4(SwitchSound);
            if (index<indexmin)
            {
                index = 1;
            }
            SwitchBtnViscal();
        }
        else if (DealCommand.GetKeyDown(1,AppKeyCode.Flight))
        {
            index++;
            AudioManager.Instance.playerEffect4(SwitchSound);
            if (index>indexmax)
            {
                index = 0;
            }
            SwitchBtnViscal();
        }
        else if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore))
        {
            SwitchBtnClickEvent();
        }
    }
    private void SwitchBtnViscal()
    {
        switch (index)
        {
            case 1:
                surebtnAni.SetBool(BtnSelectbool,false);
                cancelBtnAni.SetBool(BtnSelectbool,true);
                break;
            case 0:
                surebtnAni.SetBool(BtnSelectbool,true);
                cancelBtnAni.SetBool(BtnSelectbool,false);
                break;
        }
    }
}