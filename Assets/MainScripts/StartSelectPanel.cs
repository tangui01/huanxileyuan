using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/****************************************************
    文件：StartSelectPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class StartSelectPanel : MonoBehaviour
{
    private Animator Ani;

    private void Awake()
    {
        Ani = GetComponent<Animator>();
    }
    public void ClickAni()
    {
        Ani.SetTrigger("Click");
    }

    public void UpdateAni()
    {
        Ani.SetTrigger("Update");
    }

    public void ExitAni()
    {
        Ani.SetTrigger("Exit");
    }
}
