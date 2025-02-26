using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class Laser2D : MonoBehaviour {

	private LineRenderer lr;
	public float z = -1;
	public float startWidth, endWidth;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();
		lr.SetWidth(startWidth,endWidth);
		lr.SetPosition(0, new Vector3(0,0, z));
		lr.SetPosition(0, new Vector3(0,40, z));
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.up, PlaneManager.Instance.boundUp-transform.position.y+5);
	
//		
		if(hit!=null && hit.collider!=null)
		{
			if( hit.transform.tag == "Enemy")
			{
				hit.transform.GetComponent<Enemy>().TakeDamage(100);

			}

				
		}

	}
}
