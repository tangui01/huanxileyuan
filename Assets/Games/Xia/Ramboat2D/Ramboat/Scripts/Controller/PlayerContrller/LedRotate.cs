using UnityEngine;
using System.Collections;

public class LedRotate : MonoBehaviour {
	float angleRotation=0;
	float angleZStart=0;
	public float speedAngle=5;
	public float maxAngle=7;
	// Use this for initialization
	void Start () {
		angleZStart = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		angleRotation = angleZStart+Mathf.Sin( Time.time * speedAngle) * maxAngle;
		transform.rotation = Quaternion.Euler(0, 0,angleRotation);
	}
}
