using UnityEngine;
using System.Collections;

public class PlayCard : MonoBehaviour {
	public static PlayCard THIS;
	public Sprite[] spriteCards;
	public Sprite[] spriteNumber;

	public GameObject button;
	// Use this for initialization
	void Awake(){
		THIS = this;
	}
	void UpDate(){
		
	}
}
