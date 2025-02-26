using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheNameOfAUIManager : MonoBehaviour
{
    private Dictionary<string, Transform> panelDic;
    private  TheNameOfAUIManager instance;
    public  TheNameOfAUIManager Instance
    { 
        get
        { return instance; }
    }
     
    private  void Awake()
    {
        instance = this;    
        panelDic = new Dictionary<string, Transform>();
        GameObject[] panelAll = GameObject.FindGameObjectsWithTag("Panel");
        for (int i = 0; i < panelAll.Length; i++)
            panelDic.Add(panelAll[i].name, panelAll[i].transform);
    }

    /// <summary>
    /// 根据UI名称，获取变换组件
    /// </summary>
    /// <param name="panelName">面板名称</param>
    /// <param name="childName">UI名称</param>
    /// <returns>UI变换组件</returns>
    public Transform GetUIByName(string panelName, string childName)
    {
        return GetUIByName(panelDic[panelName], childName);
    }

    /// <summary>
    ///  根据UI名称，获取变换组件
    /// </summary>
    /// <param name="parentTf">父物体变换组件</param>
    /// <param name="childName">UI名称</param>
    /// <returns>UI变换组件</returns>
    public Transform GetUIByName(Transform parentTf, string childName)
    {
        Transform childTf = parentTf.Find(childName);
        if (childTf != null)
        {
            return childTf;
        }
        int count = parentTf.childCount;
        for (int i = 0; i < count; i++)
        {
            childTf = GetUIByName(parentTf.GetChild(i), childName);
            if (childTf != null)
                break;
        }
        return childTf;
    }

    /// <summary>
    /// 获取UI面板
    /// </summary>
    /// <param name="panelName">面板名称</param>
    /// <returns>面板变换组件</returns>
    public Transform GetPanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName];
        return null;
    }
}
