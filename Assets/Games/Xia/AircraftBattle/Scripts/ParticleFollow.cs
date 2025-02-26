using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleFollow : MonoBehaviour {

	Transform FinishObject; 
	public float speed = 10;
	ParticleSystem.Particle[] p;
	public float angle;

	// Use this for initialization
	void Start () {
		string name = transform.name;
		string index = transform.name;
		index = index.Substring(index.Length-1,1);
		name=name.Remove(name.Length-1);
		int intIndex=int.Parse(index)+1;
		FinishObject = transform.parent.Find(name+intIndex).transform;
		Vector3 op = transform.position-FinishObject.position;
		GetComponent<ParticleSystem>().startLifetime = Mathf.Sqrt(Mathf.Pow(op.x,2)+Mathf.Pow(op.y,2))/speed;
//		Vector3 test = FinishObject.transform.position-transform.position;
//		test = FinishObject.transform.InverseTransformDirection(test);
//		angle = Mathf.Atan2(test.x, test.y);
//		angle = (Mathf.Atan2(FinishObject.transform.position.y-transform.position.y, FinishObject.transform.position.x-transform.position.x)*Mathf.Rad2Deg)-Mathf.PI/2;
//		particleSystem.startRotation = angle*Mathf.Deg2Rad;
//		Debug.Log(" BROJKA ZA "+transform.name+" je "+angle);
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
