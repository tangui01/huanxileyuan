using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TTUITool
{

	/// <summary>
    /// 根据最小depth设置目标所有Panel深度，从小到大
    /// </summary>
    /// 
    private class CompareSubPanels : IComparer<UIPanel>
    {
        public int Compare(UIPanel left, UIPanel right)
        {
            return left.depth - right.depth;
        }
    }

    public static void SetTargetMinPanel(GameObject obj, int depth)
    {
        List<UIPanel> lsPanels = GetPanelSorted(obj, true);
        if (lsPanels != null)
        {
            int i = 0;
            while (i < lsPanels.Count)
            {
                lsPanels[i].depth = depth + i;
                i++;
            }
        }
    }

    /// <summary>
    /// 获得指定目标最大depth值
    /// </summary>
    public static int GetMaxTargetDepth(GameObject obj, bool includeInactive = false)
    {
        int minDepth = -1;
        List<UIPanel> lsPanels = GetPanelSorted(obj, includeInactive);
        if(lsPanels != null)
            return lsPanels[lsPanels.Count - 1].depth;
        return minDepth;
    }

    /// <summary>
    /// 返回最大或者最小Depth界面
    /// </summary>
    public static GameObject GetPanelDepthMaxMin(GameObject target, bool maxDepth, bool includeInactive)
    {
        List<UIPanel> lsPanels = GetPanelSorted(target, includeInactive);
        if(lsPanels != null)
        {
            if (maxDepth)
                return lsPanels[lsPanels.Count - 1].gameObject;
            else
                return lsPanels[0].gameObject;
        }
        return null;
    }

    private static List<UIPanel> GetPanelSorted(GameObject target, bool includeInactive = false)
    {
        UIPanel[] panels = target.transform.GetComponentsInChildren<UIPanel>(includeInactive);
        if (panels.Length > 0)
        {
            List<UIPanel> lsPanels = new List<UIPanel>(panels);
            lsPanels.Sort(new CompareSubPanels());
            return lsPanels;
        }
        return null;
    }

	/// <summary>
    /// 添加子节点
    /// </summary>
    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;

        ChangeChildLayer(child, target.gameObject.layer);
    }

    /// <summary>
    /// 修改子节点Layer  NGUITools.SetLayer();
    /// </summary>
    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }
}
