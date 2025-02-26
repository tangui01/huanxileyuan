using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SignalR;
using UnityEngine;
/****************************************************
    文件：StoveCounterVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房平底锅视觉效果
*****************************************************/
namespace Crzaykitchen
{
    public class StoveCounterVisual : MonoBehaviour
    {
       [SerializeField] private StoveCounter stove;
       [SerializeField] private GameObject stoveonGameObject;
       [SerializeField] private GameObject parGameObject;
       private bool ISShow;
       private void Start()
       {
           stove.OnStateChanged += OnStoveStateChanged;
       }

       private void OnStoveStateChanged(StoveCounter.StoveCounterState state)
       {
           ISShow = state == StoveCounter.StoveCounterState.Frying || state == StoveCounter.StoveCounterState.Fried;
           stoveonGameObject.SetActive(ISShow);
           parGameObject.SetActive(ISShow);
       }
    }
}

