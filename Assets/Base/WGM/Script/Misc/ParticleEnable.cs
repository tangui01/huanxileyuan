using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleEnable : MonoBehaviour {

	public bool enable = true;

	protected bool preState;

	// Use this for initialization
	void Start () {
		preState = enable;
	}
	
	// Update is called once per frame
	void Update () {
		if(preState != enable) {
			preState = enable;
			ParticleSystem[] systems = transform.GetComponentsInChildren<ParticleSystem>(true);
			foreach(ParticleSystem system in systems) {
				system.gameObject.SetActive(enable);
			}
		}
	}
}
