using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：Food.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 13:08
    功能：食物
*****************************************************/

namespace SnakeGame
{
    public enum FoodTypes
    {
        NormalFood,
        GhostFood
    }

    public class FoodController : MonoBehaviour
    {
        [Header("Food Type")] public FoodTypes foodType = FoodTypes.NormalFood;
        public static float normalFoodMoveSpeed = 2.5f;
        public static float normalFoodRadius = 1.5f;
        public static float normalFoodBodyScaleRatio = 1f;
        public static float ghostFoodMoveSpeed = 3.5f;
        public static float ghostFoodRadius = 1.875f;
        public static float ghostFoodBodyScaleRatio = 3f;
        public static float scaleAnimSpeed = 1f;

        public SpriteRenderer Shape;
        public Transform Body;

        [Header("Animation")] float animSpeed;
        float normalAnimSpeed = 2f;
        float ghostAnimSpeed = 6f;
        float rndAnimSpeed;


        private Vector3 startSize;
        private Vector3 targetSize;
        private float currentMoveSpeed;
        private bool isMoving;
        private Vector3 startPosition;
        public CircleCollider2D myCollider;
        private float bodyScaleRatio;

        [Header("Food Skin Settings")] private static int selectedFoodSkinID;
        public List<FoodSkin> availableFoodSkins;
        private float EatFoodScore;

        private float FoodDestoryTime = 12;
        private float GhostFoodDestoryTime = 6f;
        private float CurrentLiveTime = 0;

        private float HideSpeed = 2;
        [SerializeField]private GameObject Trail;
        public void Init(int _selectedFoodSkinID)
        {
            myCollider.enabled = true;
            isMoving = false;
            selectedFoodSkinID = _selectedFoodSkinID;
            startSize = Body.localScale;
            startPosition = transform.position;
            targetSize = startSize * 1.3f;
            Shape.color = Color.white;
            rndAnimSpeed = Random.Range(0, 3);
            SizeAnimation();
            Trail.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Body != null)
            {
                SizeAnimation();
            }
        }
        /// <summary>
        /// 食物动画
        /// </summary>
        public void SizeAnimation()
        {
            Body.transform.localScale = new Vector3(
                Mathf.SmoothStep(startSize.x, targetSize.x, Mathf.PingPong(Time.time * GetAnimSpeed(), 1)),
                Mathf.SmoothStep(startSize.x, targetSize.x, Mathf.PingPong(Time.time * GetAnimSpeed(), 1)),
                startSize.z);
        }
        public float GetAnimSpeed()
        {
            return animSpeed + rndAnimSpeed;
        }

        public void InitObjectBasedOnType()
        {
            myCollider.radius = 1;
            Body.localScale = Vector3.one;
            if (gameObject.CompareTag("Food"))
            {
                foodType = FoodTypes.NormalFood;
                currentMoveSpeed = normalFoodMoveSpeed;
                myCollider.radius *= normalFoodRadius;
                bodyScaleRatio = availableFoodSkins[selectedFoodSkinID].normalSize;
                animSpeed = normalAnimSpeed;
                CurrentLiveTime = FoodDestoryTime;
            }
            else if (gameObject.CompareTag("GhostFood"))
            {
                foodType = FoodTypes.GhostFood;
                currentMoveSpeed = ghostFoodMoveSpeed;
                myCollider.radius *= ghostFoodRadius;
                bodyScaleRatio = availableFoodSkins[selectedFoodSkinID].bigSize;
                animSpeed = ghostAnimSpeed;
                CurrentLiveTime = GhostFoodDestoryTime;
                Invoke("Destorymy", 6f);
            }

            //Apply a random skin from all available skin in this category
            Shape.sprite = availableFoodSkins[selectedFoodSkinID]
                .availableIcons[Random.Range(0, availableFoodSkins[selectedFoodSkinID].availableIcons.Length)];

            //Apply color randomization if needed
            if (availableFoodSkins[selectedFoodSkinID].canUseRandomizedColors)
            {
                Shape.color = availableFoodSkins[selectedFoodSkinID]
                    .availableColors[Random.Range(0, availableFoodSkins[selectedFoodSkinID].availableColors.Length)];
            }

            //Apply size variations
            float bodyScale = Random.Range(availableFoodSkins[selectedFoodSkinID].minRandomSize,
                availableFoodSkins[selectedFoodSkinID].maxRandomSize) * bodyScaleRatio;
            EatFoodScore = bodyScale * 10;
            Shape.transform.localScale = new Vector3(bodyScale, bodyScale, 1);
        }

        public IEnumerator MoveTowardsPlayer(GameObject target)
        {
            isMoving = true;
            if (Vector3.Distance(target.transform.position,transform.position)>3)
            {
                Trail.gameObject.SetActive(true);
            }
            float t = 0;
            while (t < 1)
            {
                //if target is dead, exit!
                if (!target)
                    yield break;
                t += Time.deltaTime * currentMoveSpeed;
                transform.position = new Vector3(
                    Mathf.SmoothStep(startPosition.x, target.transform.position.x, t),
                    Mathf.SmoothStep(startPosition.y, target.transform.position.y, t),
                    0);
                yield return 0;
            }
            Trail.gameObject.SetActive(false);
            Destorymy();
        }

        private void Destorymy()
        {
            StopAllCoroutines();
            FoodsGenerateManager.instance.RemoveFood();
            PoolManager.Instance.PushObj(SnakeGameConstant.FoodPreName, gameObject);
        }

        public void Absorb(GameObject target)
        {
            if (isMoving)
            {
                return;
            }
            StopAllCoroutines();
            myCollider.enabled = false;
            StartCoroutine(MoveTowardsPlayer(target));
        }

        public int GetEatScore()
        {
            return Mathf.RoundToInt(EatFoodScore);
        }
    }

    [Serializable]
    public class FoodSkin
    {
        public float normalSize = 1f; //1x
        public float bigSize = 3f; //3x
        public float minRandomSize = 0.3f;
        public float maxRandomSize = 0.8f;

        [Space(25)] public bool canUseRandomizedColors = false;
        public Color[] availableColors;

        [Space(25)] public Sprite[] availableIcons;
    }
}