using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：PlatesCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房盘子计数器
*****************************************************/
namespace Crzaykitchen
{
    public class PlatesCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO platesKitchenObjectSo;
        private int PlatesCount = 0;
        private int PlatesCountMax = 4;
        private float SpawnPlatesTimer = 0;
        private float SpawnPlatesTimerMax = 4;
        
        public Action OnPlatesSpawn;
        public Action OnPlatesRemoved;
        private void Update()
        {
            SpawnPlatesTimer += Time.deltaTime;
            if (SpawnPlatesTimer>=SpawnPlatesTimerMax)
            {
                SpawnPlatesTimer = 0;
                if (PlatesCount<PlatesCountMax)
                {
                    PlatesCount++;
                    OnPlatesSpawn?.Invoke();
                }
            }
        }

        public override void Interact(CrazyKitchenPlayer player)
        {
            if (!player.HasKitchenObject())
            {
                if (PlatesCount>0)
                {
                    OnPlatesRemoved?.Invoke();
                    PlatesCount--;
                    KitchenObject.SpawnKitchenObject(platesKitchenObjectSo, player);
                }
            }
           
        }
    }
}


