using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using WGM;

public class BuyLife : MonoBehaviour {
	public bool seconChange;
	public float time=5f;
	public Button buttonBuyLife;
	Image imgYourCoin;
	Text textPays;
	Animator anim;
	public GameObject[] m_levels;
	bool isPokerEnable;
	void OnEnable(){
		Ramboat2DPlayerController.Intance.setUpGunUITime = true;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
		buttonBuyLife.interactable = true;
		int levelPass = (Ramboat2DLevelManager.THIS.checkGameState + 1) / 2;
		for (int i = 0; i < levelPass; i++) {
			m_levels [i].SetActive (false);
		}
		imgYourCoin = GameObject.Find ("ButtonBuyLife").gameObject.GetComponent<Image> ();
		GameObject.Find ("NumberDiamon").gameObject.GetComponent<Text> ().text=(Ramboat2DPlayerController.Intance.numberClickReload*200).ToString();
		seconChange = false;
		anim = gameObject.GetComponent<Animator> ();
		StartCoroutine (Change (3f));
	}

	private void Update()
	{
		if(!Ramboat2DPlayerController.Intance.setUpGunUITime)
			return;
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.J))
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)3))
#endif
		{
			UseSecondChange();
			Ramboat2DPlayerController.Intance.setUpGunUITime = false;
		}
	}

	IEnumerator Change(float timeRun){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.tictac);
		float time = 0;
		float countSec = 0;
		while (time < timeRun) {
			time += Time.deltaTime;
			countSec += Time.deltaTime;
			imgYourCoin.fillAmount = time / timeRun;
			if (time >= timeRun) {
				buttonBuyLife.interactable = false;
			}
			if (countSec > 1) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.tictac);
				countSec = 0;
			}
			yield return new WaitForFixedUpdate ();
			if (seconChange)
				yield break;
		}
		UseSecondChange();
		Ramboat2DPlayerController.Intance.setUpGunUITime = false;
		
		if (!seconChange) {
			anim.SetTrigger ("Out");
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
			if (PlayerPrefs.GetInt ("Pocker") <= 0) {
				if (ReadWriteTextMission.THIS.numberMissionComplete.Count > 0) {
					GameObject.Find ("Ramboat2DCanvas").transform.GetChild (8).gameObject.SetActive (true);
					Time.timeScale = 0;
				} else {
					StartCoroutine (Ramboat2DPlayerController.Intance.ResetGame());
					if(Ramboat2DFXSound.THIS.fxSound.isPlaying) 
						Ramboat2DFXSound.THIS.fxSound.Stop ();
					StartCoroutine (WaitForSecondHideBuyLife (5f));
				}
			} else {
				anim.SetTrigger ("Out");
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
				StartCoroutine (WaitForSecondHideBuyLife (5f));
				StartCoroutine (EnablePockerCanvas ());
			}
		}
	}
	public void UseSecondChange(){
		seconChange = true;
		anim.SetTrigger ("Out");
		Ramboat2DPlayerController.Intance.PlayerReload ();
		StartCoroutine (WaitForSecondHideBuyLife (1f));

		// mua gem de co them mang choi
//		StartCoroutine (PlayerController.Intance.LoadScene ());
//		StartCoroutine (WaitForSecondHideBuyLife (5f));

	}


	IEnumerator WaitForSecondHideBuyLife(float time){
		yield return new WaitForSeconds (time);
		gameObject.SetActive (false);
	}
	IEnumerator EnableArchivement(){
		yield return new WaitForSeconds (2f);
		GameObject.Find ("Ramboat2DCanvas").transform.GetChild (8).gameObject.SetActive (true);
	}
	IEnumerator EnablePockerCanvas(){
		yield return new WaitForSeconds (2f);
		GameObject.Find ("Ramboat2DCanvas").transform.GetChild (5).gameObject.SetActive (true);
	}
}
