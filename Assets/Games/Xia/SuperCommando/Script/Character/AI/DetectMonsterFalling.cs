using UnityEngine;
using System.Collections;

public class DetectMonsterFalling : MonoBehaviour {
	public Rigidbody2D monsterIV;
	public AudioClip soundShowUp;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			SuperCommandoSoundManager.Instance.PlaySfx (soundShowUp);
			monsterIV.isKinematic = false;
//			Destroy (gameObject);
		}
	}
}
