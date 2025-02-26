using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MissionUI : MonoBehaviour {
	public Text levelTxt;
	public Image levelImg;
	public Image[] missionImg;
	public Text[] missionTxt;
	public GameObject[] missionSkips;
	bool isClickButton;
	// Use this for initialization
	void OnEnable () {
		levelTxt.text = "LEVEL "+PlayerPrefs.GetInt ("LevelMission").ToString ();
		levelImg.sprite = Ramboat2DLevelManager.THIS.missionLevelSmall [ReadWriteTextMission.THIS.currentLevelMission];
		levelImg.SetNativeSize ();
		for (int i = 0; i < missionImg.Length; i++) {
			if (ReadWriteTextMission.THIS.isCompleteMissions [i] == 0) {
				missionImg[i].sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [i]];
				missionSkips [i].SetActive (true);
			} else {
				missionImg[i].sprite= Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions [i]];
				missionSkips [i].SetActive (false);
			}
			missionTxt [i].text = ReadWriteTextMission.THIS.infomationMission [i];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Disable(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonClose);
		gameObject.SetActive (false);
	}

	//use Mission UI Pause
	public void HomeClicked(){
		if (!isClickButton) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonClose);
			Time.timeScale = 1;
			isClickButton = true;
			StartCoroutine (Ramboat2DPlayerController.Intance.LoadScene ());
		}
	}
	public void ResumeClick(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonClose);
		Time.timeScale = 4;
		gameObject.SetActive (false);
	}
}
