using System;
using UnityEngine;
using DG.Tweening;

public class BricksBreakerSoundManager : MonoBehaviour
{
    public static BricksBreakerSoundManager Instance;
    public AudioClip bgm;
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
		AudioManager.Instance.playerBGm(bgm);
	}
	
	/// <summary>
	/// Sound effect - Play
	/// </summary>
	public void PlayEffect(string effect)
	{
		AudioClip clip = Resources.Load(string.Format("{0}{1}", Data.path_sound, effect)) as AudioClip;
		AudioManager.Instance.playerEffect1(clip);
	}
}