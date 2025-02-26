using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

/****************************************************
    文件：KitchenObjectSO.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房对象静态数据
*****************************************************/

public enum kitchenObjectType
{
    InputkitchenObject,
    outputkitchenObject
}

[CreateAssetMenu(fileName = "New KitchenObjectSO",menuName = "游戏静态数据/疯狂厨房/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    [PreviewField(75),HideLabel,HorizontalGroup("厨房对象",75)]public Sprite icon;
    [VerticalGroup("厨房对象/预制体"),LabelText("预制体"),LabelWidth(50)]public Transform prefab;
    [VerticalGroup("厨房对象/预制体/名字"),LabelText("名字"),LabelWidth(50)]public string objectName;
    [VerticalGroup("厨房对象/预制体/名字/厨房物体种类"),LabelText("物体种类"),LabelWidth(50)]public  kitchenObjectType objectType;
    
}
