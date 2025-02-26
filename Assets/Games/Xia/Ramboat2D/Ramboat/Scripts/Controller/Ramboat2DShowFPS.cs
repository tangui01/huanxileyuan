using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Ramboat2DShowFPS : MonoBehaviour {
	float deltaTime;
	public Text SHOW;
	// Use this for initialization
	void Start () {
		deltaTime = 0;

	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		SHOW.text ="FPS : "+ fps.ToString();
	}
}
