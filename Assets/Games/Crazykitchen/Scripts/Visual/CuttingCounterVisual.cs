using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/****************************************************
    文件：CuttingCounterVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：切割台动画
*****************************************************/


namespace   Crzaykitchen
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        private Animator Ani;
        [SerializeField]private CuttingCounter cuttingCounter;
        private void Awake()
        {
            Ani = GetComponent<Animator>();
        }

        private void Start()
        {
            cuttingCounter.cuttingProcessAni = IdleTOCut;
        }

        private void IdleTOCut()
        {
            Ani.SetTrigger("Cut");
        }
    }
}

