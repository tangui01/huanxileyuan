using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuPlayingGame : MonoBehaviour {
	public static MenuPlayingGame THIS;
	public Image mission1;
	public Image mission2;
	public Image mission3;
	public Image mission4;
	public Text ms1Text;
	public Text ms2Text;
	public Text ms3Text;
	public Text ms4Text;
	public Image avatar;
	public GameObject canvasMissionUI;
	void Awake(){
		THIS = this;
	}
	void OnEnable(){
		avatar.sprite = Ramboat2DLevelManager.THIS.avatarPlayer [Ramboat2DLevelManager.THIS.playerNumber];//PlayerPrefs.GetInt ("ChoosePlayer")
		if (ReadWriteTextMission.THIS.isCompleteMissions[0] == 0) {
			mission1.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions[0]];
			ms1Text.text = "0/" + ReadWriteTextMission.THIS.numberCompletes [0].ToString ();
		} else {
			mission1.sprite= Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions[0]];
			ms1Text.text = "";
		}

		if (ReadWriteTextMission.THIS.isCompleteMissions[1] == 0) {
			mission2.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions[1]];
			ms2Text.text = "0/" + ReadWriteTextMission.THIS.numberCompletes [1].ToString ();
		} else {
			mission2.sprite= Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions[1]];
			ms2Text.text = "";
		}

		if (ReadWriteTextMission.THIS.isCompleteMissions[2]== 0) {
			mission3.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions[2]];
			ms3Text.text = "0/" + ReadWriteTextMission.THIS.numberCompletes [2].ToString ();
		} else {
			mission3.sprite= Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions[2]];
			ms3Text.text = "";
		}

		if (ReadWriteTextMission.THIS.isCompleteMissions[3] == 0) {
			mission4.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions[3]];
			ms4Text.text = "0/" + ReadWriteTextMission.THIS.numberCompletes [3].ToString ();
		} else {
			mission4.sprite= Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions[3]];
			ms4Text.text = "";
		}

	}
	
	public void PauseClicked(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
		Time.timeScale = 0;
		canvasMissionUI.SetActive (true);
	}
}
