using System;
using UnityEngine;
using System.Collections;
using WGM;
using Random = UnityEngine.Random;

public class AirController : MonoBehaviour
{
	float speed;
	Vector3 velocity;
	float numberBomb;
	public float timeDelayShoot;
	bool shoot = true;
	public GameObject posBombUsed;
	public GameObject airBomb;
	float healthAir;

	void Start ()
	{
		GetComponent<AudioSource>().volume *= LibWGM.machine.SeVolume / 10f;
		
		healthAir=20+70*((Ramboat2DLevelManager.THIS.level+1) / 3f);
		velocity = new Vector3 (3, 0, 0);
		numberBomb = 2;
	}
	

	// Update is called once per frame
	void Update ()
	{
		transform.Translate (velocity * Time.deltaTime);
		if (!Ramboat2DPlayerController.Intance.playerDead) {
			if (shoot && transform.position.x > -4 && transform.position.x < 4 && numberBomb > 0) {
				StartCoroutine (UseBombDelay ());
			}
		}
		if (transform.position.x > 8) {
			transform.position = new Vector3 (-10, transform.position.y, 0);
			if (numberBomb == 0 || Ramboat2DPlayerController.Intance.playerDead) {
				Ramboat2DFXSound.THIS.fxSound.clip = Ramboat2DFXSound.THIS.none;
				Ramboat2DFXSound.THIS.fxSound.loop = false;
				Ramboat2DFXSound.THIS.fxSound.Stop ();
				Destroy (this.gameObject);

			}
		}
	}

	IEnumerator UseBombDelay ()
	{
		shoot = false;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.airFlyShot);
		GameObject obj = Instantiate (airBomb) as GameObject;
		//GameObject obj = Instantiate (Resources.Load("Prefabs/BulletEnemy/AirBomb")) as GameObject;
		obj.transform.position = posBombUsed.transform.position;
		numberBomb-=1;
		yield return new WaitForSeconds (timeDelayShoot);
		if (numberBomb == 0) {
			posBombUsed.SetActive (false);
			shoot = false;
		}
		else
		shoot = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.pistolHitMetal[Random.Range(0,3)]);
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
		if (healthAir > 0) {
			healthAir -= dame;
		} else
			Death ();
	}
	public void FireGunOnTriggerEnter2D()
	{
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.pistolHitMetal[Random.Range(0,3)]);
		GameObject effectShootPlayer = Ramboat2DLevelManager.THIS.GetPooledObject (10);
		if (effectShootPlayer != null) {
			effectShootPlayer.transform.position = transform.position;
			effectShootPlayer.transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
			effectShootPlayer.SetActive (true);
		}
		TakeDame (Ramboat2DPlayerController.Intance.gunPower);
	}
	public void Death(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision[Random.Range(0,3)]);
		ReadWriteTextMission.THIS.CheckMission (34);
		GameObject obj = Instantiate (Resources.Load ("Prefabs/MapGame/DestroyAir"), transform.position, Quaternion.identity) as GameObject;
		int appearDollar = UnityEngine.Random.Range (0,5);
		if (appearDollar % 3 == 0) {
			GameObject dollar = Ramboat2DLevelManager.THIS.GetPooledObject (12);
			if (dollar != null) {
				dollar.transform.position = transform.position;
				dollar.SetActive (true);
			}
			int numberCoinAppear = UnityEngine.Random.Range (1, 4);
			for (int i = 0; i < numberCoinAppear; i++) {
				GameObject coin = Ramboat2DLevelManager.THIS.GetPooledObject (11);
				if (coin != null) {
					coin.transform.position = transform.position;
					coin.SetActive (true);
					coin.GetComponent<Rigidbody2D> ().velocity = new Vector3 (i * 5 - 5, Random.Range (6, 12), 0);
				}
			}
		} else {
			int numberCoinAppear = UnityEngine.Random.Range (1, 4);
			for (int i = 0; i < numberCoinAppear; i++) {
				GameObject coin = Ramboat2DLevelManager.THIS.GetPooledObject (11);
				if (coin != null) {
					coin.transform.position = transform.position;
					coin.SetActive (true);
					coin.GetComponent<Rigidbody2D> ().velocity = new Vector3 (i * 5 - 5, Random.Range (6, 12), 0);
				}
			}
		}

		Destroy (obj, 1f);  
		gameObject.SetActive (false);
		Resources.UnloadUnusedAssets (); 
		System.GC.Collect ();
	}
	void OnParticleCollision(GameObject other) {          
		TakeDame (Ramboat2DPlayerController.Intance.gunPower);
	}
}
