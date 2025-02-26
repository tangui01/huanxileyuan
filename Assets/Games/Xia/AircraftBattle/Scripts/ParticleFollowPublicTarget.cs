using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleFollowPublicTarget : MonoBehaviour {

	public Transform FinishObject; 
	public float speed = 10;
	ParticleSystem.Particle[] p;

	// Use this for initialization
	void Start () {

		Vector3 op = transform.position-FinishObject.position;
		GetComponent<ParticleSystem>().startLifetime = Mathf.Sqrt(Mathf.Pow(op.x,2)+Mathf.Pow(op.y,2))/speed;
	}
	
	// Update is called once per frame
	void Update () {
		Trail ();
	}

	void Trail (){
		 p = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount+1];
		int l = GetComponent<ParticleSystem>().GetParticles(p);
		Vector3 D1 = FinishObject.position - transform.position;
		for(int i=0; i<l;i++)
		{
			if (p[i].remainingLifetime<(p[i].startLifetime)){
				p[i].velocity = D1 / p[i].startLifetime ;
			}
		}

		GetComponent<ParticleSystem>().SetParticles(p, l); 
	}


}
