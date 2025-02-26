using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WGM;

/****************************************************
    文件：Bird.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：飞行的小鸟控制器
*****************************************************/
namespace FlyBird
{
    public class Bird : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Animator Ani;
        [SerializeField] private float flyForce;
        [SerializeField] private float flyRotate = 1;
        [SerializeField] private GameObject look;

        private float initGravityScale = 0.3f;
        [SerializeField]private ParticleSystem AddScoreef;
        
        public AddScoreEF addScoreEF;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = initGravityScale;
            Ani = GetComponent<Animator>();
            addScoreEF=transform.Find("AddScoreEF/AddScore").GetComponent<AddScoreEF>();
        }

        private void Update()
        {
            if (Time.timeScale==0)
            {
                return;
            }
            if (DealCommand.GetKeyDown(1, AppKeyCode.UpScore))
            {
                Fly();
            }

            look.transform.DORotateQuaternion(Quaternion.Euler(0, 0, rb.velocity.y * flyRotate),0.3f );
        }

        private void Fly()
        {
            if (GameController.Instance.GetIsGameover())
            {
                return;
            }

            GameController.Instance.flyBirdAudioManager.PlayerFlySound();
            rb.velocity = Vector2.up * flyForce;
        }

        private void DieFly()
        {
            if (GameController.Instance.GetIsGameover())
            {
                return;
            }

            rb.gravityScale = 1;
            Ani.SetBool("Die", true);
            rb.velocity = Vector2.up * flyForce;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameController.Instance.GetIsGameover())
            {
                return;
            }

            if (other.gameObject.CompareTag("Pipe")||other.gameObject.CompareTag("Grand"))
            {
                DieFly();
                GameController.Instance.GameOver();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PointTrigger"))
            {
                Pipe pipe = other.GetComponentInParent<Pipe>();
                if (pipe!=null)
                {
                    GameController.Instance.AddScore();
                    GameController.Instance.AddScoreef.Play();
                    AddScoreef.Play();
                    pipe.AddScore();
                }
            }
        }
    }
}