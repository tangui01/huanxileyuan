using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ramboat2DMission : MonoBehaviour
{
	public GameObject[] missionGameobject;
	// Use this for initialization
	void OnEnable ()
	{
		StartCoroutine (SetUpMission ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	IEnumerator SetUpMission ()
	{
		//set up mission 1
		Image ms1 = missionGameobject [0].GetComponent<Image> ();
		Image ms2 = missionGameobject [1].GetComponent<Image> ();
		Image ms3 = missionGameobject [2].GetComponent<Image> ();
		Image ms4 = missionGameobject [3].GetComponent<Image> ();
		Text ms1Text = missionGameobject [0].GetComponentInChildren<Text> ();
		Text ms2Text = missionGameobject [1].GetComponentInChildren<Text> ();
		Text ms3Text = missionGameobject [2].GetComponentInChildren<Text> ();
		Text ms4Text = missionGameobject [3].GetComponentInChildren<Text> ();
		//set text
		ms1Text.text = ReadWriteTextMission.THIS.infomationMission[0];
		ms2Text.text = ReadWriteTextMission.THIS.infomationMission[1];
		ms3Text.text = ReadWriteTextMission.THIS.infomationMission[2];
		ms4Text.text = ReadWriteTextMission.THIS.infomationMission[3];

		if (ReadWriteTextMission.THIS.isCompleteMissions [0] == 0) {
			ms1.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [0]];
		} else {
			ms1.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions [0]];
			ms1Text.color = Color.green;
		}

		if (ReadWriteTextMission.THIS.isCompleteMissions [1] == 0) {
			ms2.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [1]];
		} else {
			ms2.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions [1]];
			ms2Text.color = Color.green;
		}

		if (ReadWriteTextMission.THIS.isCompleteMissions [2] == 0) {
			ms3.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [2]];
		} else {
			ms3.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions [2]];
			ms3Text.color = Color.green;
		}

		if (ReadWriteTextMission.THIS.isCompleteMissions [3] == 0) {
			ms4.sprite = Ramboat2DLevelManager.THIS.missions [ReadWriteTextMission.THIS.orderMissions [3]];
		} else {
			ms4.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [ReadWriteTextMission.THIS.orderMissions [3]];
			ms4Text.color = Color.green;
		}
		yield return new WaitForSeconds (2f);

		GetComponent<Animator> ().SetTrigger ("Out");
		yield return new WaitForSeconds (3f);
		gameObject.SetActive (false);

	}

}
