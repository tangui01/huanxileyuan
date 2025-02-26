using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repell : MonoBehaviour {

    public bool isInPipe = false;
    public float repellForce;

    GameObject toRepell;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isInPipe)
            RepellBall();
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            toRepell = collision.transform.gameObject;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Pipe")
        {
            isInPipe = true;
        }
    }

    void RepellBall()
    {
        if(toRepell != null)
        {
            //First we need to get the direction between this position and the other ball pos
            Vector3 dir = toRepell.transform.position - transform.position;

            toRepell.GetComponent<Rigidbody2D>().AddForce(dir * repellForce);
        }
    }
}
