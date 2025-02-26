using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    protected AudioSource mAudioSource;
    public AudioClip  bg_normal;
    public AudioClip warn;
    public AudioClip bg_Boss;
    public AudioClip bg_gameOver;
    public AudioClip bg_victory;
    public static Background instance;
	// Use this for initialization
    void Awake()
    {
        instance = this;
         mAudioSource=GetComponent <AudioSource >();
        PlayBg_normal();
    }
	
	// Update is called once per frame
    public void PlayBg_normal()
    {
        mAudioSource.clip = bg_normal;
        mAudioSource.Play();
    }
    public void PlayWarn()
    {
        mAudioSource.clip = warn;
        mAudioSource.Play(); }
    public void PlayBg_Boss()
    {
        mAudioSource.clip = bg_Boss;
      
        mAudioSource.Play();
    }
    public void PlayBg_GameOver()
    {
        mAudioSource.clip = bg_gameOver;
        mAudioSource.Play();
    }
    public void PlayBg_victory()
    {
        mAudioSource.clip = bg_victory;
        mAudioSource.Play();
    }
    
}
