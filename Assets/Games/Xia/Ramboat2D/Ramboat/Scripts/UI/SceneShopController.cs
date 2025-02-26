using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneShopController : MonoBehaviour
{
	public GameObject[] pagesUpgrade;
	public Text[] textButtonChoose;
	//UI top information
	public Image levelMission;
	public Text textMission;
	public Text yourCoin;
	public Sprite[] levelMissionSprite;
	bool isBackClick;
	// wepons UI
	public GameObject[] starGunNomal,starSixBallet,starRocket,starThreeLineGun,starFirer,starMissle,starLaserGun;
	public GameObject[] upgradeNormal, upgradeSixBallet, upgradeRocket, upgradeThreeLineGun, upgradeFirer,upgradeMissle,upgradeLaserGun;
	public GameObject fullNormal,fullSixBallet,fullRocket,fullThreeLine,fullFirer,fullMissle,fullLaserGun,lockMissle,lockLaserGun;
	public Text coinPaysNormal, coinPaysSixBallet, coinPaysRocket, coinPaysThreeLineGun, coinPaysFirer,coinPaysMissle,coinPaysLaserGun;

	//Character UI
	public GameObject[] buttonSelect,buttonSelected,buttonBuy,imgLockCharacter,buttonCoins;
	// boat UI
	public GameObject[] boatSelect,boatSelected,boatBuy,imgLockBoat;
	// GadGet
	public GameObject[] gadgetSelect,gadgetSelected,gadgetBuy,imgLockGadget;
	void Start ()
	{
//		PlayerPrefs.DeleteAll ();
		// PlayerPrefs.SetInt ("LevelMission", 9);
		levelMission.sprite = levelMissionSprite [PlayerPrefs.GetInt ("LevelMission")];
		textMission.text = PlayerPrefs.GetInt ("LevelMission").ToString ();
		yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();

		for (int i = 0; i < 4; i++) {
			pagesUpgrade [i].SetActive (false);
			textButtonChoose [i].color = Color.gray;
		}
		pagesUpgrade [PlayerPrefs.GetInt("ChooseStore")].SetActive (true);
		textButtonChoose [PlayerPrefs.GetInt("ChooseStore")].color = Color.green;
		SetUpWeponsUI ();
		ShowInformationCharacter ();
		ShowInformationBoat ();
		ShowInformationGadget ();
	}
//SET UP WEPONS
	public void ClickWePons ()
	{
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
		for (int i = 0; i < 4; i++) {
			pagesUpgrade [i].SetActive (false);
			textButtonChoose [i].color = Color.grey;
		}
		pagesUpgrade [0].SetActive (true);
		textButtonChoose [0].color = Color.green;
	}

	public void ClickCharacter ()
	{
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
		for (int i = 0; i < 4; i++) {
			pagesUpgrade [i].SetActive (false);
			textButtonChoose [i].color = Color.grey;
		}
		pagesUpgrade [1].SetActive (true);
		textButtonChoose [1].color = Color.green;
	}

	public void ClickVehicles ()
	{
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
		for (int i = 0; i < 4; i++) {
			pagesUpgrade [i].SetActive (false);
			textButtonChoose [i].color = Color.grey;
		}
		pagesUpgrade [2].SetActive (true);
		textButtonChoose [2].color = Color.green;

	}

	public void ClickGadgets ()
	{
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonOpen);
		for (int i = 0; i < 4; i++) {
			pagesUpgrade [i].SetActive (false);
			textButtonChoose [i].color = Color.grey;
		}
		pagesUpgrade [3].SetActive (true);
		textButtonChoose [3].color = Color.green;
	}

	void SetUpWeponsUI(){
//normal gun
			for (int i = 0; i < PlayerPrefs.GetFloat ("Star0"); i++) {
				starGunNomal [i].SetActive (false);
			}
			if (PlayerPrefs.GetFloat ("Star0") == 5) {
				fullNormal.SetActive (true);
			} else {
				coinPaysNormal.text = ((PlayerPrefs.GetFloat ("Star0")+1) * 500).ToString ();
			upgradeNormal [(int)PlayerPrefs.GetFloat ("Star0")].SetActive (true);
			}

//six ballet
			for (int i = 0; i < PlayerPrefs.GetFloat ("Star1"); i++) {
				starSixBallet [i].SetActive (false);
			}
			if (PlayerPrefs.GetFloat ("Star1") == 5) {
				fullSixBallet.SetActive (true);
			} else {
				coinPaysSixBallet.text = ((PlayerPrefs.GetFloat ("Star1") + 1) * 500).ToString ();
			upgradeSixBallet [(int)PlayerPrefs.GetFloat ("Star1")].SetActive (true);
			}
//rocket
			for (int i = 0; i < PlayerPrefs.GetFloat ("Star2"); i++) {
				starRocket [i].SetActive (false);
			}
			if (PlayerPrefs.GetFloat ("Star2") == 5) {
				fullRocket.SetActive (true);
			} else {
				coinPaysRocket.text = ((PlayerPrefs.GetFloat ("Star2") + 1) * 500).ToString ();
			upgradeRocket [(int)PlayerPrefs.GetFloat ("Star2")].SetActive (true);
			}
//three line gun
			for (int i = 0; i < PlayerPrefs.GetFloat ("Star3"); i++) {
				starThreeLineGun [i].SetActive (false);
			}
			if (PlayerPrefs.GetFloat ("Star3") == 5) {
				fullThreeLine.SetActive (true);
			} else {
				coinPaysThreeLineGun.text = ((PlayerPrefs.GetFloat ("Star3") + 1) * 500).ToString ();
			upgradeThreeLineGun [(int)PlayerPrefs.GetFloat ("Star3")].SetActive (true);
			}
// firer
			for (int i = 0; i < PlayerPrefs.GetFloat ("Star4"); i++) {
				starFirer [i].SetActive (false);
			}
			if (PlayerPrefs.GetFloat ("Star4") == 5) {
				fullFirer.SetActive (true);
			} else {
				coinPaysFirer.text = ((PlayerPrefs.GetFloat ("Star4") + 1) * 500).ToString ();
			upgradeFirer [(int)PlayerPrefs.GetFloat ("Star4")].SetActive (true);
			}
//homing missle
		if(PlayerPrefs.GetInt("LevelMission")>=2){
			lockMissle.SetActive (false);
		}
		for (int i = 0; i < PlayerPrefs.GetFloat ("Star5"); i++) {
			starMissle [i].SetActive (false);
		}
		if (PlayerPrefs.GetFloat ("Star5") == 5) {
			fullMissle.SetActive (true);
		} else {
			coinPaysMissle.text = ((PlayerPrefs.GetFloat ("Star5") + 1) * 500).ToString ();
			upgradeMissle [(int)PlayerPrefs.GetFloat ("Star5")].SetActive (true);
		}
// laser gun
		if(PlayerPrefs.GetInt("LevelMission")>=6){
			lockLaserGun.SetActive (false);
		}
		for (int i = 0; i < PlayerPrefs.GetFloat ("Star6"); i++) {
			starLaserGun [i].SetActive (false);
		}
		if (PlayerPrefs.GetFloat ("Star6") == 5) {
			fullLaserGun.SetActive (true);
		} else {
			coinPaysLaserGun.text = ((PlayerPrefs.GetFloat ("Star6") + 1) * 500).ToString ();
			upgradeLaserGun [(int)PlayerPrefs.GetFloat ("Star6")].SetActive (true);
		}
	}

	//upgrade gun
	public void ClickButtonUpgradeNormal(){
		float star = PlayerPrefs.GetFloat ("Star0");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star0", PlayerPrefs.GetFloat("Star0") + 1);
			star = PlayerPrefs.GetFloat ("Star0");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeNormal [i].SetActive (false);
			}
			for (int i = 0; i < star; i++) {
				starGunNomal [i].SetActive (false);
			}

			coinPaysNormal.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Power0",18);
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Rate0",2.7f);
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Power0",21);
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Rate0",3f);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Power0",25);
			}
			if (star == 5) {
				fullNormal.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeNormal [i].SetActive (false);
				}
			} else {
				upgradeNormal [(int)star].SetActive (true);
			}
			PlayerPrefs.Save ();
		}
	}
	public void ClickButtonUpgradeSixBallet(){
		float star = PlayerPrefs.GetFloat ("Star1");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star1", PlayerPrefs.GetFloat("Star1") + 1);
			star = PlayerPrefs.GetFloat ("Star1");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeSixBallet [i].SetActive (false);
			}
			for (int i = 0; i < star; i++) {
				starSixBallet [i].SetActive (false);
			}

			coinPaysSixBallet.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Rate1",2.9f);
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Power1",22f);
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Ammo1",45);
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Power1",27);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Ammo1",55);
			}
			if (star == 5) {
				fullSixBallet.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeSixBallet [i].SetActive (false);
				}
			} else upgradeSixBallet [(int)star].SetActive (true);
			PlayerPrefs.Save ();
		}
	}
	public void ClickButtonUpgradeRocket(){

		float star = PlayerPrefs.GetFloat ("Star2");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star2", PlayerPrefs.GetFloat("Star2") + 1);
			star = PlayerPrefs.GetFloat ("Star2");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeRocket [i].SetActive (false);//加成数据UI
			}
			for (int i = 0; i < star; i++) {
				starRocket [i].SetActive (false);//空星覆盖UI
			}
		
			coinPaysRocket.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Power2",36f);//攻击力
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Rate2",3.2f);//射速
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Ammo2",27);//弹药
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Power2",42);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Rate2",3.5f);
			}
			if (star == 5) {
				fullRocket.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeRocket [i].SetActive (false);
				}
			}else 	
				upgradeRocket [(int)star].SetActive (true);
			PlayerPrefs.Save ();
		}
	}
	public void ClickButtonUpgradeThreeLine(){
		float star = PlayerPrefs.GetFloat ("Star3");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star3", PlayerPrefs.GetFloat("Star3") + 1);
			star = PlayerPrefs.GetFloat ("Star3");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeThreeLineGun [i].SetActive (false);
			}
			for (int i = 0; i < star; i++) {
				starThreeLineGun [i].SetActive (false);
			}

			coinPaysThreeLineGun.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Power3",18f);
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Rate3",2.7f);
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Ammo3",35);
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Power3",24);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Rate3",3f);
			}
			if (star == 5) {
				fullThreeLine.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeThreeLineGun [i].SetActive (false);
				}
			}else upgradeThreeLineGun [(int)star].SetActive (true);
			PlayerPrefs.Save ();
		}
	}
	public void ClickButtonUpgradeFirer(){
		float star = PlayerPrefs.GetFloat ("Star4");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star4", PlayerPrefs.GetFloat("Star4") + 1);
			star = PlayerPrefs.GetFloat ("Star4");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeFirer [i].SetActive (false);
			}
			for (int i = 0; i < star; i++) {
				starFirer [i].SetActive (false);
			}

			coinPaysFirer.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Rate4",3.3f);
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Ammo4",45);
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Power4",15);
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Rate4",3.6f);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Power4",50);
			}
			if (star == 5) {
				fullFirer.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeFirer [i].SetActive (false);
				}
			} else upgradeFirer [(int)star].SetActive (true);
			PlayerPrefs.Save ();
		}
	}
	//upgrade missle
	public void ClickButtonUpgradeMissle(){
		float star = PlayerPrefs.GetFloat ("Star5");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star5", PlayerPrefs.GetFloat("Star5") + 1);
			star = PlayerPrefs.GetFloat ("Star5");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeMissle [i].SetActive (false);
			}
			for (int i = 0; i < star; i++) {
				starMissle [i].SetActive (false);
			}

			coinPaysMissle.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Ammo5",45f);
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Rate5",3.2f);
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Power5",35);
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Rate5",3.8f);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Power5",45f);
			}
			if (star == 5) {
				fullMissle.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeMissle [i].SetActive (false);
				}
			} else upgradeMissle [(int)star].SetActive (true);
			PlayerPrefs.Save ();
		}
	}
	// upgrade lasergun
	public void ClickButtonUpgradeLaserGun(){
		float star = PlayerPrefs.GetFloat ("Star6");
		if (PlayerPrefs.GetFloat ("CoinCollected") > (star+1)*500) {
			PlayerPrefs.SetFloat ("Star6", PlayerPrefs.GetFloat("Star6") + 1);
			star = PlayerPrefs.GetFloat ("Star6");
			//show particle upgrade

			float coin=PlayerPrefs.GetFloat("CoinCollected")-star*500;
			PlayerPrefs.SetFloat("CoinCollected",coin);
			yourCoin.text = coin.ToString ();
			for (int i = 0; i < 5; i++) {
				upgradeLaserGun [i].SetActive (false);
			}
			for (int i = 0; i < star; i++) {
				starLaserGun [i].SetActive (false);
			}
			coinPaysLaserGun.text = ((star + 1) * 500).ToString ();
			if (star == 1) {
				PlayerPrefs.SetFloat("Power6",50f);
			} else if (star == 2) {
				PlayerPrefs.SetFloat("Rate6",3.4f);
			} else if (star == 3) {
				PlayerPrefs.SetFloat("Power6",65f);
			} else if (star == 4) {
				PlayerPrefs.SetFloat("Rate6",3.8f);
			} else if (star == 5) {
				PlayerPrefs.SetFloat("Ammo6",55f);
			}
			if (star == 5) {
				fullLaserGun.SetActive (true);
				for (int i = 0; i < 5; i++) {
					upgradeLaserGun [i].SetActive (false);
				}
			} else upgradeLaserGun [(int)star].SetActive (true);
			PlayerPrefs.Save ();
		}
	}

	// SET UP CHARACTER
	void ShowInformationCharacter(){
		for (int i = 0; i < buttonSelected.Length; i++) {
			buttonSelected [i].gameObject.SetActive (false);
		}
		buttonSelect [PlayerPrefs.GetInt ("ChoosePlayer")].SetActive (false);
		buttonSelected[PlayerPrefs.GetInt ("ChoosePlayer")].SetActive (true);
		UnlockCharacter ();
	}
	public void ClickSelect(int numberButton){
		for (int i = 0; i < buttonSelected.Length; i++) {
			buttonSelected [i].gameObject.SetActive (false);
			buttonSelect [i].gameObject.SetActive (true);
		}
		buttonSelect [numberButton].SetActive (false);
		buttonSelected [numberButton].SetActive (true);
		PlayerPrefs.SetInt ("ChoosePlayer",numberButton);
		PlayerPrefs.Save ();
	}
	void UnlockCharacter(){
		float level = PlayerPrefs.GetInt ("LevelMission");
		if (level >= 2 && level < 6) {
			imgLockCharacter [1].SetActive (false);
			buttonBuy [1].SetActive (false);
		} else if (level >= 6 && level < 9) {
			for (int i = 1; i < 3; i++) {
				imgLockCharacter [i].SetActive (false);
				buttonBuy [i].SetActive (false);
			}
		} else if (level >= 9 && level < 14) {
			for (int i = 1; i < 4; i++) {
				imgLockCharacter [i].SetActive (false);
				buttonBuy [i].SetActive (false);
			}
		}else if(level>=14 && level<18){
			for (int i = 1; i < 5; i++) {
				imgLockCharacter [i].SetActive (false);
				buttonBuy [i].SetActive (false);
			}
		}else if(level>=18 )//&& level<23
		{
			for (int i = 1; i < 7; i++) {
				imgLockCharacter [i].SetActive (false);
				buttonBuy [i].SetActive (false);
			}
		}
		// else if(level>=23 ){
		// 	for (int i = 1; i < 8; i++) {
		// 		imgLockCharacter [i].SetActive (false);
		// 		buttonBuy [i].SetActive (false);
		// 	}
		// }
		if (PlayerPrefs.GetInt ("UnlockRose") == 1) {
			imgLockCharacter [1].SetActive (false);
			buttonBuy [1].SetActive (false);
		}
		if (PlayerPrefs.GetInt ("UnlockKing") == 1) {
			imgLockCharacter [2].SetActive (false);
			buttonBuy [2].SetActive (false);
			buttonCoins [2].SetActive (false);
		}
		if (PlayerPrefs.GetInt ("UnlockArnol") == 1) {
			imgLockCharacter [3].SetActive (false);
			buttonBuy [3].SetActive (false);
			buttonCoins [3].SetActive (false);
		}
		if (PlayerPrefs.GetInt ("UnlockAmber") == 1) {
			imgLockCharacter [4].SetActive (false);
			buttonBuy [4].SetActive (false);
			buttonCoins [4].SetActive (false);
		}
		if (PlayerPrefs.GetInt ("UnlockDrakhelis") == 1) {
			imgLockCharacter [5].SetActive (false);
			buttonBuy [5].SetActive (false);
			buttonCoins [5].SetActive (false);
		}
		if (PlayerPrefs.GetInt ("UnlockSammy") == 1) {
			imgLockCharacter [6].SetActive (false);
			buttonBuy [6].SetActive (false);
			buttonCoins [6].SetActive (false);
		}

	}
	public void BuyUnlockCharacter(int number){
		float pays = float.Parse (buttonBuy [number].transform.GetChild (2).GetComponent<Text> ().text);
		if (PlayerPrefs.GetFloat ("CoinCollected") > pays) {
			if (number == 1)
				PlayerPrefs.SetInt ("UnlockRose", 1);
			else if (number == 2)
				PlayerPrefs.SetInt ("UnlockKing", 1);
			else if (number == 3)
				PlayerPrefs.SetInt ("UnlockArnol", 1);
			else if (number == 4)
				PlayerPrefs.SetInt ("UnlockAmber", 1);
			else if (number == 5)
				PlayerPrefs.SetInt ("UnlockDrakhelis", 1);
			else if (number == 6)
				PlayerPrefs.SetInt ("UnlockSammy",1);
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") - pays);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
			imgLockCharacter [number].SetActive (false);
			buttonBuy [number].SetActive (false);
			if(number!=1)
				buttonCoins [number].SetActive (false);
			PlayerPrefs.Save ();
		}
	}

	public void BuyCoinsCharacter(int number){
		float pays = float.Parse (buttonCoins [number].transform.GetChild (2).GetComponent<Text> ().text);
		if (PlayerPrefs.GetFloat ("CoinCollected") > pays) {
			if (number == 1)
				PlayerPrefs.SetInt ("UnlockRose", 1);
			else if (number == 2)
				PlayerPrefs.SetInt ("UnlockKing", 1);
			else if (number == 3)
				PlayerPrefs.SetInt ("UnlockArnol", 1);
			else if (number == 4)
				PlayerPrefs.SetInt ("UnlockAmber", 1);
			else if (number == 5)
				PlayerPrefs.SetInt ("UnlockDrakhelis", 1);
			else if (number == 6)
				PlayerPrefs.SetInt ("UnlockSammy",1);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
			buttonCoins [number].SetActive (false);
			PlayerPrefs.Save ();
		}
	}

	//SET UP BOAT
	void ShowInformationBoat(){
		for (int i = 0; i < buttonSelected.Length; i++) {
			boatSelected [i].gameObject.SetActive (false);
		}
		boatSelect [PlayerPrefs.GetInt ("ChooseBoat")].SetActive (false);
		boatSelected[PlayerPrefs.GetInt ("ChooseBoat")].SetActive (true);
		UnlockBoat ();
	}
	void UnlockBoat(){
		float level = PlayerPrefs.GetInt ("LevelMission");
		if (level >= 3 && level < 5) {
			imgLockBoat [1].SetActive (false);
			boatBuy [1].SetActive (false);
		} else if (level >= 5 && level < 7) {
			for (int i = 1; i < 3; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		} else if (level >= 7 && level < 10) {
			for (int i = 1; i < 4; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}else if(level>=10 && level<13){
			for (int i = 1; i < 5; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}else if(level>=13 && level<17){
			for (int i = 1; i < 7; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}else if(level>=17 && level<20 ){
			for (int i = 1; i < 8; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}else if(level>=20 && level<23 ){
			for (int i = 1; i < 8; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}
		else if(level>=23){
			for (int i = 1; i < 8; i++) {
				imgLockBoat [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}
	}
	public void ClickSelectBoat(int number){
		for (int i = 0; i < buttonSelected.Length; i++) {
			boatSelected [i].gameObject.SetActive (false);
			boatSelect [i].gameObject.SetActive (true);
		}
		boatSelect [number].SetActive (false);
		boatSelected [number].SetActive (true);
		PlayerPrefs.SetInt ("ChooseBoat",number);
		PlayerPrefs.Save ();
	}

	//GADGET SETUP UI
	void ShowInformationGadget(){
		for (int i = 0; i < buttonSelected.Length; i++) {
			boatSelected [i].gameObject.SetActive (false);
		}
		boatSelect [PlayerPrefs.GetInt ("ChooseBoat")].SetActive (false);
		boatSelected[PlayerPrefs.GetInt ("ChooseBoat")].SetActive (true);
		UnlockGadGet ();
	}
	void UnlockGadGet(){
		float level = PlayerPrefs.GetInt ("LevelMission");
		if (level >= 2 && level < 3) {
			imgLockGadget [0].SetActive (false);
			gadgetBuy [0].SetActive (false);
		} else if (level ==3) {
			for (int i = 1; i < 1; i++) {
				imgLockGadget [i].SetActive (false);
				gadgetBuy [i].SetActive (false);
			}
		} else if (level ==4) {
			for (int i = 1; i < 2; i++) {
				imgLockGadget [i].SetActive (false);
				gadgetBuy [i].SetActive (false);
			}
		}else if(level==5){
			for (int i = 1; i < 3; i++) {
				imgLockGadget [i].SetActive (false);
				gadgetBuy [i].SetActive (false);
			}
		}else if(level==6){
			for (int i = 1; i < 4; i++) {
				imgLockGadget [i].SetActive (false);
				gadgetBuy [i].SetActive (false);
			}
		}else if(level>=7 && level<9 ){
			for (int i = 1; i < 5; i++) {
				imgLockGadget [i].SetActive (false);
				gadgetBuy [i].SetActive (false);
			}
		}else if(level>=9 && level<11 ){
			for (int i = 1; i < 6; i++) {
				imgLockGadget [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}
		else if(level>=11 && level<16){
			for (int i = 1; i < 7; i++) {
				imgLockGadget [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}else if(level>=16 && level<20){
			for (int i = 1; i < 8; i++) {
				imgLockGadget [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}else if(level>=20){
			for (int i = 1; i < 9; i++) {
				imgLockGadget [i].SetActive (false);
				boatBuy [i].SetActive (false);
			}
		}
	}
	public void ClickSelectGadget(){
	
	}

	//back game
	public void BackClick(){
		if (!isBackClick) {
			isBackClick = true;
			StartCoroutine (LoadMainScene ());
		}
	}
	IEnumerator LoadMainScene(){
		Animator animConvert = GameObject.Find ("NextSceneCanvas").GetComponentInChildren<Animator> ();
		yield return new WaitForSeconds (1f);
		animConvert.SetTrigger ("In");
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("MainScene");
		animConvert.SetTrigger ("Out");
	}
}
