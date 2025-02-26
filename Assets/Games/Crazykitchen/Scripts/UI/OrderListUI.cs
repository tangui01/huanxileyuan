using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：OrderListUI.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：客户需求菜单
*****************************************************/
namespace Crzaykitchen  
{
    public class OrderListUI : MonoBehaviour
    {
        [SerializeField] private Transform RecipeTemPlate;
        [SerializeField] private Transform RecipeListTemPlate;

        private void Start()
        {
            CrzayKitchenGameManager.instance.DeliveRecipesAction = DeliveRecipesVisual;
        }

        public void DeliveRecipesVisual(List<RecipeSo> recipes)
        {
            for (int i = 0; i < RecipeListTemPlate.childCount; i++)
            {
                Destroy(RecipeListTemPlate.GetChild(i).gameObject);
            }
            for (int i = 0; i < recipes.Count; i++)
            {
                 Transform re= Instantiate(RecipeTemPlate,RecipeListTemPlate);
                 re.GetComponent<RecipeTemplateUI>().SetRecipeVisual(recipes[i]);
            }
        }
    }
}

