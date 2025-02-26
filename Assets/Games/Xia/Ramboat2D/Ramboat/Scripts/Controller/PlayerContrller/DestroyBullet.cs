using UnityEngine;
using System.Collections;

public class DestroyBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet" || other.tag=="BulletEnemy"  || other.tag=="GunBulletEnemy" || other.tag=="Coin" || other.tag=="Dollar" || other.tag=="Enemy" || other.tag=="Bomb") {
			other.gameObject.SetActive (false);
		}
		if (other.tag == "BulletSubmarine") {
			other.transform.GetChild (0).gameObject.SetActive (false);
			Destroy (other.gameObject);
			Resources.UnloadUnusedAssets (); 
			System.GC.Collect();
		}
		if (other.tag == "Pocker"|| other.tag=="Clover" || other.tag=="GunType") {
			Destroy (other.gameObject);
			Resources.UnloadUnusedAssets (); 
			System.GC.Collect ();;
		}
	}
}
