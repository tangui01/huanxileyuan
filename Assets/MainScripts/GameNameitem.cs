using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：GameNameitem.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：游戏item
*****************************************************/
public class GameNameitem : MonoBehaviour
{
    private MainMenuGameconf conf;
    public Image SelectImage;
    public Image NoSelectImage;
    private Animator Ani;
    private bool ISSelect;
    private ScrollRect scrollRect;
    
    public void Init(MainMenuGameconf conf,ScrollRect _scrollRect)
    {
        this.conf = conf;
        scrollRect=_scrollRect;
        SelectImage.sprite = this.conf.icon;
        NoSelectImage.sprite = this.conf.NoSelectimg;
        Ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (ISSelect)
        {
            CheckItemVisibility();
        }
    }

    private void CheckItemVisibility()
    {
       
    }

    public string GetGameSceneName()
    {
        return conf.SceneName;
    }
    public void SelectGame()
    {
        Ani.SetTrigger("Enter");
        ISSelect = true;
        GetComponent<ScrollRectViewController>().isSelected = true;
    }

    public void ExitSelect()
    {
        Ani.SetTrigger("Exit");
        ISSelect = false;
        GetComponent<ScrollRectViewController>().isSelected = false;
    }
}
