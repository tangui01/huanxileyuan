using UnityEngine;
using System.Collections;

public class SmallWingTurret : MonoBehaviour {

	public Transform turretHit;
	public Transform wingHit;
	public Transform normalWing;
	public Transform damagedTurret;
	public Transform damagedWing;
	public Transform turretGun;

	public BossPlane bossPlane;
	public GameObject wholeParent;
	public GameObject explosion;
	public Transform bulletPool;

	int health = 10;
	float fireRate = 0.4f;
	float fireRateCounter = 0;
	int bulletIndex = 0;
	int burstCount = 0;
	int burstLimit = 5;
	bool alreadyInvokedReset = false;
	float burstDelay = 3.5f;
	bool continuousDamage = false;
	int damage;

	void Start () 
	{
		if(bossPlane.bossLevel == 1)
		{
			health = bossPlane.smallWingTurretHealth;
			fireRate = 0.4f;
			SmallWingTurretBullet.speed = 5;
			burstLimit = 5;
			burstDelay = 3.5f;
		}
		else if(bossPlane.bossLevel == 2)
		{
			health = bossPlane.smallWingTurretHealth;
			fireRate = 0.2f;
			SmallWingTurretBullet.speed = 10;
			burstLimit = 5;
			burstDelay = 2.5f;
		}
		else if(bossPlane.bossLevel == 3)
		{
			health = bossPlane.smallWingTurretHealth;
			fireRate = 0.2f;
			SmallWingTurretBullet.speed = 10;
			burstLimit = 10;
			burstDelay = 2.5f;
		}
		//bulletPool.parent = bossPlane.transform;
		damage = 20 + 10*(LevelGenerator.currentStage-1);

	}

	void Update()
	{
//		if(fireRateCounter >= fireRate)
//		{
//			fireRateCounter = 0;
//			if(burstCount < burstLimit)
//			{
//				FireBullet();
//				burstCount++;
//			}
//			else
//			{
//				if(!alreadyInvokedReset)
//				{
//					Invoke("ResetBurstCount",burstDelay);
//					alreadyInvokedReset = true;
//				}
//			}
//		}
//		fireRateCounter+=Time.deltaTime;
		TargetAndFire();
	}

	IEnumerator DoContinuousDamage(int damage, GameObject obj)
	{
		while(continuousDamage)
		{
			if(obj != null && obj.activeSelf)
			{
				TakeDamage(damage);
				yield return new WaitForSeconds(0.1f);
			}
			else
				continuousDamage = false;
		}
	}

	void TakeDamage(int damage)
	{
		if(damage >= health)
		{
			bossPlane.health-=health;
			health-=damage;

		}
		else
		{
			health-=damage;
			bossPlane.health-=damage;
		}
		bossPlane.SetHealthBar();

		if(health<=0)
		{
			SoundManager.Instance.Play_BossTurretExplosion();
			BossStars.Instance.spawnPosition = transform.position;
			BossStars.Instance.GenerateCoins(10,21);
			health=0;
			continuousDamage = false;
			//disable all hits just in case
			turretHit.GetComponent<Renderer>().enabled = false;
			wingHit.GetComponent<Renderer>().enabled = false;
			
			CancelInvoke("Fire");
			GetComponent<Collider2D>().enabled = false;
			//col.gameObject.SetActive(false);
			transform.parent.gameObject.SetActive(false);
			normalWing.GetComponent<Renderer>().enabled = false;
			damagedTurret.GetComponent<Renderer>().enabled = true;
			damagedWing.GetComponent<Renderer>().enabled = true;
			explosion.SetActive(true);
			
			bossPlane.currentWave--;
			if(bossPlane.currentWave == 0)
			{
				bossPlane.EnableSecondWave();
			}
		}
		else
		{
			//health-=damage;
			//bossPlane.health-=damage;
			//bossPlane.SetHealthBar();
			SoundManager.Instance.Play_BossTurretHit();
			StartCoroutine("Hit");
		}
	}

	void Fire()
	{
		//if(fireRateCounter >= fireRate)
		{
			fireRateCounter = 0;
			if(burstCount < burstLimit)
			{
				FireBullet();
				burstCount++;
			}
			else
			{
				if(!alreadyInvokedReset)
				{
					Invoke("ResetBurstCount",burstDelay);
					alreadyInvokedReset = true;
				}
			}
		}
		fireRateCounter+=Time.deltaTime;
		//TargetAndFire();
	}

	void TargetAndFire()
	{
		float speedOfRotation = Time.deltaTime*200;
		Vector3 targetPos = PlaneManager.Instance.transform.position;
		Vector3 diffPos = targetPos-turretGun.position;
		float angle = -Mathf.Atan2(diffPos.y, diffPos.x) * Mathf.Rad2Deg;
		angle -=90;
		Quaternion rot = Quaternion.AngleAxis(angle,  Vector3.back);
		turretGun.rotation = Quaternion.RotateTowards(turretGun.rotation, rot,speedOfRotation*100);
	}

	void ResetBurstCount()
	{
		if(burstCount >= burstLimit)
		{
			burstCount = 0;
			alreadyInvokedReset = false;
		}
	}

	void FireBullet()
	{
//		for(int i=0;i<bulletPool.childCount;i++)
//		{
//
//		}
		//"DA SE IZBACI KINEMATIC RIGIDBODY I DA SE NAPRAVI BALANS IZMEDJU OBJEKATA U POOLU I INSTANCIRANJA!!!!!"

		if(bulletIndex == bulletPool.childCount)
			bulletIndex = 0;

		GameObject bullllet = Instantiate(bulletPool.GetChild(bulletIndex).gameObject) as GameObject;
		
		//SmallWingTurretBullet tempScript = bulletPool.GetChild(bulletIndex).GetComponent<SmallWingTurretBullet>();
		SmallWingTurretBullet tempScript = bullllet.GetComponent<SmallWingTurretBullet>();
		bullllet.GetComponent<EnemyDamage>().damage = damage;

		if(tempScript.available)
		{
			//tempScript.initialized = true;
			tempScript.damage = damage;
			tempScript.available = false;
			tempScript.transform.position = turretGun.position;
			//tempScript.direction = turretGun.position - PlaneManager.Instance.transform.position;
			tempScript.FireBullet();
			//break;
		}
		//bulletIndex++;
	}

//	void OnTriggerEnter2D(Collider2D col)
//	{
//		if(col.gameObject.tag.Equals("PlayerBullet"))
//		{
//			if(health<=0)
//			{
//				//disable all hits just in case
//				turretHit.renderer.enabled = false;
//				wingHit.renderer.enabled = false;
//
//				CancelInvoke("Fire");
//				collider2D.enabled = false;
//				col.gameObject.SetActive(false);
//				transform.parent.gameObject.SetActive(false);
//				normalWing.renderer.enabled = false;
//				damagedTurret.renderer.enabled = true;
//				damagedWing.renderer.enabled = true;
//				explosion.SetActive(true);
//
//				bossPlane.currentWave--;
//				if(bossPlane.currentWave == 0)
//				{
//					bossPlane.EnableSecondWave();
//				}
//			}
//			else
//			{
//				health--;
//				col.gameObject.SetActive(false);
//				//StopCoroutine("Hit");
//				StartCoroutine("Hit");
//			}
//		}
//	}

	void OnTriggerEnter2D(Collider2D col)
	{
		
		if(col.tag.Equals("PlayerBullet"))
		{
			//health--;
			col.gameObject.SetActive(false);
			TakeDamage(PandaPlane.Instance.mainGunDamage);
		}
		else if(col.tag.Equals("WingBullet"))
		{
			col.gameObject.SetActive(false);
			TakeDamage(PandaPlane.Instance.wingGunDamage);
		}
		else if(col.tag.Equals("SideBullet"))
		{
			col.gameObject.SetActive(false);
			TakeDamage(PandaPlane.Instance.sideGunDamage);
		}
		else if(col.tag.Equals("Laser"))
		{
			//health-=20;
			continuousDamage = true;
			StartCoroutine(DoContinuousDamage(PandaPlane.Instance.laserDamage, col.gameObject));
		}
		else if(col.tag.Equals("Tesla"))
		{
			//health-=10;
			continuousDamage = true;
			StartCoroutine(DoContinuousDamage(PandaPlane.Instance.teslaDamage, col.gameObject));
		}
		else if(col.tag.Equals("Blades"))
		{
			//health-=5;
			TakeDamage(PandaPlane.Instance.bladesDamage);
		}
		else if(col.tag.Equals("Bomb"))
		{
			//health-=50;
			TakeDamage(PandaPlane.Instance.bombDamage);
		}
		
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(continuousDamage)
			continuousDamage = false;
	}
	
	IEnumerator Hit()
	{
		turretHit.GetComponent<Renderer>().enabled = true;
		wingHit.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.1f);
		turretHit.GetComponent<Renderer>().enabled = false;
		wingHit.GetComponent<Renderer>().enabled = false;
	}

	void HideTurret()
	{
		wholeParent.SetActive(false);
	}
	
	void ShowTurret()
	{
		wholeParent.SetActive(true);
	}

	void FireAway()
	{
		InvokeRepeating("Fire",1,fireRate);
	}

}
