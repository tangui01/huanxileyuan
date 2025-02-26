using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：AddScoreUI.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：2024/11/21 
    功能：吃到食物分数显示
*****************************************************/
public class AddScoreUI : MonoBehaviour
{
      private TextMeshProUGUI scoreText;
      public Animator Ani;
      public float Delay;

      private void Awake()
      {
          scoreText = GetComponentInChildren<TextMeshProUGUI>();
          Ani=GetComponentInChildren<Animator>();
      }

      public void SetScore(int addscore,bool Double)
      {
          if (!Double)
          {
              scoreText.text = "+"+addscore.ToString();
          }
          else
          {
              scoreText.text = "+"+addscore.ToString()+"X2";
          }
          Ani.Play("AddScoreAni");
          Invoke("Destroymy",Delay);
      }
      public void Destroymy()
      {
          scoreText.text = "";
          PoolManager.Instance.PushObj(SnakeGameConstant.AddScoreUIName,gameObject);
      }
}
