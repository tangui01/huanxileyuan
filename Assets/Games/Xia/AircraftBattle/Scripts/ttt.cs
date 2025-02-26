using UnityEngine;
using System.Collections;

public class ttt : MonoBehaviour {

	public Transform Target;
	public float MaxSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//First we get the direction we need to travel in
		Vector2 direction = (Target.position - transform.position).normalized;
		
		//Multiply it by the maximum speed we're trying to reach
		Vector2 desiredVelocity = direction * MaxSpeed;
		
		//Subtract the current velocity. This is the calibration force
		Vector2 steeringForce = desiredVelocity - GetComponent<Rigidbody2D>().velocity;
		
		//Apply the steering. The less the mass, the more effective the steering
		GetComponent<Rigidbody2D>().AddRelativeForce (steeringForce);
	}
}
