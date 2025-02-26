using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：CuttingRecipeSo.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房切割食谱
*****************************************************/
[CreateAssetMenu(fileName = "CuttingRecipeSo", menuName = "游戏静态数据/疯狂厨房/疯狂厨房切割食谱")]
public class CuttingRecipeSo :ScriptableObject
{
      public KitchenObjectSO InputKitchenObject;
      public KitchenObjectSO OutputKitchenObject;
      public int CuttingProcessMax;
}
