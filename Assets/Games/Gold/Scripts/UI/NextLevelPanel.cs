using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：NextLevelPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：下一个关卡的加载动画
*****************************************************/
public class NextLevelPanel : MonoBehaviour
{
        private Animator Ani;

        private void Awake()
        {
            Ani = GetComponent<Animator>();
        }

        public void Enter()
        {
            Ani.SetTrigger("Enter");
        }

        public void Exit()
        {
            Ani.SetTrigger("Exit");
        }
}
