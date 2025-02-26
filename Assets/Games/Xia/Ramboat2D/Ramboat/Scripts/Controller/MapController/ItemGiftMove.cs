using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ItemGiftMove : MonoBehaviour {
	public int gunType;
	Vector3 pointEnd;
	float time = 0;
	private Vector3 velocity = Vector3.zero;
	float distance;
	void Start(){
		
		pointEnd = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).transform.position;
		distance = Vector3.Distance (transform.position, pointEnd);
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player" || other.tag=="Boat") {
			StartCoroutine (MoveGunType ());
		} 

	}

	IEnumerator MoveGunType(){
		float startTime = 0;
		while (startTime < 0.5f) {
			startTime += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, pointEnd, startTime / 0.5f);
			yield return new WaitForFixedUpdate ();
		}
//		while (distance <= 0.01f) {
//			distance=Vector3.Distance (transform.position, pointEnd);
//			transform.position = Vector3.Slerp (transform.position, pointEnd,20*Time.deltaTime);
//			yield return new WaitForFixedUpdate ();
//		}	

		GameObject effectChangeGun = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), pointEnd, Quaternion.identity) as GameObject;
		GameObject.Find ("MenuPlayingGame").transform.GetChild (2).GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [gunType];
		Destroy (effectChangeGun, 0.5f);
		Destroy (gameObject);
		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();

	}
}
