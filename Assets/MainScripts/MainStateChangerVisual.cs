using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：MainStateChangerVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：游戏大厅状态改变视觉和音乐效果
*****************************************************/
public class MainStateChangerVisual : MonoBehaviour
{
    public AudioClip IdleClip;
    public AudioClip PalyerClip;
    
    public GameObject PlayerrEf;
    public StartSelectPanel StartSelectPanel;
   
    private void Start()
    {
        SwitchState(GameStateManager.Instance.GetCurrentGameState());
        GameStateManager.Instance.stateChangedAction += SwitchState;
    }

    public void SwitchState(GameState state)
    {
        switch (state)
        {
            case GameState.Idle :
                AudioManager.Instance.playerBGm(IdleClip);
                if(PlayerrEf.activeSelf)
                {
                    PlayerrEf.SetActive(false);
                    StartSelectPanel.ExitAni();
                }     
                break;
            case GameState.NoCoinCount :
                if (PlayerrEf.activeSelf)
                {
                    AudioManager.Instance.playerBGm(IdleClip);
                    PlayerrEf.SetActive(false);
                    StartSelectPanel.ExitAni();
                }
                break;
            case GameState.Waitpalyer :
                if (PlayerrEf.activeSelf)
                {
                    AudioManager.Instance.playerBGm(IdleClip);
                    PlayerrEf.SetActive(false);
                    StartSelectPanel.ExitAni();
                }
                break;
            case GameState.Play :
                AudioManager.Instance.playerBGm(PalyerClip);
                StartSelectPanel.UpdateAni();
                PlayerrEf.SetActive(true);
                break;
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.stateChangedAction -= SwitchState;
    }
}
