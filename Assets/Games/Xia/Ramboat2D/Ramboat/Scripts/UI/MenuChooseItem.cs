using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuChooseItem : MonoBehaviour {
	public Image avatarPlayer;
	public Image boat;
	public Image[] gargets;
	public Image[] guns;
	public GameObject[] starGunNormal,starGun1, starGun2, starGun3, starGun4;
	public Text levelText;
	public GameObject canvasSetUp;
	bool upgradeClick;
	// Use this for initialization
	void Start () {
		upgradeClick = false;
		avatarPlayer.sprite=Ramboat2DLevelManager.THIS.avatarPlayer[PlayerPrefs.GetInt("ChoosePlayer")];
		
		boat.sprite=Ramboat2DLevelManager.THIS.avatarBoat[PlayerPrefs.GetInt("ChooseBoat") % Ramboat2DLevelManager.THIS.avatarBoat.Length];
		levelText.text = "LEVEL " + PlayerPrefs.GetInt ("LevelMission");
		for (int i = 0; i < guns.Length; i++) {
			guns [i].sprite = Ramboat2DLevelManager.THIS.gunSprites [GunData.THIS.gunType[i]];
		}
		for (int i = 0; i < PlayerPrefs.GetFloat("Star0"); i++) {
			starGunNormal [i].SetActive (false);
		}
		for (int i = 0; i < GunData.THIS.gunStar [0]; i++) {
			starGun1 [i].SetActive (false);
		}
		for (int i = 0; i < GunData.THIS.gunStar [1]; i++) {
			starGun2 [i].SetActive (false);
		}
		for (int i = 0; i < GunData.THIS.gunStar [2]; i++) {
			starGun3 [i].SetActive (false);
		}
		for (int i = 0; i < GunData.THIS.gunStar [3]; i++) {
			starGun4 [i].SetActive (false);
		}

	}

	public IEnumerator LoadScene ()
	{

		Animator animConvert = GameObject.Find ("NextSceneCanvas").GetComponentInChildren<Animator> ();
		yield return new WaitForSeconds (1f);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.openCloseDoor[1]);
		animConvert.SetTrigger ("In");
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("MainScene");
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.openCloseDoor[0]);
		animConvert.SetTrigger ("Out");
	}
	public void LoadUpgradeScene(int numberStore){
		if (!upgradeClick) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
			upgradeClick = true;
			PlayerPrefs.SetInt ("ChooseStore", numberStore);
			PlayerPrefs.Save ();
			StartCoroutine (UpgradeScene ());
		}
	}

	public IEnumerator UpgradeScene(){
		Animator animConvert = GameObject.Find ("NextSceneCanvas").GetComponentInChildren<Animator> ();
		yield return new WaitForSeconds (1f);
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.openCloseDoor[1]);
		animConvert.SetTrigger ("In");
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("Store");
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.openCloseDoor[0]);
		animConvert.SetTrigger ("Out");
	}
	public void EnableSetUp(){
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.menu);
		canvasSetUp.SetActive (true);

	}
}
