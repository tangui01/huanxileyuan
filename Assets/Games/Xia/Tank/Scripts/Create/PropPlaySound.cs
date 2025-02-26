using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class PropPlaySound : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<AudioSource>().volume *= LibWGM.machine.SeVolume / 10;
        GetComponent<AudioSource>().Play();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
