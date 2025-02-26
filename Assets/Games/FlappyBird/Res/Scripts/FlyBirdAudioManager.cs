using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/****************************************************
    文件：AudioManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace FlyBird
{
    public class FlyBirdAudioManager : MonoBehaviour
    {      
           [SerializeField] private AudioClip[] audioClips;
           private void Start()
           {
               AudioManager.Instance.playerBGm(audioClips[4]);
           }
           public void PlayerFlySound()
           {
                AudioManager.Instance.playerEffect1(audioClips[3]);
           }
           public void PlayerDieSound()
           {
               AudioManager.Instance.playerEffect1(audioClips[0]);
           }
           public void PlayerhitSound()
           {
               AudioManager.Instance.playerEffect1(audioClips[1]);
           }
           public void PlayerAddScoreSound()
           {
               AudioManager.Instance.playerEffect1(audioClips[2]);
           }
           public void PlayerGetMedalSound()
           {
               AudioManager.Instance.playerEffect1(audioClips[5]);
           }
    }

}
