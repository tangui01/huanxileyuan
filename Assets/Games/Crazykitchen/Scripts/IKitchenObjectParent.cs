using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：IKitchenObjectParent.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace  Crzaykitchen
{
public interface IKitchenObjectParent
{
    public void SetKitchenObject(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObject();
    public Transform GetKitchenObjectFollow(KitchenObjectSO ko);
    public void ClearKitchenObject();
    public bool HasKitchenObject();
}
}
