using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：GameRoot.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：游戏跟节点
*****************************************************/
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
          LoadABManger.Instance.LoadAB(MainConstant.MainSceneName);
    }
}
