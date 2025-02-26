using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：BuenedRecipSo.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房燃烧菜谱
*****************************************************/
[CreateAssetMenu(fileName = "BurnedRecipSo", menuName = "游戏静态数据/疯狂厨房/烧焦食谱")]
public class BurnedRecipSo : ScriptableObject
{
    public KitchenObjectSO InputKitchenObject;
    public KitchenObjectSO OutputKitchenObject;
    public int BurnedRecipSoTimeMax;
}
