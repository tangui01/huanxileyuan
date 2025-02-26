using UnityEngine;
using System.Collections;

public class AutoDisableGameObject : MonoBehaviour {
	public float timeDisable;
	// Use this for initialization
	void OnEnable () {
		StartCoroutine (Disable ());
	}
	
	public IEnumerator Disable(){
		yield return new WaitForSeconds (timeDisable);
		gameObject.SetActive (false);
	}
}
