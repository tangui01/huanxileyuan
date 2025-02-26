using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：RecipeListSo.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
[CreateAssetMenu(fileName = "New RecipeListSo",menuName = "游戏静态数据/疯狂厨房/全部的食谱")]
public class RecipeListSo : ScriptableObject
{
    public List<RecipeSo> recipeList = new List<RecipeSo>();
}
