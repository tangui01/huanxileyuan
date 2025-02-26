using UnityEngine;
using System.Collections;
using WGM;

public class AirFollowController : MonoBehaviour
{
	bool shoot;
	bool checkPlayer;
	GameObject player;
	int numberShoot;
	//public GameObject startPoint;
	Vector3 stopAir;
	float distance;
	float time;
	public float smoothTime = 1f;
	private Vector3 velocity = Vector3.zero;
	public GameObject redRange;
	public float mindistance;
	Vector3 velocityBullet;
	float healthEnemy;
	
	void Start ()
	{
		GetComponent<AudioSource>().volume *= LibWGM.machine.SeVolume / 10f;
		
		healthEnemy = Ramboat2DLevelManager.THIS.healthAirFollow;
		shoot = false;
		checkPlayer = true;
		numberShoot = Ramboat2DLevelManager.THIS.countAirFollowShoot;
		player = GameObject.FindGameObjectWithTag ("Player");
		time = 0;
		if (player.transform.position.x > 4f) {
			stopAir = new Vector3 (4f, 2.5f, 0);
		} else {
			stopAir = new Vector3 (player.transform.position.x + 2f, 2.5f, 0f);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!Ramboat2DPlayerController.Intance.playerDead) {
			if (!shoot && checkPlayer && numberShoot > 0)
				RotateAir ();
			else if (shoot && !checkPlayer)
				StartCoroutine (Shoot ());
		}
		if (numberShoot == 0 || Ramboat2DPlayerController.Intance.playerDead) {
			transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, new Vector3 (0, 0, 0), 2f * Time.deltaTime);
			transform.Translate (new Vector3 (-1.5f, 0.2f, 0) * Time.deltaTime * 3);
			if (transform.position.x < -6f) {
				Destroy (gameObject);
				Ramboat2DFXSound.THIS.fxSound.clip = Ramboat2DFXSound.THIS.none;
				Ramboat2DFXSound.THIS.fxSound.loop = false;
				Ramboat2DFXSound.THIS.fxSound.Stop ();
			}
		}

	}

	void RotateAir ()
	{
		
		transform.position = Vector3.SmoothDamp (transform.position, stopAir, ref velocity, UnityEngine.Random.Range (smoothTime - 0.5f, smoothTime));
		float distance = Vector3.Distance (transform.position, stopAir);

		float angle = 45 - Mathf.Abs (Vector3.Angle (Vector3.down, player.transform.position - transform.position));
		if (distance < mindistance) {
			if (player.transform.position.x > 4f) {
				stopAir = new Vector3 (4f, 2.5f, 0);
			} else {
				stopAir = new Vector3 (UnityEngine.Random.Range (player.transform.position.x + 2f, player.transform.position.x + 3f), UnityEngine.Random.Range (2.3f, 2.7f), 0f);
			}
			if (angle >= 0) {
				transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, new Vector3 (0, 0, angle), 10 * Time.deltaTime);

			}
				
		} else {
			
			transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, new Vector3 (0, 0, 0), 2 * Time.deltaTime);

		}
		time += Time.deltaTime;
		if (time > Ramboat2DLevelManager.THIS.timeAirFollowShoot) {
			time = 0;
			shoot = true;
			checkPlayer = false;
			velocityBullet = (redRange.transform.position- transform.position) / (Vector3.Distance (redRange.transform.position,transform.position))*10f;

		}
	}

	IEnumerator Shoot(){

		checkPlayer = true;
		float timeWarning = 0;
		GameObject range = transform.GetChild (1).gameObject;
		Color colorRange = range.GetComponent<SpriteRenderer> ().color;
		while(timeWarning<1f){
			timeWarning += Time.deltaTime;
			colorRange.a =UnityEngine.Random.Range (0.3f, 0.7f);
			range.GetComponent<SpriteRenderer> ().color = colorRange;
			yield return new WaitForFixedUpdate ();
		}
		colorRange.a = 0;
		range.GetComponent<SpriteRenderer> ().color = colorRange;
		for(int i=0;i<10;i++){
			GameObject obj = Ramboat2DLevelManager.THIS.GetPooledObject (18);
			if (obj != null) {
				obj.transform.position =transform.position;
				obj.SetActive (true);
				obj.GetComponent<Rigidbody2D> ().velocity = velocityBullet+ new Vector3(Random.Range(-1f,0),Random.Range(-1f,2f),0);
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForFixedUpdate ();
		numberShoot-=1;
		shoot = false;
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
		if (healthEnemy > 0) {
			healthEnemy -= dame;
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
		ReadWriteTextMission.THIS.CheckMission (34);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision[Random.Range(0,3)]);
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
	}
}
