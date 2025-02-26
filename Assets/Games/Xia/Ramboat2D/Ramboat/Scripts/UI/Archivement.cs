using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Archivement : MonoBehaviour
{
	public Image avatarLevel;
	public GameObject nextLevel;
	public Image[] missionImage;
	public Text[] missionText;
	public Text levelText;
	bool isClicked;
	public GameObject[] particleGetIt;
	public GameObject buttonNext;
	bool loadScene, haveUnlock;
	public GameObject[] effectCollected;
	Color colorNormal;
	//unlock
	public GameObject unlockPanel;
	public GameObject characterObj;
	public GameObject weponsObj;
	public GameObject boatObj;
	public GameObject gadgetObj;
	public GameObject particleUnLock;
	public Sprite[] characterUnlockSprite;
	public string[] txtCharacter;
	public Sprite[] weponsUnlockSprite;
	public string[] txtWepons;
	public Sprite[] boatUnlockSprite;
	public string[] txtBoat;
	public Sprite[] gadgetUnlockSprite;
	public string[] txtGadget;

	public Image ImgCharacterUnlock;
	public Image ImgWeponsUnlock;
	public Image ImgBoatUnlock;
	public Image ImgGadgetUnlock;
	public Text txtCharacterUnlock;
	public Text txtWeponsUnlock;
	public Text txtBoatUnlock;
	public Text txtGadgetUnlock;
	// Use this for initialization
	void OnEnable ()
	{
		
		loadScene = false;
		colorNormal = missionImage [0].color;
		int level = ReadWriteTextMission.THIS.currentLevelMission;
		nextLevel.GetComponentInChildren<Image> ().sprite = Ramboat2DLevelManager.THIS.missionsLevel [level];
		levelText.text = "LEVEL " + level.ToString ();
		for (int i = 0; i < missionImage.Length; i++) {
			if (ReadWriteTextMission.THIS.isCompleteMissions [i] == 0) {
				missionImage [i].sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [i]];
			} else {
				missionImage [i].sprite = Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions [i]];
			}
			missionText [i].text = ReadWriteTextMission.THIS.infomationMission [i];
		}
		foreach (int i in ReadWriteTextMission.THIS.numberMissionComplete) {
			transform.GetChild (0).transform.GetChild (i).GetComponent<Animator> ().enabled = true;
			transform.GetChild (0).transform.GetChild (i).GetComponent<Animator> ().updateMode = AnimatorUpdateMode.UnscaledTime;
		}
	}

	public void GetIt ()
	{
		StartCoroutine (GetItWait ());
	}

	public IEnumerator GetItWait ()
	{
		if (!isClicked) {
			isClicked = true;
			foreach (int i in ReadWriteTextMission.THIS.numberMissionComplete) {
				effectCollected [i].SetActive (true);
				transform.GetChild (0).transform.GetChild (i).GetComponent<Animator> ().enabled = false;
				missionImage [i].color = colorNormal;
				PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 100);
				PlayerPrefs.Save ();
				yield return StartCoroutine (WaitForRealSeconds (0.5f));
			}
			yield return StartCoroutine (WaitForRealSeconds (0.5f));
			if (ReadWriteTextMission.THIS.orderMissionOneLevel.Count == 0) {
				int level = PlayerPrefs.GetInt ("LevelMission") + 1;
				PlayerPrefs.SetInt ("LevelMission", PlayerPrefs.GetInt ("LevelMission") + 1);
				PlayerPrefs.Save ();
				nextLevel.GetComponentInChildren<Image> ().sprite = Ramboat2DLevelManager.THIS.missionsLevel [level];
				nextLevel.SetActive (true);
	
				avatarLevel.sprite = Ramboat2DLevelManager.THIS.missionsLevel [level];
				levelText.text = "LEVEL " + level.ToString ();
				StartCoroutine (ShowParticleGetIt ());
			} else {
				buttonNext.SetActive (true);
			}
			//show unlock here

		

		}
	}
	// Update is called once per frame
	public void CloseMenu ()
	{
		if (!loadScene) {
			
			loadScene = true;
			StartCoroutine (Ramboat2DPlayerController.Intance.LoadScene ());
			Time.timeScale = 1;
		}	
	}

	IEnumerator ShowParticleGetIt ()
	{
		ReadWriteTextMission.THIS.LoadDataFromLocal (PlayerPrefs.GetInt ("LevelMission"));
		yield return StartCoroutine (WaitForRealSeconds (1f));
		for (int i = 0; i < particleGetIt.Length; i++) {
			particleGetIt [i].SetActive (true);
			yield return StartCoroutine (WaitForRealSeconds (0.5f));
			missionImage [i].sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [i]];
			missionText [i].text = ReadWriteTextMission.THIS.infomationMission [i];
			levelText.text = "LEVEL " + PlayerPrefs.GetInt ("LevelMission").ToString ();
			yield return StartCoroutine (WaitForRealSeconds (0.5f));
		}

		StartCoroutine (ShowUnLock ());
	}


	public static IEnumerator WaitForRealSeconds (float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time) {
			yield return null;
		}
	}

//	//show unlock
//	IEnumerator NextHaveUnlockClick ()
//	{
//		haveUnlock = true;
//		particleUnLock.SetActive (true);
//		yield return  StartCoroutine (WaitForRealSeconds (1f));
//		particleUnLock.SetActive (false);
//		haveUnlock = false;
//
//	}
//
//	public void HaveUnLockNew ()
//	{
//		if (!haveUnlock) {
//			StartCoroutine (NextHaveUnlockClick ());
//		}
//	}
	bool isHaveCharacterUnlock(int level){
		if (level == 2 || level == 6 || level == 9 || level == 14 || level == 18 || level == 23 || level == 26)
			return true;
		else
			return false;
	}
	bool isHaveWeponsUnlock(int level){
		if (level == 2 || level == 6 || level == 9 || level == 14 || level == 18 || level == 23 || level == 26)
			return true;
		else
			return false;
	}
	bool isHaveBoatUnlock(int level){
		if (level == 3 || level == 5 || level == 7 || level == 10 || level == 13 || level == 17 || level == 20||level==23)
			return true;
		else
			return false;
	}
	bool isHaveGadgetUnlock(int level){
		if (level == 2 || level == 3 || level == 4 || level == 5 || level == 6 || level == 7 || level == 9||level==11||level==16||level==20)
			return true;
		else
			return false;
	}
	void CharacterUnlock(int level){
		
		characterObj.SetActive (true);
		if (level == 2) {
			ImgCharacterUnlock.sprite = characterUnlockSprite [0];
			txtCharacterUnlock.text = txtCharacter [0];
		} else if (level == 6) {
		} else if (level == 9) {
		} else if (level == 14) {
		} else if (level == 18) {
		} else if (level == 23) {
		} else if (level == 26) {
		}
	}
	void WeponsUnlock(int level){
		weponsObj.SetActive (true);
		if (level == 2) {
			ImgWeponsUnlock.sprite = weponsUnlockSprite [0];
			txtWeponsUnlock.text = txtWepons [0];
		} else if (level == 6) {
		} else if (level == 9) {
		} else if (level == 14) {
		} else if (level == 18) {
		} else if (level == 23) {
		} else if (level == 26) {
		}
	}
	void BoatUnlock(int level){
		boatObj.SetActive (true);
		if (level == 3) {
			ImgBoatUnlock.sprite = boatUnlockSprite [0];
			txtBoatUnlock.text = txtBoat [0];
		} else if (level == 5) {
		} else if (level == 7) {
		} else if (level == 10) {
		} else if (level == 13) {
		} else if (level == 17) {
		} else if (level == 20) {
		}else if (level == 23) {
		}
	}
	void GadgetUnlock(int level){
		gadgetObj.SetActive (true);
		if (level == 2) {
			ImgGadgetUnlock.sprite = gadgetUnlockSprite [0];
			txtGadgetUnlock.text = txtGadget [0];
		} else if (level == 3) {
		} else if (level == 4) {
		} else if (level == 5) {
		} else if (level == 6) {
		} else if (level == 7) {
		}else if (level == 9) {
		}else if (level == 11) {
		}else if (level == 16) {
		}else if (level == 20) {
		}
	}
	IEnumerator ShowUnLock(){
		if(isHaveBoatUnlock(PlayerPrefs.GetInt ("LevelMission"))||isHaveCharacterUnlock(PlayerPrefs.GetInt ("LevelMission"))||isHaveGadgetUnlock(PlayerPrefs.GetInt ("LevelMission"))||isHaveWeponsUnlock(PlayerPrefs.GetInt ("LevelMission"))){
			unlockPanel.SetActive (true);
			if(isHaveCharacterUnlock(PlayerPrefs.GetInt ("LevelMission"))){
				particleUnLock.SetActive (true);
				CharacterUnlock (PlayerPrefs.GetInt ("LevelMission"));
				yield return  StartCoroutine (WaitForRealSeconds (2f));
				characterObj.SetActive (false);
				particleUnLock.SetActive (false);
			}
			if(isHaveWeponsUnlock(PlayerPrefs.GetInt ("LevelMission"))){
				particleUnLock.SetActive (true);
				WeponsUnlock (PlayerPrefs.GetInt ("LevelMission"));
				yield return  StartCoroutine (WaitForRealSeconds (2f));
				weponsObj.SetActive (false);
				particleUnLock.SetActive (false);
			}
			if(isHaveBoatUnlock(PlayerPrefs.GetInt ("LevelMission"))){
				particleUnLock.SetActive (true);
				BoatUnlock (PlayerPrefs.GetInt ("LevelMission"));
				yield return  StartCoroutine (WaitForRealSeconds (2f));
				boatObj.SetActive (false);
				particleUnLock.SetActive (false);
			}
			if(isHaveGadgetUnlock(PlayerPrefs.GetInt ("LevelMission"))){
				particleUnLock.SetActive (true);
				GadgetUnlock (PlayerPrefs.GetInt ("LevelMission"));
				yield return  StartCoroutine (WaitForRealSeconds (2f));
				gadgetObj.SetActive (false);
				particleUnLock.SetActive (false);
			}

		}
		buttonNext.SetActive (true);
		unlockPanel.SetActive (false);
	}
}
