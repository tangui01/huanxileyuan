using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public enum GunType
{
	NormalGun,
	SixBarreled,
	Rocket,
	ThreeLineGun,
	FireGun,
}

public class Ramboat2DInitGame : MonoBehaviour {
	public static Ramboat2DInitGame Instance;
	string[] ms;
	byte[] bytes;
	public bool isResetGame = false;
	// Use this for initialization
	void Awake(){
		Instance = this;
		if (PlayerPrefs.GetInt("Lauched") == 0 || isResetGame)
		{    
			//First lauching
	 		PlayerPrefs.SetInt ("ChoosePlayer", 0);
	 		PlayerPrefs.SetInt ("ChooseBoat",0);
	 //normal gun
	 		PlayerPrefs.SetFloat ("Star0",0);
	 		PlayerPrefs.SetFloat("Power0",15);
	 		PlayerPrefs.SetFloat("Rate0",2.5f);
	 		PlayerPrefs.SetFloat ("Ammo0", 10000);
	 //sixBallet
	 		PlayerPrefs.SetFloat ("Star1",0);
	 		PlayerPrefs.SetFloat("Power1",24);
	 		PlayerPrefs.SetFloat("Rate1",2.7f);
	 		PlayerPrefs.SetFloat ("Ammo1",35);
	 		PlayerPrefs.SetInt ("Free1", 0);
	 //rocket
	 		PlayerPrefs.SetFloat ("Star2",0);
	 		PlayerPrefs.SetFloat("Power2",35);
	 		PlayerPrefs.SetFloat("Rate2",3f);
	 		PlayerPrefs.SetFloat ("Ammo2", 20);
	 		PlayerPrefs.SetInt ("Free2", 0);
	 //three line gun
	 		PlayerPrefs.SetFloat ("Star3",0);
	 		PlayerPrefs.SetFloat("Power3",17);
	 		PlayerPrefs.SetFloat("Rate3",2.5f);
	 		PlayerPrefs.SetFloat ("Ammo3", 35);
	 		PlayerPrefs.SetInt ("Free3", 0);
	 //firer
	 		PlayerPrefs.SetFloat ("Star4",0);
	 		PlayerPrefs.SetFloat("Power4",45);
	 		PlayerPrefs.SetFloat("Rate4",3f);
	 		PlayerPrefs.SetFloat ("Ammo4", 20);
	 		PlayerPrefs.SetInt ("Free4", 0);
	 //rocket follow
	 		PlayerPrefs.SetFloat ("Star5",0);
	 		PlayerPrefs.SetFloat("Power5",30);
	 		PlayerPrefs.SetFloat("Rate5",2.8f);
	 		PlayerPrefs.SetFloat ("Ammo5", 25);
	 		PlayerPrefs.SetInt ("Free5", 0);
	 //laze blue
	 		PlayerPrefs.SetFloat ("Star6",0);
	 		PlayerPrefs.SetFloat("Power6",40);
	 		PlayerPrefs.SetFloat("Rate6",3f);
	 		PlayerPrefs.SetFloat ("Ammo6", 30);
	 		PlayerPrefs.SetInt ("Free6", 0);
	//Store choose
	 		PlayerPrefs.SetInt("ChooseStore",0);
	 		PlayerPrefs.SetInt ("PokerClick", 0);
	
	 		PlayerPrefs.SetFloat("CoinCollected",50000);
	 		PlayerPrefs.SetFloat ("Gems",5000);
	 		PlayerPrefs.SetInt ("Pocker", 100);
	 //unlock character
	 		PlayerPrefs.SetInt ("UnlockRose",0);
	 		PlayerPrefs.SetInt ("UnlockKing",0);
	 		PlayerPrefs.SetInt ("UnlockArnol",0);
	 		PlayerPrefs.SetInt ("UnlockAmber",0);
	 		PlayerPrefs.SetInt ("UnlockDrakhelis",0);
	 		PlayerPrefs.SetInt ("UnlockSammy",0);
	 		
	 		PlayerPrefs.SetInt ("LevelMission", 25);
	 		PlayerPrefs.SetInt("Music", 1);
	 		PlayerPrefs.SetInt("Sound", 1);
	 		CreateText ();
	 		PlayerPrefs.SetInt("Lauched", 1);
	 		PlayerPrefs.Save();

		}
//	PlayerPrefs.DeleteAll ();
	}


	void CreateText(){
		ms = new string[11];
		ms[0] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[1]=	"Mission1/0/34/35/ACCUM 35 KILLS" + "\n" +
			"Mission2/0/13/3/DIVE 3 TIMES IN ONE GAME" + "\n" +
			"Mission3/0/31/1/PLAY WITH AN UPGRADE WEAPON" + "\n" +
			"Mission4/0/1/2000/GET 2000 POINTS IN ONE GAME";
		ms[2]=	"Mission1/0/5/100/EARN 100 COINS IN ONE GAME" + "\n" +
			"Mission2/0/4/1/PLAY WITH THE CHARACTER 'ROSE'" + "\n" +
			"Mission3/0/17/6/CATCH 6 FISHES IN ONE GAME" + "\n" +
			"Mission4/0/33/1/PICK I POKER CHIP IN ONE GAME";
		ms[3]=	"Mission1/0/35/30/KILL 30 ENEMIES IN ONE GAME WITH THE PISTOL" + "\n" +
			"Mission2/0/36/10000/ACCUM 10000 POINT" + "\n" +
			"Mission3/0/38/1000/ACCUM 1000 COINS" + "\n" +
			"Mission4/0/37/1/PLAY ONE GAME USING GADGET 'FIRE POWER'";
		ms[4] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[5] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[6] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[7] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[8] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[9] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		ms[10] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
			"Mission2/0/1/1/REACH STAGE 2" + "\n" +
			"Mission3/0/3/1/PLAY I GAME" + "\n" +
			"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
		if(PlayerPrefs.GetInt("Lauched")==0){
			for (int i = 0; i < ms.Length; i++) {
				bytes=System.Text.Encoding.UTF8.GetBytes(ms[i]);
				ms[i] = Convert.ToBase64String (bytes);
				CreateFileText (ms[i],"/ms",i+1);
			}
		}

	}
	void CreateFileText(string appendString,string filename,int level){
		TextWriter tw = File.CreateText (Application.persistentDataPath + filename+level+".txt");
		tw.WriteLine(appendString);
		tw.Close();
	}
}
