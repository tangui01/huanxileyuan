using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveBg : MonoBehaviour {

	float bottomLimit;
	[HideInInspector] public Transform snapObject;
	Transform endPos;
	public float speed;
	public static bool canMove = false;
	[SerializeField]
	public bool isAvailable = true;
	public int minimumStage = 1;
	public bool notAvailableUntilBoss = false;
	public static bool hasBridge = false;

	public enum stage {woods, ice, lava, sea, tropic, jungle, wasteland, darkWoods};



	//public enum terrain {OnlyGround, GroundToWater, OnlyWater, WaterToGround};
	public int TerrainType;
	//public bool firstTerrain = false;

	[HideInInspector] public List<Transform> availableTerrains;
	[HideInInspector] public List<int> availableTerrainIndexes;
	public int[] availableTerrainTypes;
	Transform cameraTransform;

	//@@@@@@@ ZA BRISANJE
	TextMesh terena;

	public stage stageSelected;
	// Use this for initialization
	void Start () 
	{
		cameraTransform = Camera.main.transform;
		snapObject = transform.Find("SnapObject");
		bottomLimit = cameraTransform.position.y - Camera.main.orthographicSize;
		endPos = transform.Find("EndPos");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(endPos.position.y <= cameraTransform.transform.position.y - Camera.main.orthographicSize)
		{
			//gameObject.SetActive(false);
			//transform.position = snapObject.position;
			if(!isAvailable)
			{
				isAvailable = true;
				LevelGenerator.terrainsPassed++;
				//Debug.Log("TErrainS passed: " + LevelGenerator.terrainsPassed);

				if(AircraftBattleGameManager.Instance.bossTime)
				{
					LevelGenerator.Instance.RepositionSingleTerrainForBoss();
				}

				if(LevelGenerator.terrainsPassed == LevelGenerator.Instance.terenaUPocetku-2)
				{
					if(!AircraftBattleGameManager.Instance.bossTime)
					{
						LevelGenerator.Instance.RepositionTerrainForBoss(stageSelected);
						AircraftBattleGameManager.Instance.bossTime = true;
					}
				}

				else if(name.Contains("Bridge"))
				{
					//Camera.main.transform.Find("BossTepach").GetComponent<Boss>().SummonBoss();
					LevelGenerator.Instance.SummonBoss();
					//LevelGenerator.terrainsPassed = 0;
				}

				else if(LevelGenerator.terrainsPassed == LevelGenerator.Instance.terenaUPocetku)
				{
					if(!hasBridge)
					{
						//Camera.main.transform.Find("BossTepach").GetComponent<Boss>().SummonBoss();
						LevelGenerator.Instance.SummonBoss();
						hasBridge = true;
					}
					//LevelGenerator.terrainsPassed = 0;
				}
			}
		}

		//if(canMove && !isAvailable)
		//	transform.Translate(0,speed * Time.deltaTime, 0);
	}



//	void OnBecameInvisible()
//	{
//		gameObject.SetActive(false);
//	}
}
