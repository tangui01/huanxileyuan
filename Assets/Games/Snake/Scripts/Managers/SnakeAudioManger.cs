
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WGM;
using Random = UnityEngine.Random;

/****************************************************
    文件：AudioManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：音频管理器
*****************************************************/
public class SnakeAudioManger : MonoBehaviour
{
    public static   SnakeAudioManger instance;
    [LabelText("背景音乐")]public AudioClip[] BgClip;
    [LabelText("加速时的音效")]public AudioClip UpSpeedSound;
    //音效
    public AudioClip EatFoodEfClip;
    public AudioClip EatGhostFoodEfClip;
    public AudioClip EatPropEfClip;
    public AudioClip HitEfClip;
    public AudioClip SpeedupFailClip;
    public AudioClip PropTimerEnd;
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
    /// <summary>
/// 场景音频初始化
/// </summary>
    public void GameAudioInit()
    {
        AudioManager.Instance.playerBGm(BgClip[Random.Range(0, BgClip.Length)]);
    }

    public void PlayEatFoodEfClip()
    {
        AudioManager.Instance.playerEffect1(EatFoodEfClip);
    }
    public void PlayEatGhostFoodEfClip()
    {
        AudioManager.Instance.playerEffect1(EatGhostFoodEfClip);
    }
    public void PlayEatGhostHitEfClip()
    {
        AudioManager.Instance.playerEffect2(HitEfClip);
    }
    public void IsPlayUpSpeedSound(bool isPlay)
    {
        if (isPlay)
        {
            AudioManager.Instance.playerEffect5(UpSpeedSound);
        }
        else
        {
            AudioManager.Instance.StopEffect5Player();
        }
    }

    public void PlayerEatPropSound()
    {
        AudioManager.Instance.playerEffect1(EatPropEfClip);
    }  
    public void PlayerPropTimerEnd()
    {
        AudioManager.Instance.playerEffect4(PropTimerEnd);
    }
//播放加速失败音效
    public void PlaySpeedupFailSound()
    {
        AudioManager.Instance.playerEffect2(SpeedupFailClip);
    }
}