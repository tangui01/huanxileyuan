using UnityEngine;
using System.Collections;

public class BoxBreak : MonoBehaviour {
	public GameObject boat;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void WhenTouchToScreenStart(){


		GameObject obj = Instantiate (Resources.Load ("Prefabs/MapGame/BreakBox"), transform.position, Quaternion.identity) as GameObject;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.boxExpolison);
		Destroy (obj, 1f);

		GameObject.Find("Ramboat2DCanvas").transform.GetChild(1).gameObject.SetActive(true);
		Ramboat2DLevelManager.THIS.ShowLifeCurrent (3);//刷新生命值UI
		boat.SetActive (true);	//激活玩家(包含坐骑)
		Ramboat2DLevelManager.THIS.startingGame = true;
		Resources.UnloadUnusedAssets (); 
		System.GC.Collect ();

		gameObject.SetActive (false);
	}
	public void PlaySoundShootBox(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.pistol [1]);
	}
}
