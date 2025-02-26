using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：IHasPrecess.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public interface IHasPrecess 
    {
        public event EventHandler<OnPrecessChangedEventArgs> OnPrecessChanged;
        public class OnPrecessChangedEventArgs:EventArgs
        {
            public float Precess;
        }
    }
}

