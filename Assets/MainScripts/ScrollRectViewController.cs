using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectViewController : MonoBehaviour
{
    public float topSpace = 0;
    public float downSpace = 0;
    public float scrollRectSpeed = 0.2f;
    /// <summary>
    /// 是否被选中
    /// </summary>
    [HideInInspector]
    public bool isSelected = false;
    private ScrollRect scrollRect;
    private RectTransform GameNameListRect;
    private void Awake()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        GameNameListRect = scrollRect.GetComponent<RectTransform>();
        Debug.Assert(scrollRect);
    }
    void Update()
    {
        if (isSelected)
            CheckItemVisibility();
    }

    public void CheckItemVisibility()
    {
        Camera camera = Camera.main;
        float z = -camera.transform.position.z;
        
        float downY =  Camera.main.WorldToScreenPoint(GameNameListRect.transform.position).y - GameNameListRect.sizeDelta.y/2; 
        float topY  =  Camera.main.WorldToScreenPoint(GameNameListRect.transform.position).y  +  GameNameListRect.sizeDelta.y/2;
        downY += downSpace;
        topY -= topSpace;
        
        RectTransform rect =(RectTransform)transform;
        float height = rect.sizeDelta.y;
        float itemTopY = Camera.main.WorldToScreenPoint(transform.position).y;
        float itemDownY = Camera.main.WorldToScreenPoint(transform.position).y;
        // float downY = downSpace;
        // float topY = Screen.height - topSpace;
        //
        // RectTransform rect =(RectTransform)transform;
        // float height = rect.sizeDelta.y;
        // float itemTopY = transform.position.y + height/2;
        // float itemDownY = transform.position.y - height/2;
        
        if (itemTopY > topY)
        {
            float anchY = scrollRect.content.anchoredPosition.y - (itemTopY - topY);
            float normalizedY = 1 - Mathf.Clamp01(anchY/ (scrollRect.content.rect.size.y - scrollRect.viewport.rect.size.y ));
            scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(scrollRect.verticalNormalizedPosition, normalizedY, Time.deltaTime * scrollRectSpeed);
        }
        else if (itemDownY < downY)
        {
            float anchY = scrollRect.content.anchoredPosition.y + (downY - itemDownY);
            float normalizedY = 1 - Mathf.Clamp01(anchY/ (scrollRect.content.rect.size.y - scrollRect.viewport.rect.size.y ));
            scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(scrollRect.verticalNormalizedPosition, normalizedY, Time.deltaTime * scrollRectSpeed);
        }
    }
}
