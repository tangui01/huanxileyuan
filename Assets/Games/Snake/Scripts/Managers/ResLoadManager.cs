using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：ResLoadManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/19 12:36
    功能：资源加载
*****************************************************/
public class ResLoadManager : MonoBehaviour
{
    public static ResLoadManager instance;

    private void Awake()
    { 
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public T LoadGoObj<T>(string name,string path,Vector3 bornPos,Quaternion quaternion,Transform parent=null)
    {
        T t = PoolManager.Instance.GetObj(name,Resources.Load<GameObject>(path),bornPos,quaternion,parent).GetComponent<T>();
        return t;
    }
    public GameObject LoadGoObj(string path)
    {
        GameObject go= Resources.Load<GameObject>(path);
        return go;
    }
}
