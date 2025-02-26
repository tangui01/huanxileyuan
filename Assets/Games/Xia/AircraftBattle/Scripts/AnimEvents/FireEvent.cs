using UnityEngine;
using System.Collections;

public class FireEvent : MonoBehaviour {

	GameObject BossGun;
	int bulletIndex=0;
	public int fireRate = 25;
	int fireRateCounter = 0;
	// Use this for initialization

	void Start()
	{
		BossGun = this.gameObject;
	}
	void BossGunFireBullet()
	{
		if(bulletIndex == BossGun.transform.parent.Find("BulletPool").childCount)
			bulletIndex = 0;
		
		EnemyBullet tempScript = BossGun.transform.parent.Find("BulletPool").GetChild(bulletIndex).GetComponent<EnemyBullet>();
		if(tempScript.available)
		{
			tempScript.initialized = true;
			//break;
		}
		bulletIndex++;
	}

	
}
