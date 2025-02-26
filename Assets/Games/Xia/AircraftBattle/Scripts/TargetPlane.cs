using UnityEngine;
using System.Collections;

public class TargetPlane : MonoBehaviour {

	GameObject thisGun;

	float speedOfRotation=3;
	// Use this for initialization
	void Start () {
		thisGun = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
		TargetAndFire();
	}

	void TargetAndFire()
	{
		Vector3 targetPos = PlaneManager.Instance.transform.position;
		Vector3 diffPos = targetPos-this.gameObject.transform.position;
		float angle = -Mathf.Atan2(diffPos.y, diffPos.x) * Mathf.Rad2Deg;
		angle -=90;
		Quaternion rot = Quaternion.AngleAxis(angle,  Vector3.back);
//		this.gameObject.transform.localRotation = Quaternion.RotateTowards(this.gameObject.transform.rotation, rot,speedOfRotation);
		this.gameObject.transform.localRotation = Quaternion.RotateTowards(this.gameObject.transform.localRotation, rot,speedOfRotation);
	}
	
}
