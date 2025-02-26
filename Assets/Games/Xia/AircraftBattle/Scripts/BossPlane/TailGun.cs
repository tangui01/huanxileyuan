using UnityEngine;
using System.Collections;

public class TailGun : MonoBehaviour {

	public Transform tailGunHit;
	public Transform tailGunBodyHit;
	Animation shotAnimation;
	public Transform damaged;
	int health = 10;

	public BossPlane bossPlane;
	public GameObject wholeParent;
	public GameObject explosion;
	public float laserInterval = 7f;
	public FireLaserEvent laserEvent;
	public float laserInitialTime = 2f;
	bool continuousDamage = false;
	int damage;

	// Use this for initialization
	void Start () 
	{
		shotAnimation = transform.parent.parent.GetComponent<Animation>();
		health = bossPlane.tailGunHealth;
		damage = 100;
		laserEvent.Laser.GetChild(0).GetComponent<EnemyDamage>().damage = damage;
	}

	void FireLaser()
	{
		shotAnimation.Play();
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
			health = 0;
			continuousDamage = false;
			//disable all hits just in case
			tailGunHit.GetComponent<Renderer>().enabled = false;
			tailGunBodyHit.GetComponent<Renderer>().enabled = false;
			
			CancelInvoke("FireLaser");
			if(shotAnimation.isPlaying)
				shotAnimation.Stop();
			laserEvent.dontInvertLaser = false;
			laserEvent.Laser.GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
			laserEvent.Laser.GetComponent<Animation>()["LaserLaunch"].speed = -1;
			if(laserEvent.laserShooting)
				laserEvent.Laser.GetComponent<Animation>().Play();

			Invoke("DisableLaser_JustInCase", 0.75f);
			GetComponent<Collider2D>().enabled = false;
			//col.gameObject.SetActive(false);
			transform.parent.gameObject.SetActive(false);
			damaged.GetComponent<Renderer>().enabled = true;
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
			SoundManager.Instance.Play_BossTurretHit();
			StartCoroutine("Hit");
		}
	}

	void DisableLaser_JustInCase()
	{
		laserEvent.Laser.gameObject.SetActive(false);
	}
//	void OnTriggerEnter2D(Collider2D col)
//	{
//
//		if(col.gameObject.tag.Equals("PlayerBullet"))
//		{
//			if(health<=0)
//			{
//				//disable all hits just in case
//				tailGunHit.renderer.enabled = false;
//				tailGunBodyHit.renderer.enabled = false;
//
//				CancelInvoke("FireLaser");
//				if(shotAnimation.isPlaying)
//					shotAnimation.Stop();
//				laserEvent.dontInvertLaser = false;
//				laserEvent.Laser.animation["LaserLaunch"].normalizedTime = 1;
//				laserEvent.Laser.animation["LaserLaunch"].speed = -1;
//				if(laserEvent.laserShooting)
//					laserEvent.Laser.animation.Play();
//				collider2D.enabled = false;
//				col.gameObject.SetActive(false);
//				transform.parent.gameObject.SetActive(false);
//				damaged.renderer.enabled = true;
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
			col.gameObject.gameObject.SetActive(false);
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
		tailGunHit.GetComponent<Renderer>().enabled = true;
		tailGunBodyHit.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.1f);
		tailGunHit.GetComponent<Renderer>().enabled = false;
		tailGunBodyHit.GetComponent<Renderer>().enabled = false;
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
		InvokeRepeating("FireLaser", laserInitialTime, laserInterval);
	}

}
