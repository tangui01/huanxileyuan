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
    Idle,//没有投币
    NoCoinCount,//投了币但是不够数量
    Waitpalyer,//币数足够 等待按开始健
    Play//游玩状态中
}
public class GameStateManager : MonoBehaviour
{
     public static GameStateManager Instance;
     private GameState currentState = GameState.Idle;
     
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
     private void Start()
     {
         SwitchState(GameState.Idle);
     }
     public void SwitchState(GameState newState)
     {
         if (currentState==newState)
         {
             return;
         }
         currentState =newState;
         if (currentState== GameState.Idle)
         {
             LedScreenAudioController.PlayBoxAudio();
         }
         else
         {
             LedScreenAudioController.PlayMachineAudio();
         }
         stateChangedAction?.Invoke(currentState);
     }
     public void SetGamestateByCoinCount()
     {
         //一种没有一个币的，显示请投币
         if (LibWGM.playerData[1].coin_in+LibWGM.playerData[0].Free_coin_in==0)
         {
             SwitchState(GameState.Idle);
         }
         //投了一个币以上的，显示还差多少币
         else if (LibWGM.playerData[1].coin_in+LibWGM.playerData[0].Free_coin_in<LibWGM.machine.Cp_coin&&LibWGM.playerData[1].coin_in>=1)
         {
             SwitchState(GameState.NoCoinCount);
         }
         else if (GameTimeManager.instance.GetCurrentTime()<=0)
         {
             SwitchState(GameState.Waitpalyer);
         }
         //如果还有游戏时间
         else  if (GameTimeManager.instance.GetCurrentTime()>0)
         {
             if (!GetTarGetGameStateIsEqual(GameState.Play))
             {
                 SwitchState(GameState.Play);
             }
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
