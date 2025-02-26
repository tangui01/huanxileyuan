using UnityEngine;
using System.Collections;

public class Ramboat2DAspectCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// EnableCamera ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void EnableCamera ()
	{
		float aspect = (float)Screen.width / (float)Screen.height;

		float aspects = (float)Mathf.Round (aspect * 100) / 100f;
		if (aspects == 1.67f)
			GetComponent<Camera> ().orthographicSize = 3.68f;    //16:9
		else if (aspects == 1.78f)
			GetComponent<Camera> ().orthographicSize = 3.45f;                  //3:2
		else if (aspects == 1.71f)
			GetComponent<Camera> ().orthographicSize = 3.55f;                  //4:3
		else if (aspects == 1.6f)
			GetComponent<Camera> ().orthographicSize = 3.65f;                  //5:3
		else if (aspects == 1.5f)
			GetComponent<Camera> ().orthographicSize = 3.6f;
		else
			GetComponent<Camera> ().orthographicSize = 3.65f;


		//Debug.Log (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0f,0f)));
	}
}
