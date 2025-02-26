using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：FryingRecipSo.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房烧煎食谱
*****************************************************/
[CreateAssetMenu(fileName = "FryingRecipSo", menuName = "游戏静态数据/疯狂厨房/烧煎食谱")]
public class FryingRecipSo :ScriptableObject
{
    public KitchenObjectSO InputKitchenObject;
    public KitchenObjectSO OutputKitchenObject;
    public int FryingTimeMax;
}
