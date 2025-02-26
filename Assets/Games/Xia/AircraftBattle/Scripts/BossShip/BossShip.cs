using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossShip : MonoBehaviour {

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
	
	public int bottomGunHealth = 0;
	public int turretBottomHealth = 0;
	public int turretMiddleHealth = 0;
	public int turretTopHealth = 0;
	public int middleLaserGunHealth = 0;
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
	// BossLevel1: Gadjaju se oba bottom turreta, dva najsira middle turreta i main gun
	// BossLevel2: Gadjaju se oba bottom turreta + tail gun, dva najsira middle turreta + dva laser gun-a i main gun
	// BossLevel3: Gadjaju se oba bottom turreta + tail gun, dva najsira middle turreta + dva skrivena turreta + dva laser gun-a i main gun
	
	void Start () 
	{
		if(LevelGenerator.currentStage > 9)
			ColorTheShip(bossColors[Random.Range(0,bossColors.Length)]);

		if(bottomGunHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				bottomGunHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				bottomGunHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(turretBottomHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				turretBottomHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				turretBottomHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(turretMiddleHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				turretMiddleHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				turretMiddleHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(turretTopHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				turretTopHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				turretTopHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(middleLaserGunHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				middleLaserGunHealth += factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*LevelGenerator.currentStage/9f);
			else
				middleLaserGunHealth += factorX * PandaPlane.Instance.mainGunLvl + factorY;//*PandaPlane.mainGunLvl;
		}
		if(mainGunHealth != 0)
		{
			if(LevelGenerator.currentStage > 9)
				mainGunHealth += (int)(factorX * PandaPlane.Instance.mainGunLvl + (int)(factorY*1.5f*LevelGenerator.currentStage/9f));
			else
				mainGunHealth += (int)(factorX * PandaPlane.Instance.mainGunLvl + factorY*1.5f);//*PandaPlane.mainGunLvl;
		}
		health = bottomGunHealth + turretBottomHealth*2 + turretMiddleHealth*2 + turretTopHealth*2 + middleLaserGunHealth*2 + mainGunHealth;
		healthStart = health;
		camera = Camera.main.transform;
		healthBar = GameObject.Find("BossHealthBar");
		healthText = GameObject.Find("BossHealthBar/HealthBarText").GetComponent<Text>();
		
		if(bossLevel == 1)
		{
			turretsFirstWave = 2;
			turretsSecondWave = 2;
			turretsThirdWave = 1;
		}
		else if(bossLevel == 2)
		{
			turretsFirstWave = 3;
			turretsSecondWave = 4;
			turretsThirdWave = 1;
		}
		else
		{
			turretsFirstWave = 3;
			turretsSecondWave = 6;
			turretsThirdWave = 1;
		}
		currentWave = turretsFirstWave;
		if(bossLevel < 3)
			rightWingCount = leftWingCount = turretsSecondWave/2;
		else
			rightWingCount = leftWingCount = turretsSecondWave/2-1;

		//FireAway();
		SetHealthBar();
	}

	void ColorTheShip(Color color)
	{
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart1").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart4").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart44").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart5").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart6").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart7").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Normal/ColorBossShipPart8").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/BossShipBodyHolder/Damaged/ColorBossShipPart1Damaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart1").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart2").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart3").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart4").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart5").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart6").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart7").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderRight/AnimationHolder/Hatch/ColorBossShipSmallPart8").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart1").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart2").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart3").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart4").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart5").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart6").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart7").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/TurretUpperHolderLeft/AnimationHolder/Hatch/ColorBossShipSmallPart8").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/ShipBodyHolder/MainLaser/AnimationHolder/LaserHolder/AnimationHolder/Normal/ColorBossShipEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/MainLaser/AnimationHolder/LaserHolder/AnimationHolder/Damaged/ColorBossShipEngineDamaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/ShipBodyHolder/LaserRightHolder/AnimationHolder/Normal/ColorBossShipEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/LaserRightHolder/AnimationHolder/Damaged/ColorBossShipEngineDamaged").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/LaserLeftHolder/AnimationHolder/Normal/ColorBossShipEngine").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/ShipBodyHolder/LaserLeftHolder/AnimationHolder/Damaged/ColorBossShipEngineDamaged").GetComponent<SpriteRenderer>().color = color;

		transform.Find("AnimationHolder/RightWingHolder/Normal/ColorBossShipPart3").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/RightWingHolder/Normal/ColorBossShipPart9").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/RightWingHolder/Damaged/ColorBossShipPart3Damaged").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/LeftWingHolder/Normal/ColorBossShipPart3").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/LeftWingHolder/Normal/ColorBossShipPart9").GetComponent<SpriteRenderer>().color = color;
		transform.Find("AnimationHolder/LeftWingHolder/Damaged/ColorBossShipPart3Damaged").GetComponent<SpriteRenderer>().color = color;
	}

	public void SetHealthBar()
	{
		healthFillRate = health/(float)healthStart;
		healthBar.GetComponent<Image>().fillAmount = healthFillRate;
		//healthText.text = health.ToString();
	}
	
	void Update () 
	{
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
		SoundManager.Instance.Play_BossShipMovement();
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

	public void DestroyBoss()
	{
		if (PopUpHandler.Instance.bossDestroyed)
		{
			return;	
		}
		PopUpHandler.Instance.bossDestroyed = true;
		SoundManager.Instance.Stop_BossShipMovement();
		SoundManager.Instance.Stop_BossMusic();
		SoundManager.Instance.Play_BossExplosion();
		//gameObject.SetActive(false);
		//transform.GetChild(0).animation.Play("DeathAnimation");
		transform.position += Vector3.forward*5;
		bossCanMove = false;
		transform.GetChild(0).GetComponent<Animation>().Stop();
		//bgMovingSpeed = 3;
		//StartCoroutine(AdjustCameraSpeedForBoss());
		StartCoroutine(PandaPlane.Instance.MoveCameraSlowly(8f));
		PlaneManager.Instance.dontSlowTime = 2;
		Invoke("BossDestroyed",11f);
		Invoke("StopBossExplosion",7f);
	}

	void StopBossExplosion()
	{
		SoundManager.Instance.Stop_BossExplosion();
	}
	
	void BossDestroyed()
	{
		//PlaneManager.Instance.guiCamera.transform.Find("Splendid").renderer.enabled = true;
		//SoundManager.Instance.Stop_BossExplosion();
		GameObject warpZone = Instantiate(LevelGenerator.Instance.warpZonePrefab) as GameObject;
		warpZone.transform.parent = Camera.main.transform;
		warpZone.transform.localPosition = new Vector3(0,3.75f,27);
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
		Camera.main.transform.parent.position = new Vector3(0,0,Camera.main.transform.parent.position.z);
		AircraftBattleGameManager.Instance.speed = 0;
		LevelGenerator.Instance.StageClear();
	}
	/// <summary>
	/// 击败BossShip
	/// </summary>
	void StartLoadNewStage()
	{
		//PlaneManager.Instance.guiCamera.transform.Find("Splendid").renderer.enabled = false;
		//LevelGenerator.Instance.StageClear();
		GameObject.Destroy(this.gameObject);
		if (LevelGenerator.currentBossShipLevel < 3)
		{
			LevelGenerator.currentBossShipLevel++;
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
