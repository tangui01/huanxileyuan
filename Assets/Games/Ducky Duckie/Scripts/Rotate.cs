using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public GameObject Kaaba;

    private Vector3 touchPosition;
    private bool canMove;
    public float coefDiminish;

	// Use this for initialization
	void Start () {
        canMove = false;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetMouseButtonDown(0) && !canMove)
        {
            touchPosition = Input.mousePosition;
            canMove = true;
        } else if(Input.GetMouseButtonUp(0))
        {
            canMove = false;
        }

        if(canMove)
            MovePlatform();

	}

    void RotateKaaba()
    {
        //Rotate Horizontally
        Kaaba.transform.RotateAround(Vector3.zero, Vector3.up, 
            (touchPosition.x - Input.mousePosition.x) * 
            coefDiminish);

        //Rotate Vertically
        Kaaba.transform.RotateAround(Vector3.zero, Vector3.right,
            (Input.mousePosition.y - touchPosition.y) *
            coefDiminish);
    }

    void MovePlatform()
    {

    }
}
