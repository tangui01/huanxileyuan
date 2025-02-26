using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
public class ReadWriteTextMission: MonoBehaviour
{
	// mission
	//1:kill enemy in one game
	//2:Reach stage 2
	//3:Play I game
	//4:Jump 3 times in one game


	public static ReadWriteTextMission THIS;

	bool levelLoaded;
	[HideInInspector]
	public int[] orderMissions;
	[HideInInspector]
	public int[] isCompleteMissions;
	[HideInInspector]
	public string[] infomationMission;
	[HideInInspector]
	string numberMission1, numberMission2, numberMission3, numberMission4;
	public int[] numberCompletes;
	int[] currentMissionCompleted;
	//int oldMissionComplete1,oldMissionComplete2,oldMissionComplete3,oldMissionComplete4;


	public int currentLevelMission;
	//public Sprite missionSprite;
	//	public string currentLevel;
	//set up string and save to current level
	string writeData;
	//danh sach nhung nhiem vu phai hoan thanh trong game hien tai
	[HideInInspector]
	public Hashtable orderMissionOneLevel;
	//nhung nhiem vu da hoan thanh trong 1 game (1,2,3,4)
	[HideInInspector]
	public List<int> numberMissionComplete;
	// Use this for initialization
	void Start ()
	{
		THIS = this;


		numberCompletes = new int[4];
		currentMissionCompleted = new int[4];
		isCompleteMissions = new int[4];
		orderMissions = new int[4];
		infomationMission = new string[4];
		for (int i = 0; i < 4; i++) {
			currentMissionCompleted [i] = 0;
		}
		orderMissionOneLevel = new Hashtable ();
		orderMissionOneLevel.Clear ();
		currentLevelMission = PlayerPrefs.GetInt ("LevelMission");
		LoadDataFromLocal (currentLevelMission);
		// add order mission
		for(int i=0;i<4;i++){
			if (isCompleteMissions [i] == 0 && !orderMissionOneLevel.ContainsKey(orderMissions[i])) {
//				print (orderMissions[i]);
				orderMissionOneLevel.Add (orderMissions [i], i + 1);
			}
		}
	}

	void Update ()
	{
		
	}

	void AppendString (string appendString, string currentLevel)
	{
		TextWriter tw = new StreamWriter(Application.persistentDataPath+"/ms"+currentLevel+".txt");
		tw.WriteLine(appendString);
		tw.Close();
		}

	public void LoadDataFromLocal (int currentLevel)
	{
// 		levelLoaded = false;
// 		TextReader missionText = File.OpenText (Application.persistentDataPath +"/ms"+currentLevel+".txt");
// //		print (Application.persistentDataPath + "/ms" + currentLevel + ".txt");
// 		if (missionText == null) {
// 			missionText = File.OpenText(Application.persistentDataPath + "/ms"+currentLevel+".txt");
// 		}
// 		byte[] decodedBytes = Convert.FromBase64String (missionText.ReadToEnd());
// 		string decodedText = Encoding.UTF8.GetString (decodedBytes);
// 		ProcessGameDataFromString (decodedText);
	}


	void ProcessGameDataFromString (string mapText)
	{
		string[] lines = mapText.Split (new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
	
		foreach (string line in lines) {
			
			if (line.StartsWith ("Mission1")) {
				numberMission1 = "Mission1";
				string enemyNormal = line.Replace ("Mission1", string.Empty).Trim ();
				string[] enemysData = enemyNormal.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isCompleteMissions[0] = int.Parse (enemysData [0]);
				orderMissions[0] = int.Parse (enemysData [1]);
				numberCompletes [0] = int.Parse (enemysData [2]);
				infomationMission[0] = enemysData [3];
				//Debug.Log (numberMission1 +"\n" + completeMission1.ToString() +"\n"+ orderMission1.ToString()+"\n"+ numberComplete1.ToString()+ "\n" + infomationMission1);
				
			} else if (line.StartsWith ("Mission2")) {
				numberMission2 = "Mission2";
				string enemyNormal = line.Replace ("Mission2", string.Empty).Trim ();
				string[] enemysData = enemyNormal.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isCompleteMissions[1] = int.Parse (enemysData [0]);
				orderMissions[1] = int.Parse (enemysData [1]);
				numberCompletes [1] = int.Parse (enemysData [2]);
				infomationMission[1] = enemysData [3];
			} else if (line.StartsWith ("Mission3")) {
				numberMission3 = "Mission3";
				string enemyNormal = line.Replace ("Mission3", string.Empty).Trim ();
				string[] enemysData = enemyNormal.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isCompleteMissions[2] = int.Parse (enemysData [0]);
				orderMissions[2] = int.Parse (enemysData [1]);
				numberCompletes [2] = int.Parse (enemysData [2]);
				infomationMission[2] = enemysData [3];
			} else if (line.StartsWith ("Mission4")) {
				numberMission4 = "Mission4";
				string enemyNormal = line.Replace ("Mission4", string.Empty).Trim ();
				string[] enemysData = enemyNormal.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isCompleteMissions[3] = int.Parse (enemysData [0]);
				orderMissions[3] = int.Parse (enemysData [1]);
				numberCompletes [3] = int.Parse (enemysData [2]);
				infomationMission[3] = enemysData [3];
			} 
			levelLoaded = true;
		}
	}

	public void WriteToData (string currentLevel)
	{
		// numbermision,complete,order,numbercomplete,infomation;
		writeData = numberMission1 + "/" + isCompleteMissions[0].ToString () + "/" + orderMissions[0].ToString () + "/" + numberCompletes [0].ToString () + "/" + infomationMission[0] + "\n"
			+ numberMission2 + "/" + isCompleteMissions[1].ToString () + "/" + orderMissions[1].ToString () + "/" + numberCompletes [1].ToString () + "/" + infomationMission[1] + "\n"
			+ numberMission3 + "/" + isCompleteMissions[2].ToString () + "/" + orderMissions[2].ToString () + "/" + numberCompletes [2].ToString () + "/" + infomationMission[2] + "\n"
			+ numberMission4 + "/" + isCompleteMissions[3].ToString () + "/" + orderMissions[3].ToString () + "/" + numberCompletes [3].ToString () + "/" + infomationMission[3];

		byte[] bytes=System.Text.Encoding.UTF8.GetBytes(writeData);
		string encode = Convert.ToBase64String (bytes);
		AppendString (encode, currentLevel);
	}

	public void CheckMission (int missionType)
	{
		if (orderMissionOneLevel.ContainsKey (missionType)) {
				Check (missionType);
		}
	}

	void Check(int missionType){
		int key = (int)orderMissionOneLevel [missionType];
		int index = key - 1;
		currentMissionCompleted [index] += 1;
		if (currentMissionCompleted [index] < numberCompletes [index]) {
			if (key == 1) {
				MenuPlayingGame.THIS.ms1Text.text = currentMissionCompleted [index].ToString () + "/" + numberCompletes [index].ToString ();
			} else if (key == 2) {
				MenuPlayingGame.THIS.ms2Text.text = currentMissionCompleted [index].ToString () + "/" + numberCompletes [index].ToString ();
			} else if (key == 3) {
				MenuPlayingGame.THIS.ms3Text.text = currentMissionCompleted [index].ToString () + "/" + numberCompletes [index].ToString ();
			} else if (key == 4) {
				MenuPlayingGame.THIS.ms4Text.text = currentMissionCompleted [index].ToString () + "/" + numberCompletes [index].ToString ();
			}
		} else {
			//complete mission
			isCompleteMissions [index] = 1;
			WriteToData (currentLevelMission.ToString());
			if (key == 1) {
				MenuPlayingGame.THIS.mission1.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [orderMissions[0]];
				MenuPlayingGame.THIS.ms1Text.text = "";
			} else if (key == 2) {
				MenuPlayingGame.THIS.mission2.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [orderMissions[1]];
				MenuPlayingGame.THIS.ms2Text.text = "";
			} else if (key == 3) {
				MenuPlayingGame.THIS.mission3.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [orderMissions[2]];
				MenuPlayingGame.THIS.ms3Text.text = "";
			} else if (key == 4) {
				MenuPlayingGame.THIS.mission4.sprite = Ramboat2DLevelManager.THIS.missionsComPlete [orderMissions[3]];
				MenuPlayingGame.THIS.ms4Text.text = "";
			}
			orderMissionOneLevel.Remove (missionType);
			numberMissionComplete.Add (index);

		}
	}


	public void LoadDataGun(){


	}
}


