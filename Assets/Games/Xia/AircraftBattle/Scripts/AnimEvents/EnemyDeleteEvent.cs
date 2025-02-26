using UnityEngine;
using System.Collections;

public class EnemyDeleteEvent : MonoBehaviour {

	void DeleteThisObject()
	{
		Destroy(this.gameObject);
	}
}
