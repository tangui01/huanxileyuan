using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossPlane : MonoBehaviour {

	//public enum terrain {OnlyGround, Everything, OnlyWater, GroundToWater, WaterToGround};
	//public int[] AvailableTerrainTypes;

	// AVAILABLE TERRAIN TYPES:
	// EVERYTHING: -1
	// ONLY GROUND: 0
	// ONLY WATER: 2

	public int availableTerrainType;
	public float bgMovingSpeed;
	[HideInInspector] public int health = 400;
	public int bossLevel = 1;
	int levelOfDestruction = 1;

	public List<Transform> TurretsLevel1;
	public List<Transform> TurretsLevel2;
	public List<Transform> TurretsLevel3;

	[HideInInspector] public int turretsFirstWave = 0;
	[HideInInspector] public int turretsSecondWave = 0;
	[HideInInspector] public int turretsThirdWave = 0;
	[HideInInspector] public int currentWave = 0;
	[HideInInspector] public int leftWingCount = 0;
	[HideInInspector] public int rightWingCount = 0;

	public int smallWingTurretHealth = 0;
	public int tailGunHealth = 0;
	public int wingTurretSmallHealth = 0;
	public int wingTurretLargeHealth = 0;
	public int wingLaserGunSmallHealth = 0;
	public int wingLaserGunLargeHealth = 0;
	public int mainGunHealth = 0;
	int eventCounter = 0;

	[HideInInspector] public int cameraOffsetY;
	bool bossCanMove = false;

	Transform camera;
	float healthFillRate;
	int healthStart;
	GameObject healthBar;
	Text healthText;
	int factorX = -20;
	int factorY = 1300;
	Color[] bossColors = new Color[] {new Color(1,0.89f,0,1), new Color(0.86f,0.85f,0.78f,1), new Color(0.87f,0.12f,0,1), new Color(0.9f,0.71f,0.4f,1), new Color(0.1f,0.56f,0.56f,1), new Color(0.34f,0.4f,0.33f,1) };
	// BossLevel1: Gadjaju se oba turreta na malim krilima, dva najsira turreta na krilima i main gun
	// BossLevel2: Gadjaju se oba turreta na malim krilima + tail gun, dva najsira turreta na krilima + dva big laser gun-a i main gun
	// BossLevel3: Gadjaju se oba turreta na malim krilima + tail gun, sva 4 turreta na krilima + sva 4 laser gun-a i main gun

	void Start () 
	{
		if(LevelGenerator.currentStage > 9)
			ColorThePlane(bossColors[Random.Range(0,bossColors.Length)]);

		if(smallWingTurretHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				smallWingTurretHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				smallWingTurretHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(tailGunHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				tailGunHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				tailGunHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(wingTurretSmallHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				wingTurretSmallHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				wingTurretSmallHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(wingTurretLargeHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				wingTurretLargeHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				wingTurretLargeHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(wingLaserGunSmallHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				wingLaserGunSmallHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				wingLaserGunSmallHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(wingLaserGunLargeHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				wingLaserGunLargeHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				wingLaserGunLargeHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(mainGunHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				mainGunHealth += (int)(factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*1.5f*LevelGenerator.currentStage/9f));
			else
				mainGunHealth += (int)(factorX * PandaPlane.Instance.mainGunLvl + factorY*1.5f);//*PandaPlane.mainGunLvl;
		}

		health = smallWingTurretHealth*2 + tailGunHealth + wingTurretSmallHealth*2 + wingTurretLargeHealth*2 + wingLaserGunSmallHealth*2 + wingLaserGunLargeHealth*2 + mainGunHealth;
		healthStart = health;
		camera = Camera.main.transform;
		healthBar = GameObject.Find("BossHealthBar");
		healthText = GameObject.Find("BossHealthBar/HealthBarText").GetComponent<Text>();
		//StartCoroutine(AdjustCameraSpeedForBoss());
		//TurretsLevel1 = new List<Transform>();
		//TurretsLevel2 = new List<Transform>();
		//TurretsLevel3 = new List<Transform>();

		if(bossLevel == 1)
		{
			turretsFirstWave = 2;
			turretsSecondWave = 2;
			turretsThirdWave = 1;

//			for(int i=0;i<turretsFirstWave;i++)
//			{
//				TurretsLevel1[i].collider2D.enabled = true;
//				//TurretsLevel1[i].SendMessage("ShowTurret", SendMessageOptions.DontRequireReceiver);
//			}
		}
		else if(bossLevel == 2)
		{
			turretsFirstWave = 3;
			turretsSecondWave = 4;
			turretsThirdWave = 1;

//			for(int i=0;i<turretsFirstWave;i++)
//			{
//				TurretsLevel2[i].collider2D.enabled = true;
//				//TurretsLevel2[i].SendMessage("ShowTurret", SendMessageOptions.DontRequireReceiver);
//			}
		}
		else
		{
			turretsFirstWave = 3;
			turretsSecondWave = 8;
			turretsThirdWave = 1;

//			for(int i=0;i<turretsFirstWave;i++)
//			{
//				TurretsLevel3[i].collider2D.enabled = true;
//				//TurretsLevel3[i].SendMessage("ShowTurret", SendMessageOptions.DontRequireReceiver);
//			}
		}
		currentWave = turretsFirstWave;
		rightWingCount = leftWingCount = turretsSecondWave/2;
		SetHealthBar();
	}

	void ColorThePlane(Color color)
	{
		transform.Find("AnimationHolder/PlaneBodyHolder/PlaneBody").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneBodyHolder/PlaneBodyDamaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/PlaneWingLeftHolder/PlaneWing").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingLeftHolder/PlaneWingDamaged").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingLeftHolder/PlaneEngineLaserGunBigLeft/AnimationHolder/Normal/PlaneEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingLeftHolder/PlaneEngineLaserGunBigLeft/AnimationHolder/Damaged/PlaneEngineDamaged").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingLeftHolder/PlaneEngineLaserGunSmallLeft/AnimationHolder/Normal/PlaneEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingLeftHolder/PlaneEngineLaserGunSmallLeft/AnimationHolder/Damaged/PlaneEngineDamaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/PlaneWingRightHolder/PlaneWing").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingRightHolder/PlaneWingDamaged").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingRightHolder/PlaneEngineLaserGunBigRight/AnimationHolder/Normal/PlaneEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingRightHolder/PlaneEngineLaserGunBigRight/AnimationHolder/Damaged/PlaneEngineDamaged").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingRightHolder/PlaneEngineLaserGunSmallRight/AnimationHolder/Normal/PlaneEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingRightHolder/PlaneEngineLaserGunSmallRight/AnimationHolder/Damaged/PlaneEngineDamaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/PlaneWingSmallLeftHolder/Normal/PlaneWingSmall").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingSmallLeftHolder/Damaged/PlaneWingDamaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/PlaneWingSmallRightHolder/Normal/PlaneWingSmall").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/PlaneWingSmallRightHolder/Damaged/PlaneWingDamaged").GetComponent<SpriteRenderer>().color = color;
	}

	public void SetHealthBar()
	{
		healthFillRate = health/(float)healthStart;
		healthBar.GetComponent<Image>().fillAmount = healthFillRate;
		//healthText.text = health.ToString();
	}

	void Update () 
	{
//		eventCounter++;
//
//		if(turretsFirstWave > 0)
//		{
//
//		}
//		else if(turretsSecondWave > 0)
//		{
//
//		}
//		else
//		{
//
//		}
		if(bossCanMove)
			transform.position = new Vector3(transform.position.x, camera.position.y + cameraOffsetY, transform.position.z);
	}
	
	IEnumerator AdjustCameraSpeedForBoss()
	{
		yield return new WaitForSeconds(1f);
		while(AircraftBattleGameManager.Instance.speed != bgMovingSpeed)
		{
			yield return null;
			AircraftBattleGameManager.Instance.speed = Mathf.MoveTowards(AircraftBattleGameManager.Instance.speed, bgMovingSpeed, 0.2f);
		}
		LevelGenerator.Instance.SendMessage("PositionBoss",SendMessageOptions.DontRequireReceiver);

		transform.GetChild(0).GetComponent<Animation>().Play();
		transform.GetChild(0).GetComponent<Animation>().PlayQueued("BossPlaneMovement1",QueueMode.CompleteOthers);
		PlaneManager.Instance.dontSlowTime = 0;
		PlaneManager.Instance.gameActive = true;
		PlaneManager.Instance.isShooting = true;
		PlaneManager.Instance.bossTimePart = false;
		PopUpHandler.Instance.NormalDialog();
		PlaneManager.Instance.ResetCoordinates();
		//PlaneManager.Instance.transform.GetChild(0).Find("Elipse").animation["PandaPlaneElipseRotationAnimation"].speed = 1;
		//Time.timeScale = 1;


		//DialogPanda.dialogPressed = false;
		//DialogBoss.dialogPressed = false;
	}

//	void OnTriggerEnter2D(Collider2D col)
//	{
//		if(col.tag.Equals("PlayerBullet"))
//		{
//			if(health <= 0)
//			{
//				gameObject.SetActive(false);
//				Camera.main.transform.Find("Splendid").renderer.enabled = true;
//				Transform warpZone = GameObject.Find("WarpZone").transform;
//				warpZone.GetComponent<OffsetTexture>().enabled = true;
//				warpZone.position = new Vector3(warpZone.position.x,Camera.main.transform.position.y,warpZone.position.z);
//				warpZone.parent = Camera.main.transform;
//				warpZone.renderer.enabled = true;
//				Camera.main.transform.position = new Vector3(0,0,-30);
//				Invoke("StartLoadNewStage",1f);
//				GameManager.Instance.bossTime = false;
//			}
//			else
//			{
//				health-=5;
//				col.gameObject.SetActive(false);
//				transform.Find("HealthBar").localScale -= new Vector3(1f/80, 0, 0);
//				transform.Find("BossHit").renderer.enabled = true;
//				StartCoroutine(TurnOffDamageSprite());
//			}
//		}
//	}

	public void DestroyBoss()
	{
		if (PopUpHandler.Instance.bossDestroyed)
		{
			return;
		}
		PopUpHandler.Instance.bossDestroyed = true;
		SoundManager.Instance.Stop_BossPlaneMovement();
		SoundManager.Instance.Stop_BossMusic();
		SoundManager.Instance.Play_BossExplosion();
		//gameObject.SetActive(false);
		transform.GetChild(0).GetComponent<Animation>().Play("DeathAnimation");
		PopUpHandler.popupType = 5;
		Invoke("BossDestroyed",9f);
		PlaneManager.Instance.dontSlowTime = 2;
		Invoke("StopBossExplosion",5f);
	}

	void StopBossExplosion()
	{
		SoundManager.Instance.Stop_BossExplosion();
	}

	void BossDestroyed()
	{
		//Camera.main.transform.Find("Splendid").renderer.enabled = true;
		//PlaneManager.Instance.guiCamera.transform.Find("Splendid").renderer.enabled = true;
		//Transform warpZone = GameObject.Find("WarpZone").transform;
		//SoundManager.Instance.Stop_BossExplosion();
		GameObject warpZone = Instantiate(LevelGenerator.Instance.warpZonePrefab) as GameObject;
		//warpZone.GetComponent<OffsetTexture>().enabled = true;
		warpZone.transform.parent = Camera.main.transform;
		//warpZone.transform.position = new Vector3(warpZone.transform.position.x,Camera.main.transform.position.y,warpZone.transform.position.z);
		warpZone.transform.localPosition = new Vector3(0,3.75f,27);
		//warpZone.parent = Camera.main.transform;
		//warpZone.renderer.enabled = true;
		Invoke("ResetCameraPosition",0.5f);
		Invoke("StartLoadNewStage",1.5f);
		AircraftBattleGameManager.Instance.bossTime = false;
	}

	IEnumerator TurnOffDamageSprite()
	{
		yield return new WaitForSeconds(0.15f);
		transform.Find("BossHit").GetComponent<Renderer>().enabled = false;
	}

	void ResetCameraPosition()
	{
		Camera.main.transform.parent.position = new Vector3(0,0,Camera.main.transform.position.z);
		AircraftBattleGameManager.Instance.speed = 0;
		LevelGenerator.Instance.StageClear();
	}
	/// <summary>
	/// 击败BossPlane
	/// </summary>
	void StartLoadNewStage()
	{
		//PlaneManager.Instance.guiCamera.transform.Find("Splendid").renderer.enabled = false;
		//LevelGenerator.Instance.StageClear();
		GameObject.Destroy(this.gameObject);
		if (LevelGenerator.currentBossPlaneLevel < 3)
		{
			LevelGenerator.currentBossPlaneLevel++;
		}
		LevelGenerator.Instance.DestroyTerrains();
	}

	public void EnableSecondWave()
	{
		StartCoroutine(EnableSecondWaveCoroutine());
	}

	IEnumerator EnableSecondWaveCoroutine()
	{
		if(bossLevel == 1)
		{
			yield return new WaitForSeconds(0.75f);
			for(int i=turretsFirstWave;i<turretsFirstWave+turretsSecondWave;i++)
			{
				TurretsLevel1[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel1[i].SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(bossLevel == 2)
		{
			yield return new WaitForSeconds(0.75f);
			for(int i=turretsFirstWave;i<turretsFirstWave+turretsSecondWave;i++)
			{
				TurretsLevel2[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel2[i].SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(bossLevel == 3)
		{
			yield return new WaitForSeconds(0.75f);
			for(int i=turretsFirstWave;i<turretsFirstWave+turretsSecondWave;i++)
			{
				TurretsLevel3[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel3[i].SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
			}
		}
		currentWave = turretsSecondWave;
	}

	public void EnableThirdWave()
	{
		StartCoroutine(EnableThirdWaveCoroutine());
	}

	IEnumerator EnableThirdWaveCoroutine()
	{
		if(bossLevel == 1)
		{
			yield return new WaitForSeconds(0.75f);
			for(int i=turretsFirstWave+turretsSecondWave;i<turretsFirstWave+turretsSecondWave+turretsThirdWave;i++)
			{
				TurretsLevel1[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel1[i].SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(bossLevel == 2)
		{
			yield return new WaitForSeconds(0.75f);
			for(int i=turretsFirstWave+turretsSecondWave;i<turretsFirstWave+turretsSecondWave+turretsThirdWave;i++)
			{
				TurretsLevel2[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel2[i].SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(bossLevel == 3)
		{
			yield return new WaitForSeconds(0.75f);
			for(int i=turretsFirstWave+turretsSecondWave;i<turretsFirstWave+turretsSecondWave+turretsThirdWave;i++)
			{
				TurretsLevel3[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel3[i].SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
			}
		}
		currentWave = turretsThirdWave;
	}

	void GetAvailableTerrainType()
	{
		LevelGenerator.Instance.currentBossAvailableTerrainType = availableTerrainType;
	}
	void SetCameraOffsetY()
	{
		cameraOffsetY = LevelGenerator.Instance.currentBossCameraOffsetY;
		bossCanMove = true;
		//SoundManager.Instance.Play_BossPlaneMovement();
	}

	void AdjustSpeed()
	{
		StartCoroutine(AdjustCameraSpeedForBoss());
	}

	void FireAway()
	{
		if(bossLevel == 1)
		{
			for(int i=0;i<turretsFirstWave;i++)
			{
				TurretsLevel1[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel1[i].SendMessage("FireAway", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(bossLevel == 2)
		{
			for(int i=0;i<turretsFirstWave;i++)
			{
				TurretsLevel2[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel2[i].SendMessage("FireAway", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(bossLevel == 3)
		{
			for(int i=0;i<turretsFirstWave;i++)
			{
				TurretsLevel3[i].GetComponent<Collider2D>().enabled = true;
				TurretsLevel3[i].SendMessage("FireAway", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
