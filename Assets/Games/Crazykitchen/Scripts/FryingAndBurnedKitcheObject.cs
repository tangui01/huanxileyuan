using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：CuttingKitcheObject.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class FryingAndBurnedKitcheObject : KitchenObject
    {
        private float timer = 0;

        public void Resettimer()
        {
            timer = 0;
        }

        public void Settimer(int _timer)
        {
            timer=_timer;
        }

        public float Gettimer()
        {
            return timer;
        }
    }
}

