﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop_Duck_Bar : MonoBehaviour {

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
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
