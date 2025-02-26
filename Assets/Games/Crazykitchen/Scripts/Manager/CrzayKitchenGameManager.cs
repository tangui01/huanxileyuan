using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：DeliveryManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class CrzayKitchenGameManager : MonoBehaviour
    {
        public static CrzayKitchenGameManager instance;
        [SerializeField] private ScorePanel scorePanel;

        private int Score;

        public bool GameOver { get;private set; }

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
        [SerializeField] private RecipeListSo recipes;
        private List<RecipeSo> waitingRecipes=new List<RecipeSo>();
        
        private float SpawnRecipesTimer;
        private float SpawnRecipesTimerMax = 4;
        private int MaxWaitingRecipesCount=4;
        private int WaitingRecipesCount;

        public Action<List<RecipeSo>> DeliveRecipesAction;

        public Action OnRecipeSuccessed;
        public Action OnRecipeFailed;
        private void Start()
        {
            GameOver=false;
            GameTimeManager.instance.TimeOverAction = TimeGameOver;
        }

        public void TimeGameOver()
        {
            CommonUI.instance.StartCouterColdDown();
        }

        private void Update()
        {
            if (GameOver)
            {
                return;
            }
            SpawnRecipesTimer+=Time.deltaTime;
            if (SpawnRecipesTimer >= SpawnRecipesTimerMax)
            {
                SpawnRecipesTimer = 0;
                if (WaitingRecipesCount < MaxWaitingRecipesCount)
                {
                    WaitingRecipesCount++;
                    RecipeSo so = recipes.recipeList[Random.Range(0, recipes.recipeList.Count)];
                    Debug.Log(so.recipeName);
                    waitingRecipes.Add(so);
                    DeliveRecipesAction?.Invoke(waitingRecipes);
                }
            }
        }
/// <summary>
/// 递交食物
/// </summary>
/// <param name="plateKitchenObject"></param>
        public void DeliveRecipes(PlateKitchenObject plateKitchenObject)
        {
            for (int i = 0; i < waitingRecipes.Count; i++)
            {
                if (plateKitchenObject.GetKitchenObjectList().Count == waitingRecipes[i].kitchenObjects.Count)
                {
                    foreach (var p in plateKitchenObject.GetKitchenObjectList())
                    {
                        foreach (var w in waitingRecipes[i].kitchenObjects)
                        {
                            if (w == p)
                            {
                                waitingRecipes.RemoveAt(i);
                                WaitingRecipesCount--;
                                Score+=10*plateKitchenObject.GetKitchenObjectList().Count;
                                scorePanel.setScore(Score);
                                DeliveRecipesAction?.Invoke(waitingRecipes);
                                OnRecipeSuccessed?.Invoke();
                                return;
                            }
                        }
                    }
                }

            }
           OnRecipeFailed?.Invoke();
        }
    }
}


