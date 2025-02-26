using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BombController : MonoBehaviour {
	private float healthEnemy;
	bool isDead = false;
	// Use this for initialization
	void OnEnable () {
		healthEnemy=60+10*((Ramboat2DLevelManager.THIS.level+1) / 3f);
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == ("Bullet")) {
			Debug.Log(other.name);
			
			other.gameObject.SetActive (false);
			GameObject effectShootPlayer = Ramboat2DLevelManager.THIS.GetPooledObject (10);
			if (effectShootPlayer != null) {
				effectShootPlayer.transform.position = transform.position;
				effectShootPlayer.transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
				effectShootPlayer.SetActive (true);
			}
			TakeDame (Ramboat2DPlayerController.Intance.gunPower);
		}

	}
	void TakeDame(float dame){
		if (healthEnemy > 0) {
			healthEnemy -= dame;
		} 
		if(healthEnemy<=0 && !isDead)
			Death ();
	}

	public void FireGunOnTriggerEnter2D()
	{
		GameObject effectShootPlayer = Ramboat2DLevelManager.THIS.GetPooledObject (10);
		if (effectShootPlayer != null) {
			effectShootPlayer.transform.position = transform.position;
			effectShootPlayer.transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
			effectShootPlayer.SetActive (true);
		}
		TakeDame (Ramboat2DPlayerController.Intance.gunPower);
	}
	

	void Death(){
		isDead = true;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision[Random.Range(0,3)]);
		GameObject obj = Instantiate (Resources.Load("Prefabs/MapGame/HitCharacter"),transform.position,Quaternion.identity) as GameObject;
		Destroy (obj, 1f);
		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
		gameObject.SetActive (false);
	}
	// void OnParticleCollision(GameObject other) {          
	// 	TakeDame (Ramboat2DPlayerController.Intance.gunPower);
	// }
}
