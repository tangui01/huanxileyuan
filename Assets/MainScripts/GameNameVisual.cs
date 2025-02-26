using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：GameNameVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：选择游戏名字
*****************************************************/
public class GameNameVisual : MonoBehaviour
{
    private Text nametext;

    private void Awake()
    {
        nametext = GetComponent<Text>();
    }

    public void setGameName(Language currentLanguage,MainMenuGameconf gameconf)
    {
        switch (currentLanguage)
        {
            case Language.English:
                nametext.text = gameconf.gameName_English;
                nametext.fontSize = 25;
                break;
            case Language.Chinese:
                nametext.text = gameconf.gameName_Cn;
                nametext.fontSize = 30;
                break;
        }
    }
}
