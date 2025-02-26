using UnityEngine;
using System.Collections;

public class SoundParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Stop () {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.swish[0]);
	}
    public void Hit()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.hit);
    }

}
