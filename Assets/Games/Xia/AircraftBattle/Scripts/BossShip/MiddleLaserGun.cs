using UnityEngine;
using System.Collections;

public class MiddleLaserGun : MonoBehaviour {

	public Transform EngineHit;
	public Transform LaserGunRootHit;
	public Transform LaserGunHit;
	public Transform LaserGunBodyHit;
	public Transform wingHit;
	public Animation LaserGunFire;
	public Transform EngineNormal;
	public Transform LaserGunRootNormal;
	public Transform LaserGunNormal;
	public Transform LaserGunBodyNormal;
	public Transform wingNormal;
	public Transform LaserGunEngineDamaged;
	public Transform wingDamaged;
	
	public BossShip bossShip;
	public GameObject wholeParent;
	public GameObject explosion;
	//public Transform otherLaserGunEngineNormal;
	//public Transform otherLaserGunEngineDamaged;
	
	public bool leftWing;
	
	int health = 20;
	public float laserInterval = 7;
	public FireLaserEvent laserEvent;
	public float laserInitialTime = 2;
	bool continuousDamage = false;
	int damage;
	
	void Start () 
	{
		if(wholeParent.name.Contains("Small"))
			health = bossShip.middleLaserGunHealth;
		else
			health = bossShip.middleLaserGunHealth;

		damage = 100;
		laserEvent.Laser.GetChild(0).GetComponent<EnemyDamage>().damage = damage;
	}
	
	void FireLaser()
	{
		LaserGunFire.Play();
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
			bossShip.health-=health;
			health-=damage;
			
		}
		else
		{
			health-=damage;
			bossShip.health-=damage;
		}
		bossShip.SetHealthBar();

		if(health<=0)
		{
			SoundManager.Instance.Play_BossTurretExplosion();
			BossStars.Instance.spawnPosition = transform.position;
			BossStars.Instance.GenerateCoins(10,21);
			continuousDamage = false;
			//disable all hits just in case
			EngineHit.GetComponent<Renderer>().enabled = false;
			LaserGunRootHit.GetComponent<Renderer>().enabled = false;
			LaserGunHit.GetComponent<Renderer>().enabled = false;
			LaserGunBodyHit.GetComponent<Renderer>().enabled = false;
			wingHit.GetComponent<Renderer>().enabled = false;
			
			CancelInvoke("FireLaser");
			if(LaserGunFire.isPlaying)
				LaserGunFire.Stop();
			laserEvent.dontInvertLaser = false;
			laserEvent.Laser.GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
			laserEvent.Laser.GetComponent<Animation>()["LaserLaunch"].speed = -1;
			if(laserEvent.laserShooting)
				laserEvent.Laser.GetComponent<Animation>().Play();

			Invoke("DisableLaser_JustInCase", 0.75f);
			GetComponent<Collider2D>().enabled = false;
			//col.gameObject.SetActive(false);
			transform.parent.gameObject.SetActive(false);
//				EngineNormal.renderer.enabled = false;
//				LaserGunRootNormal.renderer.enabled = false;
//				LaserGunNormal.renderer.enabled = false;
//				LaserGunBodyNormal.renderer.enabled = false;
//				LaserGunEngineDamaged.renderer.enabled = true;
			explosion.SetActive(true);
			
			if(leftWing)
				bossShip.leftWingCount--;
			else
				bossShip.rightWingCount--;
			
			
			if(leftWing)
			{
				if(bossShip.leftWingCount == 0 && wingNormal.gameObject.activeSelf)
				{
					//wingNormal.renderer.enabled = false;
					//wingDamaged.renderer.enabled = true;
					wingNormal.gameObject.SetActive(false);
					wingDamaged.gameObject.SetActive(true);
					
//						if(otherLaserGunEngineNormal.gameObject.activeSelf)
//						{
//							//otherLaserGunEngineNormal.renderer.enabled = false;
//							//otherLaserGunEngineDamaged.renderer.enabled = true;
//							otherLaserGunEngineNormal.gameObject.SetActive(false);
//							otherLaserGunEngineDamaged.gameObject.SetActive(true);
//							//otherLaserGunEngineNormal.parent.Find("Elipse").gameObject.SetActive(false);
//						}
				}
			}
			else
			{
				if(bossShip.rightWingCount == 0 && wingNormal.gameObject.activeSelf)
				{
					//wingNormal.renderer.enabled = false;
					//wingDamaged.renderer.enabled = true;
					wingNormal.gameObject.SetActive(false);
					wingDamaged.gameObject.SetActive(true);
					
//						if(otherLaserGunEngineNormal.gameObject.activeSelf)
//						{
//							//otherLaserGunEngineNormal.renderer.enabled = false;
//							//otherLaserGunEngineDamaged.renderer.enabled = true;
//							otherLaserGunEngineNormal.gameObject.SetActive(false);
//							otherLaserGunEngineDamaged.gameObject.SetActive(true);
//							//otherLaserGunEngineNormal.parent.Find("Elipse").gameObject.SetActive(false);
//						}
				}
			}
			
			bossShip.currentWave--;
			if(bossShip.currentWave == 0)
			{
				bossShip.EnableThirdWave();
			}
			LaserGunEngineDamaged.gameObject.SetActive(true);
			//LaserGunRootNormal.gameObject.SetActive(false);
			//LaserGunNormal.renderer.enabled = false;
			//LaserGunBodyNormal.renderer.enabled = false;
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
//	void OnTriggerEnter2D(Collider2D col)
//	{
//		if(col.gameObject.tag.Equals("PlayerBullet"))
//		{
//			if(health<=0)
//			{
//				//disable all hits just in case
//				EngineHit.renderer.enabled = false;
//				LaserGunRootHit.renderer.enabled = false;
//				LaserGunHit.renderer.enabled = false;
//				LaserGunBodyHit.renderer.enabled = false;
//				wingHit.renderer.enabled = false;
//				
//				CancelInvoke("FireLaser");
//				if(LaserGunFire.isPlaying)
//					LaserGunFire.Stop();
//				laserEvent.dontInvertLaser = false;
//				laserEvent.Laser.animation["LaserLaunch"].normalizedTime = 1;
//				laserEvent.Laser.animation["LaserLaunch"].speed = -1;
//				if(laserEvent.laserShooting)
//					laserEvent.Laser.animation.Play();
//				
//				collider2D.enabled = false;
//				col.gameObject.SetActive(false);
//				transform.parent.gameObject.SetActive(false);
//				//				EngineNormal.renderer.enabled = false;
//				//				LaserGunRootNormal.renderer.enabled = false;
//				//				LaserGunNormal.renderer.enabled = false;
//				//				LaserGunBodyNormal.renderer.enabled = false;
//				//				LaserGunEngineDamaged.renderer.enabled = true;
//				explosion.SetActive(true);
//				
//				if(leftWing)
//					bossShip.leftWingCount--;
//				else
//					bossShip.rightWingCount--;
//				
//				
//				if(leftWing)
//				{
//					if(bossShip.leftWingCount == 0 && wingNormal.gameObject.activeSelf)
//					{
//						//wingNormal.renderer.enabled = false;
//						//wingDamaged.renderer.enabled = true;
//						wingNormal.gameObject.SetActive(false);
//						wingDamaged.gameObject.SetActive(true);
//						
////						if(otherLaserGunEngineNormal.gameObject.activeSelf)
////						{
////							//otherLaserGunEngineNormal.renderer.enabled = false;
////							//otherLaserGunEngineDamaged.renderer.enabled = true;
////							otherLaserGunEngineNormal.gameObject.SetActive(false);
////							otherLaserGunEngineDamaged.gameObject.SetActive(true);
////							//otherLaserGunEngineNormal.parent.Find("Elipse").gameObject.SetActive(false);
////						}
//					}
//				}
//				else
//				{
//					if(bossShip.rightWingCount == 0 && wingNormal.gameObject.activeSelf)
//					{
//						//wingNormal.renderer.enabled = false;
//						//wingDamaged.renderer.enabled = true;
//						wingNormal.gameObject.SetActive(false);
//						wingDamaged.gameObject.SetActive(true);
//						
////						if(otherLaserGunEngineNormal.gameObject.activeSelf)
////						{
////							//otherLaserGunEngineNormal.renderer.enabled = false;
////							//otherLaserGunEngineDamaged.renderer.enabled = true;
////							otherLaserGunEngineNormal.gameObject.SetActive(false);
////							otherLaserGunEngineDamaged.gameObject.SetActive(true);
////							//otherLaserGunEngineNormal.parent.Find("Elipse").gameObject.SetActive(false);
////						}
//					}
//				}
//				
//				bossShip.currentWave--;
//				if(bossShip.currentWave == 0)
//				{
//					bossShip.EnableThirdWave();
//				}
//				LaserGunEngineDamaged.gameObject.SetActive(true);
//				//LaserGunRootNormal.gameObject.SetActive(false);
//				//LaserGunNormal.renderer.enabled = false;
//				//LaserGunBodyNormal.renderer.enabled = false;
//				
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
		EngineHit.GetComponent<Renderer>().enabled = true;
		LaserGunRootHit.GetComponent<Renderer>().enabled = true;
		LaserGunHit.GetComponent<Renderer>().enabled = true;
		LaserGunBodyHit.GetComponent<Renderer>().enabled = true;
		wingHit.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.1f);
		EngineHit.GetComponent<Renderer>().enabled = false;
		LaserGunRootHit.GetComponent<Renderer>().enabled = false;
		LaserGunHit.GetComponent<Renderer>().enabled = false;
		LaserGunBodyHit.GetComponent<Renderer>().enabled = false;
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
		InvokeRepeating("FireLaser", laserInitialTime, laserInterval);
	}
}
