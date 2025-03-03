using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SignalR;
using UnityEngine;
using WGM;

/****************************************************
    文件：AutoGameManger.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：自动游戏管理器
*****************************************************/
public class AutoGameManger : MonoBehaviour
{
    public static AutoGameManger Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (AutoGame())
        {
            GameStateManager.Instance.stateChangedAction+=Test;
        }
    }

    public static Action WaitplayCanToplay ;
    public static Action PlayCanToInGame ;
    
    public void Test(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Waitpalyer:
                WaitplayCanToplay?.Invoke();
                break;
            case GameState.Play:
                PlayCanToInGame?.Invoke();
                break;
        }
    }
    //判断是否自动
    public bool AutoGame()
    {
        if (LibWGM.machine.AutoTime>LibWGM.machine.GameTime)
        {
            CommonUI.instance.AddTips("自动游戏时间设置错误");
            return false;
        }
        if (LibWGM.machine.AutoTime <= 0)
        {
            return false;
        }
        return true;
    }

    private void Update()
    {
        
    }
}
