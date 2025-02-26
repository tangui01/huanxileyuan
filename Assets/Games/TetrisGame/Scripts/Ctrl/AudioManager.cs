using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：AudioManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public class AudioManager : MonoBehaviour
    {
        private Controller ctrl;
        
        public AudioClip drop;
        public AudioClip control;
        public AudioClip lineClear;
        public AudioClip ShapeSound;
        public AudioClip BGSound;
        private void Awake()
        {
            ctrl = GetComponent<Controller>();
        }

        private void Start()
        {
            global::AudioManager.Instance.playerBGm(BGSound);
        }
        public void PlayDrop()
        {
            global::AudioManager.Instance.playerEffect1(drop);
        }

        public void PlayControl()
        {
            global::AudioManager.Instance.playerEffect2(control);
        }

        public void PlayLineClear()
        {
            global::AudioManager.Instance.playerEffect1(lineClear);
        }

        public void PlayerPlaceShapeSound()
        {
            global::AudioManager.Instance.playerEffect1(ShapeSound);
        }
    }
}

