using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：BaseCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace Crzaykitchen
{
    public class BaseCounter : MonoBehaviour,IKitchenObjectParent
    {
        [SerializeField] protected GameObject[] SelectPres;
        protected KitchenObject kitchenObject;
        [SerializeField] protected Transform countTopPoint;
        [SerializeField] private Transform cuttingPoint;
        public virtual void Interact(CrazyKitchenPlayer player)
        {
            
        }
        public virtual void Select()
        {
            for (int i = 0; i < SelectPres.Length; i++)
            {
                SelectPres[i].SetActive(true);
            }
        }

        public virtual void Deselect()
        {
            for (int i = 0; i < SelectPres.Length; i++)
            {
                SelectPres[i].SetActive(false);
            }
        }
         public Transform GetKitchenObjectFollow(KitchenObjectSO ko)
        {
            if (ko.objectType==kitchenObjectType.InputkitchenObject)
            {
                return countTopPoint;
            }
            else
            {
                return cuttingPoint;
            }
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }
/// <summary>
/// 是否有厨房物体
/// </summary>
/// <returns></returns>
        public bool HasKitchenObject()
        {
            return kitchenObject != null;
        }
    }
}


