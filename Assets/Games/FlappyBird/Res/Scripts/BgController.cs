using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：BgController.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：飞行的小鸟:背景移动
*****************************************************/
namespace FlyBird
{
    public class BgController : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed;
        private Vector3 startPos;
        [SerializeField]private float MinX = -1f;

        private void Start()
        {
            startPos=transform.position;
        }

        private void FixedUpdate()
        {
            if (GameController.Instance.GetIsGameover())
            {
                return;
            }
            if (transform.position.x < MinX)
            {
                transform.position = startPos;
            }
            transform.Translate(new Vector3(-MoveSpeed, 0, 0));
        }
    }
}

