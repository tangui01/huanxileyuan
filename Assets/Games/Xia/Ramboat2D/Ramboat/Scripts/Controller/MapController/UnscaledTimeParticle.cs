using UnityEngine;
using System.Collections;

public class UnscaledTimeParticle : MonoBehaviour
{
	ParticleSystem par;
	void Start(){
		par = gameObject.GetComponent<ParticleSystem> ();
	}
	// Update is called once per frame
	void Update()
	{
		if (Time.timeScale < 0.01f)
		{
			par.Simulate(Time.unscaledDeltaTime,true,false);
		}
	}
}