using System;
using System.Collections;
using System.Collections.Generic;
using Crzaykitchen;
using UnityEngine;
/****************************************************
    文件：PlateKitchenObject.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class PlateKitchenObject : KitchenObject
    {
        //当前拥有的物品
        private List<KitchenObjectSO> CurrentkitchenSoLists = new List<KitchenObjectSO>();
        //能够添加的物品
        [SerializeField]private  List<KitchenObjectSO> kitchenObjectLists ;

        public Action<KitchenObjectSO> OnIngredientadd;
        public Action<KitchenObjectSO> OnIngredientVisual;
        public Action OnDestroyAction;
        public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
        {
            if (!kitchenObjectLists.Contains(kitchenObjectSO))
            {
                return false;
            }
            if (!CurrentkitchenSoLists.Contains(kitchenObjectSO))
            {
                CurrentkitchenSoLists.Add(kitchenObjectSO);
                OnIngredientadd?.Invoke(kitchenObjectSO);
                OnIngredientVisual?.Invoke(kitchenObjectSO);
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<KitchenObjectSO> GetKitchenObjectList()
        {
            return CurrentkitchenSoLists;
        }

        public override void DestroySelf()
        {
            CurrentkitchenSoLists.Clear();
            OnDestroyAction?.Invoke();
            base.DestroySelf();
        }
    }
}

