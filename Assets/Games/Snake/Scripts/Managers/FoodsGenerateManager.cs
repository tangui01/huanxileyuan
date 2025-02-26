using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：FoodsGenerateManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 13:04
    功能：地图内食物的生成
*****************************************************/
namespace SnakeGame
{
    public class FoodsGenerateManager : MonoBehaviour
    {
        public static FoodsGenerateManager instance;
        [LabelText("生成食物间隔时间")] private float SpawnTimer;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public int defaultAmountOfFoods = 50;
        public int MaxFoods = 200;
        public int foodAmount;
        public int selectedFoodSkinID;

        public void Init()
        {
            foodAmount = defaultAmountOfFoods;
            RaodomselectedFoodSkinID();
            SpawnInitialFoods();
        }

        private void Update()
        {
            SpawnTimer += Time.deltaTime;
            if (SpawnTimer >= 0.5f && foodAmount < MaxFoods)
            {
                SpawnTimer = 0;
                SpawnFood();
            }
        }

        public void RaodomselectedFoodSkinID()
        {
            selectedFoodSkinID = Random.Range(0, 6);
        }

        public void SpawnInitialFoods()
        {
            for (int i = 0; i < defaultAmountOfFoods; i++)
            {
                FoodController food = ResLoadManager.instance.LoadGoObj<FoodController>("Food",
                    SnakeGameConstant.FoodPre, SnakeGameConstant.GetRandomPositionInMap(), Quaternion.identity);
                food.tag = "Food";
                food.InitObjectBasedOnType();
                food.Init(selectedFoodSkinID);
                food.name = "Food-" + i;
                food.transform.parent = transform;
            }
        }

        /// <summary>
        /// 当开始食物数量降到一定数量后，生成食物
        /// </summary>
        public void SpawnFood()
        {
            FoodController food = ResLoadManager.instance.LoadGoObj<FoodController>("Food", SnakeGameConstant.FoodPre,
                SnakeGameConstant.GetRandomPositionInMap(), Quaternion.identity);
            food.tag = "Food";
            foodAmount++;
            food.InitObjectBasedOnType();
            food.Init(selectedFoodSkinID);
            food.name = "Food-" + foodAmount;
            food.transform.parent = transform;
        }

        public void RemoveFood()
        {
            if (SnakeGameManager.isGameFinished)
            {
                return;
            }

            foodAmount--;
        }

        /// <summary>
        /// Spawn GhostFood
        /// </summary>
        /// <param name="spawnPosition"></param>
        public void SpawnGhostFood(Vector3 spawnPosition)
        {
            FoodController food = ResLoadManager.instance.LoadGoObj<FoodController>("Food", SnakeGameConstant.FoodPre,
                spawnPosition, Quaternion.identity);
            foodAmount++;
            food.tag = "GhostFood";
            food.InitObjectBasedOnType();
            food.Init(selectedFoodSkinID);
            food.name = "GhostFood-" + foodAmount;
            food.transform.parent = transform;
        }
    }
}