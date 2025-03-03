using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

/****************************************************
    文件：GameStateManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：游戏状态管理器
*****************************************************/
public enum GameState
{
    None,//初始状态
    Idle,//没有投币
    NoCoinCount,//投了币但是不够数量
    Waitpalyer,//币数足够 等待按开始健
    Play,//选择进入游戏中
    InGame//在游戏场景中
}
public class GameStateManager : MonoBehaviour
{
     public static GameStateManager Instance;
     private GameState currentState = GameState.None;
     
     public Action<GameState> stateChangedAction;

     private void Awake()
     {
         if (Instance==null)
         {
             Instance=this;
         }
         else
         {
             Destroy(gameObject);
         }
     }
     public void SwitchState(GameState newState)
     {
         if (currentState==newState)
         {
             return;
         }
         currentState =newState;
         SetGamesStateByCoinCount();
         stateChangedAction?.Invoke(currentState);
     }
     public void SetGamesStateByCoinCount()
     {
         //一种没有一个币的，显示请投币
         if ((LibWGM.playerData[1].coin_in+LibWGM.playerData[0].Free_coin_in==0)&&(GameTimeManager.instance.GetCurrentTime()<=0))
         {
             DealCommand.Instance.SerialPortManager.SendGameState(0);
             Debug.Log("播放盒子音乐");
         }
         else
         {
             DealCommand.Instance.SerialPortManager.SendGameState(1);
             Debug.Log("播放背景音乐");
         }
     }
     public GameState GetCurrentGameState()
     {
         return  currentState;
     }

     public bool GetTarGetGameStateIsEqual(GameState tarGetGameState)
     {
         if (tarGetGameState==currentState)
         {
             return true;
         }
         return false;
     }
     
}
