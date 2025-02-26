using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：TimePanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：游戏时间面版
*****************************************************/

public enum TimeUIEf
{
    [LabelText("无特效")]None,
    [LabelText("心跳特效")]HeartbeatEffect,
}

public class TimePanel : MonoBehaviour
{
    public Text timeText;
    public Image timebg;
    public TimeUIEf ef;
    private void Start()
    {
        SetTimerText(GameTimeManager.instance.GetCurrentTime());
        GameTimeManager.instance.UpdateTimeUI = SetTimerText;
    }

    public void SetTimerText(int seconds)
    {
        int min = seconds / 60;
        int sec = seconds % 60;
        timeText.text = string.Format("{0}:{1:D2}", min, sec);
        UseTimeUIEf();
    }
    public void UseTimeUIEf()
    {
        switch (ef)
        {
            case TimeUIEf.None:
                break;
            case TimeUIEf.HeartbeatEffect:
                HeartbeatEffectAni();
                break;
        }
    }

    public void HeartbeatEffectAni()
    {
        StartCoroutine("OnHeartbeatEffectAni");
    }
    IEnumerator OnHeartbeatEffectAni()
    {
        timebg.transform.DOScale(Vector3.one * 1.02f, 0.01f);
        yield return new WaitForSeconds(0.01f);
        timebg.transform.DOScale(Vector3.one, 0.01f);
        yield return new WaitForSeconds(0.01f);
    }
}