using UnityEngine;
using System.Collections;

public class EnemyInstance : MonoBehaviour {

	void OnBecameVisible()
	{
		transform.GetChild(0).GetComponent<Animation>().Play();
	}

	void OnBecameInvisible()
	{
		transform.GetChild(0).GetComponent<Animation>().Stop();
		transform.GetChild(0).gameObject.SetActive(true);
	}


}
