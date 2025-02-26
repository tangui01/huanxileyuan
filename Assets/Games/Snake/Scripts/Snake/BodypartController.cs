using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：BodypartController.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 11:56
    功能：蛇的身体控制器
*****************************************************/
namespace SnakeGame
{
    public class BodypartController : MonoBehaviour
    {
        public Snake snake;
        public Transform target;

        private SpriteRenderer bodyShape;

        private float velX;
        private float velY;
        private float rotDump = 10f;

        public void Init(Snake _snake, Transform _target, Sprite _skin, int index)
        {
            bodyShape = GetComponentInChildren<SpriteRenderer>();
            bodyShape.sortingOrder = index;
            bodyShape.sprite = _skin;
            snake = _snake;
            target = _target;
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            //Graceful
            SmoothFollow();
        }

        public void SmoothFollow()
        {
            transform.position = new Vector3(
                Mathf.SmoothDamp(transform.position.x, target.position.x, ref velX,
                    snake.bodypartsFollowDelay * snake.bodypartsBoostFollowDelay),
                Mathf.SmoothDamp(transform.position.y, target.position.y, ref velY,
                    snake.bodypartsFollowDelay * snake.bodypartsBoostFollowDelay),
                transform.position.z);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(target.rotation.eulerAngles.x, target.rotation.eulerAngles.y,
                    target.rotation.eulerAngles.z), Time.deltaTime * rotDump);
        }

        public void DestoryBodyParts()
        {
            target = null;
            snake = null;
        }
    }
}

