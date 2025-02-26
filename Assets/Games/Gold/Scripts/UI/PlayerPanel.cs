using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/****************************************************
    文件：PlayerPanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class PlayerPanel : MonoBehaviour
{
    private Text scoreText;
    private Text targetText;
    private Text timeText;
    private Text levleText;

    private void Awake()
    {
        scoreText=transform.Find("Text Score").GetComponent<Text>();
        targetText=transform.Find("Text Target").GetComponent<Text>();
        timeText=transform.Find("Text Time").GetComponent<Text>(); 
        levleText=transform.Find("textLevelCount").GetComponent<Text>();
    }

    private void Start()
    {
        SetTimerText( GameTimeManager.instance.GetCurrentTime());
        GameTimeManager.instance.UpdateTimeUI = SetTimerText;
    }

    public void SetScore(int score)
    {
        scoreText.text= "$" + score;
        SetFontSizebyValue(score,scoreText);
    }
    public void SetLevel(int Level)
    {
        levleText.text=Level.ToString();
    }
    public void SetTargetScore(int target)
    {
        targetText.text = "$" + target;
        SetFontSizebyValue(target, targetText);
    }

    public void SetFontSizebyValue(int value, Text text)
    {
        if (text.text.Length <5)
        {
            text.fontSize = 23;
        }
        else
        {
            text.fontSize = Mathf.Clamp(23 - (text.text.Length - 4)*4,1,23);
        }
    }
    public void SetTimerText(int seconds)
    {
        int min = seconds / 60;
        int sec = seconds % 60;
        timeText.text=string.Format("{0}:{1:D2}", min, sec);
    }
}
