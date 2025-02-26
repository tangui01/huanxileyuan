using UnityEngine;
using System.Collections;

public class DrawCurve : MonoBehaviour {

	public GameObject start;
	public GameObject middle;
	public GameObject end ;
	public GameObject Raketa;
	bool gotovo=false,pozvana=false;
	Vector3[] ListaTacaka = new Vector3[20];
	public Color color = Color.white;
	public float width = 0.2f;
	public int numberOfPoints = 20;
	LineRenderer lineRenderer;
	Vector3 p0, p1, p2;
	void Start() 
	{
		// initialize line renderer component
		lineRenderer = GetComponent<LineRenderer>();
		if (null == lineRenderer)
		{
			gameObject.AddComponent<LineRenderer>();
		}
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = true;
		lineRenderer.material = new Material(
			Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(color, color);
		lineRenderer.SetWidth(width, width);
		p0 = start.transform.position;
		p1 = middle.transform.position;
		Pozovi ();
	}
	
	void Update() 
	{

				// check parameters and components
				if (null == lineRenderer || null == start 
						|| null == middle || null == end) {
						return; // no points specified
				} 
		
				// update line renderer


				if (numberOfPoints > 0) {
						lineRenderer.SetVertexCount (numberOfPoints);
				}
		
				// set points of quadratic Bezier curve

				p2 = end.transform.position;
				double t; 
				Vector3 position;
				for (int i = 0; i < numberOfPoints; i++) {
						t = i / (numberOfPoints - 1.0);
						position = new Vector3 (
				(float)((1.0 - t) * (1.0 - t) * p0.x + 2.0 * (1.0 - t) * t * p1.x + t * t * p2.x),
				(float)((1.0 - t) * (1.0 - t) * p0.y + 2.0 * (1.0 - t) * t * p1.y + t * t * p2.y),
				(float)((1.0 - t) * (1.0 - t) * p0.z + 2.0 * (1.0 - t) * t * p1.z + t * t * p2.z)
						);
						lineRenderer.SetPosition (i, position);
				ListaTacaka[i] = position;
//			Debug.Log("Broj je "+i+" a pozicija je "+position+ " a proba je ");
				}
				



	}

	void Pozovi()
	{
		StartCoroutine("Test");
	}
	IEnumerator Test()
	{
		yield return new WaitForSeconds(1.2f);
		pozvana=true;
		for(int i=0;i<ListaTacaka.Length;i++)
		{
			Raketa.transform.position = ListaTacaka[i];
//			Vector3 dir = Raketa.transform.position - end.transform.position;
//			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg-90;
//			Raketa.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

//			Raketa.transform.rotation = Quaternion.LookRotation(Vector3.forward, end.transform.position - Raketa.transform.position);
			Raketa.transform.LookAt(Raketa.transform.position+new Vector3(0,0,1),end.transform.position-Raketa.transform.position);
//			Vector3 dir = Raketa.transform.position - end.transform.position;
//			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg-90;
//			Raketa.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward),Time.deltaTime*2);
			yield return new WaitForSeconds(0.05f);
		}
	}

//	function Bezier2(Start : Vector2, Control : Vector2, End : Vector2 , t :float) : Vector2
//	{
//		return (((1-t)*(1-t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
//	}
//	
//	function Bezier2(Start : Vector3, Control : Vector3, End : Vector3 , t :float) : Vector3
//	{
//		return (((1-t)*(1-t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
//	}
//	
//	function Bezier3(s : Vector2, st : Vector2, et : Vector2, e : Vector2, t : float) : Vector2
//	{
//		return (((-s + 3*(st-et) + e)* t + (3*(s+et) - 6*st))* t + 3*(st-s))* t + s;
//	}
//	
//	function Bezier3(s : Vector3, st : Vector3, et : Vector3, e : Vector3, t : float) : Vector3
//	{
//		return (((-s + 3*(st-et) + e)* t + (3*(s+et) - 6*st))* t + 3*(st-s))* t + s;
//	}
}
