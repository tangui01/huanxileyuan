using UnityEngine;
using System.Collections;

public class Ramboat2DCoinController : MonoBehaviour
{
	public bool moveSpecial = false;
	float timeDelay=0.2f;
	GameObject endCoinPositon;
	void OnEnable(){
		endCoinPositon = GameObject.FindGameObjectWithTag ("Finish");
		transform.GetChild (0).gameObject.SetActive(false);
		if (moveSpecial) {
			gameObject.GetComponentInChildren<CircleCollider2D> ().enabled = false;
			gameObject.transform.GetChild (0).gameObject.SetActive (true);
			StartCoroutine (MoveCoin ());
		} else {
			gameObject.GetComponentInChildren<CircleCollider2D> ().enabled = true;
		}
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if ( other.tag=="Boat") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.coinPickup [Random.Range (0, 3)]);
			gameObject.transform.GetChild (0).gameObject.SetActive (true);
			CollectedCoinEffectShow ();
			StartCoroutine (MoveCoin ());
			ReadWriteTextMission.THIS.CheckMission (5);
		}

	}

	public IEnumerator MoveCoin ()
	{
		float startTime = 0;
		Ramboat2DPlayerController.Intance.textCoinCollect.text = (int.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text) + 1).ToString ();
		Ramboat2DPlayerController.Intance.AddScore(Random.Range(2,6));
		while (startTime < 1f) {
			startTime += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, endCoinPositon.transform.position, startTime / 1f);
			yield return new WaitForFixedUpdate ();
		}

		moveSpecial = false;
		gameObject.SetActive (false);

	}
	void CollectedCoinEffectShow(){
		GameObject obj = Ramboat2DLevelManager.THIS.GetPooledObject (15);
		if (obj != null) {
			obj.transform.position = transform.position;
			obj.SetActive (true);
			StartCoroutine(DisableCollectedCoinEffect(obj,timeDelay));
		}
	}
	IEnumerator DisableCollectedCoinEffect(GameObject obj,float timeDelay){
		yield return new WaitForSeconds (timeDelay);
		obj.SetActive (false);
	}

}
