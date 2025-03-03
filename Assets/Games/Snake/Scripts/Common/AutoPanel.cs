using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WGM;

/****************************************************
    文件：AutoPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class AutoPanel : MonoBehaviour
{
    private Image image;
    private Text text;
    float timer;
    private float t0;
    private bool isStart;
    private void Awake()
    {
        image=GetComponent<Image>();
        text=GetComponentInChildren<Text>();
    }
    public void StartColddwon()
    {
        gameObject.SetActive(true);
        timer=LibWGM.machine.AutoTime;
        t0=0;
        text.text = timer+"S";
        image.fillAmount = 1;
    }

    public void Stop()
    {
        transform.DOScale(1.2f, 0.2f).onComplete += () =>
        {
            transform.DOScale(1f, 0.2f).onComplete += () => {  gameObject.SetActive(false);};
        };
       
    }

    private void Update()
    {
        ColdDown();
    }

    private void ColdDown()
    {
        if (t0>=1)
        {
            if (timer<=0)
            {
                Stop();
            }
            else
            {
                timer -= 1;
                t0=0;
                text.text = timer+"S";
                transform.DOScale(1.2f, 0.2f).onComplete += () =>
                {
                    transform.DOScale(1f, 0.2f).onComplete += () => { };
                };
                image.fillAmount = 1;
            }
        }
        else
        {
            t0+=Time.unscaledDeltaTime; 
            image.fillAmount = 1-t0;
        }
    }
}
