using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WGM;

public class FreeGameController : MonoBehaviour
{
    public static bool isFreeGame = false;
    // [Header("�����Ϸ��ʾ")]
    //public GameObject freeGameDisplay;

    public GameObject[] freeGameWin;
    static FreeGameController ins;
    public float maxTime = 90;
    float timer;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (ins != null)
            return;
        ///freeGameDisplay.SetActive(false);
        ins = this;
        LedScreenSerialPort.onFreeGame += OnFreeGame;
        LedScreenSerialPort.onFreeGameWinCoin += OnFreeGameWinCoin;
        LedScreenSerialPort.onFreeGameWinGift += OnFreeGameWinCoin;


    }
    void OnDisable()
    {
        if (ins != this)
            return;
        LedScreenSerialPort.onFreeGame -= OnFreeGame;
        LedScreenSerialPort.onFreeGameWinCoin -= OnFreeGameWinCoin;
        LedScreenSerialPort.onFreeGameWinCoin -= OnFreeGameWinCoin;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreeGame)
        {
            timer -= Time.unscaledDeltaTime;
            if (timer < 0)
                OnExitGame();
        }
        if (LibWGM.machine.Language == 2)
        {
            //freeGameDisplay.SetActive(false);
            return;
        }
        if (isFreeGame)
        {
            if (LedScreenAudioController.playMachineAudio)
            {
                LedScreenAudioController.playMachineAudio = false;
                DealCommand.Instance.SerialPortManager.SendSetVolume((int)LibWGM.machine.BgmVolume*10);
                AudioListener.volume = 0;
            }

            if (DealCommand.GetAnyKeyDown())
            {

                DealCommand.SendGameKetStateToLedScreen(DealCommand.anyKeys);



            }
            else if (DealCommand.GetAnyKeyUp())
            {
                DealCommand.SendGameKetStateToLedScreen(DealCommand.anyKeys);
            }


        }
    }
    bool up;
    void OnFreeGame(int state)
    {
        switch (state)
        {
            case 0:

                OnExitGame();
                break;
            case 1:
                OnEnterFreeGame();

                break;
        }
    }

    void OnEnterFreeGame()
    {
        //Ӣ����Ч
        if (LibWGM.machine.Language == 2)
            return;
        timer = maxTime;
        isFreeGame = true;

        //freeGameDisplay.SetActive(isFreeGame);
        AudioListener.volume = 0;

    }
    void OnExitGame()
    {
        //Ӣ����Ч
        if (LibWGM.machine.Language == 2)
            return;
        isFreeGame = false;

        //freeGameDisplay.SetActive(isFreeGame);
        EventManager.onVolumeChange?.Invoke();
        if (!LedScreenAudioController.playMachineAudio)
        {
            LedScreenAudioController.playMachineAudio = true;

            DealCommand.Instance.SerialPortManager.SendSetVolume(0);
            if (SceneManager.GetActiveScene().name == "Demo")
            {
                float vol = LibWGM.machine.BgmVolume / 10.0f;
                AudioListener.volume = vol * vol;
            }
            else
            {

                float vol = LibWGM.machine.SeVolume / 10.0f;
                AudioListener.volume = vol * vol;
            }
        }
    }

    void OnFreeGameWinCoin(int cnt)
    {
        StartCoroutine(FreeGameWinCoin());
    }
    IEnumerator FreeGameWinCoin()
    {
        for (int i = 0; i < freeGameWin.Length; i++)
        {
            freeGameWin[i].SetActive(true);
        }
        yield return new WaitForSeconds(5);

        for (int i = 0; i < freeGameWin.Length; i++)
        {
            freeGameWin[i].SetActive(false);
        }
    }

}
