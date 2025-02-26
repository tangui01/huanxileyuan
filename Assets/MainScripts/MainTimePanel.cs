using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：MainTimePanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：大厅时间面版
*****************************************************/
public class MainTimePanel : MonoBehaviour
{
    public Text timeText;
    public void Enter()
    {
        if (GameTimeManager.instance.GetCurrentTime()==0)
        {
            return;
        }
        gameObject.SetActive(true);
        SetTimerText(GameTimeManager.instance.GetCurrentTime());
        GameTimeManager.instance.UpdateTimeUI += SetTimerText;
        GameTimeManager.instance.TimeOverAction+=TimeOver;
    }

    public void Exit()
    {
        GameTimeManager.instance.TimeOverAction-=TimeOver;
        GameTimeManager.instance.UpdateTimeUI -= SetTimerText;
        gameObject.SetActive(false);
    }
    public void TimeOver()
    {
        CommonUI.instance.CoinCountPanel.SetGamestateByCoinCount();
        Exit();
    }

    public void SetTimerText(int seconds)
    {
        int min = seconds / 60;
        int sec = seconds % 60;
        timeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }
}
