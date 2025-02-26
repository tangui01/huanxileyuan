using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMove : MonoBehaviour
{
    public Transform snakePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        try
        {
            if (snakePos.GetChild(0) != null && snakePos.GetChild(0).transform.position.y - transform.position.y > 20f)
                 transform.position += new Vector3(0f, 13.3f * 4, 0f);
        }
        catch (Exception e)
        {

        }

    }
}
