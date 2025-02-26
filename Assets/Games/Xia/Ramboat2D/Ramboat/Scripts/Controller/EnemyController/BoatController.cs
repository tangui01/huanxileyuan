using UnityEngine;
using System.Collections;

public class BoatController : MonoBehaviour {
	public GameObject spawnBullet;
	public float healthEnemy;
	public GameObject bulletBoat;
	// Use this for initialization
	void Start () {
		healthEnemy = 60*((Ramboat2DLevelManager.THIS.level+1) / 3f);
		transform.position = new Vector3 (6.31f, -0.76f, 0f);
	}
	void OnEnable(){
		Start ();
	}
	public void SpawnBulletBoat(){
		if (transform.position.x < 5.5f && !Ramboat2DPlayerController.Intance.playerDead) {
			Instantiate (bulletBoat, spawnBullet.transform.position, Quaternion.identity);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet") {
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
		} else
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
	public void Death(){
		ReadWriteTextMission.THIS.CheckMission (34);
		GameObject obj = Instantiate (Resources.Load ("Prefabs/MapGame/HitCharacter"), transform.position, Quaternion.identity) as GameObject;
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

		Destroy(this.gameObject,1f);
		Destroy (obj, 1f);
		gameObject.SetActive (false);
		Resources.UnloadUnusedAssets (); 
		System.GC.Collect ();
	}
	public void DisableBoat(){
		Destroy(this.gameObject,1f);
		gameObject.SetActive (false);

	}
	void OnParticleCollision(GameObject other) {          
		TakeDame (Ramboat2DPlayerController.Intance.gunPower);
		Debug.Log(111+Ramboat2DPlayerController.Intance.gunPower);
	}
}
