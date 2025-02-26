using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShootAndCollisionPlayer : MonoBehaviour
{
	bool canTakeDamage;
	// Use this for initialization
	void Start ()
	{
		canTakeDamage = true;
	}

	void OnEnable ()
	{
		Start ();
	}
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void ShootByPlayer ()
	{
		Ramboat2DPlayerController.Intance.isShoot = true;
	}

	public void DontShoot ()
	{
		Ramboat2DPlayerController.Intance.isShoot = false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		
		if (other.tag == "BulletEnemy" && canTakeDamage) {
			if (!Ramboat2DPlayerController.Intance.playerDead)
				StartCoroutine (TakeDameBulletEnemy (other.gameObject));
		}
		if (other.tag == "GunBulletEnemy" && canTakeDamage) {
			if (!Ramboat2DPlayerController.Intance.playerDead)
				StartCoroutine (TakeDameGunBulletEnemy (other.gameObject));
		}
		if (other.tag == "BulletSubmarine" && canTakeDamage) {
			
			if (!Ramboat2DPlayerController.Intance.playerDead)
				StartCoroutine (TakeDameBulletSubmarine (other.gameObject));
		}
		if (other.tag == "Bomb" && canTakeDamage) {
			
			if (!Ramboat2DPlayerController.Intance.playerDead)
				StartCoroutine (TakeDameBomb (other.gameObject));
		}
		if (Ramboat2DPlayerController.Intance.liveCurrent == 0) {
			Ramboat2DPlayerController.Intance.playerDead = true;
		}
	
	}

	IEnumerator TakeDameBulletEnemy (GameObject other)
	{
		canTakeDamage = false;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.enemyGunHit);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.playerTakeDame [PlayerPrefs.GetInt ("ChoosePlayer")]);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision [Random.Range (0, 3)]);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.playerTakeDame [PlayerPrefs.GetInt ("ChoosePlayer")]);
		Ramboat2DPlayerController.Intance.liveCurrent -= 1;
		Ramboat2DLevelManager.THIS.ShowLifeCurrent (Ramboat2DPlayerController.Intance.liveCurrent);
		GameObject par = other.transform.GetChild (0).gameObject;
		par.GetComponent<ParticleSystem> ().Stop ();
		GameObject explosion = other.transform.GetChild (2).gameObject;
		explosion.transform.eulerAngles = new Vector3 (0, 0, 0);
		explosion.SetActive (true);
		explosion.transform.parent = null;
		par.transform.parent = null;
		Destroy (other.gameObject);
		Destroy (par, 1f);
		Destroy (explosion, 1f);
		float time = 0;
		Image dameImage = GameObject.Find ("DamageImage").GetComponent<Image> ();
		Color colorDamageImg = dameImage.color;
		while (time < 0.3f) {
			time += Time.deltaTime;
			float a = Mathf.Abs (Mathf.Sin (UnityEngine.Random.Range (0, 50)));
			colorDamageImg.a = a;
			dameImage.color = colorDamageImg;
			yield return new WaitForFixedUpdate ();
		}
		colorDamageImg.a = 0;
		dameImage.color = colorDamageImg;

		// player show dame hit
		time = 0;
		SpriteRenderer spritePlayer = GameObject.Find ("Player").GetComponent<SpriteRenderer> ();
		Color colorPlayer = spritePlayer.color;
		int colorChange = 0;
		while (time < 1f) {
			time += Time.deltaTime;
			Color changeColor;
			if (colorChange == 0) {
				changeColor = Color.red;
				colorChange = 1;
			} else {
				changeColor = colorPlayer;
				colorChange = 0;
			}
			spritePlayer.color = changeColor;

			yield return new WaitForSeconds (0.1f);
		}
		spritePlayer.color = colorPlayer;
		canTakeDamage = true;
	}


	IEnumerator TakeDameGunBulletEnemy (GameObject other)
	{
		canTakeDamage = false;
		Ramboat2DPlayerController.Intance.liveCurrent -= 1;
		Ramboat2DLevelManager.THIS.ShowLifeCurrent (Ramboat2DPlayerController.Intance.liveCurrent);
		//img color dame
		float time = 0;
		Image dameImage = GameObject.Find ("DamageImage").GetComponent<Image> ();
		Color colorDamageImg = dameImage.color;
		while (time < 0.3f) {
			time += Time.deltaTime;
			float a = Mathf.Abs (Mathf.Sin (UnityEngine.Random.Range (0, 60)));
			colorDamageImg.a = a;
			dameImage.color = colorDamageImg;
			yield return new WaitForFixedUpdate ();
		}
		colorDamageImg.a = 0;
		dameImage.color = colorDamageImg;

		// player show dame hit
		time = 0;
		SpriteRenderer spritePlayer = GameObject.Find ("Player").GetComponent<SpriteRenderer> ();
		Color colorPlayer = spritePlayer.color;
		int colorChange = 0;
		while (time < 1f) {
			time += Time.deltaTime;
			Color changeColor;
			if (colorChange == 0) {
				changeColor = Color.red;
				colorChange = 1;
			} else {
				changeColor = colorPlayer;
				colorChange = 0;
			}
			spritePlayer.color = changeColor;

			yield return new WaitForSeconds (0.1f);
		}
		spritePlayer.color = colorPlayer;
		canTakeDamage = true;
	}

	IEnumerator TakeDameBulletSubmarine (GameObject other)
	{
		canTakeDamage = false;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision [Random.Range (0, 3)]);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.playerTakeDame [PlayerPrefs.GetInt ("ChoosePlayer")]);
		Ramboat2DPlayerController.Intance.liveCurrent -= 1;
		Ramboat2DLevelManager.THIS.ShowLifeCurrent (Ramboat2DPlayerController.Intance.liveCurrent);
		GameObject explosion = other.gameObject.transform.GetChild (1).gameObject;
		explosion.transform.eulerAngles = new Vector3 (0, 0, 0);
		explosion.SetActive (true);
		explosion.transform.parent = null;
		Destroy (explosion, 1f);
		Destroy (other.gameObject);
		float time = 0;
		Image dameImage = GameObject.Find ("DamageImage").GetComponent<Image> ();
		Color colorDamageImg = dameImage.color;
		while (time < 0.3f) {
			time += Time.deltaTime;
			float a = Mathf.Abs (Mathf.Sin (UnityEngine.Random.Range (0, 70)));
			colorDamageImg.a = a;
			dameImage.color = colorDamageImg;
			yield return new WaitForFixedUpdate ();
		}
		colorDamageImg.a = 0;
		dameImage.color = colorDamageImg;
		// player show dame hit
		time = 0;
		SpriteRenderer spritePlayer = GameObject.Find ("Player").GetComponent<SpriteRenderer> ();
		Color colorPlayer = spritePlayer.color;
		int colorChange = 0;
		while (time < 1f) {
			time += Time.deltaTime;
			Color changeColor;
			if (colorChange == 0) {
				changeColor = Color.red;
				colorChange = 1;
			} else {
				changeColor = colorPlayer;
				colorChange = 0;
			}
			spritePlayer.color = changeColor;

			yield return new WaitForSeconds (0.1f);
		}
		spritePlayer.color = colorPlayer;
		canTakeDamage = true;
	}

	public void TakeDameInBomb(GameObject other)
	{
		StartCoroutine(TakeDameBomb(other));
	}
	IEnumerator TakeDameBomb (GameObject other)
	{
		canTakeDamage = false;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision [Random.Range (0, 3)]);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.playerTakeDame [PlayerPrefs.GetInt ("ChoosePlayer")]);
		Ramboat2DPlayerController.Intance.liveCurrent -= 1;
		other.SetActive (false);
		Ramboat2DLevelManager.THIS.ShowLifeCurrent (Ramboat2DPlayerController.Intance.liveCurrent);
		GameObject explosion = Instantiate (Resources.Load ("Prefabs/MapGame/HitCharacter"), other.transform.position, Quaternion.identity) as GameObject;
		Destroy (explosion, 1f);
		float time = 0;
		Image dameImage = GameObject.Find ("DamageImage").GetComponent<Image> ();
		Color colorDamageImg = dameImage.color;
		while (time < 0.3f) {
			time += Time.deltaTime;
			float a = Mathf.Abs (Mathf.Sin (UnityEngine.Random.Range (0, 70)));
			colorDamageImg.a = a;
			dameImage.color = colorDamageImg;
			yield return new WaitForFixedUpdate ();
		}
		colorDamageImg.a = 0;
		dameImage.color = colorDamageImg;
		// player show dame hit
		time = 0;
		SpriteRenderer spritePlayer = GameObject.Find ("Player").GetComponent<SpriteRenderer> ();
		Color colorPlayer = spritePlayer.color;
		int colorChange = 0;
		while (time < 1f) {
			time += Time.deltaTime;
			Color changeColor;
			if (colorChange == 0) {
				changeColor = Color.red;
				colorChange = 1;
			} else {
				changeColor = colorPlayer;
				colorChange = 0;
			}
			spritePlayer.color = changeColor;

			yield return new WaitForSeconds (0.1f);
		}
		spritePlayer.color = colorPlayer;
		canTakeDamage = true;
		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}
}
	