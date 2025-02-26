using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ItemGetType {
    Coin,
    Gem
}

public class GetItem : MonoBehaviour
{
    public GetItemObject _itemCoin;
    public GetItemObject _itemGem;

    public Transform posCoin;
    public Transform posGem;
}
