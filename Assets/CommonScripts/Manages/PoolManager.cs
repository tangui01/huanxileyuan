using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：PoolManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：对象池管理器
*****************************************************/
public class PoolManager 
{
    private static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PoolManager();
            }
            return instance;
        }
    }

    //对象池字典
    private Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
    private Transform poolRoot;
    /// <summary>
    /// 进入对象池
    /// </summary>
    /// <param name="name">对象物体类别</param>
    /// <param name="obj">对象物体</param>
    public void PushObj(string name, GameObject obj)
    {
        //检测有没有对象池的根节点
        if (poolRoot == null)
        {
            poolRoot = new GameObject("PoolRoot").transform;
        }
        //检查对象管理字典有没有相应的key
        if (!poolDic.ContainsKey(name))
        {
            poolDic.Add(name, new List<GameObject>());
        }
        //添加obj
        poolDic[name].Add(obj);
        //判断PoolRoot的子物体有没有相应类别的父物体
        if (poolRoot.transform.Find(name) == false)
        {
            GameObject typeParent = new GameObject(name);
            typeParent.transform.SetParent(poolRoot);
        }
        obj.SetActive(false);
        obj.transform.SetParent(poolRoot.Find(name));
    }

    /// <summary>
    /// 从对象池中得到物体
    /// </summary>
    /// <param name="name">对象物体类别</param>
    /// <param name="obj">对象物体类别</param>
    public GameObject GetObj(string name, GameObject obj,Vector3 BornPosition,Quaternion quaternion,Transform Parent=null)
    {
        GameObject Pre = null;
        //检测对象字典中有没有key,并且对象池中还有余粮
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            Pre = poolDic[name][0];
            poolDic[name].RemoveAt(0);
        }
        else
        {
            Pre = GameObject.Instantiate(obj, BornPosition,quaternion,Parent);
        }
        Pre.SetActive(true);
        Pre.transform.position = BornPosition;
        Pre.transform.rotation = quaternion;
        Pre.transform.SetParent(Parent);
        return Pre;
    }
    /// <summary>
    /// 清空对象池
    /// </summary>
    public void ClearPoolDic()
    {
        poolDic.Clear();
    }
}