using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：ContainerCounterVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：厨房容器视觉效果
*****************************************************/

namespace Crzaykitchen
{
public class ContainerCounterVisual : MonoBehaviour
{
    private Animator Ani;
    [SerializeField]private ContainerCounter Counter;
    private void Awake()
    {
        Ani = GetComponent<Animator>();
    }

    private void Start()
    {
        Counter.OnPlayerCaptureEvent += ContainerCounter_OnPlayerCaptureEvent;
    }

    private void ContainerCounter_OnPlayerCaptureEvent(object source, EventArgs args)
    {
        Ani.SetTrigger(CrzaykitchenConst.ContainerCounterOPenAni);
    }
}
}
