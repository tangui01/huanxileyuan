using UnityEngine;
using System.Collections;

public class CannonTank : MonoBehaviour {

	public Transform baseHit;
	public Transform pipe1Hit;
	public Transform pipe2Hit;
	public Transform pipe3Hit;
	Animation shotAnimation;
	public Animation doorAnimation;
	public Transform damaged;
	int health = 10;
	
	public BossTank bossTank;
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
		shotAnimation = transform.parent.parent.parent.GetComponent<Animation>();
		health = bossTank.cannonHealth;
		
		damage = 100;
		laserEvent.Laser.GetChild(0).GetComponent<EnemyDamage>().damage = damage;
		Invoke("LaserArrival",11.5f);
	}

	void Update()
	{
		TargetAndFire();
	}

	void TargetAndFire()
	{
		float speedOfRotation = Time.deltaTime*200;
		Vector3 targetPos = PlaneManager.Instance.transform.position;
		Vector3 diffPos = targetPos-transform.position;
		float angle = -Mathf.Atan2(diffPos.y, diffPos.x) * Mathf.Rad2Deg;
		angle -=90;
		angle = Mathf.Clamp(angle,-60,60);
		Quaternion rot = Quaternion.AngleAxis(angle,  Vector3.back);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rot,speedOfRotation*0.12f);//*100
	}

	void TurnOnPipeColliders()
	{
		pipe1Hit.parent.GetComponent<Collider2D>().enabled = true;
		pipe2Hit.parent.GetComponent<Collider2D>().enabled = true;
		pipe3Hit.parent.GetComponent<Collider2D>().enabled = true;
	}
	
	void FireLaser()
	{
		shotAnimation.Play("MainCanonFire");
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
			bossTank.health-=health;
			health-=damage;
			
		}
		else
		{
			health-=damage;
			bossTank.health-=damage;
		}
		bossTank.SetHealthBar();
		
		if(health<=0)
		{
			SoundManager.Instance.Play_BossTurretExplosion();
			BossStars.Instance.spawnPosition = transform.position;
			BossStars.Instance.GenerateCoins(10,21);
			//disable all hits just in case
			continuousDamage = false;
			baseHit.GetComponent<Renderer>().enabled = false;
			pipe1Hit.GetComponent<Renderer>().enabled = false;
			pipe2Hit.GetComponent<Renderer>().enabled = false;
			pipe3Hit.GetComponent<Renderer>().enabled = false;
			
			CancelInvoke("FireLaser");
			if(shotAnimation.isPlaying)
				shotAnimation.Stop();
			//@@@@@ DA SE VRATI AKO OCE LASER
			laserEvent.dontInvertLaser = false;
			laserEvent.Laser.GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
			laserEvent.Laser.GetComponent<Animation>()["LaserLaunch"].speed = -1;
			if(laserEvent.laserShooting)
				laserEvent.Laser.GetComponent<Animation>().Play();

			Invoke("DisableLaser_JustInCase", 0.75f);
			//@@@@@
			GetComponent<Collider2D>().enabled = false;
			//col.gameObject.SetActive(false);
			transform.parent.gameObject.SetActive(false);
			//damaged.renderer.enabled = true;
			damaged.gameObject.SetActive(true);
			explosion.SetActive(true);
			
			bossTank.currentWave--;
			if(bossTank.currentWave == 0)
			{
				bossTank.EnableThirdWave();
			}
		}
		else
		{
			SoundManager.Instance.Play_BossTurretHit();
			//health-=damage;
			StartCoroutine("Hit");
		}
	}

	void DisableLaser_JustInCase()
	{
		laserEvent.Laser.gameObject.SetActive(false);
	}
	
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
		//col.gameObject.SetActive(false);
		//StopCoroutine("Hit");
		//StartCoroutine("Hit");
		
		//if(col.gameObject.tag.Equals("PlayerBullet"))
		//		{
		//			if(health<=0)
		//			{
		//				//disable all hits just in case
		//				continuousDamage = false;
		//				tailGunHit.renderer.enabled = false;
		//				tailGunBodyHit.renderer.enabled = false;
		//				tailGunRootHit.renderer.enabled = false;
		//				engineHit.renderer.enabled = false;
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
		//				//damaged.renderer.enabled = true;
		//				damaged.gameObject.SetActive(true);
		//				explosion.SetActive(true);
		//				
		//				BossShip.currentWave--;
		//				if(BossShip.currentWave == 0)
		//				{
		//					BossShip.EnableSecondWave();
		//				}
		//			}
		//			else
		//			{
		//				if(col.gameObject.tag.Equals("PlayerBullet"))
		//				{
		//					health--;
		//					col.gameObject.SetActive(false);
		//				}
		//				else if(col.gameObject.tag.Equals("Laser"))
		//				{
		//					health-=20;
		//				}
		//				else if(col.gameObject.tag.Equals("Tesla"))
		//				{
		//					health-=10;
		//				}
		//				else if(col.gameObject.tag.Equals("Blades"))
		//				{
		//					health-=5;
		//				}
		//				else if(col.gameObject.tag.Equals("Bomb"))
		//				{
		//					health-=50;
		//				}
		//				col.gameObject.SetActive(false);
		//				//StopCoroutine("Hit");
		//				StartCoroutine("Hit");
		//			}
		//		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(continuousDamage)
			continuousDamage = false;
	}
	
	IEnumerator Hit()
	{
		baseHit.GetComponent<Renderer>().enabled = true;
		pipe1Hit.GetComponent<Renderer>().enabled = true;
		pipe2Hit.GetComponent<Renderer>().enabled = true;
		pipe3Hit.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.1f);
		baseHit.GetComponent<Renderer>().enabled = false;
		pipe1Hit.GetComponent<Renderer>().enabled = false;
		pipe2Hit.GetComponent<Renderer>().enabled = false;
		pipe3Hit.GetComponent<Renderer>().enabled = false;
	}
	
	void HideTurret()
	{
		wholeParent.SetActive(false);
	}
	
	void ShowTurret()
	{
		wholeParent.SetActive(true);
	}
	
	void LaserArrival()
	{
		doorAnimation.Play();
		InvokeRepeating("FireLaser", laserInitialTime, laserInterval);
	}
	
	void FireAway()
	{
		//Invoke("LaserArrival",2f);
		TurnOnPipeColliders();
	}
}
