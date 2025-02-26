using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	Text starsNumberTextShop, healthPriceText, armorPriceText, mainGunPriceText, wingGunPriceText, sideGunPriceText, magnetPriceText, shieldPriceText, doubleStarsPriceText, bombPriceText, teslaPriceText, laserPriceText, bladesPriceText;
	string[] upgradesStatesInfo = new string[12];
	Text UpgradeTitleText, UpgradeInfoPriceText, UpgradeDescriptionText;
	int availablePurchases = 0;
	int clickedUpgrade=0;
	Image ExplanationImage;
	GameObject ExplanationPopUp;
	Image MainGun, SideGun, WingGun;
	int currentStateHealth, currentStateArmor, currentStateMainGun, currentStateWingGun, currentStateSideGun,  currentStateMagnet, currentStateDoubleStars, currentStateShield;
	Color Blue = new Color(0.00000f, 0.92549f, 1.00000f);
	Color Green = new Color(0.63529f, 0.99216f, 0.36863f);
	Color Grey = new Color(0.80000f, 0.80000f, 0.80000f);
	public static int highScore, laserNumber, teslaNumber, bladesNumber, bombNumber;
	public static int currentStateTesla, currentStateBomb, currentStateLaser, currentStateBlades;
	public static int stars;

	public static int[] healthPrice = new int[] {200, 350, 700, 1100, 1600, 2200, 3000, 3900, 5200, 7000};
	public static int[] armorPrice = new int[] {200, 300, 600, 1000, 1500, 2000, 2700, 3500, 5000, 6500};
	public static int[] mainGunPrice = new int[] {200, 400, 800, 1400, 2100, 2900, 4000, 5100, 6300, 8000};
	public static int[] wingGunPrice = new int[] {200, 400, 700, 1300, 2000, 2800, 3800, 4900, 6100, 7700};
	public static int[] sideGunPrice = new int[] {200, 350, 600, 900, 1200, 1600, 2000, 2500, 3200, 4000};
	public static int[] magnetPrice = new int[] {150, 300, 450, 700, 1000, 1300, 1600, 1900, 2200, 2500};
	public static int[] shieldPrice = new int[] {150, 300, 450, 700, 1000, 1300, 1600, 1900, 2200, 2500};
	public static int[] doubleStarsPrice = new int[] {150, 300, 450, 700, 1000, 1300, 1600, 1900, 2200, 2500};
	public static int[] bombPrice = new int[] {600, 800, 1000, 1300, 1700, 2200, 2700, 3400, 4500, 6000};
	public static int[] teslaPrice = new int[] {500, 750, 900, 1200, 1500, 2000, 2500, 3100, 3800, 5000};
	public static int[] laserPrice = new int[] {500, 750, 900, 1200, 1500, 2000, 2500, 3100, 3800, 5000};
	public static int[] bladesPrice = new int[] {400, 600, 800, 1100, 1400, 1800, 2300, 2800, 3300, 4000};
	// Use this for initialization
	void Awake () {

		// if(!PlayerPrefs.HasKey("TutorialCompleted"))
		// {
		// 	PlayerPrefs.SetInt("TutorialCompleted",1);
		// }

		if(SoundManager.forceTurnOff)
		{
			SoundManager.soundOn = 1;
			SoundManager.forceTurnOff = false;
		}

		SoundManager.Instance.Play_MenuMusic();

		ExplanationPopUp = GameObject.Find("ExplanationPopUp/AnimationHolder");
		
		string sarma_San = "7#7#24#31#45#66#91#157#248#395#633#1018";
		string[] sarma = sarma_San.Split('#');
		currentStateHealth = int.Parse(sarma[0])-7;
		currentStateArmor = int.Parse(sarma[1])-7;
		currentStateMainGun = int.Parse(sarma[2]) - 14;
		currentStateWingGun = int.Parse(sarma[3]) - 21;
		currentStateSideGun = int.Parse(sarma[4]) - 35;
		currentStateMagnet = int.Parse(sarma[5]) - 56;
		currentStateShield = int.Parse(sarma[6]) - 91;
		currentStateDoubleStars = int.Parse(sarma[7]) - 147;
		currentStateLaser = int.Parse(sarma[8]) - 238;
		currentStateTesla = int.Parse(sarma[9]) - 385;
		currentStateBlades = int.Parse(sarma[10]) - 623;
		currentStateBomb = int.Parse(sarma[11]) - 1008;

		string uzengije = "10#-20#5#5#5#5";
		string[] kamaraUzengije = uzengije.Split('#');
		stars = int.Parse(kamaraUzengije[0]) - 10;
//			stars=654321;
		highScore = int.Parse(kamaraUzengije[1]) + 20;
		laserNumber = int.Parse(kamaraUzengije[2]) - 5;
		teslaNumber = int.Parse(kamaraUzengije[3]) - 5;
		bladesNumber = int.Parse(kamaraUzengije[4]) - 5;
		bombNumber = int.Parse(kamaraUzengije[5]) - 5;

			
		// else
		// {
		// 	stars=MenuManager.defaultStarsNumber;
		// 	currentStateHealth=MenuManager.defaultHealthLvl;
		// 	currentStateArmor=MenuManager.defaultArmorLvl;
		// 	currentStateMainGun=MenuManager.defaultMainGunLvl;
		// 	currentStateWingGun=MenuManager.defaultWingGunLvl;
		// 	currentStateSideGun=MenuManager.defaultSideGunLvl;
		// 	currentStateMagnet=MenuManager.defaultMagnetLvl;
		// 	currentStateShield=MenuManager.defaultShieldLvl;
		// 	currentStateDoubleStars=MenuManager.defaultDoubleStarsLvl;
		// 	currentStateBomb=MenuManager.defaultBombLvl;
		// 	currentStateTesla=MenuManager.defaultTeslaLvl;
		// 	currentStateLaser=MenuManager.defaultLaserLvl;
		// 	currentStateBlades=MenuManager.defaultBladesLvl;
		//
		// 	highScore = 0;
		// 	laserNumber = MenuManager.defaultLaserWeaponNumber;
		// 	teslaNumber = MenuManager.defaultTeslaWeaponNumber;
		// 	bladesNumber = MenuManager.defaultBladesWeaponNumber;
		// 	bombNumber = MenuManager.defaultBombWeaponNumber;
		// 	SavePlaneState();
		// }



		MainGun = GameObject.Find("MainGun").GetComponent<Image>();
		SideGun = GameObject.Find("SideGun").GetComponent<Image>();
		WingGun = GameObject.Find("WingGun").GetComponent<Image>();

		if(currentStateArmor>=8)
		{

			GameObject.Find("PandaPlaneArmorLvl1").SetActive(false);
			GameObject.Find("PandaPlane").transform.GetChild(2).transform.gameObject.SetActive(true);
		}
		else if(currentStateArmor>=4)
		{

			GameObject.Find("PandaPlaneArmorLvl1").SetActive(false);
			GameObject.Find("PandaPlane").transform.GetChild(1).transform.gameObject.SetActive(true);
		}


		if(currentStateMainGun>=8)
		{
			MainGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
		}
		else if(currentStateMainGun>=4)
		{
			MainGun.sprite = GameObject.Find("GunLvl2Reference").GetComponent<Image>().sprite;
		}

		if(currentStateSideGun>=8)
		{
			SideGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
		}
		else if(currentStateSideGun>=4)
		{
			SideGun.sprite = GameObject.Find("GunLvl2Reference").GetComponent<Image>().sprite;
		}
		else if(currentStateSideGun>=1)
		{
			SideGun.sprite = GameObject.Find("GunLvl1Reference").GetComponent<Image>().sprite;
		}
		else
		{
			SideGun.enabled=false;
		}

		if(currentStateWingGun>=8)
		{
			WingGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
		}
		else if(currentStateWingGun>=4)
		{
			WingGun.sprite = GameObject.Find("GunLvl2Reference").GetComponent<Image>().sprite;
		}
		else if(currentStateWingGun>=1)
		{
			WingGun.sprite = GameObject.Find("GunLvl1Reference").GetComponent<Image>().sprite;
		}
		else
		{
			WingGun.enabled = false;
		}


		// if(PlayerPrefs.HasKey("UpgradesStatesInfo"))
		// {
		// 	string upgradesSI = PlayerPrefs.GetString("UpgradesStatesInfo");
		// 	upgradesStatesInfo = upgradesSI.Split('#');
		// }
		// else
		// {
		// 	string upgradesSI = "";
		// 	for(int i=0;i<12;i++)
		// 	{
		// 		upgradesStatesInfo[i] = "0";
		// 		upgradesSI += upgradesStatesInfo[i] + "#";
		// 		PlayerPrefs.SetString("UpgradesStatesInfo",upgradesSI);
		// 	}
		// }

		for(int i=1;i<12;i++)
		{
			if(upgradesStatesInfo[i].Equals("1"))
			{
				GameObject.Find("ScollingHolder").transform.GetChild(i).GetChild(1).GetChild(1).gameObject.SetActive(false);
			}
		}

		ExplanationImage = GameObject.Find("ExplanationImage").GetComponent<Image>();
		UpgradeInfoPriceText = GameObject.Find("UpgradePriceText").GetComponent<Text>();
		UpgradeTitleText = GameObject.Find("UpgradeTitleText").GetComponent<Text>();
		UpgradeDescriptionText = GameObject.Find("UpgradeDescriptionText").GetComponent<Text>();
		starsNumberTextShop = GameObject.Find("StarsNumberShopText").GetComponent<Text>();

		healthPriceText = GameObject.Find("UpgradeHealthHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		armorPriceText = GameObject.Find("UpgradeArmorHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		mainGunPriceText = GameObject.Find("UpgradeMainGunHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		wingGunPriceText = GameObject.Find("UpgradeWingGunHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		sideGunPriceText = GameObject.Find("UpgradeSideGunHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		magnetPriceText = GameObject.Find("UpgradeMagnetHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		shieldPriceText = GameObject.Find("UpgradeShieldHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		doubleStarsPriceText = GameObject.Find("UpgradeDoubleStarsHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		bombPriceText = GameObject.Find("UpgradeBombHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		teslaPriceText = GameObject.Find("UpgradeTeslaHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		laserPriceText = GameObject.Find("UpgradeLaserHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();
		bladesPriceText = GameObject.Find("UpgradeBladesHolder/HeaderAndScaleHolder/Price").GetComponent<Text>();

		starsNumberTextShop.text = stars.ToString();


		SetColorStateShop();

		if(currentStateHealth==10)
		{
			healthPriceText.text = "FULL";
			GameObject.Find("UpgradeHealthButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			healthPriceText.text = healthPrice[currentStateHealth].ToString();
		}

		if(currentStateArmor==10)
		{
			armorPriceText.text = "FULL";
			GameObject.Find("UpgradeArmorButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			armorPriceText.text = armorPrice[currentStateArmor].ToString();
		}

		if(currentStateMainGun==10)
		{
			mainGunPriceText.text = "FULL";
			GameObject.Find("UpgradeMainGunButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			mainGunPriceText.text = mainGunPrice[currentStateMainGun].ToString();
		}

		if(currentStateWingGun==10)
		{
			wingGunPriceText.text = "FULL";
			GameObject.Find("UpgradeWingGunButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			wingGunPriceText.text = wingGunPrice[currentStateWingGun].ToString();
		}

		if(currentStateSideGun==10)
		{
			sideGunPriceText.text = "FULL";
			GameObject.Find("UpgradeSideGunButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			sideGunPriceText.text = sideGunPrice[currentStateSideGun].ToString();
		}

		if(currentStateMagnet==10)
		{
			magnetPriceText.text = "FULL";
			GameObject.Find("UpgradeMagnetButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			magnetPriceText.text = magnetPrice[currentStateMagnet].ToString();
		}

		if(currentStateShield==10)
		{
			shieldPriceText.text = "FULL";
			GameObject.Find("UpgradeShieldButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			shieldPriceText.text = shieldPrice[currentStateShield].ToString();
		}

		if(currentStateDoubleStars==10)
		{
			doubleStarsPriceText.text = "FULL";
			GameObject.Find("UpgradeDoubleStarsButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			doubleStarsPriceText.text = doubleStarsPrice[currentStateDoubleStars].ToString();
		}

		if(currentStateBomb==10)
		{
			bombPriceText.text = "FULL";
			GameObject.Find("UpgradeBombButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			bombPriceText.text = bombPrice[currentStateBomb].ToString();
		}

		if(currentStateTesla==10)
		{
			teslaPriceText.text = "FULL";
			GameObject.Find("UpgradeTeslaButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			teslaPriceText.text = teslaPrice[currentStateTesla].ToString();
		}

		if(currentStateLaser==10)
		{
			laserPriceText.text = "FULL";
			GameObject.Find("UpgradeLaserButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			laserPriceText.text = laserPrice[currentStateLaser].ToString();
		}

		if(currentStateBlades==10)
		{
			bladesPriceText.text = "FULL";
			GameObject.Find("UpgradeBladesButton").GetComponent<Button>().interactable = false;
		}
		else
		{
			bladesPriceText.text = bladesPrice[currentStateBlades].ToString();
		}


		for(int i=0; i<currentStateHealth;i++)
		{
			GameObject.Find("UpgradeHealthHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		for(int i=0; i<currentStateArmor;i++)
		{
			GameObject.Find("UpgradeArmorHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		for(int i=0; i<currentStateMainGun;i++)
		{
			GameObject.Find("UpgradeMainGunHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}
		
		for(int i=0; i<currentStateWingGun;i++)
		{
			GameObject.Find("UpgradeWingGunHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		for(int i=0; i<currentStateSideGun;i++)
		{
			GameObject.Find("UpgradeSideGunHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}
		
		for(int i=0; i<currentStateMagnet;i++)
		{
			GameObject.Find("UpgradeMagnetHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		for(int i=0; i<currentStateShield;i++)
		{
			GameObject.Find("UpgradeShieldHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}
		
		for(int i=0; i<currentStateDoubleStars;i++)
		{
			GameObject.Find("UpgradeDoubleStarsHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		for(int i=0; i<currentStateBomb;i++)
		{
			GameObject.Find("UpgradeBombHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}
		
		for(int i=0; i<currentStateTesla;i++)
		{
			GameObject.Find("UpgradeTeslaHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		for(int i=0; i<currentStateLaser;i++)
		{
			GameObject.Find("UpgradeLaserHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}
		
		for(int i=0; i<currentStateBlades;i++)
		{
			GameObject.Find("UpgradeBladesHolder/HeaderAndScaleHolder/State").transform.GetChild(i).gameObject.SetActive(true);
		}

		ShopNotificationClass.numberNotification = ShopNotification();
	}

	void AnaliticForHangarUpgrades(int levelOfUpgrade, string nameOfUpgrade)
	{
		
			// switch(levelOfUpgrade)
			// {
			// case 1:
			//
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel1"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel1",levelOfUpgrade);
			//
			// 	}
			//
			// 	break;
			// case 2:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel2"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel2",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 3:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel3"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel3",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 4:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel4"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel4",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 5:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel5"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel5",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 6:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel6"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel6",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 7:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel7"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel7",levelOfUpgrade);
			// 	
			// 	}
			// 	break;
			// case 8:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel8"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel8",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 9:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel9"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel9",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// case 10:
			// 	if(!PlayerPrefs.HasKey("UpgradeLevel10"))
			// 	{
			// 		PlayerPrefs.SetInt("UpgradeLevel10",levelOfUpgrade);
			//
			// 	}
			// 	break;
			// }
	}
	
	public void UpgradeHealth()
	{
		if(currentStateHealth<10 && stars>=healthPrice[currentStateHealth])
		{
			SoundManager.Instance.Play_UpgradePlane();

			stars-=healthPrice[currentStateHealth];
			starsNumberTextShop.text = stars.ToString();

			currentStateHealth++;
			AnaliticForHangarUpgrades(currentStateHealth,"Health");
			if(currentStateHealth==10)
			{
				healthPriceText.text ="FULL";
				GameObject.Find("UpgradeHealthButton").GetComponent<Button>().interactable = false;
			}
			else
			{
				healthPriceText.text =healthPrice[currentStateHealth].ToString();
			}

			GameObject.Find("UpgradeHealthHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateHealth-1).gameObject.SetActive(true);

			SetColorStateShop();

			//PlayerPrefs.SetInt("HealthLvl", currentStateHealth);
			//PlayerPrefs.SetInt("Stars",stars);
			SavePlaneState();

		}
		else
		{
			GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
			SoundManager.Instance.Play_NotEnoughStars();
		}

	}

	public void UpgradeArmor()
	{
		if(currentStateArmor<10 && stars>=armorPrice[currentStateArmor])
		{
			if(upgradesStatesInfo[1].Equals("0"))
			{
				SetClickedUpgrade(1);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=armorPrice[currentStateArmor];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateArmor++;
				AnaliticForHangarUpgrades(currentStateArmor,"Armor");
				if(currentStateArmor==10)
				{
					armorPriceText.text ="FULL";
					GameObject.Find("UpgradeArmorButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					armorPriceText.text =armorPrice[currentStateArmor].ToString();
				}

				GameObject.Find("UpgradeArmorHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateArmor-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("ArmorLvl", currentStateArmor);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

				if(currentStateArmor==4)
				{
					GameObject.Find("ArmorParticles").GetComponent<ParticleSystem>().Play();
					GameObject.Find("PandaPlaneArmorLvl1").SetActive(false);
					GameObject.Find("PandaPlane").transform.GetChild(1).transform.gameObject.SetActive(true);
				}
				else if(currentStateArmor==8)
				{
					GameObject.Find("ArmorParticles").GetComponent<ParticleSystem>().Play();
					GameObject.Find("PandaPlaneArmorLvl2").SetActive(false);
					GameObject.Find("PandaPlane").transform.GetChild(2).transform.gameObject.SetActive(true);
				}
			}
		}
		else if(stars<armorPrice[currentStateArmor])
		{
			if(upgradesStatesInfo[1].Equals("0"))
			{
				SetClickedUpgrade(1);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}

	}

	public void UpgradeMainGun()
	{
		if(currentStateMainGun<10 && stars>=mainGunPrice[currentStateMainGun])
		{
			if(upgradesStatesInfo[2].Equals("0"))
			{
				SetClickedUpgrade(2);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=mainGunPrice[currentStateMainGun];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateMainGun++;
				AnaliticForHangarUpgrades(currentStateMainGun,"MainGun");
				if(currentStateMainGun==10)
				{
					mainGunPriceText.text ="FULL";
					GameObject.Find("UpgradeMainGunButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					mainGunPriceText.text =mainGunPrice[currentStateMainGun].ToString();
				}

				GameObject.Find("UpgradeMainGunHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateMainGun-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("MainGunLvl", currentStateMainGun);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

				if(currentStateMainGun==1)
				{
					GameObject.Find("MainGunParticles").GetComponent<ParticleSystem>().Play();
					MainGun.sprite = GameObject.Find("GunLvl2Reference").GetComponent<Image>().sprite;
				}
				if(currentStateMainGun==4)
				{
					GameObject.Find("MainGunParticles").GetComponent<ParticleSystem>().Play();
					MainGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
				}
				else if(currentStateMainGun==8)
				{
					GameObject.Find("MainGunParticles").GetComponent<ParticleSystem>().Play();
					MainGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
					MainGun.gameObject.transform.localScale = Vector3.one*1.3f;
				}
				

			}


		}
		else if(stars<mainGunPrice[currentStateMainGun])
		{
			if(upgradesStatesInfo[2].Equals("0"))
			{
				SetClickedUpgrade(2);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}

	}

	public void UpgradeWingGun()
	{
		if(currentStateWingGun<10 && stars>=wingGunPrice[currentStateWingGun])
		{
			if(upgradesStatesInfo[3].Equals("0"))
			{
				SetClickedUpgrade(3);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=wingGunPrice[currentStateWingGun];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateWingGun++;
				AnaliticForHangarUpgrades(currentStateWingGun,"WingGun");
				if(currentStateWingGun==10)
				{
					wingGunPriceText.text ="FULL";
					GameObject.Find("UpgradeWingGunButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					wingGunPriceText.text =wingGunPrice[currentStateWingGun].ToString();
				}

				GameObject.Find("UpgradeWingGunHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateWingGun-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("WingGunLvl", currentStateWingGun);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

				if(currentStateWingGun==1)
				{
					GameObject.Find("WingGunParticles").GetComponent<ParticleSystem>().Play();
					WingGun.enabled=true;
					WingGun.sprite = GameObject.Find("GunLvl1Reference").GetComponent<Image>().sprite;
				}
				if(currentStateWingGun==4)
				{
					GameObject.Find("WingGunParticles").GetComponent<ParticleSystem>().Play();
					WingGun.sprite = GameObject.Find("GunLvl2Reference").GetComponent<Image>().sprite;
				}
				else if(currentStateWingGun==8)
				{
					GameObject.Find("WingGunParticles").GetComponent<ParticleSystem>().Play();
					WingGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
				}
			}
		}
		else if(stars<wingGunPrice[currentStateWingGun])
		{
			if(upgradesStatesInfo[3].Equals("0"))
			{
				SetClickedUpgrade(3);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeSideGun()
	{
		if(currentStateSideGun<10 && stars>=sideGunPrice[currentStateSideGun])
		{
			if(upgradesStatesInfo[4].Equals("0"))
			{
				SetClickedUpgrade(4);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=sideGunPrice[currentStateSideGun];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateSideGun++;
				AnaliticForHangarUpgrades(currentStateSideGun,"SideGun");
				if(currentStateSideGun==10)
				{
					sideGunPriceText.text ="FULL";
					GameObject.Find("UpgradeSideGunButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					sideGunPriceText.text =sideGunPrice[currentStateSideGun].ToString();
				}

				GameObject.Find("UpgradeSideGunHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateSideGun-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("SideGunLvl", currentStateSideGun);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

				if(currentStateSideGun==1)
				{
					GameObject.Find("SideGunParticles").GetComponent<ParticleSystem>().Play();
					SideGun.enabled = true;
					SideGun.sprite = GameObject.Find("GunLvl3Reference").GetComponent<Image>().sprite;
				}
			}
			
		}
		else if(stars<sideGunPrice[currentStateSideGun])
		{
			if(upgradesStatesInfo[4].Equals("0"))
			{
				SetClickedUpgrade(4);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeMagnet()
	{
		if(currentStateMagnet<10 && stars>=magnetPrice[currentStateMagnet])
		{
			if(upgradesStatesInfo[5].Equals("0"))
			{
				SetClickedUpgrade(5);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=magnetPrice[currentStateMagnet];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateMagnet++;
				AnaliticForHangarUpgrades(currentStateMagnet,"Magnet");
				if(currentStateMagnet==10)
				{
					magnetPriceText.text ="FULL";
					GameObject.Find("UpgradeMagnetButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					magnetPriceText.text =magnetPrice[currentStateMagnet].ToString();
				}
				GameObject.Find("UpgradeMagnetHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateMagnet-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("MagnetLvl", currentStateMagnet);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<magnetPrice[currentStateMagnet])
		{
			if(upgradesStatesInfo[5].Equals("0"))
			{
				SetClickedUpgrade(5);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeShield()
	{
		if(currentStateShield<10 && stars>=shieldPrice[currentStateShield])
		{
			if(upgradesStatesInfo[6].Equals("0"))
			{
				SetClickedUpgrade(6);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=shieldPrice[currentStateShield];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateShield++;
				AnaliticForHangarUpgrades(currentStateShield,"Shield");
				if(currentStateShield==10)
				{
					shieldPriceText.text ="FULL";
					GameObject.Find("UpgradeShieldButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					shieldPriceText.text =shieldPrice[currentStateShield].ToString();
				}
				GameObject.Find("UpgradeShieldHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateShield-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("ShieldLvl", currentStateShield);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<shieldPrice[currentStateShield])
		{
			if(upgradesStatesInfo[6].Equals("0"))
			{
				SetClickedUpgrade(6);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeDoubleStars()
	{
		if(currentStateDoubleStars<10 && stars>=doubleStarsPrice[currentStateDoubleStars])
		{
			if(upgradesStatesInfo[7].Equals("0"))
			{
				SetClickedUpgrade(7);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=doubleStarsPrice[currentStateDoubleStars];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateDoubleStars++;
				AnaliticForHangarUpgrades(currentStateDoubleStars,"DoubleStars");
				if(currentStateDoubleStars==10)
				{
					doubleStarsPriceText.text ="FULL";
					GameObject.Find("UpgradeDoubleStarsButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					doubleStarsPriceText.text =doubleStarsPrice[currentStateDoubleStars].ToString();
				}
				GameObject.Find("UpgradeDoubleStarsHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateDoubleStars-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("DoubleStarsLvl", currentStateDoubleStars);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<doubleStarsPrice[currentStateDoubleStars])
		{
			if(upgradesStatesInfo[7].Equals("0"))
			{
				SetClickedUpgrade(7);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeBomb()
	{
		if(currentStateBomb<10 && stars>=bombPrice[currentStateBomb])
		{
			if(upgradesStatesInfo[8].Equals("0"))
			{
				SetClickedUpgrade(8);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=bombPrice[currentStateBomb];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateBomb++;
				AnaliticForHangarUpgrades(currentStateBomb,"Bomb");
				if(currentStateBomb==10)
				{
					bombPriceText.text ="FULL";
					GameObject.Find("UpgradeBombButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					bombPriceText.text =bombPrice[currentStateBomb].ToString();
				}
				GameObject.Find("UpgradeBombHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateBomb-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("BombLvl", currentStateBomb);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<bombPrice[currentStateBomb])
		{
			if(upgradesStatesInfo[8].Equals("0"))
			{
				SetClickedUpgrade(8);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeTesla()
	{
		if(currentStateTesla<10 && stars>=teslaPrice[currentStateTesla])
		{
			if(upgradesStatesInfo[9].Equals("0"))
			{
				SetClickedUpgrade(9);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=teslaPrice[currentStateTesla];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateTesla++;
				AnaliticForHangarUpgrades(currentStateTesla,"Tesla");
				if(currentStateTesla==10)
				{
					teslaPriceText.text ="FULL";
					GameObject.Find("UpgradeTeslaButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					teslaPriceText.text =teslaPrice[currentStateTesla].ToString();
				}
				GameObject.Find("UpgradeTeslaHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateTesla-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("TeslaLvl", currentStateTesla);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<teslaPrice[currentStateTesla])
		{
			if(upgradesStatesInfo[9].Equals("0"))
			{
				SetClickedUpgrade(9);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeLaser()
	{
		if(currentStateLaser<10 && stars>=laserPrice[currentStateLaser])
		{
			if(upgradesStatesInfo[10].Equals("0"))
			{
				SetClickedUpgrade(10);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=laserPrice[currentStateLaser];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateLaser++;
				AnaliticForHangarUpgrades(currentStateLaser,"Laser");
				if(currentStateLaser==10)
				{
					laserPriceText.text ="FULL";
					GameObject.Find("UpgradeLaserButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					laserPriceText.text =laserPrice[currentStateLaser].ToString();
				}
				GameObject.Find("UpgradeLaserHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateLaser-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("LaserLvl", currentStateLaser);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<laserPrice[currentStateLaser])
		{
			if(upgradesStatesInfo[10].Equals("0"))
			{
				SetClickedUpgrade(10);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void UpgradeBlades()
	{
		if(currentStateBlades<10 && stars>=bladesPrice[currentStateBlades])
		{
			if(upgradesStatesInfo[11].Equals("0"))
			{
				SetClickedUpgrade(11);
				ShowExplanationPopUp();
			}
			else
			{
				SoundManager.Instance.Play_UpgradePlane();
				stars-=bladesPrice[currentStateBlades];
				starsNumberTextShop.text = stars.ToString();
				
				currentStateBlades++;
				AnaliticForHangarUpgrades(currentStateBlades,"Blades");
				if(currentStateBlades==10)
				{
					bladesPriceText.text ="FULL";
					GameObject.Find("UpgradeBladesButton").GetComponent<Button>().interactable = false;
				}
				else
				{
					bladesPriceText.text =bladesPrice[currentStateBlades].ToString();
				}
				GameObject.Find("UpgradeBladesHolder/HeaderAndScaleHolder/State").transform.GetChild(currentStateBlades-1).gameObject.SetActive(true);
				
				SetColorStateShop();
				
				//PlayerPrefs.SetInt("BladesLvl", currentStateBlades);
				//PlayerPrefs.SetInt("Stars",stars);
				SavePlaneState();

			}
			
		}
		else if(stars<bladesPrice[currentStateBlades])
		{
			if(upgradesStatesInfo[11].Equals("0"))
			{
				SetClickedUpgrade(11);
				DeactivateUpgradeButton();
				ShowExplanationPopUp();
			}
			else
			{
				GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
				SoundManager.Instance.Play_NotEnoughStars();
			}
		}
	}

	public void SavePlaneState()
	{
		// string sarma = (currentStateHealth+7) + "#" + (currentStateArmor+7) + "#" + (currentStateMainGun+14) + "#" + (currentStateWingGun+21) + "#" + (currentStateSideGun+35) + "#" + (currentStateMagnet+56) + "#" + (currentStateShield+91) + "#" + 
		// 	(currentStateDoubleStars+147) + "#" + (currentStateLaser+238) + "#" + (currentStateTesla+385) + "#" + (currentStateBlades+623) + "#" + (currentStateBomb+1008);
		//
		// PlayerPrefs.SetString("Sarma",sarma);
		//
		// string uzengije = (stars+10) + "#" + (highScore-20) + "#" + (laserNumber+5) + "#" + (teslaNumber+5) + "#" + (bladesNumber+5) + "#" + (bombNumber+5);
		//
		// PlayerPrefs.SetString("Uzengije",uzengije);
		//
		// PlayerPrefs.SetInt("HealthLvl", currentStateHealth);
		// PlayerPrefs.SetInt("ArmorLvl", currentStateArmor);
		// PlayerPrefs.SetInt("MainGunLvl", currentStateMainGun);
		// PlayerPrefs.SetInt("WingGunLvl", currentStateWingGun);
		// PlayerPrefs.SetInt("SideGunLvl", currentStateSideGun);
		// PlayerPrefs.SetInt("MagnetLvl", currentStateMagnet);
		// PlayerPrefs.SetInt("ShieldLvl", currentStateShield);
		// PlayerPrefs.SetInt("DoubleStarsLvl", currentStateDoubleStars);
		// PlayerPrefs.SetInt("BombLvl", currentStateBomb);
		// PlayerPrefs.SetInt("TeslaLvl", currentStateTesla);
		// PlayerPrefs.SetInt("LaserLvl", currentStateLaser);
		// PlayerPrefs.SetInt("BladesLvl", currentStateBlades);
		// PlayerPrefs.SetInt("Stars", stars);
		// PlayerPrefs.SetInt("HighScore", currentStateMagnet*currentStateShield + stars*5);
		// PlayerPrefs.Save();
		
	}

	public void SetColorStateShop()
	{
		if(currentStateHealth<10)
		{
			if(stars<healthPrice[currentStateHealth])
			{
				GameObject.Find("UpgradeHealthHolder/UpgradeHealthButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeHealthHolder/UpgradeHealthButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeHealthHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeHealthHolder/UpgradeHealthButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeHealthHolder/UpgradeHealthButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeHealthHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeHealthHolder/UpgradeHealthButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeHealthHolder/UpgradeHealthButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeHealthHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateArmor<10)
		{
			if(stars<armorPrice[currentStateArmor])
			{
				GameObject.Find("UpgradeArmorHolder/UpgradeArmorButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeArmorHolder/UpgradeArmorButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeArmorHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeArmorHolder/UpgradeArmorButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeArmorHolder/UpgradeArmorButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeArmorHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeArmorHolder/UpgradeArmorButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeArmorHolder/UpgradeArmorButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeArmorHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}


		if(currentStateMainGun<10)
		{
			if(stars<mainGunPrice[currentStateMainGun])
			{
				GameObject.Find("UpgradeMainGunHolder/UpgradeMainGunButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeMainGunHolder/UpgradeMainGunButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeMainGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeMainGunHolder/UpgradeMainGunButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeMainGunHolder/UpgradeMainGunButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeMainGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeMainGunHolder/UpgradeMainGunButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeMainGunHolder/UpgradeMainGunButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeMainGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateSideGun<10)
		{
			if(stars<sideGunPrice[currentStateSideGun])
			{
				GameObject.Find("UpgradeSideGunHolder/UpgradeSideGunButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeSideGunHolder/UpgradeSideGunButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeSideGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeSideGunHolder/UpgradeSideGunButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeSideGunHolder/UpgradeSideGunButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeSideGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeSideGunHolder/UpgradeSideGunButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeSideGunHolder/UpgradeSideGunButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeSideGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateWingGun<10)
		{
			if(stars<wingGunPrice[currentStateWingGun])
			{
				GameObject.Find("UpgradeWingGunHolder/UpgradeWingGunButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeWingGunHolder/UpgradeWingGunButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeWingGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeWingGunHolder/UpgradeWingGunButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeWingGunHolder/UpgradeWingGunButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeWingGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeWingGunHolder/UpgradeWingGunButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeWingGunHolder/UpgradeWingGunButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeWingGunHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateMagnet<10)
		{
			if(stars<magnetPrice[currentStateMagnet])
			{
				GameObject.Find("UpgradeMagnetHolder/UpgradeMagnetButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeMagnetHolder/UpgradeMagnetButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeMagnetHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeMagnetHolder/UpgradeMagnetButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeMagnetHolder/UpgradeMagnetButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeMagnetHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeMagnetHolder/UpgradeMagnetButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeMagnetHolder/UpgradeMagnetButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeMagnetHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateShield<10)
		{
			if(stars<shieldPrice[currentStateShield])
			{
				GameObject.Find("UpgradeShieldHolder/UpgradeShieldButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeShieldHolder/UpgradeShieldButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeShieldHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeShieldHolder/UpgradeShieldButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeShieldHolder/UpgradeShieldButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeShieldHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeShieldHolder/UpgradeShieldButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeShieldHolder/UpgradeShieldButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeShieldHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateDoubleStars<10)
		{
			if(stars<doubleStarsPrice[currentStateDoubleStars])
			{
				GameObject.Find("UpgradeDoubleStarsHolder/UpgradeDoubleStarsButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeDoubleStarsHolder/UpgradeDoubleStarsButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeDoubleStarsHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeDoubleStarsHolder/UpgradeDoubleStarsButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeDoubleStarsHolder/UpgradeDoubleStarsButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeDoubleStarsHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeDoubleStarsHolder/UpgradeDoubleStarsButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeDoubleStarsHolder/UpgradeDoubleStarsButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeDoubleStarsHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateBomb<10)
		{
			if(stars<bombPrice[currentStateBomb])
			{
				GameObject.Find("UpgradeBombHolder/UpgradeBombButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeBombHolder/UpgradeBombButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeBombHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeBombHolder/UpgradeBombButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeBombHolder/UpgradeBombButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeBombHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeBombHolder/UpgradeBombButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeBombHolder/UpgradeBombButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeBombHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateTesla<10)
		{
			if(stars<teslaPrice[currentStateTesla])
			{
				GameObject.Find("UpgradeTeslaHolder/UpgradeTeslaButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeTeslaHolder/UpgradeTeslaButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeTeslaHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeTeslaHolder/UpgradeTeslaButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeTeslaHolder/UpgradeTeslaButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeTeslaHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeTeslaHolder/UpgradeTeslaButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeTeslaHolder/UpgradeTeslaButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeTeslaHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateLaser<10)
		{
			if(stars<laserPrice[currentStateLaser])
			{
				GameObject.Find("UpgradeLaserHolder/UpgradeLaserButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeLaserHolder/UpgradeLaserButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeLaserHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeLaserHolder/UpgradeLaserButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeLaserHolder/UpgradeLaserButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeLaserHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeLaserHolder/UpgradeLaserButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeLaserHolder/UpgradeLaserButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeLaserHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		if(currentStateBlades<10)
		{
			if(stars<bladesPrice[currentStateBlades])
			{
				GameObject.Find("UpgradeBladesHolder/UpgradeBladesButton").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeBladesHolder/UpgradeBladesButton/BuyPlus").GetComponent<Image>().color = Grey;
				GameObject.Find("UpgradeBladesHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
			}
			else
			{
				GameObject.Find("UpgradeBladesHolder/UpgradeBladesButton").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeBladesHolder/UpgradeBladesButton/BuyPlus").GetComponent<Image>().color = Green;
				GameObject.Find("UpgradeBladesHolder/UpgradeNameHolder").GetComponent<Image>().color = Blue;
			}
		}
		else
		{
			GameObject.Find("UpgradeBladesHolder/UpgradeBladesButton").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeBladesHolder/UpgradeBladesButton/BuyPlus").GetComponent<Image>().color = Grey;
			GameObject.Find("UpgradeBladesHolder/UpgradeNameHolder").GetComponent<Image>().color = Color.white;
		}

		ShopNotificationClass.numberNotification = ShopNotification();
	}

	public void ShowExplanationPopUp()
	{
		SoundManager.Instance.Play_ButtonClick();
		ExplanationPopUp.GetComponent<Animation>().GetComponent<Animation>()["ExplanationPopUpArrival"].speed = 0.0f;
		ExplanationPopUp.GetComponent<Animation>().GetComponent<Animation>()["ExplanationPopUpArrival"].speed = 1.0f;
		ExplanationPopUp.GetComponent<Animation>().Play();
		AircraftBattleMenuManager.popupType = 3;
	}

	public void HideExplanationPopUp()
	{
		SoundManager.Instance.Play_ButtonClick();
		ExplanationPopUp.GetComponent<Animation>().GetComponent<Animation>()["ExplanationPopUpArrival"].speed = 0.99999f;
		ExplanationPopUp.GetComponent<Animation>().GetComponent<Animation>()["ExplanationPopUpArrival"].speed = -1.0f;
		ExplanationPopUp.GetComponent<Animation>().Play();
		AircraftBattleMenuManager.popupType = 2;
	}

	public void SetClickedUpgrade(int number)
	{
		clickedUpgrade = number;
//		upgradesStatesInfo[number]="1";
//		string upgradesSI = "";
//		for(int i=0;i<12;i++)
//		{
//			upgradesSI += upgradesStatesInfo[i] + "#";
//			PlayerPrefs.SetString("UpgradesStatesInfo",upgradesSI);
//		}
//		GameObject.Find("ScollingHolder").transform.GetChild(number).GetChild(3).gameObject.SetActive(false);
		switch(number)
		{
		case 1:
			UpgradeTitleText.text = "Armor";
			UpgradeDescriptionText.text = "Plane body protection.\n\nUpgrade to increase endurance (resistance against attack).";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationArmor").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = armorPrice[currentStateArmor].ToString();
			break;
		case 2:
			UpgradeTitleText.text = "Main Gun";
			UpgradeDescriptionText.text = "Main weapon.\n\nUpgrade to increase damage and fire rate.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationMainGun").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = mainGunPrice[currentStateMainGun].ToString();
			break;
		case 3:
			UpgradeTitleText.text = "Wing Gun";
			UpgradeDescriptionText.text = "Two additional front guns.\n\nUpgrade to increase damage and fire rate.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationWingGun").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = wingGunPrice[currentStateWingGun].ToString();
			break;
		case 4:
			UpgradeTitleText.text = "Side Gun";
			UpgradeDescriptionText.text = "Two additional side guns.\n\nUpgrade to increase damage and fire rate.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationSideGun").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = sideGunPrice[currentStateSideGun].ToString();
			break;
		case 5:
			UpgradeTitleText.text = "Magnet";
			UpgradeDescriptionText.text = "<color=#0000ff>Automatic activation when collected</color>\n\nAttracts stars when collected, but has a short duration.\n\nUpgrade to extend duration.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationMagnet").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = magnetPrice[currentStateMagnet].ToString();
			break;
		case 6:
			UpgradeTitleText.text = "Shield";
			UpgradeDescriptionText.text = "<color=#0000ff>Automatic activation when collected</color>\n\nProtects plane from damage, but has a short duration.\n\nUpgrade to extend duration.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationShield").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = shieldPrice[currentStateShield].ToString();
			break;
		case 7:
			UpgradeTitleText.text = "Double Stars";
			UpgradeDescriptionText.text = "<color=#0000ff>Automatic activation when collected</color>\n\nDoubles the number of collected stars, but has a short duration.\n\nUpgrade to extend duration.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationDoubleStars").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = doubleStarsPrice[currentStateDoubleStars].ToString();
			break;
		case 8:
			UpgradeTitleText.text = "Bomb";
			UpgradeDescriptionText.text = "<color=#0000ff>Manual activation</color>\n\nUnlock Bomb super weapon with large destructive power.\n\nUpgrade to increase damage.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationBomb").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = bombPrice[currentStateBomb].ToString();
			break;
		case 9:
			UpgradeTitleText.text = "Tesla";
			UpgradeDescriptionText.text = "<color=#0000ff>Manual activation</color>\n\nUnlock Tesla super weapon which electrifies everything in its range.\n\nUpgrade to increase damage.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationTesla").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = teslaPrice[currentStateTesla].ToString();
			break;
		case 10:
			UpgradeTitleText.text = "Laser";
			UpgradeDescriptionText.text = "<color=#0000ff>Manual activation</color>\n\nUnlock Laser super weapon which cuts through all enemies.\n\nUpgrade to increase damage.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationLaser").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = laserPrice[currentStateLaser].ToString();
			break;
		case 11:
			UpgradeTitleText.text = "Blades";
			UpgradeDescriptionText.text = "<color=#0000ff>Manual activation</color>\n\nUnlock Blades super weapon which circles around plane and provides additional damage.\n\nUpgrade to increase damage.";
			ExplanationImage.sprite = GameObject.Find("ReferenceImageExplanationBlades").GetComponent<Image>().sprite;
			UpgradeInfoPriceText.text = bladesPrice[currentStateBlades].ToString();
			break;
		}
	}

	public void BuyFromInfoScreen()
	{
		if(int.Parse(UpgradeInfoPriceText.text)>stars)
		{
			GameObject.Find("HeaderShop/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
			SoundManager.Instance.Play_NotEnoughStars();
		}
		else
		{
			upgradesStatesInfo[clickedUpgrade]="1";
			string upgradesSI = "";
			// for(int i=0;i<12;i++)
			// {
			// 	upgradesSI += upgradesStatesInfo[i] + "#";
			// 	PlayerPrefs.SetString("UpgradesStatesInfo",upgradesSI);
			// }
			GameObject.Find("ScollingHolder").transform.GetChild(clickedUpgrade).GetChild(1).GetChild(1).gameObject.SetActive(false);
			switch(clickedUpgrade)
			{
			case 1:
				UpgradeArmor();
				break;
			case 2:
				UpgradeMainGun();
				break;
			case 3:
				UpgradeWingGun();
				break;
			case 4:
				UpgradeSideGun();
				break;
			case 5:
				UpgradeMagnet();
				break;
			case 6:
				UpgradeShield();
				break;
			case 7:
				UpgradeDoubleStars();
				break;
			case 8:
				UpgradeBomb();
				break;
			case 9:
				UpgradeTesla();
				break;
			case 10:
				UpgradeLaser();
				break;
			case 11:
				UpgradeBlades();
				break;
			}
			
			HideExplanationPopUp();
		}

	}

	public int ShopNotification()
	{
		availablePurchases = 0;

		if(currentStateHealth<10)
		{
			if(healthPrice[currentStateHealth]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateArmor<10)
		{
			if(armorPrice[currentStateArmor]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateMainGun<10)
		{
			if(mainGunPrice[currentStateMainGun]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateSideGun<10)
		{
			if(sideGunPrice[currentStateSideGun]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateWingGun<10)
		{
			if(wingGunPrice[currentStateWingGun]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateMagnet<10)
		{
			if(magnetPrice[currentStateMagnet]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateShield<10)
		{
			if(shieldPrice[currentStateShield]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateDoubleStars<10)
		{
			if(doubleStarsPrice[currentStateDoubleStars]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateBomb<10)
		{
			if(bombPrice[currentStateBomb]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateTesla<10)
		{
			if(teslaPrice[currentStateTesla]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateLaser<10)
		{
			if(laserPrice[currentStateLaser]<=stars)
			{
				availablePurchases++;
			}
		}

		if(currentStateBlades<10)
		{
			if(bladesPrice[currentStateBlades]<=stars)
			{
				availablePurchases++;
			}
		}

		return availablePurchases;
//		GameObject.Find("AvailablePurchases").GetComponent<Text>().text = availablePurchases.ToString();
	}

	public void DeactivateUpgradeButton()
	{

	}

}
