using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：AudioClipRefsSO.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房/音频
*****************************************************/
[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip[] stoveSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;
    public AudioClip[] generateSound;
}
