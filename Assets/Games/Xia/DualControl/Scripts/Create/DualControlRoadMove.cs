using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualControlRoadMove : MonoBehaviour
{
    public Transform carsCenter;
    public float moveDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(carsCenter.position.z - transform.position.z > 2f)
            transform.position += new Vector3(0,0,moveDistance*2);  
    }
}
