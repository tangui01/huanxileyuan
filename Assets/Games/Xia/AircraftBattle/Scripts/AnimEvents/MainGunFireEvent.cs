using UnityEngine;
using System.Collections;

public class MainGunFireEvent : MonoBehaviour {

	public GameObject mainGunWeapon;
	public float speed = 1f;
	[HideInInspector] public bool dontFire = false;
	GameObject gun;
	GameObject gunRegular;
	[HideInInspector] public int numberOfAttacks = 1;
	[HideInInspector] public int numberOfAttacksUnique = 1;
	public GameObject uniqueWeapon;
	[HideInInspector] public int bossLevel;
	int lasers = 0;
	[HideInInspector] public int regularDamage;
	[HideInInspector] public int uniqueDamage;
	bool stopRotate = false;
	int laserRotationAngle = 45;
	[HideInInspector] public bool thirdWaveEnabled = false;

	void Start()
	{
		if(LevelGenerator.currentStage > 9)
		{
			int weaponType = Random.Range(1,8);
			switch(weaponType)
			{
			case 1: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon1_Waves") as GameObject; break;
			case 2: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon2_Rockets") as GameObject; break;
			case 3: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon3_FourLasers") as GameObject; break;
			case 4: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon4_Tesla") as GameObject; break;
			case 5: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon5_Bombs") as GameObject; break;
			case 6: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon6_Blades") as GameObject; break;
			case 7: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon7_FourLasersReversible") as GameObject; break;
			default: uniqueWeapon = Resources.Load("BossWeapon/PlaneUniqueWeapon1_Waves") as GameObject; break;
			}
		}
		Invoke("SetDamage",1f);
	}

	void SetDamage()
	{
		for(int i=0;i<mainGunWeapon.transform.childCount;i++)
		{
			mainGunWeapon.transform.GetChild(i).GetChild(0).GetComponent<EnemyDamage>().damage = regularDamage;
		}
		if(uniqueWeapon.name.Contains("Waves"))
		{
			uniqueDamage = 120 + (LevelGenerator.currentStage-1)*10;
			for(int i=0;i<uniqueWeapon.transform.childCount;i++)
			{
				uniqueWeapon.transform.GetChild(i).GetChild(0).GetComponent<EnemyDamage>().damage = uniqueDamage;
			}
		}
		else if(uniqueWeapon.name.Contains("Rockets"))
		{
			uniqueDamage = 70 + (LevelGenerator.currentStage-1)*10;
			for(int i=0;i<uniqueWeapon.transform.childCount;i++)
			{
				uniqueWeapon.transform.GetChild(i).Find("AnimationHolder/Rocket").GetComponent<EnemyDamage>().damage = uniqueDamage;
			}
		}
		else if(uniqueWeapon.name.Contains("Lasers"))
		{
			uniqueDamage = 20 + (LevelGenerator.currentStage-1)*10;
			for(int i=0;i<uniqueWeapon.transform.childCount;i++)
			{
				uniqueWeapon.transform.GetChild(i).Find("LaserHolder/AnimationHolder/LaserOrange").GetComponent<EnemyDamage>().damage = uniqueDamage;
			}
		}
		else if(uniqueWeapon.name.Contains("Tesla"))
		{
			uniqueDamage = 45 + (LevelGenerator.currentStage-1)*10;
			for(int i=0;i<uniqueWeapon.transform.childCount;i++)
			{
				uniqueWeapon.transform.GetChild(i).GetComponent<EnemyDamage>().damage = uniqueDamage;
			}
		}
		else if(uniqueWeapon.name.Contains("Blades"))
		{
			uniqueDamage = 100 + (LevelGenerator.currentStage-1)*10;
			for(int i=0;i<uniqueWeapon.transform.childCount;i++)
			{
				uniqueWeapon.transform.GetChild(i).Find("AnimationHolder/BladeHolderRotation/EnemyBlades").GetComponent<EnemyDamage>().damage = uniqueDamage;
			}
		}
	}

	public void Fire()
	{
//		for(int i=0;i<mainGunWeapon.childCount;i++)
//		{
//			mainGunWeapon.GetChild(i).GetChild(0).animation.Play();
//		}
		speed = 0.75f;
		if(!dontFire)
		{
//			gun = Instantiate(mainGunWeapon, transform.position - new Vector3(0,-2,2), Quaternion.identity) as GameObject;
//			gun.animation["MainGunBulletTrajectory_Animation"].speed = speed;
//			gun.animation.Play();
//			Invoke("DestroyBullet",3f);
			StartCoroutine(FireAll());
		}
		else if(thirdWaveEnabled)
			dontFire = false;
	}

	IEnumerator FireAll()
	{
		//float angle = 0;
		float angle = Random.Range(0,20);
		for(int i=0;i<numberOfAttacks;i++)
		{
			Quaternion a = Quaternion.Euler(0,0,angle);
			gunRegular = Instantiate(mainGunWeapon, transform.position - new Vector3(0,-2,2), a) as GameObject;
			gunRegular.GetComponent<Animation>()["MainGunBulletTrajectory_Animation"].speed = speed;
			gunRegular.GetComponent<Animation>().Play();
			SoundManager.Instance.Play_BossMainGunFire();
			yield return new WaitForSeconds(0.5f);
			angle+=15;
		}
	}

	IEnumerator FireUnique()
	{
		if(bossLevel == 1)
		{
			if(uniqueWeapon.name.Contains("Waves"))
			{
				SoundManager.Instance.Play_BossUniqueAttack();
				float angle = 290;
				for(int i=0;i<numberOfAttacksUnique;i++)
				{
					//Quaternion a = Quaternion.Euler(0,0,angle);
					gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0, -2, 2),
						Quaternion.identity) as GameObject;
					int value = Random.Range(0,10);
					if(value < 5)
					{
						gun.transform.localScale = new Vector3(-1,1,1);
						for(int j=0;j<gun.transform.childCount-2;j++)
						{
							gun.transform.GetChild(j).localRotation = Quaternion.Euler(0,0,angle);
							angle-=10;
						}
						gun.transform.GetChild(6).localRotation = Quaternion.Euler(0,0,180);
						gun.transform.GetChild(7).localRotation = Quaternion.Euler(0,0,0);
					}
					//"NIJE IZASAO MAIN GUN DOK PUCA"
					//"DA SE VIDI GDE TREBA MINUS DA BI BILO KAKO TREBA"

					//gun.animation["MainGunBulletTrajectory_Animation"].speed = speed;
					gun.GetComponent<Animation>().Play();
					yield return new WaitForSeconds(0.5f);
					//angle+=15;
				}
			}
		}
		else if(bossLevel == 2)
		{
//			if(uniqueWeapon.name.Contains("Rockets"))
//			{
//				for(int i=0;i<numberOfAttacksUnique;i++)
//				{
//					gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,2,2), Quaternion.identity) as GameObject;
//					gun.animation.Play();
//					yield return new WaitForSeconds(0.5f);
//				}
//			}
			if(uniqueWeapon.name.Contains("Rockets"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,2,0.2f), Quaternion.identity) as GameObject;
				AudioSource missileLaunch = gun.GetComponent<AudioSource>();
				for(int i=0;i<gun.transform.childCount;i++)
				{
					if(missileLaunch != null)
					{
						if(missileLaunch.clip != null && SoundManager.soundOn == 1)
							missileLaunch.Play();
					}
					gun.transform.GetChild(i).gameObject.SetActive(true);
					yield return new WaitForSeconds(0.5f);
				}
			}
			else if(uniqueWeapon.name.Contains("Waves"))
			{
				SoundManager.Instance.Play_BossUniqueAttack();
				float angle = 290;
				for(int i=0;i<numberOfAttacksUnique;i++)
				{
					//Quaternion a = Quaternion.Euler(0,0,angle);
					gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,-2,2), Quaternion.identity) as GameObject;
					int value = Random.Range(0,10);
					if(value < 5)
					{
						gun.transform.localScale = new Vector3(-1,1,1);
						for(int j=0;j<gun.transform.childCount-2;j++)
						{
							gun.transform.GetChild(j).localRotation = Quaternion.Euler(0,0,angle);
							angle-=10;
						}
						gun.transform.GetChild(6).localRotation = Quaternion.Euler(0,0,180);
						gun.transform.GetChild(7).localRotation = Quaternion.Euler(0,0,0);
					}
					//"NIJE IZASAO MAIN GUN DOK PUCA"
					//"DA SE VIDI GDE TREBA MINUS DA BI BILO KAKO TREBA"
					
					//gun.animation["MainGunBulletTrajectory_Animation"].speed = speed;
					gun.GetComponent<Animation>().Play();
					yield return new WaitForSeconds(0.5f);
					//angle+=15;
				}
			}
			else if(uniqueWeapon.name.Contains("Tesla"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,1), Quaternion.identity) as GameObject;
				gun.transform.parent = transform;
				AudioSource teslaLaunch = gun.GetComponent<AudioSource>();
				if(teslaLaunch != null)
				{
					if(teslaLaunch.clip != null && SoundManager.soundOn == 1)
						teslaLaunch.Play();
				}
			}
			else if(uniqueWeapon.name.Contains("Bombs"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,1), Quaternion.identity) as GameObject;
				gun.transform.parent = transform.parent.parent;
			}
			else if(uniqueWeapon.name.Contains("Lasers"))
			{
				lasers = 1;
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,2), Quaternion.identity) as GameObject;
				gun.transform.parent = transform;
				AudioSource laserLaunch = gun.GetComponent<AudioSource>();
				if(laserLaunch != null)
				{
					if(laserLaunch.clip != null && SoundManager.soundOn == 1)
						laserLaunch.Play();
				}
//				gun.animation["UniqueFire3_Animation"].speed = Random.Range(-2.5f, 2.6f);
//				if(gun.animation["UniqueFire3_Animation"].speed < 0)
//				{
//					gun.animation["UniqueFire3_Animation"].normalizedTime = 1;
//				}
				//gun.animation.Play();
				stopRotate = false;
				yield return new WaitForSeconds(1f);
				StartCoroutine(RotateLaser());
				yield return new WaitForSeconds(4.5f);
				stopRotate = true;
				if(gun != null && lasers != 2)
				{
					for(int j=0;j<gun.transform.childCount;j++)
					{
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].speed = -1;
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>().Play();
					}
				}
				
				yield return new WaitForSeconds(0.5f);
				lasers = 0;
			}
		}
		else if(bossLevel == 3)
		{
			if(uniqueWeapon.name.Contains("LasersReversible"))
			{
				lasers = 1;
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,2f), Quaternion.identity) as GameObject;
				gun.transform.parent = transform;
				AudioSource laserLaunch = gun.GetComponent<AudioSource>();
				if(laserLaunch != null)
				{
					if(laserLaunch.clip != null && SoundManager.soundOn == 1)
						laserLaunch.Play();
				}
//				gun.animation["UniqueFire3_Animation"].speed = Random.Range(-2.5f, 2.6f);
//				if(gun.animation["UniqueFire3_Animation"].speed < 0)
//				{
//					gun.animation["UniqueFire3_Animation"].normalizedTime = 1;
//				}
				//gun.animation.Play();
				stopRotate = false;
				laserRotationAngle = Random.Range(45,70);
				yield return new WaitForSeconds(1f);
				StartCoroutine(RotateLaser());
				yield return new WaitForSeconds(2.5f);
				laserRotationAngle = Random.Range(45,70);
				laserRotationAngle = -laserRotationAngle;
				yield return new WaitForSeconds(2.5f);
				stopRotate = true;
				if(Random.Range(0,100) >= 50)
					laserRotationAngle = -laserRotationAngle;
				if(gun != null && lasers != 2)
				{
					for(int j=0;j<gun.transform.childCount;j++)
					{
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].speed = -1;
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>().Play();
					}
				}
				
				yield return new WaitForSeconds(0.5f);
				lasers = 0;
			}

			else if(uniqueWeapon.name.Contains("Lasers"))
			{
				lasers = 1;
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,2f), Quaternion.identity) as GameObject;
				gun.transform.parent = transform;
				AudioSource laserLaunch = gun.GetComponent<AudioSource>();
				if(laserLaunch != null)
				{
					if(laserLaunch.clip != null && SoundManager.soundOn == 1)
						laserLaunch.Play();
				}
//				gun.animation["UniqueFire3_Animation"].speed = Random.Range(-2.5f, 2.6f);
//				if(gun.animation["UniqueFire3_Animation"].speed < 0)
//				{
//					gun.animation["UniqueFire3_Animation"].normalizedTime = 1;
//				}
				//gun.animation.Play();
				stopRotate = false;
				yield return new WaitForSeconds(1f);
				StartCoroutine(RotateLaser());
				yield return new WaitForSeconds(4.5f);
				stopRotate = true;
				if(gun != null && lasers != 2)
				{
					for(int j=0;j<gun.transform.childCount;j++)
					{
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].speed = -1;
						gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>().Play();
					}
				}

				yield return new WaitForSeconds(0.5f);
				lasers = 0;
			}



			else if(uniqueWeapon.name.Contains("Waves"))
			{
				SoundManager.Instance.Play_BossUniqueAttack();
				float angle = 290;
				for(int i=0;i<numberOfAttacksUnique;i++)
				{
					//Quaternion a = Quaternion.Euler(0,0,angle);
					gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,-2,2), Quaternion.identity) as GameObject;
					int value = Random.Range(0,10);
					if(value < 5)
					{
						gun.transform.localScale = new Vector3(-1,1,1);
						for(int j=0;j<gun.transform.childCount-2;j++)
						{
							gun.transform.GetChild(j).localRotation = Quaternion.Euler(0,0,angle);
							angle-=10;
						}
						gun.transform.GetChild(6).localRotation = Quaternion.Euler(0,0,180);
						gun.transform.GetChild(7).localRotation = Quaternion.Euler(0,0,0);
					}
					//"NIJE IZASAO MAIN GUN DOK PUCA"
					//"DA SE VIDI GDE TREBA MINUS DA BI BILO KAKO TREBA"
					
					//gun.animation["MainGunBulletTrajectory_Animation"].speed = speed;
					gun.GetComponent<Animation>().Play();
					yield return new WaitForSeconds(0.5f);
					//angle+=15;
				}
			}
			else if(uniqueWeapon.name.Contains("Tesla"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,1), Quaternion.identity) as GameObject;
				gun.transform.parent = transform;
				AudioSource teslaLaunch = gun.GetComponent<AudioSource>();
				if(teslaLaunch != null)
				{
					if(teslaLaunch.clip != null && SoundManager.soundOn == 1)
						teslaLaunch.Play();
				}
			}

			else if(uniqueWeapon.name.Contains("Bombs"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,0,1), Quaternion.identity) as GameObject;
				gun.transform.parent = transform.parent.parent;
			}
			else if(uniqueWeapon.name.Contains("Rockets"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,2,0.2f), Quaternion.identity) as GameObject;
				AudioSource missileLaunch = gun.GetComponent<AudioSource>();
				for(int i=0;i<gun.transform.childCount;i++)
				{
					if(missileLaunch != null)
					{
						if(missileLaunch.clip != null && SoundManager.soundOn == 1)
							missileLaunch.Play();
					}
					gun.transform.GetChild(i).gameObject.SetActive(true);
					yield return new WaitForSeconds(0.5f);
				}
			}
			else if(uniqueWeapon.name.Contains("Blades"))
			{
				gun = Instantiate(uniqueWeapon, transform.position - new Vector3(0,2,2), Quaternion.identity) as GameObject;
				AudioSource bladesLaunch = gun.GetComponent<AudioSource>();
				for(int i=0;i<gun.transform.childCount;i++)
				{

					if(bladesLaunch != null)
					{
						if(bladesLaunch.clip != null && SoundManager.soundOn == 1)
							bladesLaunch.Play();
					}
					gun.transform.GetChild(i).gameObject.SetActive(true);
					gun.transform.GetChild(i).localRotation = Quaternion.Euler(0,0,Random.Range(-45,46));
					yield return new WaitForSeconds(0.5f);
				}
			}
		}

		gun.transform.parent = transform.parent.parent.parent;
	}

	public void TurnOffLasers()
	{
		if(gun != null && lasers == 1)
		{
			lasers = 2;
			for(int j=0;j<gun.transform.childCount;j++)
			{
				gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
				gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>()["LaserLaunch"].speed = -1;
				gun.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Animation>().Play();
			}
		}
	}
	
	public void UniqueFire()
	{
		StartCoroutine(FireUnique());
	}

	IEnumerator RotateLaser()
	{
		while(!stopRotate)
		{
			gun.transform.Rotate(0,0,laserRotationAngle*Time.deltaTime);
			yield return null;
		}
	}

	public void DisableUniqueWeapon()
	{
		StartCoroutine(DisableUnique());
	}

	IEnumerator DisableUnique()
	{
		if(uniqueWeapon.name.Contains("Rockets"))
		{
			for(int i=0;i<gun.transform.childCount;i++)
			{

				gun.transform.GetChild(i).GetChild(0).GetComponent<Animation>().Play("Death");
				yield return new WaitForSeconds(0.05f);
			}
			yield return  new WaitForSeconds(0.5f);
			if(gun!=null)
				gun.gameObject.SetActive(false);
		}
		else
		{
			yield return  new WaitForSeconds(1f);
			if(gun!=null)
				gun.gameObject.SetActive(false);
		}
	}
}

