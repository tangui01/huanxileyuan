using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

/****************************************************
    文件：MainAudioManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource[] effectSources;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetBGmVolume(LibWGM.machine.BgmVolume / 10);
        SetEfVolume(LibWGM.machine.SeVolume / 10);
    }

    public void SetBGmVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetEfVolume(float volume)
    {
        for (int i = 0; i < effectSources.Length; i++)
        {
            effectSources[i].volume = volume;
        }
    }

    public void playerBGm(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public bool PlayerISNGm()
    {
        return bgmSource.isPlaying;
    }

    public void StopBGm()
    {
        bgmSource.Stop();
    }

    public void PauseBGm()
    {
        bgmSource.Pause();
    }

    public void StopAllAudioSound(bool isstopBgm=false)
    {
        if (isstopBgm)
        {
            bgmSource.Stop();
        }
        for (int i = 0; i < effectSources.Length; i++)
        {
            effectSources[i].Stop();
        }
    }

    public void playerEffect1(AudioClip clip)
    {
        effectSources[0].PlayOneShot(clip);
    }

    public void playerEffect2(AudioClip clip)
    {
        effectSources[1].PlayOneShot(clip);
    }

    public void playerEffect3(AudioClip clip)
    {
        effectSources[2].PlayOneShot(clip);
    }
    public void playerEffect4(AudioClip clip)
    {
        effectSources[3].PlayOneShot(clip);
    }
    public void playerEffect5(AudioClip clip)
    {
        effectSources[4].clip=clip;
        effectSources[4].Play();
    }
    public bool Effect1ISPalyer()
    {
        return   effectSources[0].isPlaying;
    }

    public bool Effect2ISPalyer()
    {
        return   effectSources[1].isPlaying;
    }

    public bool Effect3ISPalyer()
    {
        return   effectSources[2].isPlaying;
    }
    public bool Effect4ISPalyer()
    {
        return effectSources[3].isPlaying;
    }
    public bool Effect5ISPalyer()
    {
        return effectSources[4].isPlaying;
    }
    public void StopEffect1Player()
    {
        effectSources[0].Stop();
    }

    public void StopEffect2Player()
    {
        effectSources[1].Stop();
    }

    public void StopEffect3Player()
    {
        effectSources[2].Stop();
    }
    public void StopEffect4Player()
    {
        effectSources[3].Stop();
    }
    public void StopEffect5Player()
    {
        effectSources[4].Stop();
    }
    public void SetEff(AudioClip audioClip, float volume = 1,int effID = 0)
    {
        effectSources[effID].volume = volume *LibWGM.machine.SeVolume/10;
        effectSources[Mathf.Clamp(effID,0,effectSources.Length)].PlayOneShot(audioClip);
    }

    public AudioSource GetEffect(int effID = 0)
    {
        return effectSources[Mathf.Clamp(effID, 0, effectSources.Length)];
    }
    /// <summary>
    /// 0:Stop 1:Play 2:Pause
    /// </summary>
    public void SetBgmReserve(AudioClip audioClip,float volume = 1,int Status = 3)
    {
        effectSources[2].clip = audioClip;
        effectSources[2].loop = true;
        effectSources[2].volume = volume * LibWGM.machine.BgmVolume/10;
        switch (Status)
        {
            case 0:
                effectSources[2].Stop();
                effectSources[2].loop = false;
                break;
            case 1:
                effectSources[2].Play();
                break;
            case 2:
                effectSources[2].Pause();
                break;
        }
    }
}