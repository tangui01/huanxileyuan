using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：KitchenObject.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房物体
*****************************************************/
namespace  Crzaykitchen
{
    public class KitchenObject : MonoBehaviour
    {
        [SerializeField] protected KitchenObjectSO kitchenObjectSO;

        protected IKitchenObjectParent paretnt;


        public static Action OnDrop;
        public static Action OnPickup;
        public KitchenObjectSO GetKitchenObjectSO()
        {
            return kitchenObjectSO;
        }

        public void SetKitChenObjectParent(IKitchenObjectParent _paretnt)
        {
            if (paretnt != null)
            {
                paretnt.ClearKitchenObject();
            }
            paretnt = _paretnt;
            if (paretnt.HasKitchenObject())
            {
                Debug.LogError("KitchenObject is already paretnt");
            }
            paretnt.SetKitchenObject(this);
            transform.parent = paretnt.GetKitchenObjectFollow(kitchenObjectSO);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public IKitchenObjectParent GetIKitchenObjectParent()
        {
            return paretnt;
        }

        public virtual void DestroySelf()
        {
            GetIKitchenObjectParent().ClearKitchenObject();

            PoolManager.Instance.PushObj(kitchenObjectSO.name,gameObject);
        }

        public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent _paretnt)
        {
             Transform kitchenObj = PoolManager.Instance.GetObj(kitchenObjectSO.name,kitchenObjectSO.prefab.gameObject,Vector3.zero, Quaternion.identity).transform;
             KitchenObject kitchenObject= kitchenObj.GetComponent<KitchenObject>();
             kitchenObject.SetKitChenObjectParent(_paretnt);
             return kitchenObject;
        }
        public bool TryPlate(out PlateKitchenObject _plateKitchenObject)
        {
            if (this is PlateKitchenObject)
            {
                _plateKitchenObject=this as PlateKitchenObject;
                return true;
            }
            else
            {
                _plateKitchenObject = null;
                return false;
            }
        }
    }
}

