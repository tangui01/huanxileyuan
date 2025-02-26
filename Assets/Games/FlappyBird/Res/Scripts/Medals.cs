using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：Medals.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：奖牌
*****************************************************/
namespace FlyBird
{
    public enum MedalsType
    {
        medals_0,
        medals_1,
        medals_2,
        medals_3
    }

    public class Medals : MonoBehaviour
    {
        [SerializeField] private Sprite[] sps;

        private SpriteRenderer sr;
        
        private MedalsType medalsType;

        private bool IsAdd;
        private Pipe pipe;
        public void Init(Pipe pipe)
        {
            medalsType=(MedalsType)Random.Range(0,4);
            sr = GetComponent<SpriteRenderer>();
            transform.localScale=Vector3.one*0.5f;
            sr.sprite = sps[(int)medalsType];
            IsAdd = false;
            this.pipe=pipe;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsAdd)
            {
                return;
            }
            if (other.CompareTag("Bird"))
            {
                IsAdd = true;
                pipe?.Removemedals();
                pipe = null;
                GameController.Instance.flyBirdAudioManager.PlayerGetMedalSound();
                //transform.parent = null;
                transform.DOMoveY(-0.33f,0.5f).onComplete = () =>
                {
                    transform.DOMove(new Vector3(0,3.3f,0),0.5f).onComplete= () =>
                    {
                        transform.DOScale(Vector3.zero, 0.5f).onComplete=()=>
                            {
                                GameController.Instance.AddScore((int)(medalsType+1)*10);
                                Destroymy();
                            }
                        ;
                    };
                };
            }
        }

        public void Destroymy()
        {
            PoolManager.Instance.PushObj("Medals",gameObject);
        }
    }
}

