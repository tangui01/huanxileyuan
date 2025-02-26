using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WGM;

public class PopUpHandler : MonoBehaviour {

	[HideInInspector]
	public Menu pauseMenu, gameOverMenu, keepPlayingMenu, resume;
	[HideInInspector]
	public Image GreenButton, GrayButton;
	AircraftBattleMenuManager menuMngr;
	Text teslaWeaponNumberText, laserWeaponNumberText, bladesWeaponNumberText, bombWeaponNumberText;
	Text StarsNumberTextInSlowScreen;
	Color buttonUseActiveColor = new Color(0.92941f, 0.93333f, 0.0f);
	GameObject teslaWeapon, laserWeapon, bladesWeapon, bombWeapon, starsCounter;
	public static bool gameStarted = false;
	public bool bossDestroyed = false;
	/// <summary>
	/// The type of the popup. 0 - no popup, 1 - slow time screen, 2 - paused game, 3 - keep playing, 4 - game over, 5 - popup not allowed, 6 - no video available
	/// </summary>
	public static int popupType = 5;
	bool videoReady = false;
	bool progressQuit;

	static PopUpHandler instance;
	public static PopUpHandler Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(PopUpHandler)) as PopUpHandler;
			
			return instance;
		}
	}

	void Awake()
	{
		

	}

	// Use this for initialization
	void Start () {
		starsCounter = GameObject.Find("StarsCounterHolder");
		// UpdateStateOfBuyShopButtons();
		// UpdateStateOfWeaponsAndStars();
		//WebelinxCMS.Instance.IsVideoRewardAvailable(4);
		menuMngr = GameObject.Find("AircraftBattleCanvas").GetComponent<AircraftBattleMenuManager>();
	}

	public void IsKeepPlayingVideoAvailable(bool status)
	{
		if(status)
		{
			transform.Find("KeepPlayingPopUp/AnimationHolder/PopUpHolder/AnimationHolder/Or").gameObject.SetActive(true);
			transform.Find("KeepPlayingPopUp/AnimationHolder/PopUpHolder/AnimationHolder/WatchVideoButton").gameObject.SetActive(true);
			videoReady = true;
		}
		else
			ShowNoVideoAvailablePopUp();
	}

	public void ShowWatchVideo()
	{

	}

	public void WatchVideoCompleted(string nista)
	{
//		Ressurect();
		// PandaPlane.Instance.NewPlane();
	}

	public void UpdateStateOfWeaponsAndStars()
	{

		
	}

	public void UpdateStateOfBuyShopButtons()
	{

		if(PandaPlane.Instance.teslaWeaponPrice<=PandaPlane.Instance.stars)
		{
			teslaWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GreenButton.sprite;
		}
		else
		{
			teslaWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GrayButton.sprite;
		}

		if(PandaPlane.Instance.laserWeaponPrice<=PandaPlane.Instance.stars)
		{
			laserWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GreenButton.sprite;
		}
		else
		{
			laserWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GrayButton.sprite;
		}

		if(PandaPlane.Instance.bladesWeaponPrice<=PandaPlane.Instance.stars)
		{
			bladesWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GreenButton.sprite;
		}
		else
		{
			bladesWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GrayButton.sprite;
		}

		if(PandaPlane.Instance.bombWeaponPrice<=PandaPlane.Instance.stars)
		{
			bombWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GreenButton.sprite;
		}
		else
		{
			bombWeapon.transform.GetChild(0).Find("ButtonBuy").GetComponent<Image>().sprite = GrayButton.sprite;
		}


	}

	public void ShowPauseScreen()
	{
		PlaneManager.Instance.menuIsActive = true;
		Debug.Log("TEST ShowPauseScreen poziv");
		menuMngr.ShowMenu(pauseMenu);
		popupType = 2;
		Time.timeScale = 0;
		Debug.Log("TEST timescale na 0");
	}

	void UpdateStateOfUseShopButtons()
	{
		if(PandaPlane.Instance.teslaLvl<1 && PandaPlane.Instance.laserLvl<1 && PandaPlane.Instance.bladesLvl<1 && PandaPlane.Instance.bombLvl<1)
		{
			starsCounter.SetActive(false);
		}

		if(gameStarted)
		{
			if(PandaPlane.Instance.teslaLvl<1)
			{
				teslaWeapon.SetActive(false);
			}
			else if(PandaPlane.Instance.teslaWeaponNumber>0)
			{
				teslaWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
				teslaWeapon.transform.GetChild(0).Find("PowerUpIcon").GetComponent<Button>().interactable = true;
			}
			
			if(PandaPlane.Instance.laserLvl<1)
			{
				laserWeapon.SetActive(false);
			}
			else if(PandaPlane.Instance.laserWeaponNumber>0)
			{
				laserWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
				laserWeapon.transform.GetChild(0).Find("PowerUpIcon").GetComponent<Button>().interactable = true;
			}
			
			if(PandaPlane.Instance.bladesLvl<1)
			{
				bladesWeapon.SetActive(false);
			}
			else if(PandaPlane.Instance.bladesWeaponNumber>0)
			{
				bladesWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
				bladesWeapon.transform.GetChild(0).Find("PowerUpIcon").GetComponent<Button>().interactable = true;
			}
			
			if(PandaPlane.Instance.bombLvl<1)
			{
				bombWeapon.SetActive(false);
			}
			else if(PandaPlane.Instance.bombWeaponNumber>0)
			{
				bombWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
				bombWeapon.transform.GetChild(0).Find("PowerUpIcon").GetComponent<Button>().interactable = true;
			}
		}
		else
		{
			
		}
		
	}

	public void ShowSlowTimeScreen()
	{
		if(PandaPlane.Instance.health <= 0)
		{
			ShowGameOverScreen();
		}
		else
		{
			//PlaneManager.Instance.menuIsActive = true; //IZMENA
			//StartCoroutine(Interface.Instance.SlowTime());
			// UpdateStateOfWeaponsAndStars();
			UpdateStateOfBuyShopButtons();
			UpdateStateOfUseShopButtons();
		}
	}

	public void ShowGameOverScreen()
	{
		// SoundManager.Instance.Stop_GameplayMusic();
		// SoundManager.Instance.Stop_BossMusic();
		// SoundManager.Instance.Stop_BossPlaneMovement();
		// SoundManager.Instance.Stop_BossShipMovement();
		// SoundManager.Instance.Stop_BossTankMovement();
		// SoundManager.Instance.Play_GameOver();
		
	}

	public void ShowNoVideoAvailablePopUp()
	{
		if(videoReady)
		{
			popupType = 6;
			SoundManager.Instance.Play_ButtonClick();
			transform.Find("KeepPlayingPopUp/AnimationHolder/PopUpHolderNoVideo/AnimationHolder").GetComponent<Animator>().Play("ConfirmationMessageShow");
		}
	}

	public void CloseNoVideoAvailablePopUp()
	{
		//ShowKeepPlayingScreen();
		popupType = 3;
		SoundManager.Instance.Play_ButtonClick();
		transform.Find("KeepPlayingPopUp/AnimationHolder/PopUpHolderNoVideo/AnimationHolder").GetComponent<Animator>().Play("ConfirmationMessageClose");
	}

	public void ShowKeepPlayingScreen()
	{
		
	}
	
	
	public void BuyTesla()
	{
		if(PandaPlane.Instance.teslaWeaponPrice<=PandaPlane.Instance.stars)
		{
			PandaPlane.Instance.teslaWeaponNumber++;
			PandaPlane.Instance.TakeAwayStars(PandaPlane.Instance.teslaWeaponPrice);
			teslaWeaponNumberText.text = PandaPlane.Instance.teslaWeaponNumber.ToString();
			StarsNumberTextInSlowScreen.text = PandaPlane.Instance.stars.ToString();
			if(PandaPlane.Instance.teslaWeaponNumber==1)
			{
				teslaWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
			}
			UpdateStateOfBuyShopButtons();
			SoundManager.Instance.Play_UpgradePlane();
		}
		else
		{
			starsCounter.transform.GetChild(0).GetComponent<Animator>().Play("StarsCounterGamePlayScale");
			SoundManager.Instance.Play_NotEnoughStars();
		}

	}

	public void UseTesla()
	{
		if(PandaPlane.Instance.teslaWeaponNumber>0)
		{
			SoundManager.Instance.Play_ActivateWeapon();
			//ResumeGame();	
			PlaneManager.Instance.NormalTimeAfterWeaponUse();
			PandaPlane.Instance.teslaWeaponNumber--;
			teslaWeaponNumberText.text = PandaPlane.Instance.teslaWeaponNumber.ToString();
			if(PandaPlane.Instance.teslaWeaponNumber<=0)
			{
				teslaWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = Color.white;
			}
			PlaneManager.Instance.StartTesla();
		}
		else
		{
			teslaWeapon.transform.GetChild(0).GetComponent<Animator>().Play("BuyButtonGamePlayScale");
			SoundManager.Instance.Play_NoMoreWeapons();
		}
	}

	public void TeslaProp()
	{
		SoundManager.Instance.Play_ActivateWeapon();
		PlaneManager.Instance.StartTesla();
	}
	
	public void BuyLaser()
	{
		if(PandaPlane.Instance.laserWeaponPrice<=PandaPlane.Instance.stars)
		{
			PandaPlane.Instance.laserWeaponNumber++;
			PandaPlane.Instance.TakeAwayStars(PandaPlane.Instance.laserWeaponPrice);
			laserWeaponNumberText.text = PandaPlane.Instance.laserWeaponNumber.ToString();
			StarsNumberTextInSlowScreen.text = PandaPlane.Instance.stars.ToString();
			if(PandaPlane.Instance.laserWeaponNumber==1)
			{
				laserWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
			}
			UpdateStateOfBuyShopButtons();
			SoundManager.Instance.Play_UpgradePlane();
		}
		else
		{
			starsCounter.transform.GetChild(0).GetComponent<Animator>().Play("StarsCounterGamePlayScale");
			SoundManager.Instance.Play_NotEnoughStars();
		}
	}
	
	public void UseLaser()
	{
		if(PandaPlane.Instance.laserWeaponNumber>0)
		{
			SoundManager.Instance.Play_ActivateWeapon();
			//ResumeGame();
			PlaneManager.Instance.NormalTimeAfterWeaponUse();
			PandaPlane.Instance.laserWeaponNumber--;
			laserWeaponNumberText.text = PandaPlane.Instance.laserWeaponNumber.ToString();
			if(PandaPlane.Instance.laserWeaponNumber<=0)
			{
				laserWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = Color.white;
			}
			PlaneManager.Instance.StartLaser();
			SoundManager.Instance.Play_UpgradePlane();
		}
		else
		{
			laserWeapon.transform.GetChild(0).GetComponent<Animator>().Play("BuyButtonGamePlayScale");
			SoundManager.Instance.Play_NoMoreWeapons();
		}
	}
	
	public void LaserProp()
	{
		SoundManager.Instance.Play_ActivateWeapon();
		PlaneManager.Instance.StartLaser();
	}
	
	public void BuyBlades()
	{
		if(PandaPlane.Instance.bladesWeaponPrice<=PandaPlane.Instance.stars)
		{
			PandaPlane.Instance.bladesWeaponNumber++;
			PandaPlane.Instance.TakeAwayStars(PandaPlane.Instance.bladesWeaponPrice);
			bladesWeaponNumberText.text = PandaPlane.Instance.bladesWeaponNumber.ToString();
			StarsNumberTextInSlowScreen.text = PandaPlane.Instance.stars.ToString();
			if(PandaPlane.Instance.bladesWeaponNumber==1)
			{
				bladesWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
			}
			UpdateStateOfBuyShopButtons();
			SoundManager.Instance.Play_UpgradePlane();
		}
		else
		{
			starsCounter.transform.GetChild(0).GetComponent<Animator>().Play("StarsCounterGamePlayScale");
			SoundManager.Instance.Play_NotEnoughStars();
		}
	}
	
	public void UseBlades()
	{
		if(PandaPlane.Instance.bladesWeaponNumber>0)
		{
			SoundManager.Instance.Play_ActivateWeapon();
			//ResumeGame();
			PlaneManager.Instance.NormalTimeAfterWeaponUse();
			PandaPlane.Instance.bladesWeaponNumber--;
			bladesWeaponNumberText.text = PandaPlane.Instance.bladesWeaponNumber.ToString();
			if(PandaPlane.Instance.bladesWeaponNumber<=0)
			{
				bladesWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = Color.white;
			}
			PlaneManager.Instance.StartBlades();
		}
		else
		{
			bladesWeapon.transform.GetChild(0).GetComponent<Animator>().Play("BuyButtonGamePlayScale");
			SoundManager.Instance.Play_NoMoreWeapons();
		}
	}
	public void BladesProp()
	{
		SoundManager.Instance.Play_ActivateWeapon();
		PlaneManager.Instance.StartBlades();
	}
	
	public void BuyBomb()
	{
		if(PandaPlane.Instance.bombWeaponPrice<=PandaPlane.Instance.stars)
		{
			PandaPlane.Instance.bombWeaponNumber++;
			PandaPlane.Instance.TakeAwayStars(PandaPlane.Instance.bombWeaponPrice);
			bombWeaponNumberText.text = PandaPlane.Instance.bombWeaponNumber.ToString();
			StarsNumberTextInSlowScreen.text = PandaPlane.Instance.stars.ToString();
			if(PandaPlane.Instance.bombWeaponNumber==1)
			{
				bombWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = buttonUseActiveColor;
			}
			UpdateStateOfBuyShopButtons();
			SoundManager.Instance.Play_UpgradePlane();
		}
		else
		{
			starsCounter.transform.GetChild(0).GetComponent<Animator>().Play("StarsCounterGamePlayScale");
			SoundManager.Instance.Play_NotEnoughStars();
		}
	}

	public void UseBomb()
	{
		if(PandaPlane.Instance.bombWeaponNumber>0)
		{
			SoundManager.Instance.Play_ActivateWeapon();
			//ResumeGame();
			PlaneManager.Instance.NormalTimeAfterWeaponUse();
			PandaPlane.Instance.bombWeaponNumber--;
			bombWeaponNumberText.text = PandaPlane.Instance.bombWeaponNumber.ToString();
			if(PandaPlane.Instance.bombWeaponNumber<=0)
			{
				bombWeapon.transform.GetChild(0).Find("ButtonUse").GetComponent<Image>().color = Color.white;
			}
			PlaneManager.Instance.StartBomb();
		}
		else
		{
			bombWeapon.transform.GetChild(0).GetComponent<Animator>().Play("BuyButtonGamePlayScale");
			SoundManager.Instance.Play_NoMoreWeapons();
		}
	}

	public void BombProp()
	{
		SoundManager.Instance.Play_ActivateWeapon();
		PlaneManager.Instance.StartBomb();
	}
	public void LoadMainScene()
	{
		Application.Quit();
		// SoundManager.Instance.Stop_GameplayMusic();
		// SoundManager.Instance.Stop_BossMusic();
		// SoundManager.Instance.Stop_BossPlaneMovement();
		// SoundManager.Instance.Stop_BossShipMovement();
		// SoundManager.Instance.Stop_BossTankMovement();
		// StartCoroutine("LoadMain");
	}

	IEnumerator LoadMain()
	{
		Time.timeScale = 1;
		PandaPlane.Instance.GetComponent<Collider2D>().enabled = false;
		PlaneManager.Instance.guiCamera.transform.Find("LoadingHolder 1").GetChild(0).GetComponent<Animation>().Play("LoadingArrival2New");
		SoundManager.Instance.Play_DoorClosing();
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("MainScene");
		SoundManager.Instance.Stop_BossPlaneMovement();
		SoundManager.Instance.Stop_BossShipMovement();
		SoundManager.Instance.Stop_BossTankMovement();
		popupType = 5;
	}

	public void RestartGame()
	{
		StartCoroutine("Restart");
	}

	IEnumerator Restart()
	{
		PlaneManager.Instance.guiCamera.transform.Find("LoadingHolder 1").GetChild(0).GetComponent<Animation>().Play("LoadingArrival2New");
		SoundManager.Instance.Play_DoorClosing();
		LevelGenerator.terrainsPassed = 0;
		gameStarted = false;
		MoveBg.hasBridge = false;
		LevelGenerator.checkpoint = true;
		LevelGenerator.currentStage = 1;
		LevelGenerator.currentBossPlaneLevel = 1;
		LevelGenerator.currentBossShipLevel = 1;
		LevelGenerator.currentBossTankLevel = 1;
		PandaPlane.Instance.numberOfKills = 0;
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("Woods");
		SoundManager.Instance.Stop_BossPlaneMovement();
		SoundManager.Instance.Stop_BossShipMovement();
		SoundManager.Instance.Stop_BossTankMovement();
	}

	public void KeepPlaying()
	{
		PandaPlane.Instance.TakeAwayStars(CalculateStarsForKeepPlaying());
	}

	public void ResumeGame()
	{
		//StartCoroutine("Resume");
		StartCoroutine(Resume());
	}

	public void ResumeGameFromPause()
	{
		StartCoroutine("ResumeFromPause");
		Time.timeScale = 0.1f;
		popupType = 1;

	}

	public IEnumerator Resume()
	{
		//yield return new WaitForSeconds(0.5f);
//		Interface.Instance.normalTime = true;
		//StopCoroutine(Interface.Instance.SlowTime());
		//StartCoroutine(Interface.Instance.NormalTime());
		//if(PlaneManager.pressedAndHold)
		{
			PlaneManager.Instance.menuIsActive = false;

			if(!gameStarted)
			{
				gameStarted=true;
				menuMngr.ShowMenu(resume);
				yield return new WaitForSeconds(0.5f);
				popupType = 0;
			}
//			else
//			{
//				Debug.Log("3 samo sgohwon menu resume");
//				yield return new WaitForSeconds(0.1f);
//				if(PlaneManager.pressedAndHold)
//				{
//					Debug.Log("4 reusme");
//					menuMngr.ShowMenu(resume);
//				}
//			}
		}
	}

	public void ShowMenu()
	{
		menuMngr.ShowMenu(resume);
		popupType = 0;
	}

	IEnumerator ResumeFromPause()
	{
//		if(!gameStarted)
//		{
//			gameStarted=true;
//			menuMngr.ShowMenu(pauseMenu);
//			yield return new WaitForSeconds(0.5f);
//			teslaWeapon.transform.GetChild(0).FindChild("ButtonUse").gameObject.SetActive(true);
//			laserWeapon.transform.GetChild(0).FindChild("ButtonUse").gameObject.SetActive(true);
//			bladesWeapon.transform.GetChild(0).FindChild("ButtonUse").gameObject.SetActive(true);
//			bombWeapon.transform.GetChild(0).FindChild("ButtonUse").gameObject.SetActive(true);
//			slowTimeMenu.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
//			slowTimeMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
//		}
//		else
//			//		yield return new WaitForSeconds(0.1f);

		yield return null;
	}

	int CalculateStarsForKeepPlaying()
	{
		//((2*trenutni_teren)na_kvadrat+5*trenutni_teren)+trenutni_stage*trenutni_stage*100
		int neededStars=0;
		int currentStage = LevelGenerator.currentStage;
		int currentTerrainInStage = LevelGenerator.terrainsPassed;
		
		neededStars = (int)((2*Mathf.Pow(currentTerrainInStage,2)+5*currentTerrainInStage)+Mathf.Pow(currentStage,2)*50);
		return neededStars;
	}

	public void Ressurect()
	{
		
	}

	public void SpeedUpDialog()
	{
		// DialogPanda.dialogPressed = true;
		PlaneManager.Instance.gameActive = false;
	}

//	public void SpeedUpBossDialog()
//	{
//		DialogBoss.dialogPressed = true;
//		PlaneManager.Instance.gameActive = false;
//	}

	public void NormalDialog()
	{
		// DialogPanda.dialogPressed = false;
		PlaneManager.Instance.gameActive = true;
	}
	
//	public void NormalBossDialog()
//	{
//		DialogBoss.dialogPressed = false;
//		PlaneManager.Instance.gameActive = true;
//	}

	public void AnaliticKeepPlayingQuit()
	{
//		if(GooglePlayGameServices.Instance.GameAnaliticOn)
//		{
//			GA.API.Design.NewEvent("ContinueDie"+":"+"Quit");
//		}
	}
	
	public void AnaliticKeepPlayingStars()
	{
//		if(GooglePlayGameServices.Instance.GameAnaliticOn)
//		{
//			GA.API.Design.NewEvent("ContinueDie"+":"+"Stars");
//		}
	}
	
	public void AnaliticKeepPlayingVideo()
	{
//		if(GooglePlayGameServices.Instance.GameAnaliticOn)
//		{
//			GA.API.Design.NewEvent("ContinueDie"+":"+"Video");
//		}
	}

	public void OpenConfirmationMessagePopup()
	{
		popupType = 5;
		SoundManager.Instance.Play_ButtonClick();
		transform.Find("PausePopUp/AnimationHolder/PopUpHolderConfirmationMessage/AnimationHolder").GetComponent<Animator>().Play("ConfirmationMessageShow");
	}
	
	public void CloseConfirmationMessagePopup()
	{
		popupType = 2;
		SoundManager.Instance.Play_ButtonClick();
		transform.Find("PausePopUp/AnimationHolder/PopUpHolderConfirmationMessage/AnimationHolder").GetComponent<Animator>().Play("ConfirmationMessageClose");
	}

	public void ShowBanner()
	{
	}
	
	public void HideBanner()
	{
	}

	public void ShowInterstitial()
	{
		
	}

	void OnApplicationPause(bool status)
	{
		if(status) // game paused
		{
			if(!LevelGenerator.Instance.stageCleared && PandaPlane.Instance.health>0)
			{
				Debug.Log("TEST PAUSE poziva se pause screen");
				if(!progressQuit)
				{
					ShowPauseScreen();
				}
			}
			else
			{
				Debug.Log("TEST PAUSE ne poziva se pause screen");
			}
		}

	}

	public void ProgresQuitChange(bool newValue)
	{
		progressQuit = newValue;
	}
}
