using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/****************************************************
    文件：PopPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：道具显示面版
*****************************************************/
namespace  SnakeGame
{
    public class PropPanel : MonoBehaviour
    {
         private PropUIShow Prop_UnZoom;
         private PropUIShow Prop_Magnet;
         private PropUIShow Prop_ScoreMultiplier;
         private PropUIShow Prop_ExtraSpeed;

         private void Awake()
         {
             Prop_UnZoom=transform.Find("Prop_UnZoom").GetComponent<PropUIShow>();
             Prop_Magnet=transform.Find("Prop_Magnet").GetComponent<PropUIShow>();
             Prop_ScoreMultiplier=transform.Find("Prop_ScoreMultiplier").GetComponent<PropUIShow>();
             Prop_ExtraSpeed=transform.Find("Prop_ExtraSpeed").GetComponent<PropUIShow>();
         }

         public void AddProp(PropType propType)
         {
             switch (propType)
             {
                 case PropType.UnZoom:
                     Prop_UnZoom.AddPropCount();
                     break;
                 case PropType.Magnet:
                     Prop_Magnet.AddPropCount();
                     break;
                 case PropType.ExtraSpeed:
                     Prop_ExtraSpeed.AddPropCount();
                     break;
                 case PropType.ScoreMultiplier:
                     Prop_ScoreMultiplier.AddPropCount();
                     break;
             }
         }
    }
}

