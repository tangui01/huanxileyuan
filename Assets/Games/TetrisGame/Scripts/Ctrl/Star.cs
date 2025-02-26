using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/****************************************************
    文件：Star.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public class Star : SerializedMonoBehaviour
    {
        [SerializeField] private TrailRenderer trail;
        public SpriteRenderer sprite { get; private set; }


        private Shape _shape;

        public void Init(Shape shape)
        {
            trail = transform.Find("Trial").GetComponent<TrailRenderer>();
            sprite = GetComponent<SpriteRenderer>();
            _shape = shape;
            trail.startColor = sprite.color;
            trail.endColor = new Color(255f, 255f, 255f, 158f);
            trail.gameObject.SetActive(false);
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }

        public void AddScoreEf()
        {
            trail.gameObject.SetActive(true);
            _shape.GetISStarClear(this);
            transform.DOMove(new Vector3(18, 15f, 0f), 0.5f).onComplete = () =>
            {
                transform.DOScale(Vector3.zero, 0.5f).onComplete = () =>
                    {
                        PoolManager.Instance.PushObj("Star", gameObject);
                    }
                    ;
            };
        }
    }
}