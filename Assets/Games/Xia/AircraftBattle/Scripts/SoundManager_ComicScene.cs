using System;
using UnityEngine;
using System.Collections;
using WGM;

public class SoundManager_ComicScene : MonoBehaviour {

	public AudioSource beepSound;
	public AudioSource swoosh1;
	public AudioSource swoosh2;
	public AudioSource swoosh3;
	public AudioSource swoosh4;
	public AudioSource clickToAnswer;
	public AudioSource comicMusic;
	
	private float[] volume = new float[7];
	static SoundManager_ComicScene instance;
	
	public static SoundManager_ComicScene Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SoundManager_ComicScene)) as SoundManager_ComicScene;
			}
			
			return instance;
		}
	}

	private void Start()
	{
		volume[0] = beepSound.volume;
		volume[1] = swoosh1.volume;
		volume[2] = swoosh2.volume;
		volume[3] = swoosh3.volume;
		volume[4] = swoosh4.volume;
		volume[5] = clickToAnswer.volume;
		volume[6] = comicMusic.volume;	
	}

	
	public void SyncSounds(AudioSource source,int Index)
	{
		source.volume = volume[Index] * LibWGM.machine.SeVolume / 10;
	}
	public void Play_BeepSound()
	{
		if (beepSound.clip != null && SoundManager.soundOn == 1)
		{
			SyncSounds(beepSound, 0);
			beepSound.Play();
		}
			
	}
	
	public void Play_Swoosh1()
	{
		if(swoosh1.clip != null && SoundManager.soundOn == 1)
		{
			SyncSounds(swoosh1, 1);
			swoosh1.Play();
		}
	}

	public void Play_Swoosh2()
	{
		if (swoosh2.clip != null && SoundManager.soundOn == 1)
		{
			SyncSounds(swoosh2, 2);
			swoosh2.Play();
		}
			
	}

	public void Play_Swoosh3()
	{
		if (swoosh3.clip != null && SoundManager.soundOn == 1)
		{
			SyncSounds(swoosh3, 3);
			swoosh3.Play();
		}
			
	}

	public void Play_Swoosh4()
	{
		if (swoosh4.clip != null && SoundManager.soundOn == 1)
		{
			SyncSounds(swoosh4, 4);
			swoosh4.Play();
		}
	}
	
	public void Play_ClickToAnswer()
	{
		if (clickToAnswer.clip != null && SoundManager.soundOn == 1)
		{
			SyncSounds(clickToAnswer, 5);
			clickToAnswer.Play();
		}
	}
	
	public void Play_ComicMusic()
	{
		if (comicMusic.clip != null && SoundManager.musicOn == 1)
		{
			SyncSounds(comicMusic, 6);
			comicMusic.Play();
		}
	}

	public void Stop_ComicMusic()
	{
		if(comicMusic.clip != null && SoundManager.musicOn == 1)
			StartCoroutine(FadeOut(comicMusic, 0.0035f));
	}

	IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;
		while(sound.volume != 0)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}
}
