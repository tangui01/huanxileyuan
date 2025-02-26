using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：GameConf.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19
    功能：游戏资源
*****************************************************/
[CreateAssetMenu(fileName = "New Game Conf", menuName = "Game Conf")]
public class GameConf : ScriptableObject
{
    public GameObject PlayBodyPre;
    public GameObject PlayerHeadPre;
    public GameObject FoodPre;
    public GameObject BotHeadPre;
    public GameObject BotBodyPre;
    public GameObject NicknamePre;

    

    //EF
    public GameObject ScoreFx_1;
    public GameObject Fx_Gratz;
    public GameObject Fx_Ouch;
    public GameObject SimpleHit;
    public GameObject BigHitEffect_01;
    public GameObject BigHitEffect_02;
    public GameObject TextFx_01;
    public GameObject Fx_TimeIsUp; 
    public GameObject Fx_Booster_0_Unzoom; 
    public GameObject Fx_Booster_1_Unzoom;
    public GameObject Fx_Booster_2_Unzoom;
    public GameObject Fx_Booster_3_Unzoom;
    public GameObject AddScoreUI;
}