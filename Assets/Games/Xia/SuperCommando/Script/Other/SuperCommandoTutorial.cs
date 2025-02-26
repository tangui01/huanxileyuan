using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperCommandoTutorial : MonoBehaviour {
	public static SuperCommandoTutorial Instance;

	public Image ImageTut;
	public GameObject Panel;
	public AudioClip sound;

	// Use this for initialization
	void Start () {
		Instance = this;

		Panel.SetActive (false);
	}
	
	public void Open(Sprite image){
		SuperCommandoGameManager.Instance.Player.velocity.x = 0;
		SuperCommandoSoundManager.Instance.PlaySfx (sound);
		ImageTut.sprite = image;
		Panel.SetActive (true);
		Time.timeScale = 0;
	}

	public void Close(){
		Panel.SetActive (false);
		Time.timeScale = 1;
	}
}
