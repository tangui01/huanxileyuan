using System;
using System.Collections;
using System.Collections.Generic;
using SnakeWarzIO;
using UnityEngine;

namespace SnakeGame
{
/****************************************************
    文件：Snake.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 11:57
    功能：蛇的基类
*****************************************************/
    public class Snake : MonoBehaviour
    {
        //蛇的速度和旋转
        public float moveSpeed = SnakeGameConstant.moveSpeedMax;
        protected float moveSpeedBoost = 1f;
        protected float rotationSpeed = 3f;
        protected float rotationSpeedBoostPenalty = 1f;
        public float bodypartsFollowDelay;
        public float bodypartsBoostFollowDelay = 1;
        protected float sizeBasedSpeedPenalty = 0.0015f; //Default = 0.002f
        protected int currentFoodToBodypartCounter;
        protected int bodyReduceCounter;
        protected int framesNeededForBodyReduce = 100;
        protected int minimumBodyparts = 3;

        public GameObject headGlow;

        //Direction
        protected Vector3 targetPosition;

        protected Vector3 myDirection;

        //身体相关
        protected Transform lastTransform; //最后一个身体部位的位置
        protected List<BodypartController> bodyParts = new List<BodypartController>(); //身体
        public int totalBodyParts { get; protected set; }
        protected SpriteRenderer headShape;
        protected Transform bodyPartsHolder;

        public Transform headPart;

        //身体大小数据
        protected Vector3 initialHeadScale;
        protected Vector3 initialBodyScale;
        protected float scaleUpStepsRatio = 0.01f; //default = 0.0075f

        [Header("Nickname")] public string nickname;
        protected NicknameController nick;
        protected SnakeSkin Skin;
        protected bool isBot;
        protected bool ISdie;

        protected float Score;
        protected int InitLength;
        [SerializeField]protected int MaxLengeh = 50;

        public virtual void Init(SnakeSkin _skin)
        {
            headPart = transform.Find("Head");
            headShape = transform.Find("Head/Shape").GetComponent<SpriteRenderer>();
            Skin = _skin;
            lastTransform = null;
            totalBodyParts = 0;
            ISdie = false;
            headShape.sprite = Skin.headSprite;
            GetComponent<CircleCollider2D>().enabled = true;
            bodypartsBoostFollowDelay = 1;
        }

        #region 蛇的身体生成

        public void CreateSnake(GameObject actor, bool _isBot = false)
        {
            initialHeadScale = headPart.transform.localScale;
            initialBodyScale = initialHeadScale;
            GameObject bph = new GameObject();
            bph.name = actor.name + "-BodyPartsHolder";
            bodyPartsHolder = bph.transform;
            isBot = _isBot;
            int totalBPs = SnakeGameConstant.GetInitialBodyparts();
            if (isBot)
            {
                totalBPs += SnakeGameConstant.GetRandomInitialBodyparts();
            }

            InitLength = totalBPs;
            AddBodypart(totalBPs);
        }

        public void AddBodypart(int amount = 1)
        {
            StartCoroutine(AddBodypartCo(amount));
        }

        public IEnumerator AddBodypartCo(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                AddBodypartMain();
                yield return new WaitForEndOfFrame();
            }
        }

        public void AddBodypartMain()
        {
            Vector3 addPos;
            Transform target;
            if (lastTransform == null)
            {
                addPos = transform.position;
                target = transform;
            }
            else
            {
                addPos = lastTransform.position;
                target = lastTransform;
            }

            string prepath;
            if (!isBot)
            {
                prepath = SnakeGameConstant.PlayBodyPre;
            }
            else
            {
                prepath = SnakeGameConstant.BotBodyPre;
            }

            BodypartController bc =
                ResLoadManager.instance.LoadGoObj<BodypartController>("Body", prepath, addPos, Quaternion.identity
                    , bodyPartsHolder);
            bc.target = null;
            bc.Init(this, target, Skin.bodySprite, 100 - totalBodyParts);
            bc.name = gameObject.name + "-" + "Body-" + totalBodyParts;
            totalBodyParts++;
            lastTransform = bc.transform;
            bodyParts.Add(bc);
            UpdateSize(totalBodyParts);
        }

        public virtual void UpdateSize(int totalParts)
        {
            //print("totalParts: " + totalParts);
            if (totalParts > SnakeGameConstant.maxSnakeSizeForScale)
            {
                print("Maximum scale reached!");
                //return;
                totalParts = SnakeGameConstant.maxSnakeSizeForScale;
            }
            
            headPart.transform.localScale = initialHeadScale * ((totalParts * scaleUpStepsRatio) + 1);
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].transform.localScale = initialBodyScale * ((totalParts * scaleUpStepsRatio) + 1);
            }
            
        }

        #endregion

        public void RemoveBodypart()
        {
            //cache last bodypart of this snake
            Transform bodypartToRemove = lastTransform;

            //remove last BP from array
            bodyParts.RemoveAt(bodyParts.Count - 1);

            //Update the counter
            totalBodyParts--;
            //Assign the new last BP
            lastTransform = bodyParts[bodyParts.Count - 1].transform;

            //Destroy the unused last BP
            PoolManager.Instance.PushObj("Body", bodypartToRemove.gameObject);
            UpdateSize(totalBodyParts);
            
        }

        public virtual void Die()
        {
            if (ISdie)
            {
                return;
            }

            ISdie = true;

            GetComponent<CircleCollider2D>().enabled = false;
            if (totalBodyParts > 0)
            {
                for (int i = 0; i < totalBodyParts; i++)
                {
                    if (i % 2 == 0)
                    {
                        FoodsGenerateManager.instance.SpawnGhostFood(bodyParts[i].transform.position);
                    }
                }
            }

            if (SnakeGameManager.Instance.snakePlayer.gameObject != null)
            {
                float distanceToPlayer = Vector3.Distance(this.transform.position,
                    SnakeGameManager.Instance.snakePlayer.gameObject.transform.position);
                if (distanceToPlayer < SnakeGameConstant.maxDistanceToTriggerShake)
                {
                    SnakeAudioManger.instance.PlayEatGhostHitEfClip();
                    CameraController.instance.ShakeCamera(0.1f, 10f);
                }
            }

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].DestoryBodyParts();
                PoolManager.Instance.PushObj("SnakeBody", bodyParts[i].gameObject);
            }

            bodyParts.Clear();
            if (nick != null)
            {
                nick.targetToFollow = null;
                nick = null;
            }

            PoolManager.Instance.PushObj("SnakeHead", gameObject);
        }

        public string GetNickname()
        {
            return "" + nickname;
        }

        public float GetCurrentScore()
        {
            return Score;
        }
    }
}