using UnityEngine;
using System.Collections;

public class WaterCollision : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "GunBulletEnemy") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bulletHitWater [Random.Range (0, 3)]);
			other.gameObject.SetActive (false);
			HitWater(other.gameObject,13,0.5f);
		}
		if (other.tag == "Dollar") {
			other.gameObject.SetActive (false);
			HitWater(other.gameObject,13,0.5f);
		}
		if (other.tag == "Coin") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot(Ramboat2DFXSound.THIS.waterCoinDrop[Random.Range(0,2)]);
			HitWater(other.gameObject,13,0.5f);
		}
		if (other.tag == "BulletEnemy") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.expolision [Random.Range (0, 3)]);
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.waterSplashBig [Random.Range (0, 3)]);
			HitWater(other.gameObject,14,1.5f);
			GameObject par = other.gameObject.transform.GetChild (0).gameObject;
			par.GetComponent<ParticleSystem> ().Stop ();
			GameObject explosion = other.gameObject.transform.GetChild (1).gameObject;
			explosion.transform.eulerAngles = new Vector3 (0, 0, 0);
			explosion.SetActive (true);
			explosion.transform.parent = null;
			par.transform.parent = null;
			Destroy (other.gameObject);
			Destroy (par, 1f);
			Destroy (explosion, 1f);
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "BulletSubmarine") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.waterSplashBig [Random.Range (0, 3)]);
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.boatShot);
			HitWater (other.gameObject, 14, 1.5f);
			other.transform.GetChild (0).gameObject.SetActive (true);
		}
	}


	void HitWater(GameObject other,int choiceEffectHit,float timePlay){
		StartCoroutine (CreateHitWater (other,choiceEffectHit,timePlay));
	}
	IEnumerator CreateHitWater(GameObject other,int choiceEffectHit,float timePlay){
		GameObject hit = Ramboat2DLevelManager.THIS.GetPooledObject (choiceEffectHit);
		if (hit != null) {
			hit.transform.position = new Vector3 (other.transform.position.x,other.transform.position.y-0.1f,0);
			hit.SetActive (true);
		}
		yield return new WaitForSeconds (timePlay);
		hit.SetActive (false);
	}
}
