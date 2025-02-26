using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WGM;

public class Ramboat2DPlayerController : MonoBehaviour
{
	public static Ramboat2DPlayerController Intance;
	private float scorePointFloat;
	private int scoreIntOld,scoreIntNew;
	public bool playerDead = false;
	public Rigidbody2D player;
	public Vector2 startPos;
	public Vector2 direction;
	public bool directionChosen = true;
	public float xBegan;
	public float xStationary;
	public Vector3 startAngle;
	public float speedRotate;
	private float startTime = 0;
	public bool canFly = true;
	public bool canSwim = true;
	private bool stationary = false;
	bool haveEnemy = false;
	private Animator anim;
	public LayerMask enemyLayerMask;
	public GameObject pointCheck;
	//	public
	[HideInInspector]
	public GameObject playerObj;
	[HideInInspector]
	public GameObject boatObj;
	public GameObject topWater;
	[HideInInspector]
	public GameObject flashShoot;
	public bool effectAfterSwim = true;
	public GameObject bullet, sixBarreledBullet, rocketBullet, threeLineBullet, fireBullet;
	List<GameObject> bullets, sixBarreledBullets, rocketBullets, threeLineBullets, fireBullets;
	public bool isShoot = false;
	// set up gun player
	[HideInInspector]
	public GunType gunType;
	public float gunPower;
	public float gunRate;
	public float gunAmmo, gunAmmoCurrent;
	public List<Sprite> flashShootSprite;
	Slider gunAmmoSlider;
	public Text textCoinCollect;
	public Text scorePlayer;
	[HideInInspector]
	public bool loadScene = false;
	[HideInInspector]
	public bool secondChange;
	public Sprite boatDestroyed;
	public Sprite boatNormal;
	public int liveCurrent;
	public int numberClickReload = 1;
	[HideInInspector]
	public GameObject waterEffect;
	[HideInInspector]
	public GameObject waterEffectJump;
	[HideInInspector]
	public GameObject waterEffectJump2;
	bool slowMotion = true;
	int oldState;
	int currentState;
	[HideInInspector] 
	public bool allowShoot = false;
	[HideInInspector]
	public bool setUpGunUITime;

	public GameObject FlyAndSwimWater;
	
	void Awake ()
	{
		enemyLayerMask = 256;
		Intance = this;
		oldState = -1;
		currentState = -1;
		gunType = GunType.NormalGun;
		liveCurrent = 3;
		for (int i = 0; i < 7; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
		for (int i = 7; i < 13; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
	}

	void Start ()
	{
		AudioManager.Instance.playerBGm(Ramboat2DFXSound.THIS.music [1]);

	
		
		playerObj = transform.GetChild (Ramboat2DLevelManager.THIS.playerNumber).gameObject;//激活角色 PlayerPrefs.GetInt("ChoosePlayer")
		playerObj.SetActive (true);
		ReadWriteTextMission.THIS.CheckMission (4);
//		Debug.Log(
		boatObj = transform.GetChild (Random.Range(7,13)).gameObject;//激活坐骑  PlayerPrefs.GetInt("ChooseBoat")+7  
		boatObj.SetActive (true);
		waterEffect = boatObj.transform.GetChild (0).gameObject;
		waterEffectJump = boatObj.transform.GetChild (1).gameObject;
		waterEffectJump2 = boatObj.transform.GetChild (2).gameObject;
	

		anim = playerObj.GetComponentInChildren<Animator> ();
		player = gameObject.GetComponent<Rigidbody2D> ();
		startAngle = transform.rotation.eulerAngles;
		startAngle.z = 90;

		//get slider in canvas
		gunAmmoSlider = GameObject.Find ("MenuPlayingGame").GetComponentInChildren<Slider>();

		textCoinCollect.text = "0";
		bullets = new List<GameObject> ();
		sixBarreledBullets = new List<GameObject> ();
		rocketBullets = new List<GameObject> ();
		threeLineBullets = new List<GameObject> ();
		fireBullets = new List<GameObject> ();
		for (int i = 0; i < 10; i++) {
			GameObject obj = Instantiate (bullet);
			obj.gameObject.SetActive (false);
			bullets.Add (obj);

			GameObject obj1 = Instantiate (sixBarreledBullet);
			obj1.gameObject.SetActive (false);
			sixBarreledBullets.Add (obj1);

			GameObject obj2 = Instantiate (rocketBullet);
			obj2.gameObject.SetActive (false);
			rocketBullets.Add (obj2);

			GameObject obj3 = Instantiate (threeLineBullet);
			obj3.gameObject.SetActive (false);
			threeLineBullets.Add (obj3);

			GameObject obj4 = Instantiate (fireBullet);
			obj4.gameObject.SetActive (false);
			fireBullets.Add (obj4);
		}
		flashShoot = playerObj.transform.GetChild (0).gameObject;
		SetUpGun (gunType);
		//check gun upgrade
		if (PlayerPrefs.GetInt ("ChoosePlayer")==0 && (PlayerPrefs.GetFloat ("Star0") != 0 || PlayerPrefs.GetFloat ("Star1") != 0 || PlayerPrefs.GetFloat ("Star2") != 0 || PlayerPrefs.GetFloat ("Star3") != 0)) {
			ReadWriteTextMission.THIS.CheckMission (31);
		} else if(PlayerPrefs.GetInt ("ChoosePlayer")==1 && (PlayerPrefs.GetFloat ("Star0") != 0 || PlayerPrefs.GetFloat ("Star1") != 0 || PlayerPrefs.GetFloat ("Star2") != 0 || PlayerPrefs.GetFloat ("Star4") != 0)){
			ReadWriteTextMission.THIS.CheckMission (31);
		}//more check upgradu gun here
	}

	void Update ()
	{
		if (Ramboat2DLevelManager.THIS.startingGame && !playerDead) {
			MovePlayer ();
			RotateFollowVelocity ();
			CheckAngleToFire ();
		} else if (playerDead && !loadScene) {
			PlayerDeath ();
		}

	}

public void MovePlayer()
{
    topWater.SetActive(false);
    FlyAndSwimWater.SetActive(false);
    // 处理键盘输入
    if (!setUpGunUITime && Time.timeScale!=0)
    {
	    scoreIntOld = scoreIntNew;
	    scorePointFloat += ((player.transform.position.x + 6) / 24);
	    scoreIntNew = (int)scorePointFloat;
	    scorePlayer.text = scoreIntNew.ToString();
	    // 检测不同的按键操作
#if UNITY_EDITOR
	    if (Input.GetKey(KeyCode.W))
		    
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)0))
#endif        
        {
            if (canFly && canSwim)
            {
                player.velocity = Vector3.zero;
                player.velocity = new Vector2(1f, 7f);
                Ramboat2DFXSound.THIS.fxSound.PlayOneShot(Ramboat2DFXSound.THIS.boatJump);
                ReadWriteTextMission.THIS.CheckMission(12);
            }
        }
#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.S))
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)2))
#endif    
        {
            if (canSwim && canFly && effectAfterSwim)
            {
                flashShoot.SetActive(false);
                player.velocity = Vector3.zero;
                effectAfterSwim = false;
                player.velocity = new Vector2(1f, -7f);
                Ramboat2DFXSound.THIS.fxSound.PlayOneShot(Ramboat2DFXSound.THIS.boatDive);
                ReadWriteTextMission.THIS.CheckMission(13);
            }
        }
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.D))
#else
		if(DealCommand.GetKey(1,(AppKeyCode)1))
#endif  
        {
            if ( transform.position.x < 4f)//canFly && canSwim &&
            {
	            if (canFly && canSwim)
		            player.AddForce(new Vector3(xStationary, 0, 0), ForceMode2D.Impulse);
	            else
		            player.AddForce(new Vector3(xStationary *0.4f, 0, 0), ForceMode2D.Impulse);
                Vector3 angle = new Vector3(0, 0, 95);
                StartCoroutine(RotatePlayer(speedRotate, 2f, angle));
            }
        }
#if UNITY_EDITOR
	    if(Input.GetKey(KeyCode.A))
#else
		if(DealCommand.GetKey(1,(AppKeyCode)6))
#endif         
        {
	        if(canFly || canSwim)
		        FlyAndSwimWater.SetActive(true);
	        else
				topWater.SetActive(true);
	        if (transform.position.x < -4.5f && !Ramboat2DPlayerController.Intance.playerDead)//canSwim && canFly
	        {
		        StartCoroutine(WavePlayer());
		        Ramboat2DFXSound.THIS.fxSound.PlayOneShot(Ramboat2DFXSound.THIS.boatSplash);
	        }
        }
    }

    

}

	public void AddScore(int score)
	{
		scorePointFloat += score;
	}
	public IEnumerator RotatePlayer (float speedAngle, float distCovered, Vector3 endAngleRotation)
	{
		float startTime = Time.time;
		float dist = 0;
		while (dist < distCovered) {
			dist = (Time.time - startTime) * speedAngle;
			transform.eulerAngles = Vector3.Lerp (transform.rotation.eulerAngles, endAngleRotation, dist / distCovered);
			yield return new WaitForFixedUpdate ();
		}
	}

	public IEnumerator CheckDirectionTouchMove (Vector3 beginMove)
	{
		yield return new WaitForSeconds (0.1f);
	}

	public IEnumerator WaitForSec (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
	}

	void RotateFollowVelocity ()
	{
		if (canFly && !canSwim)
			StartCoroutine (RotatePlayer (10f, 0.7f, transform.rotation.eulerAngles + new Vector3 (0, 0, player.velocity.y - 1)));
		if (!canFly && canSwim)
			StartCoroutine (RotatePlayer (10f, 0.7f, transform.rotation.eulerAngles + new Vector3 (0, 0, player.velocity.y + 1f)));
		if (canFly && canSwim && !stationary)
			StartCoroutine (RotatePlayer (10f, 0.7f, startAngle));
			
	}


	IEnumerator SlowMotion ()
	{
		Collider2D[] enemyCollider = Physics2D.OverlapCircleAll (pointCheck.transform.position, gunRate, enemyLayerMask);
		slowMotion = false;
		while (enemyCollider.Length > 0 && canFly && !canSwim) {
			enemyCollider = Physics2D.OverlapCircleAll (pointCheck.transform.position, gunRate, enemyLayerMask);
			Time.timeScale = 0.5f;
			yield return new WaitForFixedUpdate ();
		} 
		Time.timeScale = 1;
		yield return new WaitForSeconds (Random.Range (2, 5));
		slowMotion = true;
	}

	void CheckAngleToFire ()
	{
		float angle;
		bool isRight = true;
		Collider2D[] enemyCollider = Physics2D.OverlapCircleAll (pointCheck.transform.position, gunRate, enemyLayerMask);

		Collider2D enemy = MinDistance (enemyCollider);
		if (slowMotion && canFly && !canSwim) {
			
			StartCoroutine (SlowMotion ());
		}
		if (haveEnemy) {
			angle = Mathf.Abs (Vector3.Angle (-transform.right, enemy.transform.position - pointCheck.transform.position));
			if (canFly) {
				isRight = (pointCheck.transform.position.x < enemy.transform.position.x) ? true : false;
				if (!isRight) {
					
					Vector3 eulerAngle = playerObj.transform.localEulerAngles;
					eulerAngle.y = 180;
					eulerAngle.z = 90;
					playerObj.transform.localEulerAngles = eulerAngle;
				} else {
					
					Vector3 eulerAngle = playerObj.transform.localEulerAngles;
					eulerAngle.y = 0;
					eulerAngle.z = -90;
					playerObj.transform.localEulerAngles = eulerAngle;
				}
				if (angle >= 0 && angle < 95) {
					
					if (angle < 45) {
						if (angle < 5)
							anim.Play ("0_5R");
						if (angle >= 5 && angle < 15)
							anim.Play ("5_15R");
						if (angle >= 15 && angle < 25)
							anim.Play ("15_25R");
						if (angle >= 25 && angle < 35)
							anim.Play ("25_35R");
						if (angle >= 35)
							anim.Play ("35_45R");
					} else {
						if (angle < 55)
							anim.Play ("45_55R");
						if (angle >= 55 && angle < 65)
							anim.Play ("55_65R");
						if (angle >= 65 && angle < 75)
							anim.Play ("65_75R");
						if (angle >= 75 && angle < 85)
							anim.Play ("75_85R");
						if (angle >= 85)
							anim.Play ("85_95R");

					}

				} else {
					
					if (angle < 145) {
						if (angle < 105)
							anim.Play ("95_105R");
						if (angle >= 105 && angle < 115)
							anim.Play ("105_115R");
						if (angle >= 115 && angle < 125)
							anim.Play ("115_125R");
						if (angle >= 125 && angle < 135)
							anim.Play ("125_135R");
						if (angle >= 135)
							anim.Play ("135_145R");
				
					} else {
						if (angle < 155)
							anim.Play ("145_155R");
						if (angle >= 155 && angle < 165)
							anim.Play ("155_165R");
						if (angle >= 165 && angle < 175)
							anim.Play ("165_175R");
						if (angle >= 175)
							anim.Play ("175_180R");
					}
						
				}
				if (isShoot)
					StartCoroutine (ShootDelay (enemy, true));
			}

		} else {
			Vector3 eulerAngle = playerObj.transform.localEulerAngles;
			eulerAngle.y = 0;
			eulerAngle.z = -90;
			playerObj.transform.localEulerAngles = eulerAngle;
			anim.Play ("Idle");
		}

	}

	Collider2D MinDistance (Collider2D[] enemies)
	{
		Collider2D enemy = null;
		int i = 0;
		if (enemies.Length == 0) {
			haveEnemy = false;
			return enemy;
		} else if (enemies.Length == 1) {
			haveEnemy = true;
			enemy = enemies [0];
			return enemy;
		} else {
			enemy = enemies [0];
			while (i <= enemies.Length - 2) {
				if (Vector3.Distance (enemy.transform.position, pointCheck.transform.position) > Vector3.Distance (enemies [i + 1].transform.position, pointCheck.transform.position))
					enemy = enemies [i + 1];
				i++;
			}
			haveEnemy = true;
		}
		return enemy;
	}

	IEnumerator WavePlayer ()
	{
		yield return new WaitForSeconds (0.5f);
		transform.position = new Vector2 (transform.position.x, transform.position.y + Mathf.Sin (Time.time * 2) * 0.01f);
	}


	GameObject GetBulletPooling (GunType gunType)
	{
		if (gunType == GunType.NormalGun) {
			for (int i = 0; i < bullets.Count; i++) {
				if (!bullets [i].gameObject.activeInHierarchy) {
					return bullets [i];
				}
			}
		} else if (gunType == GunType.SixBarreled) {
			for (int i = 0; i < sixBarreledBullets.Count; i++) {
				if (!sixBarreledBullets [i].gameObject.activeInHierarchy) {
					return sixBarreledBullets [i];
				}
			}
		} else if (gunType == GunType.Rocket) {
			for (int i = 0; i < rocketBullets.Count; i++) {
				if (!rocketBullets [i].gameObject.activeInHierarchy) {
					return rocketBullets [i];
				}
			}
		} else if (gunType == GunType.ThreeLineGun) {
			for (int i = 0; i < threeLineBullets.Count; i++) {
				if (!threeLineBullets [i].gameObject.activeInHierarchy) {
					return threeLineBullets [i];
				}
			}
		} else if (gunType == GunType.FireGun) {
			for (int i = 0; i < fireBullets.Count; i++) {
				if (!fireBullets [i].gameObject.activeInHierarchy) {
					return fireBullets [i];
				}
			}
		}
		return null;
	}

	public IEnumerator ShootDelay (Collider2D enemy, bool orderInLayer)
	{
		isShoot = false;
		gunAmmoCurrent -= 1;
		// caculator gun ammo
		if (gunType != GunType.NormalGun)
			CaculatorValueSliderBullet (gunAmmoCurrent);
		float anglefire = 0;
		if (enemy != null)
			anglefire = Mathf.Abs (Vector3.Angle (Vector3.down, enemy.transform.position - pointCheck.transform.position));
		ShootEnemy (enemy, orderInLayer, anglefire, gunType);

		yield return new WaitForSeconds (0.05f);
		if (gunAmmoCurrent <= 0) {
			gunType = GunType.NormalGun;
			SetUpGun (gunType);
		}
				
			

	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "GunBulletEnemy") {
			other.gameObject.SetActive (false);
		}
		if (other.tag == "GunType") {
			StartCoroutine (ConvertGun (other));

		}
		if (other.tag == "Pocker") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.pokerChip);
			ReadWriteTextMission.THIS.CheckMission (33);
			StartCoroutine (CollectPocker (other.gameObject));
		}
		if (other.tag == "Clover") {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			StartCoroutine (RandomMoreCoin (other.gameObject));
		}
	}

	IEnumerator RandomMoreCoin (GameObject other)
	{
		Destroy (other);
		GameObject goodLuck = GameObject.Find ("Ramboat2DCanvas").transform.GetChild (6).gameObject;
		goodLuck.SetActive (true);
		yield return new WaitForSeconds (1f);
		goodLuck.SetActive (false);
		float i = 0;
		while (i < 15) {
			i++;
			GameObject coin = Ramboat2DLevelManager.THIS.GetPooledObject (11);
			GameObject obj2 = Ramboat2DLevelManager.THIS.GetPooledObject (15);
			if (coin != null) {
				coin.transform.position = new Vector3 (Random.Range (-5f, 5f), Random.Range (3f, 1f), 0);
				obj2.transform.position = coin.transform.position;
				coin.SetActive (true);
				obj2.SetActive (true);
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bling);
				coin.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-2, 2), Random.Range (6, 12), 0);
			}
			yield return new WaitForSeconds (0.2f);
			obj2.SetActive (false);
			yield return new WaitForSeconds (0.3f);
		}

		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}

	IEnumerator CollectPocker (GameObject other)
	{
		for (int i = 0; i < 10; i++) {
			ReadWriteTextMission.THIS.CheckMission (5);
			GameObject obj = Ramboat2DLevelManager.THIS.GetPooledObject (11);
			if (obj != null) {
				obj.transform.position=new Vector3(-1+other.transform.position.x+(i%5)/2f,other.transform.position.y-(i%2)/3f,0);
				obj.GetComponent<Ramboat2DCoinController> ().moveSpecial = true;
				obj.SetActive (true);

			}
		}
		yield return null;
		Destroy (other);
		
		// PlayerPrefs.SetInt ("Pocker", PlayerPrefs.GetInt ("Pocker") + 1);
		// GameObject obj = Instantiate (Resources.Load ("Prefabs/Gift/PockerAnim"), other.transform.position, Quaternion.identity) as GameObject;
		// yield return new WaitForSeconds (0.5f);
		// Destroy (obj);
		
		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}
	/// <summary>
	/// 更新武器数据
	/// </summary>
	/// <param name="gunType"></param>
	public void SetUpGun (GunType gunType)
	{
		gunAmmoSlider.value = 13;

		if (gunType == GunType.NormalGun)
		{
			gunPower = GunData.THIS.gunPower[4];
			gunRate = GunData.THIS.gunRate [4];
			gunAmmo = GunData.THIS.gunAmmo[4];
			// gunPower = PlayerPrefs.GetFloat("Power0");
			// gunRate = PlayerPrefs.GetFloat ("Rate0");
			// gunAmmo = PlayerPrefs.GetFloat ("Ammo0");
			gunAmmoCurrent = gunAmmo;
			flashShoot.GetComponent<SpriteRenderer> ().sprite = flashShootSprite [0];
			GameObject.Find ("MenuPlayingGame").transform.GetChild (2).gameObject.GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [0];
			anim.speed = 1f;
		} else {
			for (int i = 0; i < 4; i++) {
				if (GunData.THIS.gunType [i] == (int)gunType) {
					gunPower = GunData.THIS.gunPower[i];
					gunRate = GunData.THIS.gunRate [i];
					gunAmmo = GunData.THIS.gunAmmo[i];
					gunAmmoCurrent = gunAmmo;
				}
			}
			if (gunType == GunType.SixBarreled) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.setupGun[0]);
				flashShoot.GetComponent<SpriteRenderer> ().sprite = flashShootSprite [1];
				anim.speed = 1.2f;
			}
			else if (gunType == GunType.Rocket) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.setupGun[1]);
				flashShoot.GetComponent<SpriteRenderer> ().sprite = flashShootSprite [2];
				anim.speed = 1f;
			}
			else if (gunType == GunType.ThreeLineGun) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.setupGun[2]);
				flashShoot.GetComponent<SpriteRenderer> ().sprite = flashShootSprite [3];
				anim.speed = 1.1f;
			}
			else if (gunType == GunType.FireGun) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.setupGun[3]);
				flashShoot.GetComponent<SpriteRenderer> ().sprite = flashShootSprite [4];
				anim.speed = 1f;
			}
		}

	}

	// move gift corountine
	IEnumerator ConvertGun (Collider2D other)
	{
		yield return new WaitForSeconds (0.2f);
		gunType = (GunType)other.gameObject.GetComponent<ItemGiftMove> ().gunType;
		SetUpGun (gunType);
	}


	void ShootEnemy (Collider2D enemy, bool orderInLayer, float anglefire, GunType gunType)
	{
		
		if (gunType == GunType.NormalGun) {
			GameObject obj = GetBulletPooling (gunType);
			Vector3 pointShowEffectShot = (enemy.transform.position - pointCheck.transform.position) / (Vector3.Distance (enemy.transform.position, pointCheck.transform.position)) / 2.5f;
			if (obj != null) {
				
				if (enemy.transform.position.x > pointCheck.transform.position.x)
					obj.transform.localEulerAngles = new Vector3 (0, 0, anglefire);
				else
					obj.transform.localEulerAngles = new Vector3 (0, 0, -anglefire);
				obj.transform.position = flashShoot.transform.position;
				obj.gameObject.SetActive (true);
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.pistol[Random.Range(0,3)]);
				obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 40;
			}

		} else if (gunType == GunType.SixBarreled) {
			GameObject obj = GetBulletPooling (gunType);
			Vector3 pointShowEffectShot = (enemy.transform.position - pointCheck.transform.position) / (Vector3.Distance (enemy.transform.position, pointCheck.transform.position)) / 2.5f;
			if (obj != null) {
				
				if (enemy.transform.position.x > pointCheck.transform.position.x)
					obj.transform.localEulerAngles = new Vector3 (0, 0, anglefire);
				else
					obj.transform.localEulerAngles = new Vector3 (0, 0, -anglefire);
				obj.transform.position = flashShoot.transform.position;
				obj.gameObject.SetActive (true);
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.heavyMachineGun[Random.Range(0,3)]);
				obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 50;
			}

		} else if (gunType == GunType.Rocket) {
			GameObject obj = GetBulletPooling (gunType);
			Vector3 pointShowEffectShot = (enemy.transform.position - pointCheck.transform.position) / (Vector3.Distance (enemy.transform.position, pointCheck.transform.position)) / 2.5f;
			if (obj != null) {
				
				if (enemy.transform.position.x > pointCheck.transform.position.x)
					obj.transform.localEulerAngles = new Vector3 (0, 0, anglefire);
				else
					obj.transform.localEulerAngles = new Vector3 (0, 0, -anglefire);
				obj.transform.position = flashShoot.transform.position;
				obj.gameObject.SetActive (true);
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.rocket[Random.Range(0,3)]);
				obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 30;
			}

		} else if (gunType == GunType.ThreeLineGun) {
			Vector3 pointShowEffectShot = (enemy.transform.position - pointCheck.transform.position) / (Vector3.Distance (enemy.transform.position, pointCheck.transform.position)) / 2.5f;
			for (int i = 0; i < 3; i++) {
				GameObject obj = GetBulletPooling (gunType);
				if (obj != null) {
					
					if (enemy.transform.position.x > pointCheck.transform.position.x)
						obj.transform.localEulerAngles = new Vector3 (0, 0, anglefire);
					else
						obj.transform.localEulerAngles = new Vector3 (0, 0, -anglefire);
					obj.transform.position = flashShoot.transform.position;
					obj.gameObject.SetActive (true);
					Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.shotGun[Random.Range(0,3)]);
					if (i == 0)
						obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 30 + new Vector3 (1, 0, 0);
					if (i == 1)
						obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 30;
					if (i == 2)
						obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 30 + new Vector3 (-1, 0, 0);

				}
			}
		} else if (gunType == GunType.FireGun) {
			GameObject obj = GetBulletPooling (gunType);
			Vector3 pointShowEffectShot = (enemy.transform.position - pointCheck.transform.position) / (Vector3.Distance (enemy.transform.position, pointCheck.transform.position)) / 2.5f;
			if (obj != null) {
				
				if (enemy.transform.position.x > pointCheck.transform.position.x)
					obj.transform.localEulerAngles = new Vector3 (0, 0, anglefire - 90);
				else
					obj.transform.localEulerAngles = new Vector3 (0, 0, -anglefire - 90);
				obj.transform.position = flashShoot.transform.position;
				obj.gameObject.SetActive (true);
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.flameShoot[Random.Range(0,3)]);
				obj.GetComponent<Rigidbody2D> ().velocity = (pointShowEffectShot) * 5;
			}

		}
	}


	void CaculatorValueSliderBullet (float gunAmmoCurrent)
	{
		if (gunAmmoCurrent > (gunAmmo * 12f / 13f) && gunAmmoCurrent <= gunAmmo)
			gunAmmoSlider.value = 13;
		else if (gunAmmoCurrent > (gunAmmo * 11f / 13f) && gunAmmoCurrent <= (gunAmmo * 12f / 13f))
			gunAmmoSlider.value = 12;
		else if (gunAmmoCurrent > (gunAmmo * 10f / 13f) && gunAmmoCurrent <= (gunAmmo * 11f / 13f))
			gunAmmoSlider.value = 11;
		else if (gunAmmoCurrent > (gunAmmo * 9f / 13f) && gunAmmoCurrent <= (gunAmmo * 10f / 13f))
			gunAmmoSlider.value = 10;
		else if (gunAmmoCurrent > (gunAmmo * 8f / 13f) && gunAmmoCurrent <= (gunAmmo * 9f / 13f))
			gunAmmoSlider.value = 9;
		else if (gunAmmoCurrent > (gunAmmo * 7f / 13f) && gunAmmoCurrent <= (gunAmmo * 8f / 13f))
			gunAmmoSlider.value = 8;
		else if (gunAmmoCurrent > (gunAmmo * 6f / 13f) && gunAmmoCurrent <= (gunAmmo * 7f / 13f))
			gunAmmoSlider.value = 7;
		else if (gunAmmoCurrent > (gunAmmo * 5f / 13f) && gunAmmoCurrent <= (gunAmmo * 6f / 13f))
			gunAmmoSlider.value = 6;
		else if (gunAmmoCurrent > (gunAmmo * 4f / 13f) && gunAmmoCurrent <= (gunAmmo * 5f / 13f))
			gunAmmoSlider.value = 5;
		else if (gunAmmoCurrent > (gunAmmo * 3f / 13f) && gunAmmoCurrent <= (gunAmmo * 4f / 13f))
			gunAmmoSlider.value = 4;
		else if (gunAmmoCurrent > (gunAmmo * 2f / 13f) && gunAmmoCurrent <= (gunAmmo * 3f / 13f))
			gunAmmoSlider.value = 3;
		else if (gunAmmoCurrent > (gunAmmo * 1f / 13f) && gunAmmoCurrent <= (gunAmmo * 2f / 13f))
			gunAmmoSlider.value = 2;
		else if (gunAmmoCurrent > 0 && gunAmmoCurrent <= (gunAmmo / 13f))
			gunAmmoSlider.value = 1;
		else
			gunAmmoSlider.value = 0;
	}


	void PlayerDeath ()
	{
		loadScene = true;
		topWater.SetActive (true);
		StartCoroutine (RotatePlayer (4f, 0.7f, startAngle));
		anim.Play ("Death");
//		transform.GetComponent<SpriteRenderer> ().sprite = boatDestroyed;
		boatObj.transform.GetChild (3).gameObject.SetActive (false);
	
		Ramboat2DLevelManager.THIS.coinCollectedPassLevel = int.Parse (textCoinCollect.text) - Ramboat2DLevelManager.THIS.coinCollected;//未结算金币
		Ramboat2DLevelManager.THIS.coinCollected = int.Parse (textCoinCollect.text);//已结算金币
		// PlayerPrefs.SetFloat ("CoinCollected",	PlayerPrefs.GetFloat ("CoinCollected") + LevelManager.THIS.coinCollectedPassLevel);
		// PlayerPrefs.Save ();

		BuyLifeSetActive ();

	}
	/// <summary>
	/// 复活
	/// </summary>
	public void PlayerReload ()
	{
		float coin = int.Parse (textCoinCollect.text) - numberClickReload * 200;
		if (coin >= 0) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.buttonBuy);
			liveCurrent = 1;
			numberClickReload += 1;
			GameObject.Find ("Life").transform.GetChild (3).gameObject.SetActive (true);
			textCoinCollect.text = coin.ToString();
			//PlayerPrefs.SetFloat ("CoinCollected", coin);
			GameObject.Find("NumberDiamonHave").GetComponent<Text>().text = textCoinCollect.text;//PlayerPrefs.GetFloat ("CoinCollected").ToString ();
			boatObj.transform.GetChild (3).gameObject.SetActive (false);
			EnemyController[] enemys = GameObject.FindObjectsOfType<EnemyController> ();
			if (enemys != null) {
				for (int i = 0; i < enemys.Length; i++) {
					enemys [i].Death ();
				}
			}
			AirController[] enemys1 = GameObject.FindObjectsOfType<AirController> ();
			if (enemys1 != null) {
				for (int i = 0; i < enemys1.Length; i++) {
					enemys1 [i].Death ();
				}
			}
			BoatController[] enemys2 = GameObject.FindObjectsOfType<BoatController> ();
			if (enemys2 != null) {
				for (int i = 0; i < enemys2.Length; i++) {
					enemys2 [i].Death ();
				}
			}
			AirFollowController[] enemys3 = GameObject.FindObjectsOfType<AirFollowController> ();
			if (enemys3 != null) {
				for (int i = 0; i < enemys3.Length; i++) {
					enemys3 [i].Death ();
				}
			}
			GameObject[] bulletEnemy = GameObject.FindGameObjectsWithTag ("GunBulletEnemy");
			GameObject[] boms = GameObject.FindGameObjectsWithTag ("Bomb");
			GameObject[] bullet = GameObject.FindGameObjectsWithTag ("BulletEnemy");
			foreach (GameObject obj in bulletEnemy) {
				obj.SetActive (false);
			}
			foreach (GameObject bom in boms) {
				bom.SetActive (false);
			}
			foreach (GameObject bl in bullet) {
				Destroy (bl);
			}
			Ramboat2DLevelManager.THIS.ShowLifeCurrent (liveCurrent);
//			transform.GetComponent<SpriteRenderer> ().sprite = boatNormal;
			loadScene = false;
			playerDead = false;
			boatObj.transform.GetChild (3).gameObject.SetActive (false);
			anim.SetInteger ("SetState", -1);
			Resources.UnloadUnusedAssets ();
			System.GC.Collect ();
			PlayerPrefs.Save ();
		} else {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot(Ramboat2DFXSound.THIS.fail);
			StartCoroutine (ResetGame());
		}

	}

	public IEnumerator ResetGame()
	{
		yield return new WaitForSeconds(1f);
		CommonUI.instance.BackMainPanel_OPen();
		yield return new WaitForSeconds(0.75f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public IEnumerator LoadScene()
	{

		// Animator animConvert = GameObject.Find ("NextSceneCanvas").GetComponentInChildren<Animator> ();
		// //yield return new WaitForSeconds (1f);
		// animConvert.SetTrigger ("In");
		//
		// yield return new WaitForSeconds (1f);
		// //transform.GetChild (8).gameObject.SetActive (false);
		// playerDead = false;
		//
		// SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		// while (!SceneManager.GetActiveScene ().name.Equals("MainScene")) {
		// 	yield return new WaitForFixedUpdate ();
		// }
		// animConvert.SetTrigger ("Out");
		yield return null;
	}

	void BuyLifeSetActive ()
	{
		GameObject canvasBuyLife = GameObject.Find ("Ramboat2DCanvas").transform.GetChild (3).gameObject;
		canvasBuyLife.SetActive (true);
		//canvasBuyLife.transform.GetChild (3).transform.GetChild (3).GetComponent<Text> ().text = "100";
		GameObject.Find("NumberDiamonHave").GetComponent<Text>().text = textCoinCollect.text; //PlayerPrefs.GetFloat ("CoinCollected").ToString ();
	}
}