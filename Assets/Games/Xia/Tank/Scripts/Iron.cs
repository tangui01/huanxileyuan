using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : MonoBehaviour {


    public AudioClip hitIron;
	// Use this for initialization
	

    private void PlayAudio()
    {
        AudioManager.Instance.SetEff(hitIron,1,0);
    }

}
