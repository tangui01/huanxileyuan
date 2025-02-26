using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：CommonUI.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class CommonUI : MonoBehaviour
{
     public static CommonUI instance; 
     public Coin_inPanel CoinIn;
     public CurrentCoinCountPanel CoinCountPanel;
     public LoadingPanel Loading;
     public TipsPanel Tips;
     public BackMainPanel BackMain;
     public MainTimePanel mainTimePanel;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #region 续币面版区域
    //是否已经开始续币倒计时
    private bool StartColdDown;
    /// <summary>
    /// 开始续币倒计时
    /// </summary>
    public void StartCouterColdDown(bool isGameover=false)
    {
        if (StartColdDown)
        {
            return;
        }
        Time.timeScale = 0;
        StartColdDown = true;
        CoinIn.gameObject.SetActive(true);
        CoinIn.StartColdDown(isGameover);
    }
    public void ExitCouterColdDown()
    {
        StartColdDown = false;
    }

    #endregion
    public void SetCoinCountPanel(bool active)
    {
        CoinCountPanel.gameObject.SetActive(active);
    }

    public void SetLoadingPanel(bool active)
    {
        Loading.gameObject.SetActive(active);
    }

    public void BackMainPanel_OPen(bool gameover = false)
    {
        BackMain.StartColdDown(gameover);
    }
/// <summary>
/// 进入后台时
/// </summary>
    public void EnterBG()
    {
        BackMain.gameObject.SetActive(false);
        Loading.gameObject.SetActive(false);
        CoinCountPanel.gameObject.SetActive(false);
        CoinIn.gameObject.SetActive(false);
        mainTimePanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// 退出后台时
    /// </summary>
    public void ExitBG()
    {
        mainTimePanel.Enter();
        CurrentCoinCountPanel.instance.SetGamestateByCoinCount();
        CoinCountPanel.gameObject.SetActive(true);
        CoinCountPanel.setCoinCount();
    }

    /// <summary>
    /// 提示添加
    /// </summary>
    /// <param name="tips"></param>
    public void AddTips(string tips)
    {
        Tips.AddTip(tips);
    }
}