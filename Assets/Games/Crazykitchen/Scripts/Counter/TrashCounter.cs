using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：TrashCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace  Crzaykitchen
{
public class TrashCounter : BaseCounter
{
    public static Action OnObjectTrashed;
     public override void Interact(CrazyKitchenPlayer player)
     {
         if (player.HasKitchenObject())
         {
             OnObjectTrashed?.Invoke();
             player.GetKitchenObject().DestroySelf();
         }
     }
 }
}
