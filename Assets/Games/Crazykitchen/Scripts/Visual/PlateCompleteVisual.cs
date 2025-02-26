using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：PlateCompleteVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class PlateCompleteVisual : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField]private List<KitchenObjectSO_Gameobject> kitchenObjects = new List<KitchenObjectSO_Gameobject>();
        [Serializable]private struct KitchenObjectSO_Gameobject
        {
            public KitchenObjectSO soObject;
            public GameObject KitchenObjectSo;
        }

        private void Start()
        {
            plateKitchenObject.OnIngredientadd += plateKitchenObject_OnIngredientadd;
            plateKitchenObject.OnDestroyAction += PlateKitchenObject_OnDestroyAction;
        }

        private void PlateKitchenObject_OnDestroyAction()
        {
            foreach (var ko in kitchenObjects)
            {
                ko.KitchenObjectSo.SetActive(false);
            }
        }

        private void plateKitchenObject_OnIngredientadd(KitchenObjectSO kitchenObjectSO)
        {
            foreach (var ko in kitchenObjects)
            {
                if (ko.soObject == kitchenObjectSO)
                {
                    ko.KitchenObjectSo.SetActive(true);
                }
            }
        }
    }
}