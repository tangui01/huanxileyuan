using UnityEngine;
using System.Collections;

public class SubMarineController : MonoBehaviour {
	float numberBomb;
	Vector3 velocity;
	bool shoot;
	bool submarineIn,submarineOut;
	// Use this for initialization
	void OnEnable(){
		Start ();
	}
	void Start () {
		velocity = new Vector3 (0, 0, -3);
		shoot = true;
		numberBomb = 2;
		submarineIn = false;
		submarineOut = false;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}
	public void Move(){
		if (transform.position.y < -2.5f && numberBomb >0) {
			transform.Translate (velocity * Time.deltaTime);
			if (!submarineIn) {
				submarineIn = true;
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.submarinIn);
			}
		}
		if (transform.position.y >= -2.5 && numberBomb > 0 && shoot && !Ramboat2DPlayerController.Intance.playerDead) {
			StartCoroutine (ShootDelay ());
		}
		if (numberBomb == 0 || Ramboat2DPlayerController.Intance.playerDead) {
			transform.Translate (-velocity * Time.deltaTime);
			if (!submarineOut) {
				submarineOut = true;
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.submarineOut);
			}
		}
		if (transform.position.y < -6)
			gameObject.SetActive (false);
	}
	IEnumerator ShootDelay(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.rocket[Random.Range(0,3)]);
		shoot = false;
		GameObject obj=  Instantiate (Resources.Load ("Prefabs/BulletEnemy/SubmarineBullet")) as GameObject;
		obj.transform.position = transform.position;
		numberBomb -= 1;
		yield return new WaitForSeconds (2f);
		shoot = true;
	}

}
