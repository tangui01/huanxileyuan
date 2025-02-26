using UnityEngine;
using System.Collections;

public class LogoGlow : MonoBehaviour {

	void Glow()
	{
		SoundManager.Instance.Play_LogoGlow();
		if(SoundManager.soundOn == 1)
		{
			SoundManager.forceTurnOff = true;
			SoundManager.soundOn = 0;
		}
	}

	void Meteor()
	{
		SoundManager.Instance.Play_MeteorMovement();
	}

	void OpenDoor()
	{
		if(SoundManager.forceTurnOff)
			SoundManager.soundOn = 1;
		SoundManager.Instance.Play_DoorOpening();
	}

	void Clouds()
	{
		if(!Application.loadedLevelName.Equals("MainScene") && LevelGenerator.checkpoint)
			SoundManager.Instance.Play_CloudsPassing();
		else if(!PlayerPrefs.HasKey("TutorialCompleted"))
		{
			SoundManager.Instance.Play_CloudsPassing();
		}
	}

	void MissileLaunch()
	{
		SoundManager.Instance.Play_MissileLaunch();
	}

	IEnumerator EndComic()
	{
		SoundManager_ComicScene.Instance.Stop_ComicMusic();
		Camera.main.GetComponent<Collider>().enabled = false;
		Camera.main.transform.Find("SkipText").GetComponent<Collider>().enabled = false;
		Camera.main.transform.Find("BlackBackground").GetComponent<Animation>().Play();
		yield return new WaitForSeconds(1f);
		Application.LoadLevel("MainScene");
	}

	void DestroyEnemy()
	{
		GameObject.Destroy(this.gameObject);
	}

	void DestroyEnemyKamikaze()
	{
		GameObject.Destroy(this.transform.parent.gameObject);
	}

	void DeactivateKamikaza()
	{
		this.transform.parent.gameObject.GetComponent<Homming>().enabled=false;
	}

	void MineExplode()
	{
		//SoundManager.Instance.Play_BombLaunchMissiles();
		this.gameObject.GetComponent<Animation>().Play("Death");
	}

	void StartComicMusic()
	{
		SoundManager_ComicScene.Instance.Play_ComicMusic();
	}

	void PlayBeepSound()
	{
		SoundManager_ComicScene.Instance.Play_BeepSound();
	}

	void PlaySwoosh1()
	{
		SoundManager_ComicScene.Instance.Play_Swoosh1();
	}

	void PlaySwoosh2()
	{
		SoundManager_ComicScene.Instance.Play_Swoosh2();
	}

	void PlaySwoosh3()
	{
		SoundManager_ComicScene.Instance.Play_Swoosh3();
	}

	void PlaySwoosh4()
	{
		SoundManager_ComicScene.Instance.Play_Swoosh4();
	}

	void PlayClickToAnswer()
	{
		SoundManager_ComicScene.Instance.Play_ClickToAnswer();
	}

	void JetPackDeath()
	{
		//SoundManager.Instance.Stop_GoblinTesla();
		AudioSource goblinTesla = transform.parent.parent.parent.GetComponent<AudioSource>();
		if(goblinTesla != null)
		{
			if(goblinTesla.clip != null && SoundManager.soundOn == 1)
			{
				goblinTesla.Stop();
			}
		}
		PandaPlane.Instance.continuousDamage = false;
		if(transform.GetChild(0).childCount > 4)
			transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
	}

	void HommingParent()//f-ja za JetPack
	{
		if(transform.GetChild(0).GetChild(0).GetChild(0).gameObject.activeSelf)
		{
			transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true);
			transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).parent = null;
		}
	}

	void HommingParentHelicopter()
	{
		if(transform.parent.gameObject.activeSelf)
		{
			GameObject test = transform.GetChild(0).Find("HommingMissile").gameObject;
			test.SetActive(true);
			test.transform.parent=null;
			test.transform.GetChild(0).localRotation=Quaternion.Euler(Vector3.zero);
			test.transform.localRotation=Quaternion.Euler(Vector3.zero);
//			transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
//			transform.GetChild(0).GetChild(3).parent = null;
//			GameObject.Find("HommingMissile").transform.localRotation=Quaternion.Euler(Vector3.zero);  
		}
		
	}

	void LongBulletParent()
	{
		if(transform.GetChild(0).gameObject.activeSelf)
		{
			transform.GetChild(0).GetChild(3).parent = null;
		}
		
	}

	void GoblinLaugh()
	{
		SoundManager.Instance.Play_GoblinLaugh();
	}
	void BombCountdown()
	{
		SoundManager.Instance.Play_BombCountdown();
	}
	void BombLaunchMissiles()
	{
		SoundManager.Instance.Play_BombLaunchMissiles();
	}
	void BombBroken()
	{
		//SoundManager.Instance.Play_BombBroken();
		AudioSource bombBroken = GetComponent<AudioSource>();
		if(bombBroken.clip != null && SoundManager.soundOn == 1 && !bombBroken.isPlaying)
			bombBroken.Play();
	}
	void GoblinTesla()
	{
		//if(transform.Find("EnemyJetpack/AnimationHolder/JetPackHip").gameObject.activeSelf)
			//SoundManager.Instance.Play_GoblinTesla();
		AudioSource goblinTesla = transform.GetComponent<AudioSource>();
		if(goblinTesla.clip != null && SoundManager.soundOn == 1 && !goblinTesla.isPlaying)
			goblinTesla.Play();
	}
	void GoblinJetpackTesla()
	{
		if(transform.Find("EnemyJetpack/AnimationHolder/JetPackHip").gameObject.activeSelf)
		{
			AudioSource goblinTesla = transform.parent.GetComponent<AudioSource>();
			if(goblinTesla != null)
			{
				if(goblinTesla.clip != null && SoundManager.soundOn == 1)
				{
					goblinTesla.Play();
				}
			}
		}
			//SoundManager.Instance.Play_GoblinTesla();
	}
	void StopGoblinTesla()
	{
		//SoundManager.Instance.Stop_GoblinTesla();
		AudioSource goblinTesla = transform.parent.GetComponent<AudioSource>();
		if(goblinTesla != null)
		{
			if(goblinTesla.clip != null && SoundManager.soundOn == 1)
			{
				goblinTesla.Stop();
			}
		}
	}

	void StopGoblinTesla_LeftScene()
	{
		//SoundManager.Instance.Stop_GoblinTesla();
		AudioSource goblinTesla = transform.GetComponent<AudioSource>();
		if(goblinTesla != null)
		{
			if(goblinTesla.clip != null && SoundManager.soundOn == 1)
				goblinTesla.Stop();
		}
	}

	void GoblinMaceSwinging()
	{
		//SoundManager.Instance.Play_GoblinMaceSwinging();
		AudioSource goblinMaceSwinging = transform.parent.GetComponent<AudioSource>();
		if(goblinMaceSwinging.clip != null && SoundManager.soundOn == 1 && !goblinMaceSwinging.isPlaying)
			goblinMaceSwinging.Play();
	}
	void StopGoblinMaceSwinging()
	{
		//SoundManager.Instance.Stop_GoblinMaceSwinging();
		AudioSource goblinMaceSwinging = transform.parent.GetComponent<AudioSource>();
		if(goblinMaceSwinging.clip != null && SoundManager.soundOn == 1)
			goblinMaceSwinging.Stop();
	}
	void StopGoblinMaceSwinging_LeftScene()
	{
		//SoundManager.Instance.Stop_GoblinMaceSwinging();
		AudioSource goblinMaceSwinging = transform.GetChild(0).GetComponent<AudioSource>();
		if(goblinMaceSwinging.clip != null && SoundManager.soundOn == 1)
			goblinMaceSwinging.Stop();
	}
	void GoblinMaceHit()
	{
		//SoundManager.Instance.Play_GoblinMaceHit();
		AudioSource goblinMaceHit = transform.GetComponent<AudioSource>();
		if(goblinMaceHit.clip != null && SoundManager.soundOn == 1 && !goblinMaceHit.isPlaying)
		{
			goblinMaceHit.Play();
		}
	}
	void EnemyExplode()
	{
		SoundManager.Instance.Play_EnemyPlaneExplode();
	}
	void StopBombBroken()
	{
		//SoundManager.Instance.Stop_BombBroken();
		AudioSource bombBroken = GetComponent<AudioSource>();
		if(bombBroken != null)
		{
			if(bombBroken.clip != null && SoundManager.soundOn == 1)
				bombBroken.Stop();
		}
	}
	void BombMissileLaunch()
	{
		SoundManager.Instance.Play_BombLaunchMissiles();
	}
}
