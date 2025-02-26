using UnityEngine;
using System.Collections;

public class BulletAirController : MonoBehaviour {
	Rigidbody2D bullet;
	// Use this for initialization
	void Start () {
		bullet = GetComponent<Rigidbody2D> ();
		bullet.velocity = new Vector3 (1f, 0, 0);
	}
//	void OnEnable(){
//		bullet = GetComponent<Rigidbody2D> ();
//		bullet.velocity = new Vector3 (1f, 0, 0);
//	}
	// Update is called once per frame
	void Update () {
		float angle =Mathf.Abs (Vector3.Angle (Vector3.right, bullet.velocity.normalized))-90;
		Vector3 temp = transform.eulerAngles;
		temp.z = -angle;
		transform.eulerAngles = temp;
	}
//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.tag=="Water") {
//			GameObject par = transform.GetChild (0).gameObject;
//			par.GetComponent<ParticleSystem> ().Stop ();
//			GameObject explosion = transform.GetChild (1).gameObject;
//			explosion.transform.eulerAngles = new Vector3 (0, 0, 0);
//			explosion.SetActive (true);
//			explosion.transform.parent = null;
//			par.transform.parent = null;
//			Destroy (gameObject);
//			Destroy (par, 1f);
//			Destroy (explosion, 1f);
//		}
////		if (other.tag == "Player") {
////			GameObject par = transform.GetChild (0).gameObject;
////			par.GetComponent<ParticleSystem> ().Stop ();
////			GameObject explosion = transform.GetChild (2).gameObject;
////			explosion.transform.eulerAngles = new Vector3 (0, 0, 0);
////			explosion.SetActive (true);
////			explosion.transform.parent = null;
////			par.transform.parent = null;
////			Destroy (gameObject);
////			Destroy (par, 1f);
////			Destroy (explosion, 1f);
////		}
//
//	}
}
