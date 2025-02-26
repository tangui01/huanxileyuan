using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：RecipeSo.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：食谱
*****************************************************/
[CreateAssetMenu(fileName = "New RecipeSo",menuName = "游戏静态数据/疯狂厨房/食谱")]
public class RecipeSo : ScriptableObject
{
     public string recipeName;
     public List<KitchenObjectSO> kitchenObjects;
}
