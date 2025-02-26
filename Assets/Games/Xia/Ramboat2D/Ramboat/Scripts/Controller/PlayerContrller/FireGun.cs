using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireGun : MonoBehaviour {

	// Use this for initialization
	void OnEnable(){
		gameObject.GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (DisableFireGun ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator DisableFireGun(){
		yield return new WaitForSeconds (0.5f);
		gameObject.GetComponent<ParticleSystem> ().Stop ();
		yield return new WaitForSeconds (0.2f);
		gameObject.SetActive (false);
	}
	void OnParticleCollision(GameObject other) {
		if (other.tag == "Enemy")
		{
			other.transform.parent.GetComponentInChildren<EnemyController>()?.FireGunOnTriggerEnter2D();
			other.transform.parent.GetComponentInChildren<GiftController>()?.FireGunOnTriggerEnter2D();
			other.transform.parent.GetComponent<BoatController>()?.FireGunOnTriggerEnter2D();
			other.transform.parent.GetComponent<AirController>()?.FireGunOnTriggerEnter2D();
			other.transform.parent.GetComponent<AirFollowController>()?.FireGunOnTriggerEnter2D();
		}
		if (other.tag == "Bomb")
		{
			other.GetComponent<BombController>()?.FireGunOnTriggerEnter2D();
		}
		
	}
}
