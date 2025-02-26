using UnityEngine;
using System.Collections;

public class BulletBoatController : MonoBehaviour {
	GameObject player;
	Vector3 velocity;
	Rigidbody2D bullet;
	bool shoot=false;
	public float v0=10f;
	bool start=false;
	void Start(){
		bullet = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}




	void Update(){
		if (!shoot) {
			float distance = Vector3.Distance (transform.position,player.transform.position);
			float angle =90-( Mathf.Asin ((distance * -Physics2D.gravity.y) / Mathf.Pow (v0, 2)) / Mathf.PI * 90);
			velocity.x = -v0* Mathf.Cos(angle*Mathf.PI/180);
			velocity.y = v0 * Mathf.Sin (angle*Mathf.PI/180);
			bullet.velocity = velocity;
			shoot = true;
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.boatShot);
		}
		if(shoot) {
			float angle = Mathf.Abs (Vector3.Angle (Vector3.up, bullet.velocity.normalized))+90;
			Vector3 temp = transform.eulerAngles;
			temp.z = angle;
			transform.eulerAngles = temp;
		}
	}

}
