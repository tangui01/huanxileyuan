using UnityEngine;
using System.Collections;

public class ParticleSorting : MonoBehaviour {
    public int sortingOrder = 1;
	// Use this for initialization
	void Start () {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder+40;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
