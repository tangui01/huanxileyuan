using UnityEngine;
using System.Collections;

public class Homming : MonoBehaviour {

	GameObject Target;
	float speedOfRotation=3;
	public float speed = 3;
	// Use this for initialization
	void Start () {
		Target = GameObject.Find("PandaPlaneGameplayHOLDER");

	}
	
	// Update is called once per frame
	void Update () {
		TargetAndFire();
	}

	void TargetAndFire()
	{
		Vector3 targetPos = Target.transform.position;
		Vector3 diffPos = targetPos-this.gameObject.transform.position;
		float angle = -Mathf.Atan2(diffPos.y, diffPos.x) * Mathf.Rad2Deg;
		angle -=90;
		Quaternion rot = Quaternion.AngleAxis(angle,  Vector3.back);
		//		this.gameObject.transform.localRotation = Quaternion.RotateTowards(this.gameObject.transform.rotation, rot,speedOfRotation);
		this.gameObject.transform.localRotation = Quaternion.RotateTowards(this.gameObject.transform.localRotation, rot,speedOfRotation);

		transform.Translate(Vector3.down * Time.deltaTime*speed); //edited
	}

	IEnumerator AutoDestruct()
	{

		yield return new WaitForSeconds(2.5f);
		transform.GetComponent<Animation>().Play("Death");
		SoundManager.Instance.Play_EnemyPlaneExplode();
		yield return new WaitForSeconds(0.2f);
		transform.GetComponent<Homming>().enabled=false;
	}


	public void AvoidAutoDestruction()
	{
		StopCoroutine("AutoDestruct");
	}

	void HommingRocketHelicopterPosition()
	{
		int numberOfHelicopters=transform.parent.parent.childCount-1;
		int randomHelicopter=Random.Range(0,4);
		if(transform.parent.parent.GetChild(randomHelicopter).gameObject.activeSelf)
		{
			transform.parent.transform.localPosition=Vector3.zero;
		}
	}


}
