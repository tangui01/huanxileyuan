using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer.Equals(LayerMask.NameToLayer("EnemyBullet")) || col.gameObject.layer == 9)
		{
			if(col.tag.Equals("MineExplosion"))
			{
				col.enabled = false;

			}
//			else if(col.tag.Equals("Laser"))
//			{
////				col.enabled = false;
//				Debug.Log("TAG LASER");
//			}
			else if(col.tag.Equals("Mace"))
			{
				col.enabled = false;
				StartCoroutine("ActivatecCol",col);
			}
			else
				col.gameObject.SendMessage("ResetBullet",SendMessageOptions.DontRequireReceiver);
			if(tag.Equals("Shield"))
			{
				StartCoroutine(ShieldHit());
			}


		}
	}
	public IEnumerator ShieldHit()
	{
		transform.Find("ShieldHIT").GetComponent<Renderer>().enabled = true;
		yield return null;
//		transform.Find("ShieldHIT").renderer.enabled = false;
		StartCoroutine("ShieldHitOff");
	}

	public IEnumerator ShieldHitOff()
	{
		yield return new WaitForSeconds(0.1f);
		transform.Find("ShieldHIT").GetComponent<Renderer>().enabled = false;
	}

	public IEnumerator ActivatecCol(Collider2D col)
	{
		yield return new WaitForSeconds(1f);
		col.enabled=true;
	}
}
