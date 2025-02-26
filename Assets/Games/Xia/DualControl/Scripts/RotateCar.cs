using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;


public class RotateCar : MonoBehaviour
{
    public Vector3 RotateAmount;  // degrees per second to rotate in each axis. Set in inspector.
    float dir;
    int control;
    
    // Start is called before the first frame update
    void Start()
    {
        // int controlActive = PlayerPrefs.GetInt("Control", 2);
        // if (controlActive == 1)
        // {
        //     control = 1;
        // }
        // else
        // {
        //     control = 2;
        // }
        control = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DualControlGameManager.instance.finished)
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                    dir = -0.8f;
                if (Input.GetKey(KeyCode.D))
                    dir = 0.8f;
            } 
#else        
            if (DealCommand.GetKey(1, (AppKeyCode)6) || DealCommand.GetKey(1, (AppKeyCode)1))
            {
                if (DealCommand.GetKey(1, (AppKeyCode)1))
                    dir = 0.8f;
                if (DealCommand.GetKey(1, (AppKeyCode)6))
                    dir = -0.8f;
            } 
#endif                
            else
            {
                dir = 0;
            }
            transform.Rotate(dir * RotateAmount * Time.deltaTime);
        }
    }

    public void DestroyCars()
    {
        transform.GetChild(0).GetComponent<StopRotation>().DestroyCar();
    }
}
