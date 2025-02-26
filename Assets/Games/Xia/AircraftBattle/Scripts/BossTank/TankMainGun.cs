using UnityEngine;
using System.Collections;

public class TankMainGun : MonoBehaviour {

	public Transform mainGunHit;
	public Transform bodyNormal;
	public Transform bodyDamaged;
	
	public BossTank bossTank;
	public GameObject wholeParent;
	public GameObject explosion;
	
	float initialFire = 17;
	float fireInterval = 7;
	
	int health = 25;
	MainGunFireEvent fireEvent;
	bool alwaysVisible = false;
	bool fired = false;
	bool continuousDamage = false;
	int regularDamage;
	int uniqueDamage;
	
	void Start ()
	{
		fireEvent = transform.GetComponent<MainGunFireEvent>();
		
		if(bossTank.bossLevel == 1)
		{
			fireEvent.numberOfAttacks = 1;
			fireEvent.speed = 0.75f;
		}
		else if(bossTank.bossLevel == 2)
		{
			fireEvent.numberOfAttacks = 1;
			fireEvent.speed = 1f;
		}
		else
		{
			fireEvent.numberOfAttacks = 1;
			fireEvent.speed = 1.35f;
		}
		
		health = bossTank.mainGunHealth;
		InvokeRepeating("MainGunFire",initialFire,fireInterval);
		
		regularDamage = 80 + (LevelGenerator.currentStage-1)*10;
		uniqueDamage = 200 + (LevelGenerator.currentStage-1)*10;
		fireEvent.regularDamage = regularDamage;
		fireEvent.uniqueDamage = uniqueDamage;
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
		
		if(health<=0  && PandaPlane.Instance.health > 0)
		{
			BossStars.Instance.spawnPosition = transform.position;
			BossStars.Instance.GenerateCoins(25,36);
			continuousDamage = false;
			//disable all hits just in case
			mainGunHit.GetComponent<Renderer>().enabled = false;
			
			CancelInvoke("FireUniqueWeapon");
			GetComponent<Collider2D>().enabled = false;
			//col.gameObject.SetActive(false);
			//transform.parent.gameObject.SetActive(false);
			
			//bodyNormal.renderer.enabled = false;
			//bodyDamaged.renderer.enabled = true;
			bodyNormal.gameObject.SetActive(false);
			bodyDamaged.gameObject.SetActive(true);
			fireEvent.TurnOffLasers();
			PandaPlane.Instance.invincible = true;
			
			explosion.SetActive(true);
			Invoke("DestroyBoss",0.15f);

			CancelInvoke("MainGunFire");
			fireEvent.DisableUniqueWeapon();
		}
		else
		{
			SoundManager.Instance.Play_BossTurretHit();
			//health-=damage;
			StartCoroutine("Hit");
		}
	}
	
	void MainGunFire()
	{
		if (fireEvent.thirdWaveEnabled)
		{
			fireEvent.Fire();
		}
		else
		{
			//animation["MainGunArrivingAnimation"].normalizedTime = 0;
			//animation["MainGunArrivingAnimation"].speed = 1;
			fired = true;
			if(!GetComponent<Animation>().isPlaying)
				GetComponent<Animation>().Play();
			if(!alwaysVisible)
				Invoke("MainGunInverse", 2f);
		}
	}
	
	void MainGunInverse()
	{
		fireEvent.dontFire = true;
		GetComponent<Animation>()["MainGunArrivingAnimationTank"].normalizedTime = 1;
		GetComponent<Animation>()["MainGunArrivingAnimationTank"].speed = -1;
		GetComponent<Animation>().Play();
		Invoke("ResetAnimation",2f);
		fired = false;
	}
	
	void ResetAnimation()
	{
		fireEvent.dontFire = false;
		GetComponent<Animation>()["MainGunArrivingAnimationTank"].normalizedTime = 0;
		GetComponent<Animation>()["MainGunArrivingAnimationTank"].speed = 1;
		
	}
	
	//	void OnTriggerEnter2D(Collider2D col)
	//	{
	//		if(col.gameObject.tag.Equals("PlayerBullet"))
	//		{
	//			if(health<=0)
	//			{
	//				//disable all hits just in case
	//				mainGunHit.renderer.enabled = false;
	//				
	//				CancelInvoke("FireUniqueWeapon");
	//				collider2D.enabled = false;
	//				col.gameObject.SetActive(false);
	//				//transform.parent.gameObject.SetActive(false);
	//				
	//				//bodyNormal.renderer.enabled = false;
	//				//bodyDamaged.renderer.enabled = true;
	//				bodyNormal.gameObject.SetActive(false);
	//				bodyDamaged.gameObject.SetActive(true);
	//				fireEvent.TurnOffLasers();
	//
	//				explosion.SetActive(true);
	//				Invoke("DestroyBoss",0.15f);
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
	
	void DestroyBoss()
	{
		bossTank.DestroyBoss();
		//explosion.SetActive(true);
	}
	
	IEnumerator Hit()
	{
		mainGunHit.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.1f);
		mainGunHit.GetComponent<Renderer>().enabled = false;
	}
	
	void HideTurret()
	{
		if(wholeParent.activeSelf)
			wholeParent.SetActive(false);
	}
	
	void ShowTurret()
	{
		if(!wholeParent.activeSelf)
			wholeParent.SetActive(true);
	}
	
	void FireAway()
	{
		fireEvent.thirdWaveEnabled = true;
		//if(!animation.isPlaying)
		if(!fired)
		{
			fireEvent.dontFire = true;
			GetComponent<Animation>()["MainGunArrivingAnimationTank"].normalizedTime = 0;
			GetComponent<Animation>()["MainGunArrivingAnimationTank"].speed = 2;
			if(!GetComponent<Animation>().isPlaying)
				GetComponent<Animation>().Play();
			//Invoke("CanFireAgainRegularAttack", 0.5f);
		}
		
		//fireEvent.dontFire = true;
		//CancelInvoke("MainGunFire");

		//------ SET NEW MAIN GUN FIRE VALUES 
		CancelInvoke("MainGunFire");
		if(bossTank.bossLevel == 1)
		{
			initialFire = 2;
			fireInterval = 5;
		}
		else if(bossTank.bossLevel == 2)
		{
			initialFire = 2;
			fireInterval = 2f;
		}
		else
		{
			initialFire = 2;
			fireInterval = 3.5f;
		}
		InvokeRepeating("MainGunFire",initialFire,fireInterval);
		//------
		CancelInvoke("MainGunInverse");
		alwaysVisible = true;
		if(bossTank.bossLevel == 1)
		{
			initialFire = 0.5f;
			fireInterval = 2f;
		}
		else if(bossTank.bossLevel == 2)
		{
			if(fireEvent.uniqueWeapon.name.Contains("Waves"))
			{
				fireEvent.numberOfAttacksUnique = 1;
				initialFire = 0.5f;
				fireInterval = 1.5f;
			}
			else
			{
				fireEvent.numberOfAttacksUnique = 1;
				initialFire = 0.5f;
				fireInterval = 7f;
			}
		}
		else if(bossTank.bossLevel == 3)
		{
			if(fireEvent.uniqueWeapon.name.Contains("Waves"))
			{
				fireEvent.numberOfAttacksUnique = 1;
				initialFire = 0.5f;
				fireInterval = 1f;
			}
			else
			{
				fireEvent.numberOfAttacksUnique = 1;
				initialFire = 0.5f;
				fireInterval = 7f;
			}
		}
		InvokeRepeating("FireUniqueWeapon", initialFire, fireInterval);
	}

	void CanFireAgainRegularAttack()
	{
		fireEvent.dontFire = false;
	}

	void FireUniqueWeapon()
	{
		fireEvent.bossLevel = bossTank.bossLevel;
		fireEvent.UniqueFire();
	}
}
