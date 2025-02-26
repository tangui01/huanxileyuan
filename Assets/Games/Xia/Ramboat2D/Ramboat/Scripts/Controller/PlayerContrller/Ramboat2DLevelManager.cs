using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public enum Ramboat2DGameState
{
	PrepareGame,
	Playing,
	NextState,
	GameOver,
	Pause,
	Win,
	WaitForPopup,
	WaitAfterClose,
	PreFailed,
}

public class Ramboat2DLevelManager : MonoBehaviour
{
	public static Ramboat2DLevelManager THIS;
	[HideInInspector]
	public bool startingGame =false;
	[HideInInspector]
	public bool reloadBoxContain=true;
	private bool touchPlay = false;
	public Ramboat2DGameState gameState;
	public GameObject[] gameMapState;
	public float[] timeNextLevel;
	public float timePlayingGame = 0;
	public int level;
	public int mapCounterDestroyed = 0;
	public int checkGameState = 0;
	public List<GameObject> spawnEnemys;
	public List<GameObject> spawnSubMarine;
	public List<Sprite> gunTypeSprite;
	public GameObject boxContain;
	// need use object pooling
	public GameObject[] Enemys;
	public GameObject boatPrebs;
	public GameObject airPrebs;
	public GameObject pointEnemyDestroyed;
	int isSpawnEnemy, isSpawnEnemyFly, isSpawnBoat, isSpawnAir, isSpawnAirFollow, isSpawnSubMarine, numberSpawnOneWay, isSpawnGift,isSpawnBoms;
	//csdl
	public float coinCollectedPassLevel;
	public float coinCollected;
	public bool levelLoaded;
	private float timeEnemyToRespawn;
	private float timeEnemyFlyRespawn;
	private float timeBoatRespawn, healthBoat;
	private float timeAirRespawn, timeAirShoot, healthAir;
	private float timeAirFollowRespawn;
	private float timeSubMarineRespawn;
	[HideInInspector]
	public float timeAirFollowShoot, healthAirFollow;
	[HideInInspector]
	public int countAirFollowShoot;
	[HideInInspector]
	public float timeEnemyToShoot, timeEnenmyFlyShoot;
	public bool subTimeScale;
	//enemy pooling
	public GameObject parent;
	public GameObject enemy;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject enemy4;
	public GameObject enemy5;
	public GameObject enemy6;
	public GameObject enemy7;
	public GameObject boatEnemy;
	public GameObject submarineEnemy;
	public GameObject airEnemy;
	public GameObject bomb;

	// create bullet
	public GameObject gunEnemy;
	public GameObject effectShootPlayer;
	public GameObject coin;
	public GameObject dollar;
	public GameObject gunAir;
	public GameObject gift;
	// create effect map
	public GameObject hitWater;
	public GameObject hitExplosionWater;
	public GameObject collectedCoinEffect;
	public GameObject blood;
	// variable another class
	private GameObject setUpGunCanvas;
	public GameObject weather;
	List<GameObject> enemys, enemys2, enemys3, enemys4, enemys5, enemys6,
		enemys7, submarineEnemys, gunEnemys, efects, coins, dollars,
		hitWaters, hitExplosionWaters, collectedCoinEffects, bloods, gunAirs,bombs;
	// Use this for initialization
	//sprite mission
	public Sprite[] missions;
	public Sprite[] missionsComPlete;
	public Sprite[] missionsLevel;
	public Sprite[] missionLevelSmall;
	public GameObject missionCanvas;
	bool oneShowMission=false;
	//UI
	public Sprite[] avatarPlayer;
	public Sprite[] avatarBoat;
	public Sprite[] gunSprites;
	[HideInInspector]
	public int playerNumber;
	void Awake ()
	{
		Invoke("TouchPlayGame",1.7f);
		THIS = this;
		level = 0;
		gameState = Ramboat2DGameState.PrepareGame;
		isSpawnEnemy = 0;
		EnableCamera ();
		SetBoxColliderOnCamera ();
		CreateObjectPooling ();
		isSpawnGift = 0;
		isSpawnBoms = 0;
		setUpGunCanvas=GameObject.Find ("Ramboat2DCanvas").transform.GetChild (4).gameObject;
		coinCollectedPassLevel = 0;
		coinCollected = 0;
		
		playerNumber = Random.Range (0, 7);
	}
	void Start(){
		AudioManager.Instance.playerBGm(Ramboat2DFXSound.THIS.music[0]);
	}
	void Update ()
	{
		
		if (reloadBoxContain && !startingGame) {
			boxContain.SetActive (true);
		}
		if (!startingGame && touchPlay) {
			boxContain.GetComponent<Animator> ().enabled = true;
			GameObject.Find("Ramboat2DCanvas").transform.GetChild(2).gameObject.SetActive(false);
			GameObject.Find("Ramboat2DCanvas").transform.GetChild(0).gameObject.SetActive(false);
			touchPlay = false;

		} else if(startingGame && !Ramboat2DPlayerController.Intance.playerDead){
//		if (subTimeScale) {
//			Time.timeScale = 0.2f;
//			subTimeScale = false;
//		}
		timePlayingGame += Time.deltaTime;
		if (timePlayingGame > timeNextLevel [level]) {
			if (checkGameState % 2 == 1) {
				if (checkGameState == 3)
						ReadWriteTextMission.THIS.CheckMission (1);
				level += 1;
				gameState = Ramboat2DGameState.NextState;
				LoadDataFromLocal (level);
				weather.transform.GetChild ((checkGameState / 2)%weather.transform.childCount).gameObject.SetActive (true);
				if (!oneShowMission) {
						oneShowMission = true;
						// missionCanvas.SetActive (true);
				}

			}
			if (checkGameState % 2 == 0 && !IsHaveEnemy ()) {
					coinCollectedPassLevel = float.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text) - coinCollected;//本次游戏获得总金币-已经结算的金币
					coinCollected = float.Parse (Ramboat2DPlayerController.Intance.textCoinCollect.text);
					level += 1;
				gameState = Ramboat2DGameState.NextState;
				timePlayingGame = 0;
					for (int i = 0; i < weather.transform.childCount; i++) {
						weather.transform.GetChild (i).gameObject.SetActive (false);
					}
			}
		}
		else if(checkGameState%2==1  && timePlayingGame>2 && timePlayingGame < 7 && !setUpGunCanvas.activeInHierarchy){
				setUpGunCanvas.SetActive (true);

		}
		if (gameState == Ramboat2DGameState.Playing && timePlayingGame < timeNextLevel [level]) {
			RespawnEnemy ();
		}
		}
	}

	void EnableCamera ()
	{
		float aspect = (float)Screen.width / (float)Screen.height;

		float aspects = (float)Mathf.Round (aspect * 100) / 100f;
		if (aspects == 1.67f)
			GetComponent<Camera> ().orthographicSize = 3.68f;    //16:9
		else if (aspects == 1.78f)
			GetComponent<Camera> ().orthographicSize = 3.45f;                  //3:2
		else if (aspects == 1.71f)
			GetComponent<Camera> ().orthographicSize = 3.55f;                  //4:3
		else if (aspects == 1.6f)
			GetComponent<Camera> ().orthographicSize = 3.65f;                  //5:3
		else if (aspects == 1.5f)
			GetComponent<Camera> ().orthographicSize = 3.6f;
		else
			GetComponent<Camera> ().orthographicSize = 3.65f;


		//Debug.Log (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0f,0f)));
	}

	void SetBoxColliderOnCamera ()
	{
		GameObject rightPrevent = new GameObject ("RightPrevent");
		rightPrevent.AddComponent<BoxCollider2D> ();
		rightPrevent.gameObject.transform.localScale = new Vector3 (1f, 8f, 0);
		rightPrevent.transform.localPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height / 2, 0f));
		rightPrevent.transform.parent = transform;

		GameObject leftPrevent = new GameObject ("LeftPrevent");
		leftPrevent.AddComponent<BoxCollider2D> ();
		leftPrevent.gameObject.transform.localScale = new Vector3 (1f, 8f, 0);
		leftPrevent.transform.localPosition = Camera.main.ScreenToWorldPoint (new Vector3 (0f, Screen.height / 2, 0f));
		leftPrevent.transform.parent = transform;

		GameObject bottomPrevent = new GameObject ("BottomPrevent");
		bottomPrevent.AddComponent<BoxCollider2D> ();
		bottomPrevent.gameObject.transform.localScale = new Vector3 (0.5f, 13f, 0);
		bottomPrevent.gameObject.transform.localEulerAngles = new Vector3 (0, 0, 90);
		bottomPrevent.transform.localPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, 0f, 0f));
		bottomPrevent.transform.parent = transform;

	}

	IEnumerator SpawnEnemy (int choiceEnemy)
	{	
		GameObject obj = GetPooledObject (choiceEnemy);	
		if (obj != null) {
			obj.GetComponentInChildren<EnemyController> ().speed = UnityEngine.Random.Range (0.5f, 1.5f);
			//	yield return new WaitForSeconds (1f);	
			isSpawnEnemy = 1;
			obj.transform.position = spawnEnemys [UnityEngine.Random.Range (0, 11)].transform.position;
			obj.SetActive (true);
			yield return new WaitForSeconds (UnityEngine.Random.Range (timeEnemyToRespawn, timeEnemyToRespawn + 1f));
			isSpawnEnemy = 0;
		}
	}

	IEnumerator SpawnEnemyFly (int choiceEnemy)
	{
		GameObject obj = GetPooledObject (choiceEnemy);	
		if (obj != null) {
			isSpawnEnemyFly = 1;
			obj.transform.position = new Vector3 (UnityEngine.Random.Range (-4, 4), 4, 0);
			obj.SetActive (true);
			yield return new WaitForSeconds (UnityEngine.Random.Range (timeEnemyFlyRespawn, timeEnemyFlyRespawn + 2f));
			isSpawnEnemyFly = 0;
		}
	}

	IEnumerator SpawnBoat ()
	{
		GameObject obj = Instantiate (boatPrebs) as GameObject;
		obj.transform.position = new Vector3 (6.31f, -0.76f, 0f);
		isSpawnBoat = 1;
		yield return new WaitForSeconds (timeBoatRespawn);
		isSpawnBoat = 0;

	}

	IEnumerator SpawnAir ()
	{
		GameObject obj = Instantiate (airPrebs) as GameObject;
		isSpawnAir = 1;
		yield return new WaitForSeconds (UnityEngine.Random.Range (timeBoatRespawn, timeBoatRespawn + 4f));
		isSpawnAir = 0;
	}

	IEnumerator SpawnAirFollow ()
	{
		GameObject obj = Instantiate (Resources.Load ("Prefabs/Enemy/AirFollow")) as GameObject;
		obj.transform.position = new Vector3 (7.27f, 2.5f, 0f);
		isSpawnAirFollow = 1;
		yield return new WaitForSeconds (UnityEngine.Random.Range (timeAirFollowRespawn, timeAirFollowRespawn + 4f));
		isSpawnAirFollow = 0;
	}

	IEnumerator SpawnSubMarine (int numberSpawnOneWay)
	{
		isSpawnSubMarine = 1;
		Hashtable number = new Hashtable ();
		int count = 0;
		while (count < numberSpawnOneWay) {
			int randomPoint = UnityEngine.Random.Range (0, 4);
			if (!number.ContainsValue (randomPoint)) {
				number.Add (count, randomPoint);
				count++;
			
			}
		}
		foreach (DictionaryEntry entry in number) {
			GameObject submarine = GetPooledObject (17);
			if (submarine != null) {
				submarine.transform.position = spawnSubMarine [(int)entry.Value].transform.position;
				submarine.SetActive (true);
			}
		}
		yield return new WaitForSeconds (timeBoatRespawn);
		isSpawnSubMarine = 0;
	}

	IEnumerator SpawnGift ()
	{
		isSpawnGift = 1;
		if (!gift.activeInHierarchy) {
			gift.transform.position = spawnEnemys [UnityEngine.Random.Range (0, 11)].transform.position;
			gift.SetActive (true);
		}
		yield return new WaitForSeconds (UnityEngine.Random.Range (10+level, 15+level));
		isSpawnGift = 0;
	}

	IEnumerator SpawnBomb(){
		
		GameObject obj = GetPooledObject (19);	
		if (obj != null) {
			isSpawnBoms = 1;
			obj.transform.position = new Vector3 (6, -1, 0);
			obj.SetActive (true);
			yield return new WaitForSeconds (UnityEngine.Random.Range (15,20));
			isSpawnBoms = 0;
		}
	}

	public bool IsHaveEnemy ()
	{
		GameObject[] enemys = GameObject.FindGameObjectsWithTag ("Enemy");
		if (enemys.Length > 0)
			return true;
		else
			return false;
	}
		
	public GameObject GetPooledObject (int choiceEnemy)
	{
		//get enemy
		if (choiceEnemy == 0) {
			for (int i = 0; i < enemys.Count; i++) {
				if (!enemys [i].activeInHierarchy)
					return enemys [i];
			}
		
		} else if (choiceEnemy == 1) {
			for (int i = 0; i < enemys2.Count; i++) {
				if (!enemys2 [i].activeInHierarchy)
					return enemys2 [i];
			}

		
		} else if (choiceEnemy == 2) {
			for (int i = 0; i < enemys3.Count; i++) {
				if (!enemys3 [i].activeInHierarchy)
					return enemys3 [i];
			}
		
		} else if (choiceEnemy == 3) {
			for (int i = 0; i < enemys4.Count; i++) {
				if (!enemys4 [i].activeInHierarchy)
					return enemys4 [i];
			}

		} else if (choiceEnemy == 4) {
			for (int i = 0; i < enemys5.Count; i++) {
				if (!enemys5 [i].activeInHierarchy)
					return enemys5 [i];
			}

		} else if (choiceEnemy == 5) {
			for (int i = 0; i < enemys6.Count; i++) {
				if (!enemys6 [i].activeInHierarchy)
					return enemys6 [i];
			}

		} else if (choiceEnemy == 6) {
			for (int i = 0; i < enemys7.Count; i++) {
				if (!enemys7 [i].activeInHierarchy)
					return enemys7 [i];
			}
		} else if(choiceEnemy==9){
			for (int i = 0; i < gunEnemys.Count; i++) {
				if (!gunEnemys [i].activeInHierarchy)
					return gunEnemys [i];
			}
			//get effect of player when hit to enemy
		} else if (choiceEnemy == 10) {
			for (int i = 0; i < efects.Count; i++) {
				if (!efects [i].activeInHierarchy)
					return efects [i];
			}
			//get coin
		} else if (choiceEnemy == 11) {
			for (int i = 0; i < coins.Count; i++) {
				if (!coins [i].activeInHierarchy)
					return coins [i];
			}
		}
		//get dollar
		else if (choiceEnemy == 12) {
			for (int i = 0; i < dollars.Count; i++) {
				if (!dollars [i].activeInHierarchy)
					return dollars [i];
			}
		}
		//get effect hit to water
		else if (choiceEnemy == 13) {
			for (int i = 0; i < hitWaters.Count; i++) {
				if (!hitWaters [i].activeInHierarchy)
					return hitWaters [i];
			}
		} else if (choiceEnemy == 14) {
			for (int i = 0; i < hitExplosionWaters.Count; i++) {
				if (!hitExplosionWaters [i].activeInHierarchy)
					return hitExplosionWaters [i];
			}
		} else if (choiceEnemy == 15) {
			for (int i = 0; i < collectedCoinEffects.Count; i++) {
				if (!collectedCoinEffects [i].activeInHierarchy)
					return collectedCoinEffects [i];
			}
		} else if (choiceEnemy == 16) {
			for (int i = 0; i < bloods.Count; i++) {
				if (!bloods [i].activeInHierarchy)
					return bloods [i];
			}
		} else if (choiceEnemy == 17) {
			for (int i = 0; i < submarineEnemys.Count; i++) {
				if (!submarineEnemys [i].activeInHierarchy)
					return submarineEnemys [i];
			}
		} else if (choiceEnemy == 18) {
			for (int i = 0; i < gunAirs.Count; i++) {
				if (!gunAirs [i].activeInHierarchy)
					return gunAirs [i];
			}
		}else if (choiceEnemy == 19) {
			for (int i = 0; i < bombs.Count; i++) {
				if (!bombs [i].activeInHierarchy)
					return bombs [i];
			}
		}
		return null;
	}

	public void CreateObjectPooling ()
	{
		enemys = new List<GameObject> ();
		enemys2 = new List<GameObject> ();
		enemys3 = new List<GameObject> ();
		enemys4 = new List<GameObject> ();
		enemys5 = new List<GameObject> ();
		enemys6 = new List<GameObject> ();
		enemys7 = new List<GameObject> ();
		efects = new List<GameObject> ();

		bombs = new List<GameObject> ();
		gunEnemys = new List<GameObject> ();
		gunAirs = new List<GameObject> ();
		coins = new List<GameObject> ();
		dollars = new List<GameObject> ();
		submarineEnemys = new List<GameObject> ();
		hitWaters = new List<GameObject> ();
		hitExplosionWaters = new List<GameObject> ();
		collectedCoinEffects = new List<GameObject> ();
		bloods = new List<GameObject> ();
		for (int i = 0; i < 5; i++) {
			GameObject obj = Instantiate (enemy);
			obj.SetActive (false);
			obj.transform.parent = parent.transform;
			enemys.Add (obj);
			GameObject obj2 = Instantiate (enemy2);
			obj2.SetActive (false);
			obj2.transform.parent = parent.transform;
			enemys2.Add (obj2);
			GameObject obj3 = Instantiate (enemy3);
			obj3.SetActive (false);
			obj3.transform.parent = parent.transform;
			enemys3.Add (obj3);
			GameObject obj4 = Instantiate (enemy4);
			obj4.SetActive (false);
			obj4.transform.parent = parent.transform;
			enemys4.Add (obj4);
			GameObject obj5 = Instantiate (enemy5);
			obj5.SetActive (false);
			obj5.transform.parent = parent.transform;
			enemys5.Add (obj5);
			GameObject obj6 = Instantiate (enemy6);
			obj6.SetActive (false);
			obj6.transform.parent = parent.transform;
			enemys6.Add (obj6);

			GameObject obj7 = Instantiate (enemy7);
			obj7.SetActive (false);
			obj7.transform.parent = parent.transform;
			enemys7.Add (obj7);

			GameObject obj8 = Instantiate (dollar);
			obj8.SetActive (false);
			obj8.transform.parent = parent.transform;
			dollars.Add (obj8);
		
			GameObject obj9 = Instantiate (hitExplosionWater);
			obj9.SetActive (false);
			obj9.transform.parent = parent.transform;
			hitExplosionWaters.Add (obj9);

			GameObject obj10 = Instantiate (blood);
			obj10.SetActive (false);
			obj10.transform.parent = parent.transform;
			bloods.Add (obj10);

			GameObject obj11 = Instantiate (submarineEnemy);
			obj11.SetActive (false);
			obj11.transform.parent = parent.transform;
			submarineEnemys.Add (obj11);

			GameObject obj12 = Instantiate (bomb);
			obj12.SetActive (false);
			obj12.transform.parent = parent.transform;
			bombs.Add (obj12);
		}

		for (int i = 0; i < 20; i++) {
			GameObject obj = Instantiate (gunEnemy);
			obj.SetActive (false);
			obj.transform.parent = parent.transform;
			gunEnemys.Add (obj);

			GameObject obj1 = Instantiate (effectShootPlayer);
			obj1.SetActive (false);
			obj1.transform.parent = parent.transform;
			efects.Add (obj1);

			GameObject obj3 = Instantiate (hitWater);
			obj3.SetActive (false);
			obj3.transform.parent = parent.transform;
			hitWaters.Add (obj3);

			GameObject obj4 = Instantiate (collectedCoinEffect);
			obj4.SetActive (false);
			obj4.transform.parent = parent.transform;
			collectedCoinEffects.Add (obj4);

			GameObject obj5 = Instantiate (gunAir);
			obj5.SetActive (false);
			obj5.transform.parent = parent.transform;
			gunAirs.Add (obj5);
		}
		for (int i = 0; i < 30; i++) {
			GameObject obj = Instantiate (coin);
			obj.SetActive (false);
			obj.transform.parent = parent.transform;
			coins.Add (obj);
		}
	
	}




	public void LoadDataFromLocal (int currentLevel)
	{
		levelLoaded = false;
		TextAsset mapText = Resources.Load ("Ramboat2DLevels/" + currentLevel) as TextAsset;
		if (mapText == null) {
			mapText = Resources.Load ("Ramboat2DLevels/" + currentLevel) as TextAsset;
		}
		ProcessGameDataFromString (mapText.text);
	}

	void ProcessGameDataFromString (string mapText)
	{
		string[] lines = mapText.Split (new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

		foreach (string line in lines) {
			if (line.StartsWith ("ENEMY")) {
				string enemyNormal = line.Replace ("ENEMY", string.Empty).Trim ();
				string[] enemysData = enemyNormal.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				timeEnemyToRespawn = float.Parse (enemysData [0]);
				timeEnemyToShoot = float.Parse (enemysData [1]);
			} else if (line.StartsWith ("FLYENEMY")) {
				string enemyFly = line.Replace ("FLYENEMY", string.Empty).Trim ();
				string[] enemyFlyData = enemyFly.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isSpawnEnemyFly = int.Parse (enemyFlyData [0]);
				timeEnemyFlyRespawn = float.Parse (enemyFlyData [1]);
				timeEnenmyFlyShoot = float.Parse (enemyFlyData [2]);
				//healthEnemyFly= float.Parse (enemyFlyData [3]);
			} else if (line.StartsWith ("BOAT")) {
				string boat = line.Replace ("BOAT", string.Empty).Trim ();
				string[] boatData = boat.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isSpawnBoat = int.Parse (boatData [0]);
				timeBoatRespawn = float.Parse (boatData [1]);
				healthBoat = float.Parse (boatData [2]);
			} else if (line.StartsWith ("AIR")) {
				string air = line.Replace ("AIR", string.Empty).Trim ();
				string[] airData = air.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isSpawnAir = int.Parse (airData [0]);
				timeAirRespawn = float.Parse (airData [1]);
				timeAirShoot = float.Parse (airData [2]);
				healthAir = float.Parse (airData [3]);
			} else if (line.StartsWith ("FOLLOWAIR")) {
				string airFollow = line.Replace ("FOLLOWAIR", string.Empty).Trim ();
				string[] airFollowData = airFollow.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isSpawnAirFollow = int.Parse (airFollowData [0]);
				timeAirFollowRespawn = float.Parse (airFollowData [1]);
				timeAirFollowShoot = float.Parse (airFollowData [2]);
				countAirFollowShoot = int.Parse (airFollowData [3]);
				healthAirFollow = float.Parse (airFollowData [4]);
			} else if (line.StartsWith ("SUBMARINE")) {
				string subMarine = line.Replace ("SUBMARINE", string.Empty).Trim ();
				string[] subMarineData = subMarine.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				isSpawnSubMarine = int.Parse (subMarineData [0]);
				timeSubMarineRespawn = float.Parse (subMarineData [1]);
				numberSpawnOneWay = int.Parse (subMarineData [2]);
			}
			levelLoaded = true;
		}

	}

	public void RespawnEnemy ()
	{
		if (isSpawnEnemy == 0) {
			int choiceEnemySpawn = UnityEngine.Random.Range (0, 5);
			StartCoroutine (SpawnEnemy (choiceEnemySpawn));
		}
		if (isSpawnEnemyFly == 0) {
			int choiceEnemySpawn = UnityEngine.Random.Range (6, 7);
			StartCoroutine (SpawnEnemyFly (choiceEnemySpawn));
		}
		if (isSpawnBoat == 0) {
			StartCoroutine (SpawnBoat ());
		}
		if (isSpawnAir == 0) {
			StartCoroutine (SpawnAir ());
		}
		if (isSpawnSubMarine == 0 && timePlayingGame > 10) {
			StartCoroutine (SpawnSubMarine (UnityEngine.Random.Range (1, numberSpawnOneWay + 1)));
		}
		if (isSpawnAirFollow == 0 && timePlayingGame > 15) {
			StartCoroutine (SpawnAirFollow ());
		}
		if (isSpawnGift == 0 && timePlayingGame > 25f) {
			StartCoroutine (SpawnGift ());
		}
		if(isSpawnBoms==0 && timePlayingGame > 30f){
			StartCoroutine(SpawnBomb());
		}
	}


	public void ShowLifeCurrent(int lifeCurrent){
		if (lifeCurrent == 3) {
			for(int i=0;i<3;i++){
				GameObject.Find ("Life").transform.GetChild (i + 3).gameObject.SetActive (true);
			}
		}else if(lifeCurrent<3 && lifeCurrent>=0){
			for(int i=lifeCurrent;i<3;i++){
				GameObject.Find ("Life").transform.GetChild (i + 3).gameObject.SetActive (false);
			}
		}
	}
	public void TouchPlayGame(){
		touchPlay = true;
	}
}
