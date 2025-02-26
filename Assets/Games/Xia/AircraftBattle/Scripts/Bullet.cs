using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public bool available = true;
	public bool initialized = false;
	public static int speed = 70;//25, pa 45

	void Start () 
	{
		
	}
	
	void Update () 
	{
		if(available && initialized)
		{
			available = false;
			initialized = false;
			transform.position = PlaneManager.Instance.firePosition.position;
			transform.GetChild(0).GetComponent<Animation>().Play();
			transform.GetChild(1).GetComponent<Animation>().Play();
		}
		if(!available && transform.position.y > PlaneManager.Instance.boundUp)
		{
			available = true;
			transform.localPosition = new Vector3(-35f,0,0);
			transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
			//Invoke("PlayBulletLaunchAnimation",0.5f);
		}
		else if(!available)
		{
			transform.Translate(Vector3.up*Time.deltaTime*speed);
		}
	}

	void PlayBulletLaunchAnimation()
	{
		transform.GetChild(0).GetComponent<Animation>().Play();
		transform.GetChild(1).GetComponent<Animation>().Play();
	}

}
