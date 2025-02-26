using UnityEngine;
using System.Collections;

public class OP : MonoBehaviour {

	public Transform target;
	public float speed;
	
	public float autoDestroyAfter;
	//prefab for explosion
	public GameObject explosion;
	
	/* Represents the homing sensitivity of the missile.
Range [0.0,1.0] where 0 will disable homing and 1 will make it follow the target like crazy.
This param is fed into the Slerp (defines the interpolation point to pick) */
	float homingSensitivity  = 0.1f;
	
	void Start () {
		StartCoroutine(AutoExplode());
	}
	
	void Update () {
		if(target != null) {
			Vector3 relativePos = target.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos);
			Quaternion test1 = transform.rotation;
			Quaternion test2 = rotation;

			transform.rotation = Quaternion.Slerp(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w), new Quaternion( rotation.x, rotation.y, rotation.z, rotation.w), homingSensitivity);
		}
		
		transform.Translate(0,0,speed * Time.deltaTime,Space.Self);
	}
	
//	void OnTriggerEnter(other: Collider) {
//		//Damage the other gameobject &amp; then destroy self
//		ExplodeSelf();
//	}
	
	private void ExplodeSelf() {
		Instantiate(explosion,transform.position,Quaternion.identity);
		Destroy(gameObject);
	}
	
	IEnumerator AutoExplode() {
		yield return new WaitForSeconds(autoDestroyAfter);
		ExplodeSelf();
	}
}
