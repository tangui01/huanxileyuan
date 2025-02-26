using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeslaEnemy1 : MonoBehaviour {
	public float speed;
	Transform FirstPlane,SecondPlane;
//	List<Vector2> newVerticies = new List<Vector2>();
	// Use this for initialization
	void Start () {
		FirstPlane = transform.GetChild(0).Find("AnimationHolder/EnemiePlane");
		SecondPlane = transform.GetChild(1).Find("AnimationHolder/EnemiePlane");
	}
	
	// Update is called once per frame
	void Update () {
		TeslaLine();
	}

	void TeslaLine()
	{

		if(FirstPlane.GetComponent<Enemy>().health>0 && SecondPlane.GetComponent<Enemy>().health<=0)
		{

			SecondPlane.position =  Vector3.Lerp(SecondPlane.position, FirstPlane.position, speed * Time.deltaTime);
//			transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
		}
		else if(SecondPlane.GetComponent<Enemy>().health>0 && FirstPlane.GetComponent<Enemy>().health<=0)
		{
			FirstPlane.position =  Vector3.Lerp(FirstPlane.position, SecondPlane.position, speed * Time.deltaTime);
//			transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
		}
		else
		{
			FirstPlane.position=Vector3.one*555;
			SecondPlane.position=Vector3.one*555;
		}



		FirstPlane.GetComponent<LineRenderer>().SetPosition(0,FirstPlane.position);
		FirstPlane.GetComponent<LineRenderer>().SetPosition(1,SecondPlane.position);
		
		SecondPlane.GetComponent<LineRenderer>().SetPosition(0,SecondPlane.position);
		SecondPlane.GetComponent<LineRenderer>().SetPosition(1,FirstPlane.position);
		
	}
}
