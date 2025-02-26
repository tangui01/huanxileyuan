using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AircraftBattleMenuManager : MonoBehaviour 
{

	public  int defaultStarsNumber = 0;
	public  int defaultHealthLvl = 0;
	public  int defaultArmorLvl = 0;
	public  int defaultMainGunLvl = 0;
	public  int defaultWingGunLvl = 0;
	public  int defaultSideGunLvl = 0;
	public  int defaultMagnetLvl = 0;
	public  int defaultShieldLvl = 0;
	public  int defaultDoubleStarsLvl = 0;
	public  int defaultBombLvl = 0;
	public  int defaultTeslaLvl = 0;
	public  int defaultLaserLvl = 0;
	public  int defaultBladesLvl = 0;
	public  int defaultTeslaWeaponNumber = 0;
	public  int defaultLaserWeaponNumber = 0;
	public  int defaultBladesWeaponNumber = 0;
	public  int defaultBombWeaponNumber = 0;
	public Menu CurrentMenu;
	public Menu EmptyMenu;
	[HideInInspector]
	public Animator openObject;
	public GameObject[] disabledObjects;
	/// <summary>
	/// The type of the popup. 1 - exit game, 2 - menu, 3 - explanation popup
	/// </summary>
	public static int popupType = 1;


	// Use this for initialization

	
	void Start () 
	{
		if (disabledObjects!=null) {
			for(int i=0;i<disabledObjects.Length;i++)
				disabledObjects[i].SetActive(false);
		}


		if (Application.loadedLevelName != "MapScene" && CurrentMenu!=null)
		{
			ShowMenu(CurrentMenu);
		}
		
		// if(!CurrentMenu.name.Equals("MainMenu") && !LevelGenerator.checkpoint)//LevelGenerator.currentStage>1)
		// {
		// 	PopUpHandler.gameStarted=true;
		// 	ShowMenu(EmptyMenu);
		// }

		if(CurrentMenu !=null && CurrentMenu.name.Equals("MainMenu") )
		{
			int score = 0;
			if(score<=0)
			{
				GameObject.Find("BestScore").SetActive(false);
			}
			else
			{
				GameObject.Find("BestScoreText").GetComponent<Text>().text = score.ToString();
			}
			GameObject.Find("AvailablePurchases").GetComponent<Text>().text = ShopNotificationClass.numberNotification.ToString();
			if(ShopNotificationClass.numberNotification>0)
			{
				GameObject.Find("ButtonShop/AnimationHolder").GetComponent<Animation>().Play();
			}
			GameObject.Find("PandaCommanderLogoHolder").transform.GetChild(0).GetComponent<Animator>().Play("PandaCommanderLogoArriving");
		}
			
	}
	public Animator menuAnimator;

	void Update()
	{
		// if(Input.GetKeyUp(KeyCode.Escape))
		// {
		// 	if(Application.loadedLevelName.Equals("MainScene"))
		// 	{
		// 		if(popupType == 1)
		// 		{
		// 			//Application.Quit();
		// 			AdsManager.Instance.ShowInterstitial();
		// 		}
		// 		else if(popupType == 2)
		// 		{
		// 			Debug.Log("CURRENT MENU: " + CurrentMenu.name);
		//
		// 			ShowMenu(transform.Find("MainMenu").GetComponent<Menu>());
		// 		}
		// 		else if(popupType == 3)
		// 		{
		// 			transform.Find("ShopMenu").GetComponent<Shop>().HideExplanationPopUp();
		// 		}
		// 	}
		// }
	}

	public void EnableObject(GameObject gameObject)
	{

		if (gameObject != null) 
		{
			if (!gameObject.activeSelf) 
			{
				gameObject.SetActive (true);
			}
		}
		//menuAnimator.SetBool ("IsOpen", false);
	}

	public void DisableObject(GameObject gameObject)
	{

		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
		//menuAnimator.SetBool ("IsOpen", false);
	}

	public void PlayAnimation(string stateName)
	{

		menuAnimator.SetTrigger(stateName);
		//menuAnimator.SetBool ("IsOpen", false);
	}


	/// <summary>
	/// Animation event function that calls loading of Scene 
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadScene(string levelName )
	{
		if (levelName != "") {
						try {
								Application.LoadLevel (levelName);
						} catch (System.Exception e) {
								Debug.Log ("Can't load scene: " + e.Message);
						}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	/// <summary>
	/// Animation event function that calls loading of Scene async
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadSceneAsync(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevelAsync (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	void SecurityCheckSlowTimeScreen()
	{
		Debug.Log("TEST poziv security check");
		if(!transform.Find("SlowTimeScreen").gameObject.activeSelf)
		{
			transform.Find("SlowTimeScreen").gameObject.SetActive(true);
			transform.Find("SlowTimeScreen").GetComponent<Menu>().IsOpen = true;
		}
//		Debug.Log("Detaljan report: timeScale: " + Time.timeScale + ", alpha: " + transform.Find("SlowTimeScreen").GetComponent<CanvasGroup>().alpha + ", game active: " + PlaneManager.Instance.ReturnGameActive() 
//		          + ", normal time: " + PlaneManager.Instance.ReturnNormalTime() + ", menu is active: " + PlaneManager.Instance.ReturnMenuIsActive() + ", dontslowtime: " + PlaneManager.Instance.ReturnDontSlowTime());
		if(transform.Find("SlowTimeScreen").GetComponent<CanvasGroup>().alpha < 1)
		{
			transform.Find("SlowTimeScreen").GetComponent<Animator>().Play("Open",0,0);
		}
	}

	public void ShowMenu(Menu menu)
	{
		if ( menu ==null || menu.name.Equals("TEST ShowMenu poziv"))
		{
			return;
		}
		if (CurrentMenu != null)
		{
			CurrentMenu.IsOpen = false;
//			if(CurrentMenu.name.Equals("SlowTimeScreen"))
//			{
//				Invoke("AnotherSecurityCheck",0.08f);
//			}
		}
		// if(!menu.gameObject.name.Equals("MainMenu") && !menu.gameObject.name.Equals("EmptyMenu") && !menu.gameObject.name.Equals("SlowTimeScreen"))
		// 	SoundManager.Instance.Play_ButtonClick();

	

		CurrentMenu = menu;
		CurrentMenu.gameObject.SetActive (true);
		CurrentMenu.IsOpen = true;

		if(menu.name.Equals("SlowTimeScreen"))
		{
			Invoke("SecurityCheckSlowTimeScreen",0.08f);//bilo je 0.01
		}

		else if(menu.name.Equals("MainMenu"))
		{
			if(popupType != 1)
				SoundManager.Instance.Play_ButtonClick();
			GameObject.Find("AvailablePurchases").GetComponent<Text>().text = ShopNotificationClass.numberNotification.ToString();
			GameObject.Find("PandaCommanderLogoHolder").transform.GetChild(0).GetComponent<Animator>().Play("PandaCommanderLogoArriving");
			popupType = 1;
		}
		else
		{
			popupType = 2;
			if(menu.name.Equals("ShopMenu"))
			{
				menu.transform.Find("HeaderShop/AnimationHolder/StarsNumber/StarsNumberShopText").GetComponent<Text>().text = Shop.stars.ToString();
			}
			else if(menu.name.Equals("FreeCoinsMenu"))
			{
				menu.transform.Find("HeaderFreeCoins/AnimationHolder/StarsNumber/StarsNumberFreeCoinsText").GetComponent<Text>().text = Shop.stars.ToString();
			}
			else if(menu.name.Equals("InAppMenu"))
			{
				menu.transform.Find("HeaderInApp/AnimationHolder/StarsNumber/StarsNumberInAppText").GetComponent<Text>().text = Shop.stars.ToString();
			}
		}

	}

	public void SenseiExplane()
	{
		GameObject sensei = GameObject.Find("BossHealthArmorScoreHolder/AnimationHolder");
		sensei.GetComponent<Animation>().Play();
		sensei.transform.Find("BlackBackground").gameObject.SetActive(true);
	}

	public void CloseMenu(Menu menu)
	{
		if (menu != null) 
		{
			menu.IsOpen = false;
			popupType = 1;
		}
	}

	public void  ShowMenuMapBottom()
	{
		//ovde se gasi story meni
		if(CurrentMenu!=null)
		CurrentMenu.gameObject.SetActive (false);
		CurrentMenu = disabledObjects[9].GetComponent<Menu>();
		CurrentMenu.gameObject.SetActive (true);
		CurrentMenu.IsOpen = true;
	}


	public void  ShowMenuStory()
	{
		if (CurrentMenu != null)
			CurrentMenu.IsOpen = false;
		
		CurrentMenu = disabledObjects[10].GetComponent<Menu>();
		CurrentMenu.gameObject.SetActive (true);
	//	CurrentMenu.IsOpen = true;
	}


	public void PlayOpenAnimation(Animator animator)
	{
		if(!animator.gameObject.activeSelf)
			animator.gameObject.SetActive(true);
		if (openObject != animator) 
		{
			if (openObject != null)
			{
				PlayStopAnimation (openObject);
			}
			animator.SetBool ("IsOpen", true);
			openObject = animator;
		} 
		else 
		{
			PlayStopAnimation(openObject);	
		}
	}
	public void PlayStopAnimation(Animator animator)
	{
		openObject = null;
		animator.SetBool("IsOpen", false);
	}

	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}

}
