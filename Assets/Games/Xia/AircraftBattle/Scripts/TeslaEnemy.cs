using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeslaEnemy : MonoBehaviour {
	Transform LeftPlane,RightPlane;
	bool animPlayed=false;
//	List<Vector2> newVerticies = new List<Vector2>();
	// Use this for initialization
	void Start () {
		LeftPlane = transform.Find("EnemyTeslaHeadLeft/AnimationHolder/RocketKamikazeHolder");
		RightPlane = transform.Find("EnemyTeslaHeadRight/AnimationHolder/RocketKamikazeHolder");
	}
	
	// Update is called once per frame
	void Update () {
		TeslaActive();
	}

	void TeslaActive()
	{

			
		if(LeftPlane.gameObject.activeSelf && !RightPlane.gameObject.activeSelf && !animPlayed)
		{
			animPlayed=true;
			transform.GetComponent<Animation>().Play("TeslaDeathRight");
		}
		else if(RightPlane.gameObject.activeSelf && !LeftPlane.gameObject.activeSelf && !animPlayed)
		{
			animPlayed=true;
			transform.GetComponent<Animation>().Play("TeslaDeathLeft");
		}
		else if(!RightPlane.gameObject.activeSelf && !LeftPlane.gameObject.activeSelf && !animPlayed)
		{
			animPlayed=true;
			transform.GetComponent<Animation>().Play("TeslaDeathLeft");
		}



		
	}
}
