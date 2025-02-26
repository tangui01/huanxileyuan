using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WGM;
using Random = System.Random;

public class SoundManager : MonoBehaviour {

	public static int musicOn = 1;
	public static int soundOn = 1;
	public static bool forceTurnOff = false;
	
	


	static SoundManager instance;

	public static SoundManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			return instance;
		}
	}
	public List<AudioClip> sounds = new List<AudioClip>();
	public float thisGamevolume = 1.5f;
	private bool isEff2 = true;
	private void Awake()
	{

	}

	void Start () 
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	public void Play_ButtonClick()
	{
		AudioManager.Instance.SetEff(sounds[0],0.5f*thisGamevolume);
	}

	public void Play_UpgradePlane()
	{
		AudioManager.Instance.SetEff(sounds[0],thisGamevolume,1);
	}

	public void Play_DoorClosing()
	{
		AudioManager.Instance.SetEff(sounds[2],0.7f*thisGamevolume);
	}

	public void Play_DoorOpening()
	{
		AudioManager.Instance.SetEff(sounds[3],0.5f*thisGamevolume);
	}

	public void Play_CloudsPassing()
	{
		AudioManager.Instance.SetBgmReserve(sounds[4],0.25f*thisGamevolume,1);
		isEff2 = false;
	}

	public void Stop_CloudsPassing()
	{
		AudioManager.Instance.SetBgmReserve(sounds[4],0.25f*thisGamevolume,0);
		isEff2 = true;
	}

	public void Play_FireBullet()
	{
		AudioManager.Instance.SetEff(sounds[5],0.15f*thisGamevolume,1);
	}

	public void Play_LaunchBlades()
	{
		AudioManager.Instance.SetEff(sounds[6],0.7f*thisGamevolume,1);
	}

	public void Play_LaunchBomb()
	{
		AudioManager.Instance.SetEff(sounds[7],0.7f*thisGamevolume,1);
	}

	public void Play_CollectStar()
	{
		AudioManager.Instance.SetEff(sounds[8],0.25f*thisGamevolume,1);
	}

	public void Play_CollectPowerUp()
	{
		AudioManager.Instance.SetEff(sounds[9],0.35f*thisGamevolume,1);
	}

	public void Play_NoMoreWeapons()
	{
		AudioManager.Instance.SetEff(sounds[10],0.7f*thisGamevolume,1);
	}

	public void Play_BossPlaneArrival()
	{
		AudioManager.Instance.SetEff(sounds[11],0.4f*thisGamevolume,1);
	}

	public void Play_DialogPopupArrival()
	{
		AudioManager.Instance.SetEff(sounds[12],0.7f*thisGamevolume,1);
	}

	public void Play_DialogTextTyping()
	{
		AudioManager.Instance.SetEff(sounds[13],0.5f*thisGamevolume,1);
	}

	public void Play_ShipExplode()
	{
		if (isEff2)
			AudioManager.Instance.SetEff(sounds[14],0.45f*thisGamevolume,2);
		else
			AudioManager.Instance.SetEff(sounds[14],0.45f*thisGamevolume,1);
	}

	public void Play_TankExplode()
	{
		AudioManager.Instance.SetEff(sounds[15],0.6f*thisGamevolume,0);
	}

	public void Play_DialogTextTypingBoss()
	{
		AudioManager.Instance.SetEff(sounds[16],0.85f*thisGamevolume,1);
	}

	public void Play_BossPlaneMovement()
	{
		AudioManager.Instance.SetBgmReserve(sounds[17],0.2f*thisGamevolume,1);
		isEff2 = false;
	}

	public void Stop_BossPlaneMovement()
	{
		AudioManager.Instance.SetBgmReserve(sounds[17],0.2f*thisGamevolume,0);
		isEff2 = true;
	}

	public void Play_EnemyPlaneExplode()
	{
		AudioManager.Instance.SetEff(sounds[18],0.5f*thisGamevolume,UnityEngine.Random.Range(0, 1));
	}

	public void Play_TurretExplode()
	{
		AudioManager.Instance.SetEff(sounds[19],0.4f*thisGamevolume,3);
	}

	public void Play_LaunchLaser()
	{
		AudioManager.Instance.SetEff(sounds[20],0.4f*thisGamevolume,1);
	}

	public void Play_LaunchTesla()
	{
		AudioManager.Instance.SetEff(sounds[21],0.4f*thisGamevolume,1);
	}

	public void Play_NotEnoughStars()
	{
		AudioManager.Instance.SetEff(sounds[22],0.7f*thisGamevolume,0);
	}

	public void Play_BossUniqueAttack()
	{
		AudioManager.Instance.SetEff(sounds[23],0.35f*thisGamevolume,0);
	}

	public void Play_LogoGlow()
	{
		AudioManager.Instance.SetEff(sounds[24],0.25f*thisGamevolume,0);
	}

	public void Play_PlayerHit()
	{
		AudioManager.Instance.SetEff(sounds[25],0.6f*thisGamevolume,0);
	}

	public void Play_EnemyHit()
	{
		AudioManager.Instance.SetEff(sounds[26],0.25f*thisGamevolume,3);
	}

	public void Play_BossTankMovement()
	{
		AudioManager.Instance.SetBgmReserve(sounds[27],0.25f*thisGamevolume,1);
		isEff2 = false;
	}

	public void Stop_BossTankMovement()
	{
		AudioManager.Instance.SetBgmReserve(sounds[27],0.25f*thisGamevolume,0);
		isEff2 = true;
	}

	public void Play_MeteorMovement()
	{
		AudioManager.Instance.SetEff(sounds[28],0.4f*thisGamevolume,1);
	}

	public void Play_BossShipMovement()
	{
		AudioManager.Instance.SetBgmReserve(sounds[29],0.2f*thisGamevolume,1);
		isEff2 = false;
	}

	public void Stop_BossShipMovement()
	{
		AudioManager.Instance.SetBgmReserve(sounds[29],0.2f*thisGamevolume,0);
		isEff2 = true;
	}

	public void Play_EnemyFire()
	{
		AudioManager.Instance.SetEff(sounds[30],0.15f*thisGamevolume,1);
	}

	public void Play_BossMainGunFire()
	{
		AudioManager.Instance.SetEff(sounds[31],0.2f*thisGamevolume,1);
	}

	public void Play_MissileLaunch()
	{
		AudioManager.Instance.SetEff(sounds[32],0.6f*thisGamevolume,1);
	}

	public void Play_MenuMusic()
	{
		AudioManager.Instance.SetBgmReserve(sounds[33],0.45f*thisGamevolume,1);
		isEff2 = false;
	}

	public void Stop_MenuMusic()
	{
		AudioManager.Instance.SetBgmReserve(sounds[33],0.45f*thisGamevolume,0);
		isEff2 = true;
	}

	public void Play_GameplayMusic()
	{
		AudioManager.Instance.SetBGmVolume(0.35f*thisGamevolume * LibWGM.machine.BgmVolume/10);
		AudioManager.Instance.playerBGm(sounds[34]);
	}

	public void Stop_GameplayMusic()
	{ 
		StartCoroutine(FadeOut(0.005f));
	}

	IEnumerator FadeOut(float time)
	{
		float originalVolume = 0.35f*thisGamevolume;
		while(originalVolume != 0)
		{
			originalVolume = Mathf.MoveTowards(0.35f*thisGamevolume, 0, time);
			AudioManager.Instance.SetBGmVolume(originalVolume*thisGamevolume * LibWGM.machine.BgmVolume/10);
			yield return null;
		}
		AudioManager.Instance.StopBGm();
		AudioManager.Instance.SetBGmVolume(0.35f*thisGamevolume * LibWGM.machine.BgmVolume/10);
	}

	public void Play_BossMusic()
	{
		AudioManager.Instance.SetBGmVolume(0.45f*thisGamevolume * LibWGM.machine.BgmVolume/10);
		AudioManager.Instance.playerBGm(sounds[35]);
	}

	public void Stop_BossMusic()
	{
		AudioManager.Instance.StopBGm();
	}

	public void Play_StageClear()
	{
		AudioManager.Instance.SetEff(sounds[36],0.45f*thisGamevolume,1);
	}

	public void Play_GameOver()
	{
		AudioManager.Instance.SetEff(sounds[37],0.5f*thisGamevolume,1);
	}

	public void Play_ActivateWeapon()
	{
		AudioManager.Instance.SetEff(sounds[38],0.5f*thisGamevolume,1);
	}

	public void Play_BossTime()
	{
		AudioManager.Instance.SetEff(sounds[39],0.75f*thisGamevolume,1);
	}

	public void Play_EnemyLaser()
	{
		AudioManager.Instance.SetEff(sounds[40],0.7f*thisGamevolume,0);
	}

	public void Play_PlaneResurrect()
	{
		AudioManager.Instance.SetEff(sounds[41],0.9f*thisGamevolume,0);
	}

	public void Play_BossTurretHit()
	{
		AudioManager.Instance.SetEff(sounds[42],0.35f*thisGamevolume,0);
	}

	public void Play_BossTurretExplosion()
	{
		AudioManager.Instance.SetEff(sounds[43],0.5f*thisGamevolume,3);
	}

	public void Play_PandaPlaneExplode()
	{
		AudioManager.Instance.SetEff(sounds[44],0.5f*thisGamevolume,1);
	}

	public void Play_BossExplosion()
	{
		AudioManager.Instance.SetBGmVolume(1f*thisGamevolume * LibWGM.machine.BgmVolume/10);
		AudioManager.Instance.playerBGm(sounds[45]);
	}

	public void Stop_BossExplosion()
	{
		StartCoroutine(FadeOut(0.0035f));
	}

	public void Play_GoblinLaugh()
	{
		AudioManager.Instance.SetEff(sounds[46],thisGamevolume,1);
	}

	public void Play_BombCountdown()
	{
		AudioManager.Instance.SetEff(sounds[47],thisGamevolume,1);
	}

	public void Play_BombLaunchMissiles()
	{
		AudioManager.Instance.SetEff(sounds[48],thisGamevolume,1);
	}

	public void Play_BombBroken()
	{
		AudioManager.Instance.SetBgmReserve(sounds[49],1f,1);
		isEff2 = false;
	}

	public void Stop_BombBroken()
	{
		AudioManager.Instance.SetBgmReserve(sounds[49],1f,0);
		isEff2 = true;
	}

	public void Play_GoblinTesla()
	{
		AudioManager.Instance.SetBgmReserve(sounds[50],1f,1);
		isEff2 = false;
	}

	public void Stop_GoblinTesla()
	{
		AudioManager.Instance.SetBgmReserve(sounds[50],1f,0);
		isEff2 = true;
	}

	public void Play_GoblinMaceSwinging()
	{
		AudioManager.Instance.SetBgmReserve(sounds[51],1f,1);
		isEff2 = false;
	}

	public void Stop_GoblinMaceSwinging()
	{
		AudioManager.Instance.SetBgmReserve(sounds[51],1f,0);
		isEff2 = true;
	}

	public void Play_GoblinMaceHit()
	{
		AudioManager.Instance.SetEff(sounds[52],thisGamevolume,1);
	}

	public void Play_HelicopterMoving()
	{
		AudioManager.Instance.SetBgmReserve(sounds[53],0.35f,1);
		isEff2 = false;
	}

	public void Stop_HelicopterMoving()
	{
		AudioManager.Instance.SetBgmReserve(sounds[53],0.35f,0);
		isEff2 = true;
	}

	public void Play_BossUniqueBlades()
	{
		AudioManager.Instance.SetEff(sounds[54],thisGamevolume,1);
	}
	
}
