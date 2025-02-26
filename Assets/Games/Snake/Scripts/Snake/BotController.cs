using System;
using System.Collections;
using System.Collections.Generic;
using SnakeWarzIO;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：BotController.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/15 18:27
    功能：机器人蛇
*****************************************************/
namespace SnakeGame
{
    public class BotController : Snake
    {
        [Header("Bot Settings")] private bool botCanSearchForNewTarget; //for manual usage
        private Vector3 botTargetPoint;
        private bool botCanUseSpeedBoost;

        private float botDistanceToTarget;

        //Experimental - Crazy moves by bots to make the game more fun and engaging
        private bool isCrazy;
        [Header("Bot Target")] public GameObject botTargetPrefab;
        private GameObject myTargetInScene;

        public override void Init(SnakeSkin _skin)
        {
            base.Init(_skin);
            //Init
            targetPosition = new Vector3(0, 0, 0);
            myDirection = new Vector3(0, 0, 0);
            if (headGlow != null)
            {
                headGlow.SetActive(false);
            }

            name = NicknameGenerator.instance.GenerateNickname();
            //Bot
            botCanSearchForNewTarget = true;
            botTargetPoint = Vector3.zero;
            botCanUseSpeedBoost = false;
            botDistanceToTarget = Mathf.Infinity;
            StartCoroutine(EnableBotBoostCooldownCo());

            //Misc
            isCrazy = false;
            CreateSnake(gameObject, true);

            //Set a nickname for this bot
            nickname = NicknameGenerator.instance.GenerateNickname();
            //Create a sticky name that follows the snake on the pit
            // nick = NicknameGenerator.instance.CreateStickyNickname(gameObject).GetComponent<NicknameController>();

            myTargetInScene = Instantiate(botTargetPrefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            myTargetInScene.name = gameObject.name + "-Target";
            //Reste all states
            BotResetSpeedBoost();
        }

        private void FixedUpdate()
        {
            StartCoroutine(BotFindNewTargetCo());
            StartCoroutine(EnablePeriodicSpeedBoostCo());
            MoveBot();
        }

        public void MoveBot()
        {
            if (botTargetPoint != Vector3.zero)
            {
                targetPosition = botTargetPoint;
                myDirection = GetDirectionToMouse();
                RotateTowardsInput();
                MoveTowardsInput();
            }
        }

        /// <summary>
        /// Allows the bot to perform extra speed movements
        /// </summary>
        /// <returns></returns>
        public IEnumerator EnablePeriodicSpeedBoostCo()
        {
            if (isCrazy)
                yield break;

            if (!botCanUseSpeedBoost)
                yield break;
            botCanUseSpeedBoost = false;
            StartCoroutine(EnableBotBoostCooldownCo());

            StartCoroutine(BotSpeedBoostCo());
        }

        public void RotateTowardsInput()
        {
            myDirection.Normalize();
            float rotation_z = Mathf.Atan2(myDirection.y, myDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0,
                Mathf.LerpAngle(transform.eulerAngles.z, rotation_z,
                    Time.deltaTime * rotationSpeed * rotationSpeedBoostPenalty));
        }

        public Vector3 GetDirectionToMouse()
        {
            Vector3 dir = targetPosition - transform.position;
            //print("myDirection: " + myDirection);
            return dir;
        }

        /// <summary>
        /// 寻找目标冷却时间
        /// </summary>
        /// <returns></returns>
        public float BotGetNewCooldown()
        {
            float minRange, maxRange;
            minRange = Random.value > 0.8f ? 2f : 10f;
            maxRange = Random.value > 0.8f ? 20f : 30f;

            float cd = Random.Range(minRange, maxRange);
            //print("New Bot Cooldown: " + cd);

            return cd;
        }

        /// <summary>
        /// 随机加速的时间间隙
        /// </summary>
        /// <returns></returns>
        public float BotGetNewSpeedBoostInterval()
        {
            return Random.Range(2f, 8f);
        }

        /// When a bot snake detects a difficult situation, it will change its direction.
        /// The actual detection is done via "BotSensorController" class.
        /// </summary>
        /// <param name="hitPoint"></param>
        public void QuickManeuver(Vector3 hitPoint = default(Vector3))
        {
            //print("<b>Bot ==> Force direction change!</b>");

            //Reset speed boost (if its activated already)
            BotResetSpeedBoost();

            //Find an urgent maneuver target point
            botTargetPoint = GetNewManeuverPosition(hitPoint);
        }

        public Vector3 GetNewManeuverPosition(Vector3 obstaclePos)
        {
            Vector3 nmp = Vector3.zero;
            Vector3 currentPos = transform.position;

            nmp = currentPos + ((currentPos - obstaclePos) * 2.5f);
            //print("NMP: " + nmp);

            //Move target object to new target position
            if (myTargetInScene)
                myTargetInScene.transform.position = nmp;

            return nmp;
        }

        /// <summary>
        /// 随机加速的持续时间
        /// </summary>
        /// <returns></returns>
        public float BotGetNewSpeedBoostDuration()
        {
            return Random.Range(1f, 4f);
        }

        public void MoveTowardsInput()
        {
            transform.Translate(Vector3.right * moveSpeed * moveSpeedBoost * Time.deltaTime * 1f, Space.Self);

            //check if bot is arrived at target point
            botDistanceToTarget = Vector3.Distance(transform.position, botTargetPoint);
            if (botDistanceToTarget < 2)
            {
                StartCoroutine(BotFindNewTargetCo(true));
            }
        }

        public IEnumerator BotFindNewTargetCo(bool forceFind = false)
        {
            if (isCrazy)
                yield break;

            if (!botCanSearchForNewTarget && !forceFind)
                yield break;
            botCanSearchForNewTarget = false;
            StartCoroutine(EnableBotCooldownCo());

            botTargetPoint = GetNewTargetForBot();
            //print("Overridden New Target: " + botTargetPoint);

            //Move target object to new target position
            if (myTargetInScene)
                myTargetInScene.transform.position = botTargetPoint;
        }

        /// <summary>
        /// Experimental
        /// </summary>
        public void PerformCrazyMove()
        {
            if (isCrazy)
                return;
            isCrazy = true;

            print("CrazyMode is on");
            StartCoroutine(PerformPlayerHeadHuntCo());
        }

        /// <summary>
        /// Find the nearest player and try to kamikaze into its head to perform a suicide kill
        /// </summary>
        /// <returns></returns>
        public IEnumerator PerformPlayerHeadHuntCo()
        {
            float duration = 15f;
            float t = 0;
            GameObject target = SnakeGameManager.Instance.snakePlayer.gameObject;

            while (t < duration)
            {
                if (!target)
                {
                    isCrazy = false;
                    yield break;
                }

                t += Time.deltaTime;
                botTargetPoint = target.transform.position;
                StartCoroutine(BotSpeedBoostCo());
                yield return 0;
            }

            if (t >= duration)
            {
                isCrazy = false;
            }
        }

        public Vector3 GetNewTargetForBot()
        {
            Vector3 newTarget = Vector3.zero;
            bool useSmartWay = false;

            //Find a random pos on map anyway - we may use it later on
            Vector3 randomPosOnMap = new Vector3(
                Random.Range(SnakeGameConstant.minimumFieldX, SnakeGameConstant.maximumFieldX),
                Random.Range(SnakeGameConstant.minimumFieldY, SnakeGameConstant.maximumFieldY), 0);

            //Check if we have better options

            //#1 - check if we can go towards a food & no danger is near (no other bots or players)
            float searchRadius = 5;
            LayerMask snakeMask = LayerMask.GetMask("Snake");
            Collider[] snakeParts = Physics.OverlapSphere(transform.position, searchRadius, snakeMask);

            //Debug
            //print("snakeParts.Lenght: " + snakeParts.Length);

            //Now filter out only the colliders that are not belong us (this snake)
            List<Collider> otherSnakeParts = new List<Collider>();
            foreach (Collider c in snakeParts)
            {
                //print("OverlapSphere.Hit: " + c.name);

                //#1
                if (c.gameObject.tag == "BotHead")
                {
                    if (c.gameObject != this.gameObject)
                        otherSnakeParts.Add(c);
                }

                //#2
                if (c.gameObject.tag == "PlayerHead")
                {
                    otherSnakeParts.Add(c);
                }

                //#3
                if (c.gameObject.tag == "BotBody")
                {
                    if (c.GetComponent<BodypartController>().snake != this)
                        otherSnakeParts.Add(c);
                }
            }

            //Debug again
            //print("<b>" + gameObject.name + " ==> otherSnakeParts.Lenght: " + otherSnakeParts.Count + "</b>");

            if (otherSnakeParts.Count == 0)
            {
                //New
                LayerMask foodMask = LayerMask.GetMask("Food");
                Collider[] foods = Physics.OverlapSphere(transform.position, searchRadius, foodMask);

                if (foods.Length > 0)
                {
                    newTarget = GetClosestObject(foods);
                    useSmartWay = true;
                }
            }


            //#2 - pick a random pos
            if (!useSmartWay)
                newTarget = randomPosOnMap;

            return newTarget;
        }

        //Transform GetClosestObject(Transform[] objects)
        public Vector3 GetClosestObject(Collider[] objects)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Collider potentialTarget in objects)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }

            return bestTarget.position;
        }

        public IEnumerator EnableBotCooldownCo()
        {
            float delay = BotGetNewCooldown();
            yield return new WaitForSeconds(delay);
            botCanSearchForNewTarget = true;
        }

        public IEnumerator BotSpeedBoostCo()
        {
            moveSpeedBoost = SnakeGameConstant.moveSpeedBoostMax;
            rotationSpeedBoostPenalty = SnakeGameConstant.rotationSpeedBoostPenalty;
            bodypartsBoostFollowDelay = 0.58f;

            //Display head glow
            headGlow.SetActive(true);

            //Reduce bodyparts when speedboost is enabled
            //ReduceBodyparts();

            yield return new WaitForSeconds(BotGetNewSpeedBoostDuration());

            //reset
            BotResetSpeedBoost();
        }

        public void BotResetSpeedBoost()
        {
            moveSpeedBoost = 1f;
            rotationSpeedBoostPenalty = 1f;
            bodypartsBoostFollowDelay = 1f;
            //hide head glow
            headGlow.SetActive(false);
        }

        public IEnumerator EnableBotBoostCooldownCo()
        {
            yield return new WaitForSeconds(BotGetNewSpeedBoostInterval());
            botCanUseSpeedBoost = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Food")
            {
                currentFoodToBodypartCounter++;
                FoodController foodController = other.gameObject.GetComponent<FoodController>();
                foodController.Absorb(gameObject);
                if (currentFoodToBodypartCounter % SnakeGameConstant.foodIntoBodypart == 0)
                {
                    AddBodypart(1);
                    UpdateSize(totalBodyParts);
                }
            }

            if (other.gameObject.tag == "GhostFood")
            {
                currentFoodToBodypartCounter++;
                FoodController foodController = other.gameObject.GetComponent<FoodController>();
                foodController.Absorb(gameObject);
                if (currentFoodToBodypartCounter % SnakeGameConstant.ghostfoodIntoBodypart == 0)
                {
                    AddBodypart(1);
                    UpdateSize(totalBodyParts);
                }
            }

            //if a bots run into player's Head
            if (other.gameObject.tag == "BotHead")
            {
                if (other.gameObject.GetComponent<BotController>() != null)
                {
                    Die();
                }
            }

            if (other.gameObject.tag == "BotBody")
            {
                if (other.gameObject.GetComponent<BodypartController>().snake != this)
                {
                    Die();
                }
            }

            if (other.gameObject.tag == "PlayerBody")
            {
                if (!ISdie)
                {
                    FBParticleManager.instance.CreateParticle(6,other.transform.position);
                }
                Die();
            }
            if (other.gameObject.tag == "Border"||other.gameObject.tag == "PlayerHead")
            {
                Die();
            }
        }

        public override void Die()
        {
            base.Die();
            BotGenerateManager.instance.RemoveBot(this);
        }

        public override void UpdateSize(int totalParts)
        {
            base.UpdateSize(totalParts);
            bodypartsFollowDelay = (headPart.transform.localScale.x - 1) * 0.0001f + 0.15f;
        }
    }
}