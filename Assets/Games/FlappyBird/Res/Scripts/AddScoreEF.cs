using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：AddScoreEF.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace FlyBird
{
    public class AddScoreEF : MonoBehaviour
    {
        private Animator Ani;

        private void Awake()
        {
            Ani = GetComponent<Animator>();
        }

        public void Play()
        {
            Ani.SetTrigger("AddScore");
        }

    }
}


