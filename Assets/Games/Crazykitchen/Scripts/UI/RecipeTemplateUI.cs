using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

/****************************************************
    文件：RecipeTemplateUI.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房客户需求订单显示
*****************************************************/
namespace Crzaykitchen
{
    public class RecipeTemplateUI : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI RecipeNameText;
        [SerializeField] private Transform RecipeIconTransform;
        [SerializeField]private Transform RecipeIconTransformPreent;
        public void SetRecipeVisual(RecipeSo recipeSo)
        {
            if (LocalizationManager.Instance.GetCurrentLanguage()==Language.Chinese)
            {
                RecipeNameText.text = recipeSo.recipeName;
                RecipeNameText.fontSize = 30;
            }
            else
            {
                RecipeNameText.text = recipeSo.name;
                RecipeNameText.fontSize = 25;
            }
             for (int i = 0; i < recipeSo.kitchenObjects.Count; i++)
             {
                  Transform recipeIcon = Instantiate(RecipeIconTransform, RecipeIconTransformPreent);
                  recipeIcon.GetComponent<Image>().sprite = recipeSo.kitchenObjects[i].icon;
             }
        }
        public void EnterAni()
        {
            StartCoroutine("OnEnterAni");
        }

        IEnumerator OnEnterAni()
        {
            transform.DOMoveX(200, .2f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.2f);
        }
        public void ExitAni()
        {
            StartCoroutine("OnExitAni");
        }

        IEnumerator OnExitAni()
        {
            transform.DOMoveX(-200, .2f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.2f);
        }
    }
  
}

