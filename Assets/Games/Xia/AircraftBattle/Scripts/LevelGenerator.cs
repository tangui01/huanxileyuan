using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {

	static LevelGenerator instance;
	public static LevelGenerator Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(LevelGenerator)) as LevelGenerator;
			}
			return instance;
		}
	}
	public bool stageCleared;
	[HideInInspector] public Transform activeTerrain;
	[HideInInspector] public Transform snapObject;
	List<int> availableTerrainIndexes;
	public int terenaUPocetku = 8;
	public static int terrainsPassed = 0;

	MoveBg activeTerrainProperties;
	MoveBg newTerrainProperties;
	[HideInInspector] public static int currentStage = 1;

	public static int currentBossPlaneLevel = 1;
	public static int currentBossShipLevel = 1;
	public static int currentBossTankLevel = 1;
	GameObject currentBoss;
	[HideInInspector] public int currentBossAvailableTerrainType;
	[HideInInspector] public int currentBossCameraOffsetY;
	public GameObject warpZonePrefab;
	public static bool checkpoint = false;
	
	// Use this for initialization
	void Start () 
	{
		currentStage = PlayerPrefs.GetInt("level", 1);
		Debug.Log(currentStage);
		if (currentStage == 1)
		{
			LevelGenerator.checkpoint = true;
			PopUpHandler.gameStarted = false;
			PandaPlane.Instance.score = 0;
			MoveBg.hasBridge = false;
			LevelGenerator.terrainsPassed = 0;
			LevelGenerator.currentBossPlaneLevel = 1;
			LevelGenerator.currentBossShipLevel = 1;
			LevelGenerator.currentBossTankLevel = 1;
			PandaPlane.Instance.numberOfKills = 0;
		}
		availableTerrainIndexes = new List<int>();
		activeTerrain = transform.Find("TerrainPreset3");
		activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
		snapObject = activeTerrain.Find("SnapObject");

//		for(int i=0;i<transform.childCount;i++)
//		{
//			newTerrainProperties = transform.GetChild(i).GetComponent<MoveBg>();
//			if(newTerrainProperties.minimumStage <= currentStage && newTerrainProperties.isAvailable)
//			{
//				if(System.Array.IndexOf(newTerrainProperties.availableTerrainTypes, newTerrainProperties.TerrainType) > -1)
//					availableTerrainIndexes.Add(i);
//			}
//		}

		for(int i=0;i<terenaUPocetku-1;i++)
		{
			availableTerrainIndexes.Clear();

			for(int ii=0;ii<transform.childCount;ii++)
			{
				newTerrainProperties = transform.GetChild(ii).GetComponent<MoveBg>();
				if(newTerrainProperties.minimumStage <= currentStage && newTerrainProperties.isAvailable && !newTerrainProperties.notAvailableUntilBoss)
				{
					if(System.Array.IndexOf(activeTerrainProperties.availableTerrainTypes, newTerrainProperties.TerrainType) > -1)
						availableTerrainIndexes.Add(ii);
				}
			}

			if(availableTerrainIndexes.Count == 0)
			{
				GameObject InstantiatedTerrain;
				for(int j=0;j<transform.childCount;j++)
				{
					newTerrainProperties = transform.GetChild(j).GetComponent<MoveBg>();
					if(newTerrainProperties.minimumStage <= currentStage && !newTerrainProperties.notAvailableUntilBoss)
					{
						if(System.Array.IndexOf(activeTerrainProperties.availableTerrainTypes, newTerrainProperties.TerrainType) > -1)
						{
							availableTerrainIndexes.Add(j);
						}

					}
				}
				int randomTerrainClone = Random.Range(0,availableTerrainIndexes.Count);
				InstantiatedTerrain = Instantiate(transform.GetChild(availableTerrainIndexes[randomTerrainClone]).gameObject) as GameObject;
				activeTerrain = InstantiatedTerrain.transform;
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;

			}
			else
			{
				int randomTerrain = Random.Range(0,availableTerrainIndexes.Count);
				//Debug.Log("AVAILAHEL TERERAIN INDEXES COUNT: " + availableTerrainIndexes.Count + ", index izvucen: " + randomTerrain);

				activeTerrain = transform.GetChild(availableTerrainIndexes[randomTerrain]);
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;
				//Debug.Log("IZVUKOJE: " + randomTerrain + ", dok je teren recimo: " + transform.GetChild(availableTerrainIndexes[randomTerrain]));
				availableTerrainIndexes.RemoveAt(randomTerrain);
			}
		}
		SummoningBoss();

	}

//	void Update()
//	{
//
//		Debug.Log("Terrains passed: " + terrainsPassed);
//	}

	public void RepositionTerrain()
	{
		for(int i=0;i<terenaUPocetku-1;i++)
		{
			availableTerrainIndexes.Clear();
			
			for(int ii=0;ii<transform.childCount;ii++)
			{
				newTerrainProperties = transform.GetChild(ii).GetComponent<MoveBg>();
				// if(newTerrainProperties.isAvailable)
				if(newTerrainProperties.minimumStage <= currentStage && newTerrainProperties.isAvailable && !newTerrainProperties.notAvailableUntilBoss)
				{
					if(System.Array.IndexOf(activeTerrainProperties.availableTerrainTypes, newTerrainProperties.TerrainType) > -1)
						availableTerrainIndexes.Add(ii);
				}
			}

			//"AKO NEMA SLOBODNIH TERENA DA INSTANCIRA NEKI KOJI PASUJE!!!!!"
			if(availableTerrainIndexes.Count == 0)
			{
				GameObject InstantiatedTerrain;
				for(int j=0;j<transform.childCount;j++)
				{
					newTerrainProperties = transform.GetChild(j).GetComponent<MoveBg>();
					if(newTerrainProperties.minimumStage <= currentStage && !newTerrainProperties.notAvailableUntilBoss)
					{
						if(System.Array.IndexOf(activeTerrainProperties.availableTerrainTypes, newTerrainProperties.TerrainType) > -1)
						{
							availableTerrainIndexes.Add(j);
						}
						
					}
				}
				int randomTerrainClone = Random.Range(0,availableTerrainIndexes.Count);
				InstantiatedTerrain = Instantiate(transform.GetChild(availableTerrainIndexes[randomTerrainClone]).gameObject) as GameObject;
				activeTerrain = InstantiatedTerrain.transform;
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;
				
			}
			else
			{
				int randomTerrain = Random.Range(0,availableTerrainIndexes.Count);
				activeTerrain = transform.GetChild(availableTerrainIndexes[randomTerrain]);
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;
				//Debug.Log("IZVUKOJE: " + randomTerrain + ", dok je teren recimo: " + transform.GetChild(availableTerrainIndexes[randomTerrain]));
				availableTerrainIndexes.RemoveAt(randomTerrain);
			}
		}
	}

	public void RepositionSingleTerrainForBoss()
	{
		availableTerrainIndexes.Clear();
		for(int ii=0;ii<transform.childCount;ii++)
		{
			newTerrainProperties = transform.GetChild(ii).GetComponent<MoveBg>();
			if(newTerrainProperties.minimumStage <= currentStage && newTerrainProperties.isAvailable)
			{
				
				//for(int q=0; q<Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes.Length;q++)
				{
					if(currentBossAvailableTerrainType == -1)
					{
						if(System.Array.IndexOf(activeTerrainProperties.availableTerrainTypes, newTerrainProperties.TerrainType) > -1)
						{
							availableTerrainIndexes.Add(ii);
						}
					}
					else
					{
						if(currentBossAvailableTerrainType.Equals(newTerrainProperties.TerrainType))
						{
							availableTerrainIndexes.Add(ii);
						}
					}
				}
				//if(System.Array.IndexOf((int[])Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes, (int)newTerrainProperties.TerrainType) > -1 || Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes.Length == 0)
				//	availableTerrainIndexes.Add(ii);
			}
		}


		int randomTerrain = Random.Range(0,availableTerrainIndexes.Count);
		try
		{
			activeTerrain = transform.GetChild(availableTerrainIndexes[randomTerrain]);
			activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
			activeTerrain.position = snapObject.position;
			snapObject = activeTerrain.Find("SnapObject");
			activeTerrainProperties.isAvailable = false;
			availableTerrainIndexes.RemoveAt(randomTerrain);
		}
		catch (Exception e)
		{

		}


	}

	public void RepositionTerrainForBoss(MoveBg.stage currentStage)
	{
		switch(currentStage)
		{
		case MoveBg.stage.woods:
			if((activeTerrainProperties.TerrainType == 0 || activeTerrainProperties.TerrainType == 3 || activeTerrainProperties.TerrainType == 5) && currentBossAvailableTerrainType == 2)
			{
				activeTerrain = transform.Find("TerrainBossBridge");
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;
				MoveBg.hasBridge = true;
			}
			else if((activeTerrainProperties.TerrainType == 1 || activeTerrainProperties.TerrainType == 2 || activeTerrainProperties.TerrainType == 4) && currentBossAvailableTerrainType == 0)
			{
				activeTerrain = transform.Find("TerrainBossBridge2");
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;
				MoveBg.hasBridge = true;
			}
			break;

		case MoveBg.stage.darkWoods:
			if((activeTerrainProperties.TerrainType == 0 || activeTerrainProperties.TerrainType == 3) && currentBossAvailableTerrainType == 2)
			{
				//Debug.Log("OVDE DA SE UBACI DA DODA TEREN KOJI TREBA!!!!!");
				activeTerrain = transform.Find("TerrainBossBridge");
				activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
				activeTerrain.position = snapObject.position;
				snapObject = activeTerrain.Find("SnapObject");
				activeTerrainProperties.isAvailable = false;
				MoveBg.hasBridge = true;
			}
			else if((activeTerrainProperties.TerrainType == 1 || activeTerrainProperties.TerrainType == 2) && currentBossAvailableTerrainType == 0)
			{
				//Debug.Log("I OVDE ISTO!!!!!");
			}
			break;
		}


//		for(int i=0;i<2;i++)
//		{
//			availableTerrainIndexes.Clear();
//
//			for(int ii=0;ii<transform.childCount;ii++)
//			{
//				newTerrainProperties = transform.GetChild(ii).GetComponent<MoveBg>();
//				if(newTerrainProperties.minimumStage <= currentStage && newTerrainProperties.isAvailable)
//				{
//
//					for(int q=0; q<Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes.Length;q++)
//					{
//						Debug.Log("AVAJLIBLE TERAJN TUPE: " + Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes[q] + "TERAJIN TUPE: " + newTerrainProperties.TerrainType);
//						if(((int)(Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes[q])).Equals((int)newTerrainProperties.TerrainType))
//						{
//							Debug.Log("IDEMO BRE!!!!!");
//							availableTerrainIndexes.Add(ii);
//						}
//					}
//					Debug.Log("============================================================");
//					//if(System.Array.IndexOf((int[])Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes, (int)newTerrainProperties.TerrainType) > -1 || Camera.main.transform.Find("BossTepach").GetComponent<Boss>().AvailableTerrainTypes.Length == 0)
//					//	availableTerrainIndexes.Add(ii);
//				}
//			}
//
//			int randomTerrain = Random.Range(0,availableTerrainIndexes.Count);
//			Debug.Log("AVAILAHEL TERERAIN INDEXES COUNT: " + availableTerrainIndexes.Count + ", index izvucen: " + randomTerrain);
//			activeTerrain = transform.GetChild(availableTerrainIndexes[randomTerrain]);
//			activeTerrainProperties = activeTerrain.GetComponent<MoveBg>();
//			activeTerrain.position = snapObject.position;
//			snapObject = activeTerrain.Find("SnapObject");
//			activeTerrainProperties.isAvailable = false;
//			Debug.Log("IZVUKOJE: " + randomTerrain + ", dok je teren recimo: " + transform.GetChild(availableTerrainIndexes[randomTerrain]));
//			availableTerrainIndexes.RemoveAt(randomTerrain);
//		}
	}

	public void DestroyTerrains()
	{
		StartCoroutine(DestroyTerrainCoroutine());
	}

	IEnumerator DestroyTerrainCoroutine()
	{
		string[] ABScene = new[] { "Woods", "Wasteland", "Ice" };
		yield return new WaitForSeconds(2f);
		PlayerPrefs.SetInt("level", ++currentStage);
		terrainsPassed = 0;
		MoveBg.hasBridge = false;
		LoadABManger.Instance.UnloadAB(SceneManager.GetActiveScene().name);
		int randomTerrain = Random.Range(0,3);
		while (ABScene[randomTerrain].Equals(SceneManager.GetActiveScene().name))
		{
			randomTerrain = Random.Range(0,3);
		}
		LoadABManger.Instance.LoadAB(ABScene[randomTerrain]);
		//int randomStage = Random.Range(minimalLevel,levels.Count+1);
//		while(Application.loadedLevel == randomStage)
//		{
//			randomStage = Random.Range(minimalLevel,levels.Count+1);
//			Debug.Log("§§§§§§§§§§-- CURRENT STAGE: " + Application.loadedLevel + ", new level: " + randomStage + " --§§§§§§§§§§");
//		}
//		string stageType = System.String.Empty;
//		if(Application.loadedLevelName.Equals("Woods"))
//			stageType = "Ice";
//		else if(Application.loadedLevelName.Equals("Ice"))
//			stageType = "Wasteland";
//		else if(Application.loadedLevelName.Equals("Wasteland"))
//			stageType = "Woods";

//		Application.LoadLevel(stageType);

//		int count = transform.childCount;
//
//		//for(int i=0;i<transform.childCount;i++)
//		for(int i=transform.childCount-1;i>=0;i--)
//		{
//			//Transform obj = transform.GetChild(i);
//			//obj.parent = null;
//			Destroy(transform.GetChild(i).gameObject);
//			yield return new WaitForSeconds(0.02f);
//			//"PROBAJ LOOP UNAZAD SA DELAY-OM!!!!!"
//		}
//		yield return null;
//		yield return new WaitForSeconds(3f);
//		transform.DetachChildren();
//		StartCoroutine(LoadNewTerrains());
	}
	

//	public void SummonBoss()
//	{
//		GameObject boss = Instantiate(Resources.Load("Boss/BossPlaneHOLDER"), new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y+5, -18), Quaternion.identity) as GameObject;
//		boss.transform.parent = Camera.main.transform;
//	}

	public void SummonBoss()
	{
//		GameObject.Find("BossTime").renderer.enabled = true;
//		Invoke("HideBossText",1.5f);
//		//Invoke("PositionBoss",2.2f);
//		currentBoss.SetActive(true);
//		currentBoss.SendMessage("AdjustSpeed",SendMessageOptions.DontRequireReceiver);
		//SoundManager.Instance.Play_BossTime();
		SoundManager.Instance.Stop_GameplayMusic();
		GameObject.Find("BossTime").transform.GetChild(0).GetComponent<Animation>().Play();
		if(Time.timeScale < 1)
		{
			PlaneManager.pressedAndHold = true;
			Interface.Instance.normalTime = true;
			PopUpHandler.Instance.ResumeGame();
			StartCoroutine(PlaneManager.Instance.NormalTime(0.02f));
		}
		PlaneManager.Instance.dontSlowTime = 1;
		PlaneManager.Instance.bossTimePart = true;
	}

	public void CallBoss()
	{
		currentBoss.SetActive(true);
		currentBoss.SendMessage("AdjustSpeed",SendMessageOptions.DontRequireReceiver);
	}
	
	void SummoningBoss()
	{
		if(currentStage % 3 == 1)//da se vrati na 1
		{
			currentBoss = Instantiate(Resources.Load("Boss/BossPlaneHOLDER"+currentBossPlaneLevel.ToString()), new Vector3(0, -50, -30), Quaternion.identity) as GameObject;
			currentBoss.SendMessage("GetAvailableTerrainType",SendMessageOptions.DontRequireReceiver);
			currentBoss.SetActive(false);
		}
		else if(currentStage % 3 == 2)
		{
			currentBoss = Instantiate(Resources.Load("Boss/BossShipHOLDER"+currentBossShipLevel.ToString()), new Vector3(0, -50, -30), Quaternion.identity) as GameObject;
			currentBoss.SendMessage("GetAvailableTerrainType",SendMessageOptions.DontRequireReceiver);
			currentBoss.SetActive(false);
		}
		else
		{
			currentBoss = Instantiate(Resources.Load("Boss/BossTankHOLDER"+currentBossTankLevel.ToString()), new Vector3(0, -50, -30), Quaternion.identity) as GameObject;
			currentBoss.SendMessage("GetAvailableTerrainType",SendMessageOptions.DontRequireReceiver);
			currentBoss.SetActive(false);
		}
	}

	void PositionBoss()
	{
		if(currentStage % 3 == 1)
		{
			currentBossCameraOffsetY = 7;
			currentBoss.transform.position = new Vector3(0, Camera.main.transform.position.y+currentBossCameraOffsetY, -19.5f);//-18
			currentBoss.SendMessage("SetCameraOffsetY",SendMessageOptions.DontRequireReceiver);
		}
		else if(currentStage % 3 == 2)
		{
			currentBossCameraOffsetY = 7;
			currentBoss.transform.position = new Vector3(0, Camera.main.transform.position.y+currentBossCameraOffsetY, -21);
			currentBoss.SendMessage("SetCameraOffsetY",SendMessageOptions.DontRequireReceiver);
		}
		//currentBoss.transform.parent = Camera.main.transform;
		else if(currentStage % 3 == 0)
		{
			currentBossCameraOffsetY = 3;
			currentBoss.transform.position = new Vector3(0, Camera.main.transform.position.y+currentBossCameraOffsetY, -30);//-18
			currentBoss.SendMessage("SetCameraOffsetY",SendMessageOptions.DontRequireReceiver);
		}

	}
	
	void HideBossText()
	{
		GameObject.Find("BossTime").GetComponent<Renderer>().enabled = false;
	}

	public void StageClear()
	{
		stageCleared = true;
		SoundManager.Instance.Play_StageClear();
		// int currentMaxStage = 0;
		//
		// if(currentStage<=currentMaxStage)
		// {
		// 	GameObject.Find("CheckpointHolder")?.SetActive(false);
		// }
		// if(currentStage>currentMaxStage)
		// {
		// 	PlayerPrefs.SetInt("MaxStage",currentStage);
		// 	PlayerPrefs.SetInt("ghE67+=as23",currentStage+5);
		// 	PlayerPrefs.SetInt("Fsdfs+=as23",currentStage+11);
		// }

//		switch(currentStage)
//		{
//		case 3: case 6: case 9: case 12: case 15:
//			if(currentStage<currentMaxStage)
//			{
//				GameObject.Find("CheckpointHolder").SetActive(false);
//			}
//
//			break;
//		default:
//			GameObject.Find("CheckpointHolder").SetActive(false); //"OVDE JE IZBACIO NULL REFERENCE EXCEPTION!!!!!"
////			Debug.Log("PRC KENZO JAPANAC");
//			break;
//		}

		
		PandaPlane.Instance.AddScore(200*currentStage);
		// string uzengije = (PandaPlane.stars+10) + "#" + (PandaPlane.highScore-20) + "#" + (PandaPlane.laserWeaponNumber+5) + "#" + (PandaPlane.teslaWeaponNumber+5) + "#" + (PandaPlane.bladesWeaponNumber+5) + "#" + (PandaPlane.bombWeaponNumber+5);
		// PlayerPrefs.SetString("Uzengije",uzengije);
		// PlayerPrefs.Save();
		GameObject.Find("StageText").GetComponent<Text>().text = currentStage.ToString();
		GameObject.Find("StageClearHolder/AnimationHolder").GetComponent<Animation>().Play();
	}


}
