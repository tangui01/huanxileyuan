using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosters_Manager : MonoBehaviour {

    [Header(" Some Delays ")]
    public float DoubleDuckSpawnDelay;
    public float DoubleDuckStayTime;
    float time = 0;
    float stayTime = 0;

    [Header(" Boosters ")]
    public GameObject DoubleDuckBooster;
    bool doubleDuckOn = false;
	// Update is called once per frame
    public static Boosters_Manager instance;
    private void Awake()
    {
        if (instance == null)
        {
           instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update () {

        if (DuckyGameManager.gameOn)
        {
            if (time < DoubleDuckSpawnDelay && !doubleDuckOn)
            {
                time += Time.deltaTime;
            }
            else
            {
                //Set the Double Duck as on
                doubleDuckOn = true;

                //Enable the Double Duck
                DoubleDuckBooster.SetActive(true);

                //Reset counter
                time = 0;
            }
            //If the Double duck is on
            if (doubleDuckOn)
            {
                if (stayTime < DoubleDuckStayTime)
                {
                    stayTime += Time.deltaTime;
                }
                else
                {
                    //Set the Double Duck as off
                    doubleDuckOn = false;
                    //Disable the Double Duck
                    DoubleDuckBooster.SetActive(false);
                    //Reset counter
                    stayTime = 0;
                }

            }
        }
	}

    public bool ISDouble()
    {
        return doubleDuckOn;
    }
}
