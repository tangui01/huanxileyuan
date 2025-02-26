using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
public class CreateDataText : MonoBehaviour {
//	string[] ms;
//	byte[] bytes;
//	string[] gunData;
//	void Start(){
//		ms = new string[3];
//		gunData = new string[4];
//		ms[0] = "Mission1/0/2/10/KILL 10 ENEMY IN ONE GAME" + "\n" +
//				"Mission2/0/1/1/REACH STAGE 2" + "\n" +
//				"Mission3/0/3/1/PLAY I GAME" + "\n" +
//				"Mission4/0/12/3/JUMP 3 TIME IN ONE GAME";
//		ms[1]=	"Mission1/0/2/20/ACCUM 20 KILLS" + "\n" +
//				"Mission2/0/13/3/DIVE 3 TIMES IN ONE GAME" + "\n" +
//				"Mission3/0/7/1/PLAY WITH AN UPGRADE WEAPON" + "\n" +
//				"Mission4/0/3/3/GET 2000 POINTS IN ONE GAME";
//		ms[2]=	"Mission1/0/1/100/EARN 100 COINS IN ONE GAME" + "\n" +
//				"Mission2/0/2/3/PLAY WITH THE CHARACTER 'ROSE'" + "\n" +
//				"Mission3/0/3/1/CATCH 6 FISHES IN ONE GAME" + "\n" +
//				"Mission4/0/4/3/PICK I POKER CHIP IN ONE GAME";
//		gunData[0]="Power/15"+"\n"+"Rate/2.5"+"\n"+"Ammo/40";
//		gunData[1]="Power/24"+"\n"+"Rate/2.7"+"\n"+"Ammo/35";
//		gunData[2]="Power/35"+"\n"+"Rate/3"+"\n"+"Ammo/27";
//		gunData[3]="Power/45"+"\n"+"Rate/3"+"\n"+"Ammo/30";
//		if(PlayerPrefs.GetInt("Lauched")==0){
//			for (int i = 0; i < ms.Length; i++) {
//				bytes=System.Text.Encoding.UTF8.GetBytes(ms[i]);
//				ms[i] = Convert.ToBase64String (bytes);
//				CreateFileText (ms[i],"/ms",i+1);
//			}
//			for (int i = 0; i < 4; i++) {
//				bytes=System.Text.Encoding.UTF8.GetBytes(gunData[i]);
//				gunData[i] = Convert.ToBase64String (bytes);
//				CreateFileText (gunData[i],"/gunData",i+1);
//			}
//		}
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//	void CreateFileText(string appendString,string filename,int level){
//		TextWriter tw = File.CreateText (Application.persistentDataPath + filename+level+".txt");
//		tw.WriteLine(appendString);
//		tw.Close();
//	}

 
}
