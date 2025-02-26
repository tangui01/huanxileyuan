using UnityEngine;
using System.Collections;

public class HideAllChildren : MonoBehaviour {

	void Awake()
	{
		foreach(Transform t in transform) {
			t.gameObject.SetActive(false);
		}
	}
}
