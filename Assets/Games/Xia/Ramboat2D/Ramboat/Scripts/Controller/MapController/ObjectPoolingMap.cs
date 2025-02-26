using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolingMap : MonoBehaviour {
	float speedBoat=7;
	public float speedDivide=1;
	public float startPoint;
	public float lenghtSprite;
	//	public string nameMap;

	public Ramboat2DGameState testState=Ramboat2DGameState.PrepareGame;
	//GameObject[] list=new GameObject[2];
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Ramboat2DLevelManager.THIS.startingGame && !Ramboat2DPlayerController.Intance.playerDead) {
			MoveMap ();
			NextMapState ();
		}

	}
	void MoveMap(){

		if (transform.position.x <= -lenghtSprite && Ramboat2DLevelManager.THIS.gameState!=Ramboat2DGameState.NextState) {
			if (gameObject.tag=="Transition") {
				Ramboat2DLevelManager.THIS.checkGameState += 1;
				Destroy (this.gameObject);

			}
			GameObject[] list = GameObject.FindGameObjectsWithTag (gameObject.tag);
			for(int i=0;i<list.Length;i++){
				if (list[i].transform.position.x >= -1) {
					startPoint = list[i].transform.position.x;
					break;
				}
			}
			transform.position=new Vector3(startPoint+lenghtSprite,0,0);
		}

		transform.position -= new Vector3(((Ramboat2DPlayerController.Intance.gameObject.transform.position.x/7+speedBoat)/speedDivide)*Time.deltaTime,0,0);

	}
	void NextMapState(){

		if (transform.position.x <= -lenghtSprite && Ramboat2DLevelManager.THIS.gameState == Ramboat2DGameState.NextState && gameObject.tag == ("FirstMap"+(Ramboat2DLevelManager.THIS.level-1))) {
			Ramboat2DLevelManager.THIS.mapCounterDestroyed += 1;
			if (Ramboat2DLevelManager.THIS.mapCounterDestroyed == 1) {
				Ramboat2DLevelManager.THIS.gameMapState [Ramboat2DLevelManager.THIS.level].SetActive (true);
				gameObject.SetActive (false);
			}
			if (Ramboat2DLevelManager.THIS.mapCounterDestroyed == 2) {
				GameObject[] maps = GameObject.FindGameObjectsWithTag ("Map" + (Ramboat2DLevelManager.THIS.level));
				for (int i = 0; i < maps.Length; i++) {
					maps [i].GetComponent<ObjectPoolingMap> ().speedDivide = 5f;
				}
				Ramboat2DLevelManager.THIS.gameMapState [Ramboat2DLevelManager.THIS.level-1].SetActive (false);
				Ramboat2DLevelManager.THIS.mapCounterDestroyed = 0;
			}


		} else {
			if (transform.position.x <= -lenghtSprite) {
				if (gameObject.tag=="Transition") {
					Ramboat2DLevelManager.THIS.checkGameState += 1;
					if (Ramboat2DLevelManager.THIS.checkGameState % 2 == 1)
						Ramboat2DLevelManager.THIS.gameState = Ramboat2DGameState.NextState;
					if(Ramboat2DLevelManager.THIS.checkGameState%2==0)
						Ramboat2DLevelManager.THIS.gameState = Ramboat2DGameState.Playing;
					Destroy (this.gameObject);

				}
				GameObject[] list = GameObject.FindGameObjectsWithTag (gameObject.tag);
				for(int i=0;i<list.Length;i++){
					if (list[i].transform.position.x > -1) {
						startPoint = list[i].transform.position.x;
						break;
					}
				}
				transform.position=new Vector3(startPoint+lenghtSprite-0.1f,0,0);
			}

			transform.position -= new Vector3(((Ramboat2DPlayerController.Intance.gameObject.transform.position.x/7+speedBoat)/speedDivide)*Time.deltaTime,0,0);
		}
	}
}