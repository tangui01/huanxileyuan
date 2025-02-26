using UnityEngine;
using System.Collections;

public class OffsetTexture : MonoBehaviour {

	// Use this for initialization
	public float offsetSpeedX = 0;
	public float offsetSpeedY = 25;
	//float pos = 0; 

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
//		pos += Time.deltaTime/offsetSpeedY;
//		if(pos > 1.0f)
//			pos -= 1.0f;
//
//		renderer.material.mainTextureOffset = new Vector2(0,pos);

		if(GetComponent<Renderer>().material.mainTextureOffset.y >= 1)
			GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
		if(GetComponent<Renderer>().material.mainTextureOffset.x >= 1)
			GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
		if(offsetSpeedX != 0)
			GetComponent<Renderer>().material.mainTextureOffset += new Vector2(Time.deltaTime/offsetSpeedX,0);
		if(offsetSpeedY != 0)
			GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0,Time.deltaTime/offsetSpeedY);
	}
}
