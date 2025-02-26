using UnityEngine;
using System.Collections;

public class Blades : MonoBehaviour {
	
	public float speedOfRotation;
	public float speedRotationOfBlades;
	public float diameterOfRotation;
	bool rotationActive = false;
	float speed = 1;
	float startTime;
	float duration = 1;
	int NumberOfChildren;
	// Use this for initialization
	void Start () {
		NumberOfChildren = transform.childCount;
		
				//StartCoroutine("SetStartPositionsOfBlades");
	}

	// Update is called once per frame
	void Update () {
		RotationOfBlades();
	}

	void Activate()
	{
		StartCoroutine("SetStartPositionsOfBlades");
	}
	
	IEnumerator SetStartPositionsOfBlades()
	{
		for(float t=0;t<duration;t+=Time.deltaTime)
		{
			for(int i=0;i<NumberOfChildren;i++)
			{
				transform.GetChild(i).GetComponent<Animation>().Play();
			}
			transform.GetChild(0).transform.localPosition = Vector3.Lerp(transform.GetChild(0).transform.localPosition, new Vector3(-diameterOfRotation,0,0), t/duration);
			transform.GetChild(1).transform.localPosition = Vector3.Lerp(transform.GetChild(1).transform.localPosition, new Vector3(0,diameterOfRotation,0), t/duration);
			transform.GetChild(2).transform.localPosition = Vector3.Lerp(transform.GetChild(2).transform.localPosition, new Vector3(diameterOfRotation,0,0), t/duration);
			transform.GetChild(3).transform.localPosition = Vector3.Lerp(transform.GetChild(3).transform.localPosition, new Vector3(0,-diameterOfRotation,0), t/duration);
			yield return null;
		}
		rotationActive=true;
		StartCoroutine("DeactivateBlades");
	}
	
	void RotationOfBlades()
	{
		if(rotationActive)
		{
			transform.RotateAround(transform.position,Vector3.forward,speedOfRotation*Time.deltaTime);
			//			for(int i=0;i<NumberOfChildren;i++)
			//			{
			//				transform.GetChild(i).transform.RotateAround(transform.GetChild(i).transform.position, Vector3.forward, speedRotationOfBlades*Time.deltaTime);
			//			}
			//			transform.GetChild(0).transform.RotateAround(transform.GetChild(0).transform.position, Vector3.forward, speedRotationOfBlades*Time.deltaTime);
			//			transform.GetChild(1).transform.RotateAround(transform.GetChild(1).transform.position, Vector3.forward, speedRotationOfBlades*Time.deltaTime);
			//			transform.GetChild(2).transform.RotateAround(transform.GetChild(2).transform.position, Vector3.forward, speedRotationOfBlades*Time.deltaTime);
			//			transform.GetChild(3).transform.RotateAround(transform.GetChild(3).transform.position, Vector3.forward, speedRotationOfBlades*Time.deltaTime);
		}
		
	}
	
	IEnumerator DeactivateBlades()
	{
		yield return new WaitForSeconds(PandaPlane.Instance.bladesDuration);
		rotationActive=false;
		for(float t=0;t<duration;t+=Time.deltaTime)
		{
			for(int i=0;i<NumberOfChildren;i++)
			{
				transform.GetChild(i).GetComponent<Animation>().Stop();
			}
			transform.GetChild(0).transform.localPosition = Vector3.Lerp(transform.GetChild(0).transform.localPosition, Vector3.zero, t/duration);
			transform.GetChild(1).transform.localPosition = Vector3.Lerp(transform.GetChild(1).transform.localPosition, Vector3.zero, t/duration);
			transform.GetChild(2).transform.localPosition = Vector3.Lerp(transform.GetChild(2).transform.localPosition, Vector3.zero, t/duration);
			transform.GetChild(3).transform.localPosition = Vector3.Lerp(transform.GetChild(3).transform.localPosition, Vector3.zero, t/duration);
			yield return null;
		}
	}
}
