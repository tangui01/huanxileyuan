using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/****************************************************
    文件：LoadingPanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class LoadingPanel : MonoBehaviour
{
    public Text  ProcessText;
    public Image ProcessImage;
    private void Start()
    {
        ProcessText.text = "0%";
        ProcessImage.fillAmount = 0;
    }

    public void SetProcessText(float process)
    {
        ProcessText.text =Mathf.RoundToInt(process*100)+"%";
        ProcessImage.fillAmount = process;
    }
}
