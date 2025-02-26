using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class StartSceneFromCheckpoint : MonoBehaviour {

	[SerializeField]
	public int startingStage;
	static int minimalLevel = 2;
	public void ContinueFromStage()
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
		Debug.Log("startPrice "+startPrice);
		if(Shop.stars<startPrice)
		{
			GameObject.Find("StarsCheckpoint/AnimationHolder").GetComponent<Animator>().Play("HangarNoStars");
		}
		else
		{
			int numberOfPlays;
			if(PlayerPrefs.HasKey("NumOfPlays"))
			{
				numberOfPlays = PlayerPrefs.GetInt("NumOfPlays");
			}
			else
			{
				numberOfPlays=0;
			}

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
			

			LevelGenerator.terrainsPassed = 0;
			PandaPlane.Instance.numberOfKills = 0;
			
			if(startingStage>6)
			{
				LevelGenerator.currentBossPlaneLevel = 3;
				LevelGenerator.currentBossShipLevel = 3;
				LevelGenerator.currentBossTankLevel = 3;
			}
			else if(startingStage>3)
			{
				LevelGenerator.currentBossPlaneLevel = 2;
				LevelGenerator.currentBossShipLevel = 2;
				LevelGenerator.currentBossTankLevel = 2;
			}
			else
			{
				LevelGenerator.currentBossPlaneLevel = 1;
				LevelGenerator.currentBossShipLevel = 1;
				LevelGenerator.currentBossTankLevel = 1;
			}
			
		}
		
	}

	public void DisableCheckpointButtons()
	{
		for(int i=0;i<GameObject.Find("Checkpoints").transform.childCount;i++)
		{
			GameObject.Find("Checkpoints").transform.GetChild(i).gameObject.GetComponent<Button>().interactable=false;
		}
	}

	public IEnumerator moneyCounter(int kolicina)
	{
		//		if(kolicina>0)
		//		{
		//			if(PlaySounds.soundOn)
		//				PlaySounds.Play_CoinsSpent();
		//		}
		
		int current = int.Parse(GameObject.Find("StarsNumberCheckpointText").GetComponent<Text>().text);
		int suma = current + kolicina;
		int korak = (suma - current)/10;
		while(current != suma)
		{
			current += korak;
			GameObject.Find("StarsNumberCheckpointText").GetComponent<Text>().text = current.ToString();
			yield return new WaitForSeconds(0.07f);
		}
		Shop.stars = current;
		GameObject.Find("StarsNumberCheckpointText").GetComponent<Text>().text = Shop.stars.ToString();
		string uzengije = (Shop.stars+10) + "#" + (Shop.highScore-20) + "#" + (Shop.laserNumber+5) + "#" + (Shop.teslaNumber+5) + "#" + (Shop.bladesNumber+5) + "#" + (Shop.bombNumber+5);
		PlayerPrefs.SetString("Uzengije",uzengije);
		PlayerPrefs.Save();
		GameObject.Find("LoadingHolder/AnimationHolder").GetComponent<Animation>().Play("LoadingArrival2New");
		StartCoroutine("StarFromStage");
		
	}

	public IEnumerator StarFromStage()
	{
		string timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.Save();
		
		//LevelGenerator.currentBossShipLevel = 2;
		SoundManager.Instance.Play_DoorClosing();
		
		if(PlayButton.levels == null)
		{
			minimalLevel = 2;
			PlayButton.levels = new List<int>();
			for(int i=minimalLevel;i<=7;i++)
			{
				PlayButton.levels.Add(i);
			}
		}
		else 
		{
			if(PlayButton.levels.Count==0)
			{
				minimalLevel = 1;
				for(int i=minimalLevel;i<=7;i++)
				{
					if(i != Application.loadedLevel)
						PlayButton.levels.Add(i);
				}
			}
		}
		int randomStage = UnityEngine.Random.Range(0,PlayButton.levels.Count);
		
		int levelToLoad = PlayButton.levels[randomStage];
		PlayButton.levels.RemoveAt(randomStage);
		SoundManager.Instance.Stop_MenuMusic();
		yield return new WaitForSeconds(2.0f);
		Application.LoadLevel(levelToLoad);
		
	}
}
