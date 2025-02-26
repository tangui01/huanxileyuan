using UnityEngine;
using System.Collections;

public class BulletSubMarineController : MonoBehaviour {

	float time;
	Vector3 end;

	void Start () {
		end = transform.position + new Vector3 (0, 10, 0);
		time = 0;

	}
	
	// Update is called once per frame
	void Update () {

		time+=Time.deltaTime;
		transform.position = Vector3.Lerp (transform.position, end, time / 70f);
	}
//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.tag == "Player") {
//			GameObject explosion = transform.GetChild (1).gameObject;
//			explosion.transform.eulerAngles = new Vector3 (0, 0, 0);
//			explosion.SetActive (true);
//			explosion.transform.parent = null;
//			Destroy (explosion, 1f);
//			Destroy (gameObject);
//
//		}
//	}
}
