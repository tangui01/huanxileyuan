using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Video;

/****************************************************
    文件：MainMenuGameConf.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：游戏大厅游戏配置
*****************************************************/
[CreateAssetMenu(fileName = "MainMenuGameConf", menuName = "MainGame/MainMenuGameSConf")]
public class MainMenuGamesConf : ScriptableObject
{
     [ListDrawerSettings(ShowIndexLabels = true)]
     public List<MainMenuGameconf> games = new List<MainMenuGameconf>();
}
[System.Serializable]
public class  MainMenuGameconf
{
    [PreviewField(75),HideLabel,HorizontalGroup("游戏配置",75,LabelWidth = 50)]public Sprite icon;
    [PreviewField(75),HideLabel,HorizontalGroup("游戏配置",75,LabelWidth = 50)]public Sprite NoSelectimg;
    [VerticalGroup("游戏配置/游戏名字"),LabelWidth(75),LabelText("游戏中文名字")]public string gameName_Cn;
    [VerticalGroup("游戏配置/游戏名字"),LabelWidth(75),LabelText("游戏英文名字")]public string gameName_English;
    [VerticalGroup("游戏配置/游戏名字/游戏场景"),LabelText("游戏场景名称"),LabelWidth(75)]public string SceneName;
}