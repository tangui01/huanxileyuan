using UnityEngine;
using System.Collections;

public class Ramboat2DMusic : MonoBehaviour {
	public static Ramboat2DMusic THIS;

	[HideInInspector] public AudioSource musicAudioSource;

	// Use this for initialization
	public AudioClip[] music;
	void Awake () {
		THIS = this;
	}
}
