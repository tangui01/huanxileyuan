using UnityEngine;
using System.Collections;

public class ShipTurretBullet : MonoBehaviour {

	public bool available = true;
	public bool initialized = false;
	public static float speed = 5f;//25
	[HideInInspector] public Vector3 direction;
	[HideInInspector] public int damage;
	Transform mainCameraPosition;
	float distance;
	Vector3 targetPosition;
	
	void Start () 
	{
		mainCameraPosition = Camera.main.transform;
	}
	
	//	void Update () 
	//	{
	//		if(available && initialized)
	//		{
	//			available = false;
	//			initialized = false;
	//			transform.position = shootingPoint.position;
	//			//transform.GetChild(0).animation.Play();
	//			//transform.GetChild(1).animation.Play();
	//		}
	//		if(!available && transform.position.y > PlaneManager.Instance.boundUp)
	//		{
	//			available = true;
	//			transform.localPosition = Vector3.zero;
	//			transform.gameObject.SetActive(true);
	//			transform.gameObject.SetActive(true);
	//			//Invoke("PlayBulletLaunchAnimation",0.5f);
	//		}
	//		else if(!available)
	//		{
	//			transform.Translate(Vector3.up*Time.deltaTime*speed);
	//		}
	//	}
	public void FireBullet()
	{
		StartCoroutine(BulletFired());
		Invoke("DisableBullet", 7f);
	}
	
	IEnumerator BulletFired()
	{
		targetPosition = PlaneManager.Instance.transform.position+new Vector3(0,0,-15);
		direction = targetPosition - transform.position;
		distance = Vector3.Distance(targetPosition.normalized,transform.position.normalized);
		direction.Normalize();
		direction = new Vector3(direction.x*1/distance,direction.y*1/distance,direction.z);
		
		while(!available && gameObject.activeSelf)
		{
			yield return null;
			//transform.Translate(-direction.normalized * Time.deltaTime * speed);
			transform.Translate(direction.normalized*Time.deltaTime*speed);
		}
	}
	
	void DisableBullet()
	{
		if(!available)
		{
			//available = true;
			//transform.localPosition = Vector3.zero;
			//if(!gameObject.activeSelf)
			//	gameObject.SetActive(true);
			//Destroy(gameObject);
		}
	}
	
	void OnBecameInvisible()
	{
		if(!available)
		{
			if(transform.gameObject != null && mainCameraPosition != null)
			{
				if(transform.position.y < mainCameraPosition.position.y - 12f)
					Destroy(gameObject);
			}
		}
	}

	void ResetBullet()
	{
		if(!available)
			Destroy(gameObject);
	}
}
