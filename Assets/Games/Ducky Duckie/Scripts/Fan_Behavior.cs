using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_Behavior : MonoBehaviour {

    public Vector3 ForceDirection;
    public float ForceMagnitude;
    public bool ApplyForceOnce = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            collision.GetComponent<Rigidbody2D>().AddForce(ForceDirection * ForceMagnitude);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!ApplyForceOnce)
        {
            if (collision.transform.tag == "Ball")
            {
                collision.GetComponent<Rigidbody2D>().AddForce(ForceDirection * ForceMagnitude);
            }
        }
    }
}
