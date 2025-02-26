using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Settings : MonoBehaviour {
	public Sprite[] buttonClickSprite;
	public Image buttonMusicGame;
	public Image buttonMusicBackground;
	// Use this for initialization
	void OnEnable(){
		ChangeButtonMusic ();
		ChangeButtonBackgroundMusic ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Disable(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonClose);
		gameObject.SetActive (false);
	}
	void ChangeButtonMusic(){
		if (PlayerPrefs.GetInt ("Music") == 1) {
			buttonMusicGame.sprite = buttonClickSprite [1];
		} else {
			buttonMusicGame.sprite = buttonClickSprite [0];
		}

	}
	void ChangeButtonBackgroundMusic(){
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			buttonMusicBackground.sprite = buttonClickSprite [1];
		} else {
			buttonMusicBackground.sprite = buttonClickSprite [0];
		}
	}
	public void ButtonMusicClicked(){
		if (PlayerPrefs.GetInt ("Music") == 1) {
			PlayerPrefs.SetInt ("Music",0);
			PlayerPrefs.Save ();
		} else {
			PlayerPrefs.SetInt ("Music",1);
			PlayerPrefs.Save ();
		}
		// Ramboat2DMusic.THIS.musicAudioSource.volume = PlayerPrefs.GetInt ("Music");
		ChangeButtonMusic ();
	}
	public void ButtonBackgroundMusicClicked(){
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			PlayerPrefs.SetInt ("Sound",0);
			PlayerPrefs.Save ();
		} else {
			PlayerPrefs.SetInt ("Sound",1);
			PlayerPrefs.Save ();
		}
		// Ramboat2DFXSound.THIS.fxSound.volume = PlayerPrefs.GetInt ("Sound");
		ChangeButtonBackgroundMusic ();
	}
}
