using UnityEngine;
using System.Collections;

public class SideBullet : MonoBehaviour {

	public bool available = true;
	public bool initialized = false;
	public static int speed = 60;//25, pa 45
	public float angle = 60;
	Vector3 direction;
	Vector3 minusDirection;
	Transform leftBullet;
	Transform rightBullet;
	Transform leftBulletHolder;
	Transform rightBulletHolder;
	
	void Start () 
	{
		leftBulletHolder = transform.GetChild(0);
		rightBulletHolder = transform.GetChild(1);
		leftBullet = transform.GetChild(0).Find("Bullet_L");
		rightBullet = transform.GetChild(1).Find("Bullet_R");
	}
	
	void Update () 
	{
		if(available && initialized)
		{
			available = false;
			initialized = false;
			transform.position = PlaneManager.Instance.sideFirePosition.position;
			transform.GetChild(0).GetComponent<Animation>().Play();
			transform.GetChild(1).GetComponent<Animation>().Play();
		}
		if(!available && (transform.GetChild(0).position.x < PlaneManager.Instance.boundLeft && transform.GetChild(1).position.x > PlaneManager.Instance.boundRight))
		{
			available = true;
			transform.localPosition = new Vector3(-35f,0,0);
			transform.GetChild(0).localPosition = new Vector3(-0.5f,0,0);
			transform.GetChild(1).localPosition = new Vector3(0.5f,0,0);
			transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
			//Invoke("PlayBulletLaunchAnimation",0.5f);
		}
		else if(!available)
		{
			direction = new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad),Mathf.Sin(angle*Mathf.Deg2Rad),0);
			leftBullet.rotation = Quaternion.Euler(0,0,90-angle);
			rightBullet.rotation = Quaternion.Euler(0,0,-90+angle);
			direction.Normalize();
			minusDirection = new Vector3(-direction.x,direction.y,direction.z);
			rightBulletHolder.Translate(direction*Time.deltaTime*speed);
			leftBulletHolder.Translate(minusDirection*Time.deltaTime*speed);
		}
	}
}
