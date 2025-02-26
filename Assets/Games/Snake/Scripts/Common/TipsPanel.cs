using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：TipsPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：
    功能：游戏消息提示面版
*****************************************************/
public class TipsPanel : MonoBehaviour
{
      private Animator animator;
      private Text tipsText;
      private Queue<string> tips = new Queue<string>();
      private bool isshowTips = false;

      private float ClearTime;
      private void Awake()
      {
          animator = GetComponent<Animator>();
          tipsText = GetComponentInChildren<Text>();
      }

      public void AddTip(string tip)
      {
          tips.Enqueue(tip);
      }

      public void SetTips(string tip)
      {
          tipsText.text = tip;
          StartCoroutine("StopTips");
      }

      IEnumerator StopTips()
      {
          isshowTips = true;
          animator.SetBool("Show",isshowTips);
          yield return new WaitForSeconds(2f);
          isshowTips = false;
          animator.SetBool("Show",isshowTips);
      }

      private void Update()
      {
          if (tips.Count>0&&!isshowTips)
          {
               SetTips(tips.Dequeue());
          }
          else if(tips.Count>0)
          {
              ClearTime+=Time.deltaTime;
              if (ClearTime>2)
              {
                  tips.Clear();
              }
          }
      }
}
