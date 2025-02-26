using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using WGM;

public class SetUpGun : MonoBehaviour
{
	float timeEnd = 5;
	Animator anim;
	bool clicked;
	public Image[] imageGunsFilled;
	public Image[] imageGunsBackGround;
	Text coinHave;
	float coinShow = 0;
	Button[] buttonBuys;
	int numClickSixBallet, numClickRocket, numClickThreeGunLine, numClickFire;
	float paysForGun = 40;
	public Text sixBalletText;
	public Text rocketText;
	public Text threeLineGunText;
	public Text fireText;
	public GameObject[] starGun1, starGun2, starGun3, starGun4;
	public GameObject freeSixBallet;
	public GameObject freeRocket;
	public GameObject freeThreeGunLine;
	public GameObject freeFire;
	public GameObject[] pitchTypeGuneff;
	bool buttonClicked;
	bool firstClick;
	private int pitchIndex;
	private int firstClickIndex;
	void Awake ()
	{
		firstClick = true;
		firstClickIndex = -1;
		numClickSixBallet = 1;
		numClickRocket = 1;
		numClickThreeGunLine = 1;
		numClickFire = 1;
		
		freeSixBallet.SetActive (false);
		freeRocket.SetActive (false);
		freeThreeGunLine.SetActive (false);
		freeFire.SetActive (false);
	}

	void OnEnable ()
	{
		Ramboat2DPlayerController.Intance.setUpGunUITime = true;
		pitchTypeGuneff[pitchIndex].SetActive(false);
		pitchIndex = 0;
		pitchTypeGuneff[0].SetActive(true);
		
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.tictac);
		buttonClicked = false;
		for (int i = 0; i < 4; i++) {
			imageGunsFilled [i].sprite = Ramboat2DLevelManager.THIS.gunSprites [GunData.THIS.gunType [i]];
			imageGunsBackGround [i].sprite = Ramboat2DLevelManager.THIS.gunSprites [GunData.THIS.gunType [i]];
		}
//		for(int i=0;i<GunData.THIS.gunStar[0];i++){
//			starGun1 [i].SetActive (false);
//		}
		// for (int i = 0; i < PlayerPrefs.GetFloat ("Star" + GunData.THIS.gunType [0].ToString ()); i++) {
		// 	starGun1 [i].SetActive (false);
		// }
		// for (int i = 0; i < PlayerPrefs.GetFloat ("Star" + GunData.THIS.gunType [1].ToString ()); i++) {
		// 	starGun2 [i].SetActive (false);
		// }
		// for (int i = 0; i < PlayerPrefs.GetFloat ("Star" + GunData.THIS.gunType [2].ToString ()); i++) {
		// 	starGun3 [i].SetActive (false);
		// }
		// for (int i = 0; i < PlayerPrefs.GetFloat ("Star" + GunData.THIS.gunType [3].ToString ()); i++) {
		// 	starGun4 [i].SetActive (false);
		// }

		if (firstClick)
		{
			sixBalletText.text = 0.ToString ();
			rocketText.text = 0.ToString ();
			threeLineGunText.text = 0.ToString ();
			fireText.text = 0.ToString ();
		}
		else
		{
			sixBalletText.text = (paysForGun * numClickSixBallet).ToString ();
			rocketText.text = (paysForGun * numClickRocket).ToString ();
			threeLineGunText.text = (paysForGun * numClickThreeGunLine).ToString ();
			fireText.text = (paysForGun * numClickFire).ToString ();
		}
		
		// if (PlayerPrefs.GetInt ("Free1") > 0) {
		// 	freeSixBallet.SetActive (true);
		// 	freeSixBallet.GetComponentInChildren<Text> ().text = "FREE x" + PlayerPrefs.GetInt ("Free1").ToString ();
		// } else {
		// 	freeSixBallet.SetActive (false);
		// }
		//
		// if (PlayerPrefs.GetInt ("Free2") > 0) {
		// 	freeRocket.SetActive (true);
		// 	freeRocket.GetComponentInChildren<Text> ().text = "FREE x" + PlayerPrefs.GetInt ("Free2").ToString ();
		// } else {
		// 	freeRocket.SetActive (false);
		// }
		// if (PlayerPrefs.GetInt ("Free3") > 0) {
		// 	freeThreeGunLine.SetActive (true);
		// 	freeThreeGunLine.GetComponentInChildren<Text> ().text = "FREE x" + PlayerPrefs.GetInt ("Free3").ToString ();
		// } else {
		// 	freeThreeGunLine.SetActive (false);
		// }
		// if (PlayerPrefs.GetInt ("Free4") > 0) {
		// 	freeFire.SetActive (true);
		// 	freeFire.GetComponentInChildren<Text> ().text = "FREE x" + PlayerPrefs.GetInt ("Free4").ToString ();
		// } else {
		// 	freeFire.SetActive (false);
		// }

		//coinShow = PlayerPrefs.GetFloat ("CoinCollected");
		anim = GetComponent<Animator> ();
		clicked = false;
		//	imageGuns = gameObject.GetComponentsInChildren<Image> ();
		buttonBuys = gameObject.GetComponentsInChildren<Button> ();
		for (int i = 0; i < buttonBuys.Length; i++) {
			buttonBuys [i].interactable = true;
		}
		coinHave = GameObject.Find ("YourCoinHave").GetComponent<Text> ();
		coinHave.text = Ramboat2DPlayerController.Intance.textCoinCollect.text;//拥有的金币等于本次游戏获取的,不做持久化数据
		for (int i = 0; i < imageGunsFilled.Length; i++) {
			StartCoroutine (SetUpGunLoad (imageGunsFilled [i]));
		}
	}
	

	// Update is called once per frame
	void Update ()
	{
		coinHave.text = Ramboat2DPlayerController.Intance.textCoinCollect.text;
		if(!Ramboat2DPlayerController.Intance.setUpGunUITime)
			return;
		
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.A))
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)6))
#endif  
		{
			pitchTypeGuneff[pitchIndex].SetActive(false);
			pitchIndex--;
			if(pitchIndex > pitchTypeGuneff.Length - 1)
				pitchIndex = 0;
			if(pitchIndex < 0)
				pitchIndex = pitchTypeGuneff.Length - 1;
			pitchTypeGuneff[pitchIndex].SetActive(true);
		}
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.D))
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)1))
#endif  
		{
			pitchTypeGuneff[pitchIndex].SetActive(false);
			pitchIndex++;
			if(pitchIndex > pitchTypeGuneff.Length - 1)
				pitchIndex = 0;
			if(pitchIndex < 0)
				pitchIndex = pitchTypeGuneff.Length - 1;
			pitchTypeGuneff[pitchIndex].SetActive(true);
		}
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.J))
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)3))
#endif  
		{
			switch (pitchIndex)
			{
				case 0:
					ButtonSixBallet();
					break;
				case 1:
					ButtonRocket();
					break;
				case 2:
					ButtonThreeGun();
					break;
				case 3:
					ButtonFire();
					break;
			}
		}
		
	}

	void SuccessBuy()
	{
		buttonClicked = true;
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonBuy);
		GunData.THIS.gunPower[4] += 4;
		GunData.THIS.gunRate[4] += 0.15f;
		GunData.THIS.gunAmmo[4] += 1;
		pitchTypeGuneff[pitchIndex].SetActive(false);
		Ramboat2DPlayerController.Intance.setUpGunUITime = false;
	}
	public void ButtonSixBallet ()
	{
		if (!buttonClicked) {
			//如果拥有道具
			if (PlayerPrefs.GetInt ("Free1") > 0) {
				PlayerPrefs.SetInt ("Free1", PlayerPrefs.GetInt ("Free1") - 1);
				Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [0];//切换武器
				Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [0]);
				GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;//弹药UI
				GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
				coinHave.text = (PlayerPrefs.GetFloat ("CoinCollected")).ToString ();
				obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [0]];
				PlayerPrefs.Save ();
				Destroy (effect, 1f);
				anim.SetTrigger ("Out");
				StartCoroutine (WaitForSecondHideBuyLife (6f));
			
			} 
			else {
				float pays = int.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text) - paysForGun * numClickSixBallet;
				if (pays >= 0 || firstClick) {
					clicked = true;
					for (int i = 0; i < buttonBuys.Length; i++) {
						buttonBuys [i].interactable = false;
					}

					SuccessBuy();
					if (!firstClick)
					{
						numClickSixBallet += 1;
						coinHave.text = (pays).ToString();
						Ramboat2DPlayerController.Intance.textCoinCollect.text = pays.ToString();
					}
					else
						firstClickIndex = 0;
					// PlayerPrefs.SetFloat ("CoinCollected", pays);
					// PlayerPrefs.Save ();
					int trachIndex = 2;
					if (firstClickIndex == 0)
						trachIndex = 1;
					if (numClickSixBallet - trachIndex >= 0)
					{
						starGun1[(numClickSixBallet-trachIndex)%starGun1.Length].SetActive(false);
						GunData.THIS.gunPower[0] += 4;
						GunData.THIS.gunRate[0] += 0.37f;
						GunData.THIS.gunAmmo[0] += 8;
					}
					Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [0];
					Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [0]);
					GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
					GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
					obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [0]];
					Destroy (effect, 1f);
					anim.SetTrigger ("Out");
					StartCoroutine (WaitForSecondHideBuyLife (6f));
				}
				else
					Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.fail);
			}
			firstClick = false;
		}
	}

	public void ButtonRocket ()
	{
		if (!buttonClicked) {
		
			if (PlayerPrefs.GetInt ("Free2") > 0) {
				PlayerPrefs.SetInt ("Free2", PlayerPrefs.GetInt ("Free2") - 1);
				Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [1];
				Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [1]);
				GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
				GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
				coinHave.text = (PlayerPrefs.GetFloat ("CoinCollected")).ToString ();
				PlayerPrefs.Save ();
				obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [1]];
				Destroy (effect, 1f);
				anim.SetTrigger ("Out");
				StartCoroutine (WaitForSecondHideBuyLife (6f));

			} 
			else {
				float pays = int.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text) - paysForGun * numClickRocket;
				if (pays >= 0 || firstClick) {
					for (int i = 0; i < buttonBuys.Length; i++) {
						buttonBuys [i].interactable = false;
					}
					clicked = true;
					
					SuccessBuy();
					if (!firstClick)
					{
						numClickRocket += 1;
						coinHave.text = (pays).ToString();
						Ramboat2DPlayerController.Intance.textCoinCollect.text = pays.ToString();
					}
					else
						firstClickIndex = 1;
					// PlayerPrefs.SetFloat ("CoinCollected", pays);
					//PlayerPrefs.Save ();
					int trachIndex = 2;
					if (firstClickIndex == 1)
						trachIndex = 1;
					if (numClickRocket - trachIndex >= 0)
					{
						starGun2[(numClickRocket-trachIndex)%starGun2.Length].SetActive(false);
						GunData.THIS.gunPower[1] += 9;
						GunData.THIS.gunRate[1] += 0.12f;
						GunData.THIS.gunAmmo[1] += 3;
					}
					Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [1];
					Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [1]);
					GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
					GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
					obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [1]];
					Destroy (effect, 1f);
					anim.SetTrigger ("Out");
					StartCoroutine (WaitForSecondHideBuyLife (6f));
				}
				else
					Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.fail);
			}
			firstClick = false;
		}
	}

	public void ButtonThreeGun ()
	{
		if (!buttonClicked) {

			if (PlayerPrefs.GetInt ("Free3") > 0) {
				PlayerPrefs.SetInt ("Free3", PlayerPrefs.GetInt ("Free3") - 1);
				Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [2];
				Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [2]);
				GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
				GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
				coinHave.text = (PlayerPrefs.GetFloat ("CoinCollected")).ToString ();
				PlayerPrefs.Save ();
				obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [2]];
				Destroy (effect, 1f);
				anim.SetTrigger ("Out");
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
				StartCoroutine (WaitForSecondHideBuyLife (6f));
			}
			else {
				float pays = int.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text) - paysForGun * numClickThreeGunLine;
				if (pays >= 0 || firstClick) {
					for (int i = 0; i < buttonBuys.Length; i++) {
						buttonBuys [i].interactable = false;
					}
					clicked = true;
					
					SuccessBuy();
					if (!firstClick)
					{ 
						numClickThreeGunLine += 1;
						coinHave.text = (pays).ToString ();
						Ramboat2DPlayerController.Intance.textCoinCollect.text = pays.ToString ();	
					}
					else
						firstClickIndex = 2;
					// PlayerPrefs.SetFloat ("CoinCollected", pays);
					//PlayerPrefs.Save ();
					int trachIndex = 2;
					if (firstClickIndex == 2)
						trachIndex = 1;
					if (numClickThreeGunLine - trachIndex >= 0)
					{
						starGun3[(numClickThreeGunLine-trachIndex)%starGun3.Length].SetActive(false);
						GunData.THIS.gunPower[2] += 4;
						GunData.THIS.gunRate[2] += 0.2f;
						GunData.THIS.gunAmmo[2] += 3;
					}
					Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [2];
					Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [2]);
					GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
					GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
					obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [2]];
					Destroy (effect, 1f);
					anim.SetTrigger ("Out");
					StartCoroutine (WaitForSecondHideBuyLife (6f));
				}
				else
					Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.fail);
			}
			firstClick = false;
		}
	}

	public void ButtonFire ()
	{
		if (!buttonClicked) {
		
			if (PlayerPrefs.GetInt ("Free4") > 0) {
				PlayerPrefs.SetInt ("Free4", PlayerPrefs.GetInt ("Free4") - 1);
				Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [3];
				Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [3]);
				GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
				GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
				coinHave.text = (PlayerPrefs.GetFloat ("CoinCollected")).ToString ();
				PlayerPrefs.Save ();
				obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [3]];
				Destroy (effect, 1f);
				anim.SetTrigger ("Out");
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
				StartCoroutine (WaitForSecondHideBuyLife (6f));
			} 
			else {
				float pays = int.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text) - paysForGun * numClickFire;
				if (pays >= 0 || firstClick) {
					clicked = true;
					for (int i = 0; i < buttonBuys.Length; i++) {
						buttonBuys [i].interactable = false;
					}
					
					SuccessBuy();
					if (!firstClick)
					{ 
						numClickFire += 1;
						coinHave.text = (pays).ToString ();
						Ramboat2DPlayerController.Intance.textCoinCollect.text = pays.ToString ();	
					}
					else
						firstClickIndex = 3;
					// PlayerPrefs.SetFloat ("CoinCollected", pays);
					//PlayerPrefs.Save ();
					int trachIndex = 2;
					if (firstClickIndex == 3)
						trachIndex = 1;
					if (numClickFire - trachIndex >= 0)
					{
						starGun4[(numClickFire-trachIndex)%starGun4.Length].SetActive(false);
						GunData.THIS.gunPower[3] += 4f;
						GunData.THIS.gunRate[3] += 0.15f;
						GunData.THIS.gunAmmo[3] += 3;
					}
					Ramboat2DPlayerController.Intance.gunType = (GunType)GunData.THIS.gunType [3];
					Ramboat2DPlayerController.Intance.SetUpGun ((GunType)GunData.THIS.gunType [3]);
					GameObject obj = GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject;
					GameObject effect = Instantiate (Resources.Load ("Prefabs/MapGame/EffectChangeGun"), obj.transform.position, Quaternion.identity) as GameObject;
					obj.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [GunData.THIS.gunType [3]];
					Destroy (effect, 1f);
					anim.SetTrigger ("Out");
					StartCoroutine (WaitForSecondHideBuyLife (6f));
				}
				else
					Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.fail);
			}
			firstClick = false;
		}
	}

	IEnumerator SetUpGunLoad (Image obj)
	{
		float time = 0;
		float countSec = 0;
		while (time < timeEnd) {
			time += Time.deltaTime;
			countSec += Time.deltaTime;
			obj.fillAmount = time / timeEnd;
			if (countSec > 1) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.tictac);
				countSec = 0;
			} 
			yield return new WaitForFixedUpdate ();
			if (clicked)
				break;
		}
		anim.SetTrigger ("Out");
		// Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
		StartCoroutine (WaitForSecondHideBuyLife (6f));
	}

	IEnumerator WaitForSecondHideBuyLife (float time)
	{
		Ramboat2DPlayerController.Intance.setUpGunUITime = false;
		yield return new WaitForSeconds (time);
		gameObject.SetActive (false);
	}
}
