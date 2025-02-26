using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：DownMain.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public class DownMain : MonoBehaviour
    {
        private float DestoryTime = 1.5f;
        private float CurrentTime = 0;
        private void Update()
        {
            CurrentTime+=Time.deltaTime;
            if (CurrentTime>=DestoryTime)
            {
                CurrentTime = 0;
                PoolManager.Instance.PushObj("DownMain", gameObject);
            }
        }
    }
}

