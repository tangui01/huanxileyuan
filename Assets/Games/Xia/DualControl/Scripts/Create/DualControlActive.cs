using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualControlActive : MonoBehaviour
{
    private Transform carsCenter;
    void Start()
    {
        carsCenter = GameObject.Find("CarsCenter").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(carsCenter.position.z - transform.position.z > 5 && gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
