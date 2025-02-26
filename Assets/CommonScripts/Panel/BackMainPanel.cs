using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WGM;

/****************************************************
    文件：BackMainPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：确认是否返回大厅面版
*****************************************************/
public class BackMainPanel : MonoBehaviour
{

    private int index;
    private int indexmax=1;
    private int indexmin=0;
    [SerializeField]private Animator surebtnAni;
    [SerializeField]private Animator cancelBtnAni;
    
    private const string BtnSelectbool = "Select";
    [SerializeField] private float AutoCloseTimer = 10f;


    [SerializeField] private Image coldDownBg;
    [SerializeField] private Text  coldDownText;
    
    private float timer;
    private float timerMax;
    private bool ISStartColdDown=false;
    
    public static Action OnBackMainSuccessAction;
    
    private bool iSGameOver = false;
    [SerializeField] private AudioClip SwitchSound;
    [SerializeField] private AudioClip ClickSound;
    public void StartColdDown(bool _isGameOver)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        index = 0;
        timer = 0;
        ISStartColdDown = true;
        coldDownBg.fillAmount = 1;
        AudioManager.Instance.StopAllAudioSound();
        coldDownText.text = AutoCloseTimer+"S";
        timerMax=AutoCloseTimer;
        SwitchBtnViscal();
        iSGameOver=_isGameOver;
    }

    private void Update()
    {
        if (!ISStartColdDown)
        {
            return;
        }
        if (ColdDown()) return;
        SelectBtnInput();
    }
    private bool ColdDown()
    {
        if (timerMax>0)
        {
            timer+=Time.unscaledDeltaTime;
            if (timer>=1)
            {
                timerMax--;
                timer = 0;
                coldDownText.text = timerMax+"S";
            }
            coldDownBg.fillAmount -= Time.unscaledDeltaTime/AutoCloseTimer;
        }
        else
        {
            SwitchBtnClickEvent();
            return true;
        }
        return false;
    }

    private void SelectBtnInput()
    {
        if (DealCommand.GetKeyDown(1,AppKeyCode.TicketOut))
        {
            index--;
            AudioManager.Instance.playerEffect4(SwitchSound);
            if (index<indexmin)
            {
                index = 1;
            }
            SwitchBtnViscal();
        }
        else if (DealCommand.GetKeyDown(1,AppKeyCode.Flight))
        {
            index++;
            AudioManager.Instance.playerEffect4(SwitchSound);
            if (index>indexmax)
            {
                index = 0;
            }
            SwitchBtnViscal();
        }
        else if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore))
        {
            SwitchBtnClickEvent();
        }
    }

    private void SwitchBtnViscal()
    {
        switch (index)
        {
            case 0:
                surebtnAni.SetBool(BtnSelectbool,false);
                cancelBtnAni.SetBool(BtnSelectbool,true);
                break;
            case 1:
                surebtnAni.SetBool(BtnSelectbool,true);
                cancelBtnAni.SetBool(BtnSelectbool,false);
                break;
        }
    }
    private void SwitchBtnClickEvent()
    {
        switch (index)
        {
            case 0:
                CancelBtn();
                break;
            case 1:
                SureBtn();
                break;
        } 
    }

    public void SureBtn()
    {
        LoadABManger.Instance.LoadAB(MainConstant.MainSceneName);
        AudioManager.Instance.playerEffect4(ClickSound);
        LoadABManger.Instance.UnloadAB(SceneManager.GetActiveScene().name);
        SceneLoadManager.instance.ExitSceneACtion +=() =>
        {
            CommonUI.instance.mainTimePanel.Enter();
            Time.timeScale = 1;
            GameTimeManager.instance.SetPauseGame(false);
        };
        GameTimeManager.instance.ClearAction();
        gameObject.SetActive(false);
    }
    public void CancelBtn()
    {
        Time.timeScale = 1;
        AudioManager.Instance.playerEffect4(ClickSound);
        if (iSGameOver)
        {
            SceneLoadManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        gameObject.SetActive(false);
    }
}
