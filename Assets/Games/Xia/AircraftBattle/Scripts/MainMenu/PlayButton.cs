using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class PlayButton : MonoBehaviour {

	int numberOfPlays;
	int currentMaxStage;
	

	public GridLayoutGroup gridLayout;
	
	public RectTransform scrollContent;
	
	public ScrollRect scrollRect;

//	int currentNumberOfCheckpoints;
	int checkpointStartPrice = 200;
//	int[] checkpointPrice = new int[] {0, 0, 0, 0, 0, 0};
//	int[] checkpointPrice = new int[] {0, 800, 1600, 2400, 3200, 4000}; // inicijalna verzija je isla ovako, sad je 200 po stage-u
//	int[] checkpointPrice = new int[] {0, 500, 1000, 1500, 2000, 2500};
//	int[] checkpointPrice = new int[] {0, 600, 1200, 1800, 2400, 3000};
	Text moneyText;
	public static List<int> levels;
	static int minimalLevel = 2;

	void DirectStartGame()
	{
		StartCoroutine("LoadScene");
	}
	void Start()
	{
		Invoke("DirectStartGame",0.1f);
		
		// if(PlayerPrefs.HasKey("MaxStage"))
		// {
		// 	int cms = PlayerPrefs.GetInt("MaxStage");
		// 	int ghe = PlayerPrefs.GetInt("ghE67+=as23")-5;
		// 	int fsd = PlayerPrefs.GetInt("Fsdfs+=as23")-11;
		// 	
		// 	if(cms==ghe)
		// 	{
		// 		if(cms==fsd)
		// 		{
		// 			currentMaxStage = fsd;
		// 		}
		// 		else
		// 		{
		// 			currentMaxStage = 1;
		// 		}
		// 		
		// 	}
		// 	else
		// 	{
		// 		currentMaxStage = 1;
		// 	}
		// }
		// else
		// {
		// 	currentMaxStage=1;
		// 	PlayerPrefs.SetInt("MaxStage",currentMaxStage);
		// 	PlayerPrefs.SetInt("ghE67+=as23",currentMaxStage+5);
		// 	PlayerPrefs.SetInt("Fsdfs+=as23",currentMaxStage+11);
		// 	PlayerPrefs.Save();
		// }
		moneyText = GameObject.Find("StarsNumberCheckpointText").GetComponent<Text>();
		moneyText.text = Shop.stars.ToString();
//		for(int i=0;i<currentMaxStage;i++)
//		{
//			GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(1)
//		}
//		GameObject.Find("Checkpoints/Checkpoint4/Stage4StarsCostText").GetComponent<Text>().text = checkpointPrice[1].ToString();
//		GameObject.Find("Checkpoints/Checkpoint7/Stage7StarsCostText").GetComponent<Text>().text = checkpointPrice[2].ToString();
//		GameObject.Find("Checkpoints/Checkpoint10/Stage10StarsCostText").GetComponent<Text>().text = checkpointPrice[3].ToString();
//		GameObject.Find("Checkpoints/Checkpoint13/Stage13StarsCostText").GetComponent<Text>().text = checkpointPrice[4].ToString();
//		GameObject.Find("Checkpoints/Checkpoint16/Stage16StarsCostText").GetComponent<Text>().text = checkpointPrice[5].ToString();
		for(int i=1;i<6;i++)
		{
			GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
		}
		int numberOfAllCheckpoints;
//		if(currentMaxStage>=15)
//		{
//			currentNumberOfCheckpoints = 6;
//		}
//		else if(currentMaxStage>=12)
//		{
//			currentNumberOfCheckpoints = 5;
//		}
//		else if(currentMaxStage>=9)
//		{
//			currentNumberOfCheckpoints = 4;
//			//GameObject.Find("CheckpointsHolder").GetComponent<ScrollRect>().enabled = false;
//		}
//		else if(currentMaxStage>=6)
//		{
//			currentNumberOfCheckpoints = 3;
//			//GameObject.Find("CheckpointsHolder").GetComponent<ScrollRect>().enabled = false;
//		}
//		else if(currentMaxStage>=3)
//		{
//			currentNumberOfCheckpoints = 2;
//			//GameObject.Find("CheckpointsHolder").GetComponent<ScrollRect>().enabled = false;
//		}
//		else
//		{
//			currentNumberOfCheckpoints = 1;
//		}
		Debug.Log("currentMaxStage "+currentMaxStage);


		if(currentMaxStage>25)
		{
			Debug.Log("currentMaxStage ima vise od 25");
			for(int i=1;i<25;i++)
			{
				GameObject.Find("Checkpoints").transform.GetChild(i).gameObject.GetComponent<Button>().interactable=true;
				GameObject.Find("Checkpoints").transform.GetChild(i).gameObject.GetComponent<Image>().color=Color.white;
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(0).GetComponent<Text>().color=Color.white;
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(1).GetComponent<Text>().text = (checkpointStartPrice*i).ToString();
			}

			if(currentMaxStage==26)
			{
				GameObject checkpointX = (GameObject) Instantiate(Resources.Load("CheckpointX"));
				checkpointX.name = "Checkpoint26";
				checkpointX.transform.SetParent(GameObject.Find("Checkpoints").transform);
				checkpointX.transform.localPosition = new Vector3(284, -2550,0);
				checkpointX.transform.GetChild(0).GetComponent<Text>().text = "STAGE 26";
				checkpointX.transform.GetChild(1).GetComponent<Text>().text = "5000";
				checkpointX.GetComponent<StartSceneFromCheckpoint>().startingStage = 26;

			}
			else
			{
				for(int i=26;i<currentMaxStage+1;i++)
				{
					Debug.Log("Ulazi za "+i);
					GameObject checkpointX = (GameObject) Instantiate(Resources.Load("CheckpointX"));
					checkpointX.name = "Checkpoint"+i;
					checkpointX.transform.SetParent(GameObject.Find("Checkpoints").transform);
					checkpointX.transform.localPosition = new Vector3(284, -i*100-50,0);
					checkpointX.transform.GetChild(0).GetComponent<Text>().text = "STAGE "+i;
					checkpointX.transform.GetChild(1).GetComponent<Text>().text = ((i-1)*200).ToString();
					checkpointX.GetComponent<StartSceneFromCheckpoint>().startingStage = i;
				}
			}
			SetContentHeight();
			StartCoroutine(MoveTowardsTarget(0.2f, scrollRect.verticalNormalizedPosition, 1000));
		}
		else
		{
			Debug.Log("currentMaxStage ima do 25");
			for(int i=1;i<currentMaxStage;i++)
			{
				GameObject.Find("Checkpoints").transform.GetChild(i).gameObject.GetComponent<Button>().interactable=true;
				GameObject.Find("Checkpoints").transform.GetChild(i).gameObject.GetComponent<Image>().color=Color.white;
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(0).GetComponent<Text>().color=Color.white;
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
				GameObject.Find("Checkpoints").transform.GetChild(i).GetChild(1).GetComponent<Text>().text = (checkpointStartPrice*i).ToString();
			}
		}

		if(PlayerPrefs.HasKey("NumOfPlays"))
		{
			numberOfPlays = PlayerPrefs.GetInt("NumOfPlays");
		}
		else
		{
			numberOfPlays=0;
		}

//		if(!WebelinxCMS.startInterstitialShown)
//			WebelinxCMS.Instance.ShowStartInterstitial(6);
	}

	public void StartLoadingScreenAnimation()
	{
		//UPDATE!!!!!
		//GameObject.Find("CheckpointsScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().CancelLoading();
		//GameObject.Find("CheckpointsScreen/NativeAdHolder/AnimationHolder").GetComponent<Animator>().Play("NativeAdsDeparting");

		DisableCheckpointButtons();
		StartCoroutine("LoadScene");
		GameObject.Find("LoadingHolder/AnimationHolder").GetComponent<Animation>().Play("LoadingArrival2New");
	}
	
	
	IEnumerator LoadScene()
	{
		string timeQuitString = DateTime.Now.ToString();
		numberOfPlays++; 
		SoundManager.Instance.Play_DoorClosing();
		LevelGenerator.checkpoint = true;
		PopUpHandler.gameStarted = false;
		PandaPlane.Instance.score = 0;
		MoveBg.hasBridge = false;
		LevelGenerator.currentStage = 1;
		LevelGenerator.terrainsPassed = 0;
		LevelGenerator.currentBossPlaneLevel = 1;
		LevelGenerator.currentBossShipLevel = 1;
		LevelGenerator.currentBossTankLevel = 1;
		//#if UNITY_ANDROID
		//#endif
		//LevelGenerator.currentBossShipLevel = 2;
		PandaPlane.Instance.numberOfKills = 0;
		SoundManager.Instance.Stop_MenuMusic();
		yield return new WaitForSeconds(2.0f);
		Application.LoadLevel(1);
	}
	
	public void PlayButtonPressed()
	{
		//#if UNITY_ANDROID

		//#endif
		
//		if(currentMaxStage<3)
//		{
//			StartLoadingScreenAnimation();
//		}
//		else
//		{	
			SoundManager.Instance.Play_ButtonClick();
			GameObject.Find("CheckpointHolder/AnimationHolder/StarsCheckpoint/AnimationHolder/StarsNumber/StarsNumberCheckpointText").GetComponent<Text>().text = Shop.stars.ToString();
			GameObject.Find("CheckpointHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
//		}
		//UPDATE!!!!!
		//GameObject.Find("MainScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().CancelLoading();
		//GameObject.Find("MainScreen/NativeAdHolder/AnimationHolder").GetComponent<Animator>().Play("NativeAdsDeparting");
		//GameObject.Find("CheckpointsScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().LoadAdWithDelay(0.5f);
	}
	
	public void ContinueFromStage(int startingStage)
	{
		int startPrice = 0;
		SoundManager.Instance.Play_ButtonClick();
//		switch(startingStage)
//		{
//		case 4:
//			startPrice = checkpointPrice[1];
//			break;
//		case 7:
//			startPrice = checkpointPrice[2];
//			break;
//		case 10:
//			startPrice = checkpointPrice[3];
//			break;
//		case 13:
//			startPrice = checkpointPrice[4];
//			break;
//		case 16:
//			startPrice = checkpointPrice[5];
//			break;
//			
//		}
		startPrice = (startingStage-1)*200;
		
		if(Shop.stars<startPrice)
		{
			GameObject.Find("StarsCheckpoint/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
		}
		else
		{
			numberOfPlays++; 
			PlayerPrefs.SetInt("NumOfPlays",numberOfPlays);
			PlayerPrefs.Save();
			StartCoroutine("moneyCounter",-startPrice);
			string uzengije = (Shop.stars+10) + "#" + (Shop.highScore-20) + "#" + (Shop.laserNumber+5) + "#" + (Shop.teslaNumber+5) + "#" + (Shop.bladesNumber+5) + "#" + (Shop.bombNumber+5);
			PlayerPrefs.SetString("Uzengije",uzengije);
			PlayerPrefs.Save();
			DisableCheckpointButtons();
			LevelGenerator.checkpoint = true;
			PopUpHandler.gameStarted = false;
			PandaPlane.Instance.score = 0;
			MoveBg.hasBridge = false;
			LevelGenerator.currentStage = startingStage;

			//#if UNITY_ANDROID

			//#endif
			LevelGenerator.terrainsPassed = 0;
			PandaPlane.Instance.numberOfKills = 0;
			
			if(startingStage>6)
			{
				LevelGenerator.currentBossPlaneLevel = 3;
				LevelGenerator.currentBossShipLevel = 3;
				LevelGenerator.currentBossTankLevel = 3;
			}
			else if(startingStage>5)
			{
				LevelGenerator.currentBossPlaneLevel = 3;
				LevelGenerator.currentBossShipLevel = 3;
				LevelGenerator.currentBossTankLevel = 2;
			}
			else if(startingStage>4)
			{
				LevelGenerator.currentBossPlaneLevel = 3;
				LevelGenerator.currentBossShipLevel = 2;
				LevelGenerator.currentBossTankLevel = 2;
			}
			else if(startingStage>3)
			{
				LevelGenerator.currentBossPlaneLevel = 2;
				LevelGenerator.currentBossShipLevel = 2;
				LevelGenerator.currentBossTankLevel = 2;
			}
			else if(startingStage>2)
			{
				LevelGenerator.currentBossPlaneLevel = 2;
				LevelGenerator.currentBossShipLevel = 2;
				LevelGenerator.currentBossTankLevel = 1;
			}
			else if(startingStage>1)
			{
				LevelGenerator.currentBossPlaneLevel = 2;
				LevelGenerator.currentBossShipLevel = 1;
				LevelGenerator.currentBossTankLevel = 1;
			}
			else
			{
				LevelGenerator.currentBossPlaneLevel = 1;
				LevelGenerator.currentBossShipLevel = 1;
				LevelGenerator.currentBossTankLevel = 1;
			}
			
		}
		
	}
	
	public IEnumerator StarFromStage()
	{
		string timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.Save();

		//LevelGenerator.currentBossShipLevel = 2;
		SoundManager.Instance.Play_DoorClosing();

		if(levels == null)
		{
			minimalLevel = 2;
			levels = new List<int>();
			for(int i=minimalLevel;i<=7;i++)
			{
				levels.Add(i);
			}
		}
		else 
		{
			if(levels.Count==0)
			{
				minimalLevel = 1;
				for(int i=minimalLevel;i<=7;i++)
				{
					if(i != Application.loadedLevel)
						levels.Add(i);
				}
			}
		}
		int randomStage = UnityEngine.Random.Range(0,levels.Count);

		int levelToLoad = levels[randomStage];
		levels.RemoveAt(randomStage);
		SoundManager.Instance.Stop_MenuMusic();
		yield return new WaitForSeconds(2.0f);
		Application.LoadLevel(levelToLoad);

	}
	
	public IEnumerator moneyCounter(int kolicina)
	{
		//		if(kolicina>0)
		//		{
		//			if(PlaySounds.soundOn)
		//				PlaySounds.Play_CoinsSpent();
		//		}
		
		int current = int.Parse(moneyText.text);
		int suma = current + kolicina;
		int korak = (suma - current)/10;
		while(current != suma)
		{
			current += korak;
			moneyText.text = current.ToString();
			yield return new WaitForSeconds(0.07f);
		}
		Shop.stars = current;
		moneyText.text = Shop.stars.ToString();
		string uzengije = (Shop.stars+10) + "#" + (Shop.highScore-20) + "#" + (Shop.laserNumber+5) + "#" + (Shop.teslaNumber+5) + "#" + (Shop.bladesNumber+5) + "#" + (Shop.bombNumber+5);
		PlayerPrefs.SetString("Uzengije",uzengije);
		PlayerPrefs.Save();
		//UPDATE!!!!!
		//GameObject.Find("CheckpointsScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().CancelLoading();
		//GameObject.Find("CheckpointsScreen/NativeAdHolder/AnimationHolder").GetComponent<Animator>().Play("NativeAdsDeparting");

		GameObject.Find("LoadingHolder/AnimationHolder").GetComponent<Animation>().Play("LoadingArrival2New");
		StartCoroutine("StarFromStage");
		
	}

	public void CloseCheckpointMenu()
	{
		SoundManager.Instance.Play_ButtonClick();
		GameObject.Find("CheckpointHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardDeparting");
		//UPDATE!!!!!
		//GameObject.Find("CheckpointsScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().CancelLoading();
		//GameObject.Find("CheckpointsScreen/NativeAdHolder/AnimationHolder").GetComponent<Animator>().Play("NativeAdsDeparting");
		//GameObject.Find("MainScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().LoadAdWithDelay(0.75f);
	}

	public void DisableCheckpointButtons()
	{
		for(int i=0;i<GameObject.Find("Checkpoints").transform.childCount;i++)
		{
			GameObject.Find("Checkpoints").transform.GetChild(i).gameObject.GetComponent<Button>().interactable=false;
		}
	}

	public void SetContentHeight()
	{
		float scrollContentHeight = ((gridLayout.transform.childCount) * gridLayout.cellSize.y) + ((gridLayout.transform.childCount - 1) * gridLayout.spacing.y)+50;
		scrollContent.sizeDelta = new Vector2(gridLayout.gameObject.GetComponent<RectTransform>().sizeDelta.x, scrollContentHeight);
	}

	private IEnumerator MoveTowardsTarget(float time,float from,float target) {
		float i = 0;
		float rate = 1 / time;
		while(i<1){
			i += rate * Time.deltaTime;
			scrollRect.verticalNormalizedPosition = Mathf.Lerp(from,target,i);
			yield return 0;
		}
	}
}
