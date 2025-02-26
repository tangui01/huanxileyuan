using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirLavirint : MonoBehaviour {

	/*AirLavirint - glavni holder ima skriptu AirLavirint
		Ima decu koja mogu da se zovu kako god i da ih ima koliko god, i svako dete mora da ima componentu edge collider. Svaki od njih ima N deteta, na bilo kojoj poziciji, svaki ima LineRenderer, SpriteRenderer, skripte AnimatedTexture i ParticleFollow, kao i ParticleSystem. Bitna stavka je da se tih N deteta moraju zvati na sledeci nacin NekoIme1, pa NekoIme2 i tako redom, gde NekoIme moze biti koji god naziv, sve dok sva deca se tako zovu
	*/
	List<Vector2> newVerticies = new List<Vector2>();
//	List<LineRenderer> listOfLineRenderers = new List<LineRenderer>();
	private EdgeCollider2D col;
	GameObject currentAirHolder;

	// Use this for initialization
	void Start () {

		int numberOfAirHolder = transform.childCount;

		for(int i=0;i<numberOfAirHolder;i++)
		{
			newVerticies.Clear();
			col=null;
			currentAirHolder=null;
			currentAirHolder = transform.GetChild(i).gameObject;
			col = transform.GetChild(i).transform.GetComponent<EdgeCollider2D>();
			int numberOfPillars = currentAirHolder.transform.childCount;
			for(int j=0;j<numberOfPillars;j++)
			{
				if(j<numberOfPillars-1)
				{
					currentAirHolder.transform.GetChild(j).GetComponent<LineRenderer>().SetPosition(0,currentAirHolder.transform.GetChild(j).transform.position);
					currentAirHolder.transform.GetChild(j).GetComponent<LineRenderer>().SetPosition(1,currentAirHolder.transform.GetChild(j+1).transform.position);
				}

				newVerticies.Add(currentAirHolder.transform.GetChild(j).transform.localPosition);
			}
			col.points = newVerticies.ToArray();
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
