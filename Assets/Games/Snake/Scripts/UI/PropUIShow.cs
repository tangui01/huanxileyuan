using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：PopPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：道具显示面版
*****************************************************/
namespace SnakeGame
{
    public class PropUIShow : MonoBehaviour
    {
         private Image iconimg;
         private Image Processimg;
         private Text CountText;
         private CanvasGroup canvasGroup;
         private int Count=0;
         
         [SerializeField]private PropType  _propType;
         [SerializeField] private float PropTimer=10;//道具时间
         private float currentTime = 0;
         private void Awake()
         {
             iconimg=transform.Find("Icon").GetComponent<Image>();
             Processimg=transform.Find("TimerFillCircle").GetComponent<Image>();
             CountText=transform.Find("AmountHolder/boosterAmountUI").GetComponent<Text>();
             canvasGroup = GetComponent<CanvasGroup>();
         }

         private void Start()
         {
             currentTime = PropTimer;
             canvasGroup.alpha = 0.7f;
         }

         public void AddPropCount(int addvalue=1)
        {
            Count+=addvalue;
            canvasGroup.alpha = 1f;
            CountText.text = Count.ToString();
        }

        private void Update()
        {
            if (Count>0)
            {
                SatrtCountDown();
            }
        }
        private void SatrtCountDown()
        {
            if (currentTime<0)
            {
                Count--;
                CountText.text = Count.ToString();
                currentTime = PropTimer;
                if (Count<=0)
                {
                    canvasGroup.alpha = 0.7f;
                    PropManager.Instance.ExitPropEffect(_propType);
                }
            }
            else
            {
                currentTime-=Time.deltaTime;
                Processimg.fillAmount = currentTime / PropTimer;
            }
        }
    }
}

