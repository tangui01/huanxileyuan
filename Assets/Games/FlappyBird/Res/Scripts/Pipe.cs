using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/****************************************************
    文件：Pipe.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace FlyBird
{
    public class Pipe : MonoBehaviour
    {
        private float MoveSpeed;
        private float MinPosx;
        
        [SerializeField] private Medals PipeMedals;
        [SerializeField,Range(0,100)] private float generateMedalsPrecent;//生成奖牌的概率
        [SerializeField] private float MinMedalsPosy;
        [SerializeField] private float MaxMedalsPosy;
        
        private Medals medals;
        [SerializeField]private BoxCollider2D boxCollider;
        public void Init(float MoveSpeed, float MinPosx)
        {
            this.MoveSpeed = MoveSpeed;
            this.MinPosx=MinPosx;
            boxCollider.enabled = true;
            if (Random.Range(0,100)<generateMedalsPrecent&&medals==null)
            {
                medals=PoolManager.Instance.GetObj("Medals",PipeMedals.gameObject,transform.position+new Vector3(0,Random.Range(MinMedalsPosy,MaxMedalsPosy)),Quaternion.identity,transform).GetComponent<Medals>();
                medals.Init(this);
            }
        }

        private void FixedUpdate()
        {
            if (GameController.Instance.GetIsGameover())
            {
                return;
            }
            if (transform.position.x < MinPosx)
            {
                PoolManager.Instance.PushObj("Pipe",gameObject);
            }
            else
            {
                transform.Translate(Time.deltaTime * MoveSpeed*Vector2.left);
            }
        }

        public void Removemedals()
        {
            medals = null;
        }

        public void AddScore()
        {
            boxCollider.enabled = false;
        }
    }
}

