using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

/****************************************************
    文件：ScorePanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/15 18:41
    功能：ui得分面版
*****************************************************/
public enum ScoreUIEf
{
    [LabelText("无特效")]None,
    [LabelText("心跳特效")]HeartbeatEffect,
    [LabelText("数字滚动特效")] ScrollEffect,
}

public class ScorePanel : MonoBehaviour
{
    public Text scoreText;
    public ScoreUIEf ef;
    private int Currentscore;

    private int initFontSize;

    private void Start()
    {
        initFontSize = scoreText.fontSize;
    }

    public void setScore(int score)
    {
        if (score.ToString().Length>6)
        {
            scoreText.fontSize = Mathf.Clamp( initFontSize-score.ToString().Length%6,1,50);
        }
        UseTimeUIEf(score);
    }
    public void UseTimeUIEf(int score)
    {
        switch (ef)
        {
            case ScoreUIEf.None:
                scoreText.text = score.ToString();
                break;
            case ScoreUIEf.HeartbeatEffect:
                HeartbeatEffectAni(score);
                break;
            case  ScoreUIEf.ScrollEffect :
                ScrollEffectAni(score);
                break;
        }
    }

    public void HeartbeatEffectAni(int score)
    {
        StartCoroutine("OnHeartbeatEffect",score);
    }

    IEnumerator OnHeartbeatEffect(int score)
    {
        scoreText.gameObject.transform.DOScale(Vector3.one*1.2f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        scoreText.gameObject.transform.DOScale(Vector3.one, 0.5f);
        scoreText.text = score.ToString();
        yield return new WaitForSeconds(0.5f);
    } 
    public void ScrollEffectAni(int score)
    {
        StartCoroutine("OnScrollEffectect",score);
    }
    IEnumerator OnScrollEffectect(int score)
    {
        while (Currentscore<score)
        {
            Currentscore += 10;
            scoreText.text=Currentscore.ToString();
            scoreText.gameObject.transform.DOScale(Vector3.one*1.05f, 0.001f);
            yield return new WaitForSeconds(0.001f);
            scoreText.gameObject.transform.DOScale(Vector3.one, 0.001f);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
