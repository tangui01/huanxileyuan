using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTag : MonoBehaviour
{
    public string tag;

    private void Awake()
    {
        if(tag != string.Empty)
            gameObject.tag = tag;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
