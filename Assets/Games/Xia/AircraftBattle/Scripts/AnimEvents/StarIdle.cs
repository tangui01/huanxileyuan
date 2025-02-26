using UnityEngine;
using System.Collections;

public class StarIdle : MonoBehaviour {

	void PlayIdle()
	{
		transform.GetComponent<Animation>().Play("StarIdle");
	}
}
