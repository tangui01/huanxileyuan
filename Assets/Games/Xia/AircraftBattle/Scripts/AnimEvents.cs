using UnityEngine;
using System.Collections;

public class AnimEvents : MonoBehaviour {

	void DisableObject()
	{
		gameObject.SetActive(false);
	}

	void DisableParentObject()
	{
		transform.parent.gameObject.SetActive(false);
	}
}
