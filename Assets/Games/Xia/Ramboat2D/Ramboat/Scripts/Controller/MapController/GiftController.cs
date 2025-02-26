using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class GiftController : MonoBehaviour {
	Vector3 end;
	public float speed;
	float healthGift=30f;
	// Use this for initialization
	bool isDead;
	void Start () {
		end = Ramboat2DLevelManager.THIS.pointEnemyDestroyed.transform.position;
	}

	private void OnEnable()
	{
		isDead = false;
	}

	// Update is called once per frame
	void Update () {
		transform.parent.position = Vector3.MoveTowards (transform.parent.position, end, speed * Time.deltaTime);
		if (transform.parent.position.x <= -5.7f) {
			gameObject.transform.parent.gameObject.SetActive (false);

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
		if (healthGift > 0) {
			healthGift -= dame;
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
		if(isDead)
			return;
		isDead = true;
		GameObject obj = Instantiate (Resources.Load ("Prefabs/MapGame/HitCharacter"), transform.position, Quaternion.identity) as GameObject;
		int gunType = UnityEngine.Random.Range (1,7);
//		int gunType=6;
		if (gunType >= 1 && gunType <= 4) {
			SetGunType (gunType, obj);
		} 
		else if (gunType == 5) {
			//create clover
			CreateClover();
			SetGunType (UnityEngine.Random.Range(1,5), obj);
		}
		else if(gunType==6){
			//create pocker
			CreatePocker();
			SetGunType (UnityEngine.Random.Range(1,5), obj);
		}


	}
	void OnParticleCollision(GameObject other) {          
		TakeDame (Ramboat2DPlayerController.Intance.gunPower);
	}

	void SetGunType(int gunType,GameObject obj){
		GameObject gift= Instantiate (Resources.Load ("Prefabs/Gift/TypeGun"),transform.position,Quaternion.identity) as GameObject;
		gift.GetComponent<SpriteRenderer> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite[gunType];
		gift.GetComponent<ItemGiftMove> ().gunType = gunType;
		Destroy (obj, 1f);
		gameObject.transform.parent.gameObject.SetActive (false);

	}
	void CreateClover(){
		GameObject gift= Instantiate (Resources.Load ("Prefabs/Gift/Clover"),transform.position,Quaternion.identity) as GameObject;
		gameObject.transform.parent.gameObject.SetActive (false);
	}
	void CreatePocker(){
		GameObject gift= Instantiate (Resources.Load ("Prefabs/Gift/Pocker"),transform.position,Quaternion.identity) as GameObject;
		gameObject.transform.parent.gameObject.SetActive (false);
	}
}
