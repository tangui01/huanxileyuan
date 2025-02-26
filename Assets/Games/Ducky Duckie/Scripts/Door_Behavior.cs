using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Behavior : MonoBehaviour {

    [Header("Some Private Variables")]
    Vector3 BallStoppedPos;
    GameObject FirstBall;

    [Header("Launch Force Values")]
    public float MaxXForce;
    public float MaxYForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            LaunchBall();
        }

	}

    void LaunchBall()
    {
        if(FirstBall != null)
        {
            Debug.Log("Launching");

            //Ignore the collisions between the ball and the door
            FirstBall.GetComponent<CircleCollider2D>().isTrigger = true;

            //Unstuck the ball
            FirstBall.GetComponent<Ball_Behavior>().isStuck = false;

            //Apply a random force
            FirstBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(10, MaxXForce), Random.Range(0, MaxYForce)));

            //Reset the first ball
            FirstBall = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            BallStoppedPos = collision.transform.position;
            FirstBall = collision.transform.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball")
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.position = BallStoppedPos;
        }
    }
}
