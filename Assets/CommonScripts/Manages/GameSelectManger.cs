using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/****************************************************
    文件：GameSelectManger.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class GameSelectManger : MonoBehaviour
{
       public static GameSelectManger Instance;
       [FormerlySerializedAs("CurrentSelectGame")] public int CurrentSelectGameIndex = 0;
       private void Awake()
       {
           if (Instance==null)
           {
               Instance = this;
           }
           else
           {
               Destroy(gameObject);
           }
       }

       public void SelectGame(int inex)
       {
           CurrentSelectGameIndex=inex;
       }

       public int GetSelectGame()
       {
           return CurrentSelectGameIndex;
       }
}
