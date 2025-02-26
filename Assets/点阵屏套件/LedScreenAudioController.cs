using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WGM;

public class LedScreenAudioController : MonoBehaviour
{
    public static bool playMachineAudio;


    public static void OnVolumeChange()
    {
        if (DealCommand.Instance.SerialPortManager.handle < 0)
            return;
        if (playMachineAudio)
        {

        }
        else
        {
            DealCommand.Instance.SerialPortManager.SendSetVolume((int)LibWGM.machine.BgmVolume * 10);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (DealCommand.Instance.SerialPortManager.handle < 0)
            return;
        PlayAudioAutoSelect();
    }


    // Update is called once per frame
    void Update()
    {
        if (DealCommand.Instance.SerialPortManager.handle < 0)
            return;

        if (FreeGameController.isFreeGame)
            return;

        ///没有存活的玩家且没币播放盒子音乐，否则机器音乐
        if ((LibWGM.playerData[1].coin_in + LibWGM.playerData[0].Free_coin_in == 0 )&&GameTimeManager.instance.GetCurrentTime() <= 0)
        {
            PlayBoxAudio();
        }
        else
        {
            PlayMachineAudio();
        }

    }
   
    public static void PlayMachineAudio()
    {

        if (!playMachineAudio)
        {
            Debug.Log("Box audio 关");
            playMachineAudio = true;
            DealCommand.Instance.SerialPortManager.SendSetVolume(0);

        }
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

    public static void PlayBoxAudio()
    {
        if (playMachineAudio)
        {
            playMachineAudio = false;
            Debug.Log("Box audio 开");
            DealCommand.Instance.SerialPortManager.SendSetVolume((int)LibWGM.machine.BgmVolume * 10);
            AudioListener.volume = 0;
        }
    }
    ///自动判断当前怎么播放声音
    public static void PlayAudioAutoSelect()
    {
        if (DealCommand.Instance.SerialPortManager.handle < 0|| DealCommand.Instance.SerialPortManager.deviceType== DeviceType.Box)
            return;
        if (LibWGM.playerData[0].coin_in + LibWGM.playerData[0].Free_coin_in <= 0 && SceneManager.GetActiveScene().name == "Demo")
        {
            Debug.Log("Box audio 开");
            playMachineAudio = false;
            DealCommand.Instance.SerialPortManager.SendSetVolume((int)LibWGM.machine.BgmVolume * 10);
            AudioListener.volume = 0;
        }
        else
        {
            Debug.Log("Box audio 关");
            if (SceneManager.GetActiveScene().name == "Demo")
            {
                DealCommand.Instance.SerialPortManager.SendSetVolume(0);
                float vol = LibWGM.machine.BgmVolume / 10.0f;
                AudioListener.volume = vol * vol;
            }
            else
            {
                DealCommand.Instance.SerialPortManager.SendSetVolume(0);
                float vol = LibWGM.machine.SeVolume / 10.0f;
                AudioListener.volume = vol * vol;
            }
            playMachineAudio = true;

        }

    }
}
