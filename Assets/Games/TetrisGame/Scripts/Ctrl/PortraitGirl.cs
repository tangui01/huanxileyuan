using System;
using System.Collections;
using System.Collections.Generic;
using AnyPortrait;
using Sirenix.OdinInspector;
using UnityEngine;
/****************************************************
    文件：PortraitGirl.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public class PortraitGirl : MonoBehaviour
    {
        public apPortrait portrait;

        [SerializeField] private AudioClip SmileSound;
        [SerializeField] private AudioClip FailSound;
        [Button]
        public void Idle()
        {
            portrait.CrossFadeQueued("Idle");
        }
        [Button]
        public void Smile()
        {
            if (!portrait.IsPlaying("Smile"))
            {
                global::AudioManager.Instance.playerEffect4(SmileSound);
                portrait.CrossFade("Smile", 0.2f);
                portrait.CrossFadeQueued("Idle");
            }
           
        }
        [Button]
        public void Fail()
        {
            if (!portrait.IsPlaying("Angry"))
            {
                portrait.Play("Angry");
                global::AudioManager.Instance.playerEffect4(FailSound);
            }
        }
    }
}

