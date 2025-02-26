
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ramboat2DTimeUIController : MonoBehaviour
{
    public Text timerText;
    private Action ContinuePlayGame;//续币事件
    private Action DestroyGame;//游戏时间到，操作游戏内对象事件
    private bool isGameOver = false;
    private void Awake()
    {
        Application.targetFrameRate = 30;
    } 

    private void Start()
    {
        SetTimerText(GameTimeManager.instance.GetCurrentTime());
        GameTimeManager.instance.UpdateTimeUI = SetTimerText;
        StartCoroutine(WaitStart(2f));
    }

    private void OnEnable()
    {
        GameTimeManager.instance.TimeOverAction += GameOver;
        Coin_inPanel.Renew_money_Success += SuccessCoin;
    }

    private void OnDisable()
    {
        GameTimeManager.instance.TimeOverAction -= GameOver;
        Coin_inPanel.Renew_money_Success -= SuccessCoin;
    }
    IEnumerator WaitStart(float time)
    {
        yield return new WaitForSeconds(time);
        ReSetComtinuePlayGameAction();
        ReSetDestroyGameAction();
    }
    
    public void ClaerDestroyGameAction()
    {
        
    }
    
    public void ReSetDestroyGameAction()
    {

    }

    public void ClaerComtinuePlayGameAction()
    {

    }
    public void ReSetComtinuePlayGameAction()
    {
        
    }
    
    void Update()
    {
        
    }
    
    public void SuccessCoin()
    {
        if (timerText != null)
        {
            timerText.transform.parent.gameObject.SetActive(true);
        }
        ContinuePlayGame?.Invoke();
        isGameOver = false;
    }
    
    public void GameOver()
    {
        if (!isGameOver)
        {
            StartCoroutine(Over());
        }
        
    }

    IEnumerator Over()
    {
        isGameOver = true;
        DestroyGame?.Invoke();
        CommonUI.instance.StartCouterColdDown();
        yield return new WaitForSeconds(1f);
        isGameOver = false;
    }
    public void SetTimerText(int seconds)
    {
        if ( timerText != null && timerText.transform.parent.gameObject.activeSelf)
        {
            timerText.text = seconds+"";
        }
    }
}
