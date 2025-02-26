using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBallToSnakeTrigger : MonoBehaviour {

    public SnakeMovement SN;

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
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;

            //Add the ball to the snake
            SN.AddBodyPart(collision.transform);
        }
    }
}
