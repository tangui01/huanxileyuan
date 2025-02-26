using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck_Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            //Destroy the duck
            Destroy(collision.gameObject);
        }
    }
}
