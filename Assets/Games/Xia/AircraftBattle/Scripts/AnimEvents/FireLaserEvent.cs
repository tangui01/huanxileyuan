using UnityEngine;
using System.Collections;

public class FireLaserEvent : MonoBehaviour {

	public Transform Laser;
	public float LaserDuration = 2f;
	[HideInInspector] public bool dontInvertLaser = true;
	[HideInInspector] public bool laserShooting = false;

	void FireLaser()
	{
		Debug.Log("POZIVA SE FireLaser");
		laserShooting = true;
		if(!Laser.GetChild(0).gameObject.activeSelf)
			Laser.GetChild(0).gameObject.SetActive(true);
		Laser.GetComponent<Animation>().Play();
		SoundManager.Instance.Play_EnemyLaser();
//		AudioSource enemyLaser = GetComponent<AudioSource>();
//		if(enemyLaser != null)
//		{
//			if(enemyLaser.clip != null && SoundManager.soundOn == 1)
//				enemyLaser.Play();
//		}
		Invoke("LaserInverse", LaserDuration);
	}

	void LaserInverse()
	{
		Laser.GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
		Laser.GetComponent<Animation>()["LaserLaunch"].speed = -1;
		if(dontInvertLaser)
			Laser.GetComponent<Animation>().Play();
		Invoke("ResetAnimation",LaserDuration+1f);
		laserShooting = false;
	}

	void ResetAnimation()
	{
		Laser.GetComponent<Animation>()["LaserLaunch"].normalizedTime = 0;
		Laser.GetComponent<Animation>()["LaserLaunch"].speed = 1;

	}
}
