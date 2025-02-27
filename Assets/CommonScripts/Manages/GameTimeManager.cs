using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using WGM;

/****************************************************
    文件：GameTimeManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：游戏时间管理器
*****************************************************/
public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager instance;
    private int gameTime;
    private float CurrentTime = 0f;

    public Action<int> UpdateTimeUI;
    public Action TimeOverAction;
    private bool ISPause;
    
    private float currentKeyTimer;
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

    public void StartColdDown(int seconds)
    {
        gameTime = seconds;
        Time.timeScale = 1;
        CurrentTime = 0;
        if (UpdateTimeUI != null)
        {
            UpdateTimeUI(gameTime);
        }
    }
    private void Update()
    {
        BackMainGameByKey();
        OnCountdown();
    }
    /// <summary>
    /// 返回游戏大厅(通过按键长按)
    /// </summary>
    public void BackMainGameByKey()
    {
        if (GetCurrentTime()<5||SceneLoadManager.instance.GetCurrentScneISMainScene())
        {
            return;
        }
        if (DealCommand.GetKey(1, AppKeyCode.UpScore))
        {
            currentKeyTimer+=Time.deltaTime;
            if (currentKeyTimer>MainConstant.Keytime)
            {
                CommonUI.instance.BackMainPanel_OPen();
                currentKeyTimer = 0;
            }
        }
        else
        {
            currentKeyTimer = 0;  
        }
    }
    private void OnCountdown()
    {
        if (CurrentTime < 1 && ISPause == false)
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= 1)
            {
                Countdown();
                CurrentTime = 0;
            }
        }
    }
    /// <summary>
    /// 游戏时间倒计时
    /// </summary>
    public void Countdown()
    {
        if (ISPause)
        {
            return;
        }
        if (gameTime > 0)
        {
            gameTime -= 1;
            if (UpdateTimeUI != null)
            {
                UpdateTimeUI(gameTime);
            }
        }
        else
        {
            if (TimeOverAction != null)
            {
                TimeOverAction();
                DealCommand.SendGameResultToLedScreen(0);
                if (LocalizationManager.Instance.GetCurrentLanguage()==Language.Chinese)
                {
                    if (DealCommand.GetPrize(1))
                    {
                        CommonUI.instance.AddTips("你获得了"+LibWGM.machine.Cl_prize+"个礼品");
                        DriverManager.WinOnePrize(LibWGM.machine.Cl_prize);
                        LibWGM.playerData[1].coin_use = 0;
                        Debug.Log(LibWGM.machine.Cl_prize);
                    }
           
                }
                else
                {
                    if (DealCommand.GetPrize(1))
                    {
                        CommonUI.instance.AddTips("You get "+LibWGM.machine.Cl_prize+"gifts");
                        DriverManager.WinOnePrize(LibWGM.machine.Cl_prize);
                        LibWGM.playerData[1].coin_use = 0;
                        Debug.Log(LibWGM.machine.Cl_prize);
                    }
                }
                Debug.Log("TimeOver");
            }
        }
    }
    public void ClearAction()
    {
        TimeOverAction=null;
        UpdateTimeUI = null;
    }

    /// <summary>
    /// 获取当前游戏时间
    /// </summary>
    public int GetCurrentTime()
    {
        return gameTime;
    }
    /// <summary>
    /// 设置是否暂停计时
    /// </summary>
    public void SetPauseGame(bool _isPause)
    {
        ISPause = _isPause;
    }

    public void StopColdDown()
    {
        gameTime = 0;
        ClearAction();
    }
}