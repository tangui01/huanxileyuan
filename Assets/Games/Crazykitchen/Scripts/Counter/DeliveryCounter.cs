using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：DeliveryCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class DeliveryCounter:BaseCounter
    {
        public override void Interact(CrazyKitchenPlayer player)
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryPlate(out PlateKitchenObject plate))
                {
                    CrzayKitchenGameManager.instance.DeliveRecipes(plate);
                    player.GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}

