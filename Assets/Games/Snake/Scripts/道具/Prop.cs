using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：Prop.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
/// <summary>
/// 道具
/// </summary>
namespace SnakeGame
{
    public enum PropType
    {
        UnZoom,
        Magnet,//磁铁
        ScoreMultiplier,//双倍得分
        ExtraSpeed,//无需损耗的加速
    }
    public class Prop : MonoBehaviour
    {
        public PropType propType = PropType.UnZoom;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerHead"))
            {
                PropManager.Instance.AddCount(propType);
                SnakeAudioManger.instance.PlayerEatPropSound();
                FBParticleManager.instance.CreateParticle(4,other.transform.position);
                PoolManager.Instance.PushObj("Prop_"+propType, gameObject);
            }
        }
    }
}


