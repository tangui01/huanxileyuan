using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WGM;

/****************************************************
    文件：ClearCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace Crzaykitchen
{
    public class ClearCounter : BaseCounter
    {
        public override void Interact(CrazyKitchenPlayer player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    player.GetKitchenObject().SetKitChenObjectParent(this); 
                    KitchenObject.OnDrop?.Invoke();
                }
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    if (player.GetKitchenObject().TryPlate(out PlateKitchenObject plateObject))
                    {
                        if (plateObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.OnDrop?.Invoke();
                        }
                    }
                    else
                    {
                        if (GetKitchenObject().TryPlate(out PlateKitchenObject plateObject2))
                        {
                            if (plateObject2.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                            {
                                player.GetKitchenObject().DestroySelf();
                                KitchenObject.OnDrop?.Invoke();
                            }
                        }
                    }
                }
                else
                {
                    GetKitchenObject().SetKitChenObjectParent(player);
                    KitchenObject.OnPickup?.Invoke();
                }
            }
        }
    }
}