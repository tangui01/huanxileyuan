using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Behavior : MonoBehaviour {

    public bool isStuck = false;
    Vector3 stuckPos;
    Vector3 otherBallPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isStuck)
        {
            transform.position = stuckPos;
        }

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            if (collision.transform.GetComponent<Ball_Behavior>().isStuck)
            {
                otherBallPos = collision.transform.position;
                Vector3 offset = -transform.position + otherBallPos;

                stuckPos = transform.position - offset/2;
                isStuck = true;

                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Door")
        {
            if (!isStuck)
            {
                stuckPos = transform.position;
                isStuck = true;
            }
        } else if(collision.transform.tag == "TriggerDoor")
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }
}
