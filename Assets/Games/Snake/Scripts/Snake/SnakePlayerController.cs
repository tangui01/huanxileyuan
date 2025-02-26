using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WGM;

namespace SnakeGame
{
/****************************************************
    文件：PlayerController.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 11:57
    功能：玩家蛇控制器
*****************************************************/
    public class SnakePlayerController : Snake
    {
        private float playerrotationSpeed = 110;
        private float playerrotationSpeedUp = 1f;
        private bool isHandleBoostState;
        private float MoveX = 0;
        private float MoveY = 0;
        public Rigidbody2D rb;
        
        [SerializeField] private LengthPanel lengthPanel;
        
        
        [SerializeField] private GameObject BoosterEffects;

  //得分倍率
        private bool isAddScoremultiple;
        private bool ISExtraSpeed;
        public override void Init(SnakeSkin _skin)
        {
            base.Init(_skin);
            isHandleBoostState = false;
            nickname = "You";
            nick = NicknameGenerator.instance.CreateStickyNickname(gameObject).GetComponent<NicknameController>();
            myDirection = Vector2.right;
            CreateSnake(gameObject, false);
            lengthPanel.SetLength(InitLength,MaxLengeh);
            isAddScoremultiple = false;
        }

        private void Update()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

            HandleBoostState();
        }

        private void FixedUpdate()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

            Get2dMousePosition();
            RotateTowardsInput();
            MoveTowardsInput();
            if (isHandleBoostState&&!ISExtraSpeed)
            {
                ReduceBodyparts();
            }
        }

        #region 蛇的移动和旋转

        private void MoveTowardsInput()
        {
            rb.velocity = myDirection * (moveSpeedBoost * moveSpeed * Time.deltaTime * 20);
        }

        public void RotateTowardsInput()
        {
            myDirection.Normalize();
            float
                rotation_z =
                    Mathf.Atan2(myDirection.y, myDirection.x) *
                    Mathf.Rad2Deg; //using this formula to translate the dir into rotation gives the most accurate result and is also able to handle positive/negative directions.
            transform.eulerAngles = new Vector3(0, 0,
                Mathf.LerpAngle(transform.eulerAngles.z, rotation_z,
                    Time.deltaTime * rotationSpeed * rotationSpeedBoostPenalty));
        }

        private void Get2dMousePosition()
        {
            //获取X上的输入
            //A健
            if (DealCommand.GetKey(1, AppKeyCode.TicketOut))
            {
                myDirection.x = -1;
            }
            //J健
            else if (DealCommand.GetKey(1, AppKeyCode.Flight))
            {
                myDirection.x = 1;
            }

            //获取Y上的输入
            if (DealCommand.GetKey(1, AppKeyCode.Bet))
            {
                myDirection.y = 1;
            }
            else if (DealCommand.GetKey(1, AppKeyCode.ExtCh0))
            {
                myDirection.y = -1;
            }
        }

        #endregion

        #region 蛇吃食物与成长

        public void OnMagnetTriggerEnter(Collider2D other,bool magnet=false)
        {
            OnTriggerEnter2D(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Food")
            {
                EatFood(other,FoodTypes.NormalFood);
            }

            if (other.gameObject.tag == "GhostFood")
            {
                EatFood(other,FoodTypes.GhostFood);
            }

            //if a bots run into player's Head
            if (other.gameObject.tag == "BotHead")
            {
                //both player & bot are dead
                other.GetComponent<BotController>().Die();
                Die();
            }

            //if a bot runs into bot's body
            if (other.gameObject.tag == "BotBody"||other.gameObject.tag == "Border")
            {
                Die();
            }
        }

        public void EatFood(Collider2D other,FoodTypes foodType)
        {
//当蛇达到一定长度后长度不会在再增加
            int count = 0;
            switch (foodType)
            {
                case FoodTypes.NormalFood :
                    count = SnakeGameConstant.foodIntoBodypart;
                    SnakeAudioManger.instance.PlayEatFoodEfClip();
                    break;
                
                case FoodTypes.GhostFood :
                    SnakeAudioManger.instance.PlayEatGhostFoodEfClip();
                    count = SnakeGameConstant.ghostfoodIntoBodypart;
                    break;
            }
            if (bodyParts.Count<MaxLengeh)
            {
                currentFoodToBodypartCounter++;
                if (currentFoodToBodypartCounter%count == 0)
                {
                    //Create vfx
                    FBParticleManager.instance.CreateParticle(0, transform.position);
                    FBParticleManager.instance.CreateParticle(5, transform.position);
                    AddBodypart(1);
                    UpdateSize(totalBodyParts);
                    lengthPanel.SetLength(totalBodyParts,MaxLengeh);
                }
            }
            FoodController foodController = other.gameObject.GetComponent<FoodController>();
            foodController.Absorb(gameObject);
            SnakeGameManager.Instance.AddScore(foodController.GetEatScore(), transform.position,isAddScoremultiple);
          
        }

        #endregion

        #region 蛇的加速

        /// <summary>
        /// 判断是否加速
        /// </summary>
        public void HandleBoostState()
        {
            //当身体部位大于5时才可以加速，并且加速会消耗身体
            if (DealCommand.GetKeyDown(1, AppKeyCode.UpScore) && !isHandleBoostState && totalBodyParts >= 5)
            {
                StartCoroutine("OnHandleBoostState");
            }
            else if (DealCommand.GetKeyDown(1, AppKeyCode.UpScore) && !isHandleBoostState && totalBodyParts < 5)
            {
                if (LocalizationManager.Instance.GetCurrentLanguage()==Language.Chinese)
                {
                    CommonUI.instance.AddTips("蛇的长度不足!!!");
                }
                else
                {
                    CommonUI.instance.AddTips("No Length");
                }
                SnakeAudioManger.instance.PlaySpeedupFailSound();
            }
        }

        IEnumerator OnHandleBoostState()
        {
             EnterExtraSpeed();
             yield return new WaitForSeconds(2f);
             if (!ISExtraSpeed)
             {
                 ExitExtraSpeed();
             }
        }

        /// <summary>
        /// Decrease snake's bodyparts when extra speed mode is on for long enough!
        /// </summary>
        public void ReduceBodyparts()
        {
            bodyReduceCounter += 1; //Counter is increased by 1 each frame
            if (bodyReduceCounter % framesNeededForBodyReduce == 0)
            {
                bodyReduceCounter = 0;
                RemoveBodypart();
                UpdateSize(totalBodyParts);
                lengthPanel.SetLength(totalBodyParts,MaxLengeh);
            }
        }

        #endregion

        public override void Die()
        {
            SnakeGameManager.Instance.Gameover();
            //Create vfx
            FBParticleManager.instance.CreateParticle(2, transform.position);
            base.Die();
        }

        public override void UpdateSize(int totalParts)
        {
            base.UpdateSize(totalParts);
            ChangedSnakeSpeedByLength(totalParts);
        }

        private void ChangedSnakeSpeedByLength(int totalParts)
        {
            if (totalParts<=5)
            {
                moveSpeed = 10;
                bodypartsFollowDelay = 0.2f;
            }
            else if (totalParts>5&&totalParts<10)
            {
                moveSpeed = 10;
                bodypartsFollowDelay = 0.2f;
            }
            else if (totalParts>=10&&totalParts<15)
            {
                moveSpeed = 12;
                bodypartsFollowDelay = 0.15f;
            }
            else if (totalParts>=15&&totalParts<20)
            {
                moveSpeed = 14;
                bodypartsFollowDelay = 0.14f;
            }
            else if (totalParts>=20&&totalParts<25)
            {
                moveSpeed = 16;
                bodypartsFollowDelay = 0.12f;
            }
            else if (totalParts>=25&&totalParts<30)
            {
                moveSpeed = 18;
                bodypartsFollowDelay = 0.11f;
            }
            else if (totalParts>=30&&totalParts<35)
            {
                moveSpeed = 20;
                bodypartsFollowDelay = 0.1f;
            }
            else if (totalParts>=40&&totalParts<45)
            {
                moveSpeed = 22;
                bodypartsFollowDelay = 0.09f;
            }
            else if (totalParts>=45&&totalParts<50)
            {
                moveSpeed = 24;
                bodypartsFollowDelay = 0.09f;
            }
            else if (totalParts>=50)
            {
                moveSpeed = 26;
                bodypartsFollowDelay = 0.08f;
            }
        }

        public void EnterMagnet()
        {
            BoosterEffects.SetActive(true);
        }
        public void ExitMagnet()
        {
            BoosterEffects.SetActive(false);
        }
/// <summary>
/// 双倍得分
/// </summary>
        public void EnterDobleSCore()
        {
            isAddScoremultiple = true;
        }
        /// <summary>
        /// 双倍得分
        /// </summary>
        public void ExitDobleSCore()
        {
            isAddScoremultiple = false;
        }
        /// <summary>
        /// 没有损耗的加速
        /// </summary>
        public void EnterExtraSpeed(bool isProp=false)
        {
            playerrotationSpeedUp = 0.6f;
            isHandleBoostState = true;
            ISExtraSpeed = isProp;
            moveSpeedBoost = SnakeGameConstant.moveSpeedBoostMax;
            bodypartsBoostFollowDelay = 0.58f;
            SnakeAudioManger.instance.IsPlayUpSpeedSound(true);
            headGlow.SetActive(true);
        }
        /// <summary>
        /// 双倍得分
        /// </summary>
        public void ExitExtraSpeed()
        {
            moveSpeedBoost = 1;
            playerrotationSpeedUp = 1f;
            bodypartsBoostFollowDelay = 1f;
            AudioManager.Instance.StopEffect5Player();
            headGlow.SetActive(false);
            ISExtraSpeed = false;
            isHandleBoostState = false;
        }
    }
}