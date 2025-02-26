using UnityEngine;
using System.Collections;

public class AutoDestroyGameObject : MonoBehaviour {
	public float timeDestroy;
	// Use this for initialization
	void Start () {
		DestroyGameObject ();
	}
	public void DestroyGameObject(){
		Destroy (gameObject, timeDestroy);
	}
}
