using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DollarMove : MonoBehaviour {
	GameObject endCoinCollected;
	float MoveSpeed ;
	float frequency;  // Speed of sine movement
	float magnitude;   // Size of sine movement
	private Vector3 axis;
	List<Vector3> bendings;
	List<GameObject> coins;
	private Vector3 pos;

	void OnEnable(){
		endCoinCollected = GameObject.FindGameObjectWithTag ("Finish");
		MoveSpeed = Random.Range (0.4f, 0.7f);
		frequency = Random.Range (2f, 3f);
		magnitude = Random.Range (10f, 20f);
		pos = transform.position;
		axis = transform.right/20;  // May or may not be the axis you want
		bendings=new List<Vector3>();
		coins=new List<GameObject>();
		for(int i=0;i<5;i++){
			Vector3 bending = new Vector3 (i - 2, 0, 0);
			bendings.Add (bending);
		}
	}

	void Update () {
		
			pos -= transform.up * Time.deltaTime * MoveSpeed;
			transform.position = pos + axis * Mathf.Sin (Time.time * frequency) * magnitude;

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Water") {
			gameObject.SetActive (false);
			//tao song nuoc 
		} else if (other.tag == "Player" || other.tag=="Boat") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.dolarCollect);
			for (int i = 0; i < 5; i++) {
				ReadWriteTextMission.THIS.CheckMission (5);
				GameObject obj = Ramboat2DLevelManager.THIS.GetPooledObject (11);
				if (obj != null) {
					obj.transform.position=new Vector3(-1+transform.position.x+i/2f,transform.position.y-(i%2)/3f,0);
					obj.GetComponent<Ramboat2DCoinController> ().moveSpecial = true;
					obj.SetActive (true);

				}
			}
			gameObject.SetActive (false);
		}

	}



}