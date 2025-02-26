using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    [Header("Some Snake Variables & Storing")]
    public List<Transform> BodyParts = new List<Transform>();
    public float minDistance = 0.25f;
    public int initialAmount;
    public float speed = 1;
    public float rotationSpeed = 50;
    public float LerpTimeX;
    public float LerpTimeY;

    [Header("Path Management")]
    public Waypoint[] Waypoints;
    bool[] waypointsReached;

    [Header("Launch Settings")]
    public float maxYForce;
    public float maxXForce;

    //[Header("Snake Head Prefab")]
    //public GameObject BodyPrefab;

    [Header("Private Fields")]
    private float distance;

    private Transform curBodyPart;
    private Transform prevBodyPart;

    private bool firstPart;

	// Use this for initialization
	void Start () {

        //Initialize the waypoints triggers
        waypointsReached = new bool[Waypoints.Length];

        for (int i = 0; i < waypointsReached.Length; i++)
        {
            waypointsReached[i] = false;
        }

        //At the beginning, if it's the first part, spawn it at (0, -3, 0)
        firstPart = true;

        //Add the initial BodyParts
        for (int i = 0; i < initialAmount; i++)
        {
            //Use invoke to avoid a weird bug where the snake goes down at the beginning.
            Invoke("AddBodyPart", 0.1f);
        }
        
	}

    public void SpawnBodyParts()
    {
        firstPart = true;

        //Add the initial BodyParts
        for (int i = 0; i < initialAmount; i++)
        {
            //Use invoke to avoid a weird bug where the snake goes down at the beginning.
            Invoke("AddBodyPart", 0.1f);
        }
    }
	    
	// Update is called once per frame
	void Update () {

        
        Move();

        if(Input.GetMouseButtonDown(0) && BodyParts.Count > 0)
            LaunchBall();

      
    }

    public void LaunchBall()
    {
        if (BodyParts[0].GetComponent<Mover>().reached)
        {
            //Unparent the ball
            BodyParts[0].parent = null;

            //Restore gravity
            BodyParts[0].GetComponent<Rigidbody2D>().gravityScale = 1;

            //Delete the mover Script
            //BodyParts[0].GetComponent<Mover>().enabled = false;
            Destroy(BodyParts[0].GetComponent<Mover>());

            //Apply a random force
            BodyParts[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(10, maxXForce), Random.Range(0, maxYForce)));

            //Set the next ball as the head
            BodyParts.RemoveAt(0);

            //Associate the mover script to the next ball
            AssociateMoverScript(BodyParts[0].gameObject);
        }
    }

    public void Move()
    {
        float curSpeed = speed;

        //Move the Head to the next waypoint
        if(BodyParts.Count > 0) {


            //Move the other body parts depending on the Head, that's why we start the loop at 1
            for (int i = 1; i < BodyParts.Count; i++)
            {
                curBodyPart = BodyParts[i];
                prevBodyPart = BodyParts[i - 1];

                distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                Vector3 newPos = prevBodyPart.position;

                newPos.z = BodyParts[0].position.z;

                if (distance > minDistance)
                {

                    //Try 2 Lerps, one on the x pos and one on the Y
                    Vector3 pos = curBodyPart.position;

                    pos.x = Mathf.Lerp(pos.x, newPos.x, LerpTimeX);
                    pos.y = Mathf.Lerp(pos.y, newPos.y, LerpTimeY);

                    curBodyPart.position = pos;
                }
            }

        }
    }


    public void AddBodyPart(Transform partToAdd)
    {

        //Set the ball child of the snake
        partToAdd.SetParent(transform);

        if (firstPart)
        {
            //Add the body part to the array
            BodyParts.Add(partToAdd);

            //Associate the Mover Script with this part as it is the head
            AssociateMoverScript(partToAdd.gameObject);

            //Then disable the firstPart
            firstPart = false;
        }

        else
        {

            //Add the body part to the array
            BodyParts.Add(partToAdd);
        }

    }

    void AssociateMoverScript(GameObject head)
    {
        //Add the mover script
        head.AddComponent<Mover>();

        //Add the waypoints
        head.GetComponent<Mover>().wayPoints = Waypoints;

        //Change the speed
        head.GetComponent<Mover>().speed = 3;
    }
}
