using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：ContainerCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace Crzaykitchen
{
    public class ContainerCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSo;
    
        public  event EventHandler OnPlayerCaptureEvent;
        public override void Interact(CrazyKitchenPlayer player)
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSo,player);
                OnPlayerCaptureEvent?.Invoke(this,EventArgs.Empty);
                SoundManager.Instance.PlayerGenerateSound();
            }
        }
    } 
}

