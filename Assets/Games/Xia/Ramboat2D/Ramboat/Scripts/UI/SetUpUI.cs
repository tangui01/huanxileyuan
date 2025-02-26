using UnityEngine;
using System.Collections;

public class SetUpUI : MonoBehaviour {
	Animator anim;
	bool click;
	public GameObject settingUI,facebookUI,missionUI,dailyRewardUI;
	// Use this for initialization
	void OnEnable () {
		anim = GetComponent<Animator> ();
		click = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetUpUIOut(){
		anim.SetTrigger ("Out");
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.menu);
	}
	public void Disable(){
		click = false;
		gameObject.SetActive (false);
	}

	public void SettingClicked(){
		if (!click) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
			click = true;
			settingUI.SetActive (true);
			anim.SetTrigger ("Out");
		}
	}
	public void FacebookClicked(){
		if (!click) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
			click = true;
			facebookUI.SetActive (true);
			anim.SetTrigger ("Out");
		}
	}
	public void MissionClicked(){
		if (!click) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
			click = true;
			missionUI.SetActive (true);
			anim.SetTrigger ("Out");
		}
	}
	public void DailyRewardClicked(){
		if (!click) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
			click = true;
			dailyRewardUI.SetActive (true);
			anim.SetTrigger ("Out");
		}
	}
	public void PockerClicked(){
		if (!click) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
			click = true;
			PlayerPrefs.SetInt ("PokerClick", 1);
			PlayerPrefs.Save();
			GameObject.Find ("Ramboat2DCanvas").transform.GetChild (5).gameObject.SetActive (true);
			SetUpUIOut ();
		}
	}
}
