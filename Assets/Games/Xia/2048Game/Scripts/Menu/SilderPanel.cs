using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class SilderPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
    public int currentIndex;
    [HideInInspector]
    public int panelCount;
    private Transform parentTF;
    private Transform[] childTfList;
    private PageNumberManager pageManager;
    private void Awake()
    {
        pageManager = FindObjectOfType<PageNumberManager>();
        parentTF = this.transform.parent;
        panelCount = this.transform.childCount;
        childTfList = new Transform[panelCount];
        for (int i = 0; i < panelCount; i++)
        {
                childTfList[i] = this.transform.GetChild(i);
        }
    }

    private Vector2 beginPoint;
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginPoint = eventData.position;
    }

    public float dragSpeed = 40;
    public float moveSpeed = 10;
    public float offsetTop = 300;//光标滑动的偏移量上限
    public float offsetSpeed = 1;//光标滑动时偏移速度
    private Vector2 touchOffset;
    //private Vector2 delta;
    public void OnDrag(PointerEventData eventData)
    { 
        //delta = eventData.delta;
        this.transform.Translate(eventData.delta.x * Time.deltaTime * dragSpeed, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {  
        touchOffset = eventData.position - beginPoint;
        //有时结束拖拽事件未响应
        //if (Mathf.Abs(delta.x) > offsetSpeed || touchOffset.magnitude > offsetTop)
        if (Mathf.Abs(eventData.delta.x) > offsetSpeed || touchOffset.magnitude > Screen.width /2) 
        {
            //鼠标向左移动 物体向左移动 索引增加
            if (touchOffset.x < 0)
                currentIndex++;
            else
                currentIndex--;
            currentIndex = Mathf.Clamp(currentIndex, 0, childTfList.Length - 1);
            pageManager.ChangePageNumber();//修改页码  
        }
        isHoming = true;
        //delta = touchOffset = Vector2.zero;
        touchOffset = Vector2.zero;
    }

    private bool isHoming = false;
    //归位
    private void Homing()
    {
        if (isHoming)
        {
            //呈现当前子面板 childTfArray[currentIndex] 
            Vector3 targetPos = parentTF.position - childTfList[currentIndex].position + this.transform.position;
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * moveSpeed);
            if ((targetPos- this.transform.position).sqrMagnitude < 1f)
            {
                this.transform.position = targetPos; 
                isHoming = false;
            }
        }
    }

    private void Update()
    {
        Homing();
    }
    
    public void SetIndex(int index)
    { 
        this.currentIndex = index;
        pageManager.ChangePageNumber();//修改页码
        isHoming = true;
    }
}
