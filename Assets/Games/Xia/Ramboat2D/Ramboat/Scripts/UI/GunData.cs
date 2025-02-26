using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
public class GunData : MonoBehaviour {
	public static GunData THIS;
	[HideInInspector]
	public float[] gunStar,gunPower,gunRate,gunAmmo;
	[HideInInspector]
	public int[] gunType;
	public bool levelLoaded;

	// Use this for initialization
	void Awake () {
		THIS = this;
		gunStar = new float[4];
		gunPower = new float[]{27,45,20,9,20};
		gunRate  = new []{3f,2.25f,2.25f,2.6f,2.2f};
		gunAmmo  = new float[]{35,20,35,25,80};
		gunType = new []{1,2,3,4};
		// switch (PlayerPrefs.GetInt ("ChoosePlayer"))
		// {
		// 	default:
		// 		gunType [0] = 1;
		// 		gunType [1] = 2;
		// 		gunType [2] = 3;
		// 		gunType [3] = 4;
		// 		break;
		// }
		// for (int i = 0; i < 4; i++) {
		// 	SetUpInformationGun(gunType [i], i+1);
		// }

	}
	
	void SetUpInformationGun(int gunType,int index){
		gunStar [index-1] = PlayerPrefs.GetFloat("Star" + index.ToString ());//星级
		gunPower[index-1] = PlayerPrefs.GetFloat("Power" + index.ToString ());//火力
		gunRate[index-1]  = PlayerPrefs.GetFloat("Rate" + index.ToString ());//射速
		gunAmmo[index-1]  = PlayerPrefs.GetFloat("Ammo" + index.ToString ());//弹药
		Debug.Log(gunPower[index-1]+"||gunPower"+index);
		Debug.Log(gunRate[index-1]+"||gunRate"+index);
		Debug.Log(gunAmmo[index-1]+"||gunAmmo"+index);
	}
}
