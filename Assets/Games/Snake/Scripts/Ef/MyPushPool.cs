using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：MyPushPool.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

public enum MyPushPoolEfType
{
    ScoreFx_1,
    Fx_Gratz,
    Fx_Ouch,
    SimpleHit,
    BigHitEffect_01,
    BigHitEffect_02,
    TextFx_01,
    Fx_TimeIsUp,
    Fx_Booster_0_Unzoom,
    Fx_Booster_1_Magnet,
    Fx_Booster_2_ScoreMultiplier,
    Fx_Booster_3_ExtraSpeed,
}

public class MyPushPool : MonoBehaviour
{
    public float delay = 0.5f;
    public MyPushPoolEfType efType ;
    private void Start()
    {
        Invoke("Destroymy", delay);
    }

    public void Destroymy()
    {
        string name = "";
        switch (efType)
        {
            case MyPushPoolEfType.ScoreFx_1:
                name = "ScoreFx";
                break;
            case MyPushPoolEfType.BigHitEffect_02:
                name = "BigHitEffect";
                break;
            case MyPushPoolEfType.Fx_Ouch:
                name = "Fx_Ouch";
                break;
            case MyPushPoolEfType.Fx_Gratz:
                name = "Fx_Gratz";
                break;
            case MyPushPoolEfType.SimpleHit:
                name = "SimpleHit";
                break;
            case MyPushPoolEfType.BigHitEffect_01:
               name = "BigHitEffect";
                break;
            case MyPushPoolEfType.TextFx_01:
               name = "TextFx";
                break;
            case MyPushPoolEfType.Fx_TimeIsUp:
                name = "TimeIsUp";
                break;
            case MyPushPoolEfType.Fx_Booster_0_Unzoom:
                name = "Booster";
                break;
            case MyPushPoolEfType.Fx_Booster_1_Magnet:
               name = "Booster_1";
                break;
            case MyPushPoolEfType.Fx_Booster_2_ScoreMultiplier:
                name = "Booster_2";
                break;
            case MyPushPoolEfType.Fx_Booster_3_ExtraSpeed:
                name = "Booster_3";
                break;
        }
        if (name!="")
        {
            PoolManager.Instance.PushObj(name,gameObject);
        }
    }
}