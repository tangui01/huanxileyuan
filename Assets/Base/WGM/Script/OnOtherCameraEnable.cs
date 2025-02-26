using UnityEngine;
using System.Collections;

public class OnOtherCameraEnable : MonoBehaviour {
    float far;
    int layer;
	// Use this for initialization
	void OnEnable () {
        if (GetComponent<AudioListener>())
        {
            Camera.main.GetComponent<AudioListener>().enabled =false;
        }
        far = Camera.main.farClipPlane;
        layer = Camera.main.cullingMask;
        Camera.main.cullingMask  =0; 
	}
	
	// Update is called once per frame
	void OnDisable () {
        if (GetComponent<AudioListener>())
        {
            Camera.main.GetComponent<AudioListener>().enabled = true;
        }
        Camera.main.cullingMask = layer; 
	}
}
