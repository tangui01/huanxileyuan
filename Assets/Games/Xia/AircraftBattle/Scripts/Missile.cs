using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	public Transform target;
	public int owner;
	
	public float damage;
	public float range;
	public float rotationRate;
	
//	public void CreateObject(Vector3 start, Transform target, Vector3 facing, int owner, float damage, float range, float speed, float rotationRate) {
//		Missile missile = (Missile)Instantiate(this, start, Quaternion.identity);
//		missile.owner = owner;
//		missile.damage = damage;
//		missile.range = range;
//		missile.rotationRate = rotationRate;
//		missile.rigidbody.velocity = facing.normalized * speed;
//		missile.target = target;
//	}
	
	public void FixedUpdate() {
		range -= GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime;
		if (range <= 0) {
			GameObject.Destroy(gameObject);
		}
		
		if (target == null)
			return;
		
		Vector3 toTarget = target.position - transform.position;
		Vector3 newVelocity = Vector3.RotateTowards(GetComponent<Rigidbody>().velocity, toTarget, rotationRate * Mathf.Deg2Rad * Time.fixedDeltaTime, 0); 
		newVelocity.z = 0;
		
		GetComponent<Rigidbody>().velocity = newVelocity;
//		transform.forward = rigidbody.velocity.normalized;
	}
	
	public void OnTriggerEnter(Collider other) {
//		MonoBehaviour[] monos = other.GetComponents<MonoBehaviour>();
//		foreach (MonoBehaviour mono in monos) {
//			if (typeof(Damagable).IsAssignableFrom(mono.GetType())) {
//				Damagable damaged = (Damagable)mono;
//				if (damaged.GetOwner() >= 0 && damaged.GetOwner() != owner) {
//					damaged.TakeDamage(damage);
//					GameObject.Destroy(this.gameObject);
//				}
//			}
//		}
	}
}
