using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WGM;

/****************************************************
    文件：MainTipsPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：游戏大厅提示
*****************************************************/
public class MainTipsPanel : MonoBehaviour
{
    [SerializeField]private Text text;

    private void Start()
    {
        SwitchStateTips(GameStateManager.Instance.GetCurrentGameState());
        GameStateManager.Instance.stateChangedAction+=SwitchStateTips;
    }

    /// <summary>
    /// 根据不同状态提示相应的语言
    /// </summary>
    /// <param name="state"></param>
    public void SwitchStateTips(GameState state)
    {
        switch (state)
        {
            case GameState.Idle:
                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case Language.English:
                        text.fontSize = 40;
                        text.text = "Insert coins!!!";
                        break;
                    case Language.Chinese:
                        text.fontSize = 45;
                        text.text = "请投币!!!";
                        break;
                }
                break;
            case GameState.NoCoinCount :
                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case Language.English:
                        text.fontSize = 35;  
                        text.text = "You need to put in "+CommonUI.instance.CoinCountPanel.GetNeedCoinCount()+" coin";
                        break;
                    case Language.Chinese:
                        text.fontSize = 45;
                        text.text = "你还需要投"+CommonUI.instance.CoinCountPanel.GetNeedCoinCount()+"个币";
                        break;
                }
                break;
            case GameState.Waitpalyer: 
                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case Language.English:
                        text.fontSize = 40;
                        text.text = "Please press the Start button...";
                        break;
                    case Language.Chinese:
                        text.fontSize = 40;
                        text.text = "请按下开始键进入选择模式";
                        break;
                }
                break;
            case GameState.Play: 
                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case Language.English:
                        text.fontSize = 40;
                        text.text = "Please select your game";
                        break;
                    case Language.Chinese:
                        text.fontSize = 45;
                        text.text = "请选择你的游戏";
                        break;
                }
                break;
        }
        }

    private void OnDestroy()
    {
        GameStateManager.Instance.stateChangedAction-=SwitchStateTips;
    }
}
