using UnityEngine;
using System.Collections;

public class EnemyTriggerEvent : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag.Equals("PlayerBullet"))
		{
			gameObject.SetActive(false);
			col.gameObject.SetActive(false);
		}
		else
		{
//			Debug.Log("ELSE "+col.tag);
		}
	}
}
