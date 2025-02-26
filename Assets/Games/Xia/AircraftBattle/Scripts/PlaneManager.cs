using System;
using UnityEngine;
using System.Collections;
using WGM;

public class PlaneManager : MonoBehaviour {

	public bool isShooting = false;
	public static int controlType = 1;
	float endX;
	float offsetX;
	float offsetY;
	//Animator animator;
	[HideInInspector] public bool gameActive = true;
	string clickedItem;
	string releasedItem;
	float previousEndX;
	float minus5Frames = 0;
	[HideInInspector] public float boundLeft;
	[HideInInspector] public float boundRight;
	[HideInInspector] public float boundUp;
	[HideInInspector] public float boundDown;
	float firstQuarter;
	float thirdQuarter;
	float cameraMin;
	float cameraMax;
	Transform planeChild;
	float fireRate = 0.3f; //higher value means slower fire rate // 25, pa 15 -- //0.35f
	float fireRateCounter = 0;
	float wingFireRate = 0.3f; //higher value means slower fire rate // 25, pa 15 -- //0.35f
	float wingFireRateCounter = 0;
	float sideFireRate = 0.3f; //higher value means slower fire rate // 25, pa 15 -- //0.35f
	float sideFireRateCounter = 0;
	[HideInInspector] public Transform firePosition;
	[HideInInspector] public Transform wingFirePosition;
	[HideInInspector] public Transform sideFirePosition;
	bool cooldown = false;
	bool normalTime = true;
	[HideInInspector] public bool notControlling = false;

	public Transform bulletPool;
	public Transform wingBulletPool;
	public Transform sideBulletPool;
	float elapsedTime = 0;
	bool timerEnabled = false;
	TextMesh timer;
	Transform bounds;
	int bulletIndex = 0;
	int wingBulletIndex = 0;
	int sideBulletIndex = 0;
	[HideInInspector] public Camera guiCamera;
	[HideInInspector] public int dontSlowTime = 0; // 0 - can slow time, 1 - cannot slow time, 2 - cannot slow time after boss
	GameObject activeWeapon;
	public GameObject weaponLaser;
	public GameObject weaponTesla;
	public GameObject weaponBlades;
	public GameObject weaponBomb;
	float planeBoundsLeft;
	float planeBoundsRight;
	Transform cameraParent;
	[HideInInspector] public bool menuIsActive = false;
	public static bool pressedAndHold = false;
	Transform activeMainGuns;
	Transform activeWingGuns;
	Transform activeSideGuns;
	[HideInInspector] public bool bossTimePart = false;
	//bool speedUpTime = false;

	static PlaneManager instance;
	bool stopGame = false;
	NewPandaPlane newPanda;
	public static PlaneManager Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(PlaneManager)) as PlaneManager;

			return instance;
		}
	}

	void Awake ()
	{
		planeChild = transform.GetChild(0).Find("PandaPlane");
		newPanda = NewPandaPlane.Instance;
	}

	// Use this for initialization
	void Start () 
	{
		Invoke("StartPlay",0.1f);
		//DetermineGunsLevel();
		//SoundManager.Instance.Play_DoorOpening();
		cameraParent = Camera.main.transform.parent;
		guiCamera = GameObject.Find("GUICamera").GetComponent<Camera>();
		bounds = GameObject.Find("Bounds").transform;
		//timer = Camera.main.transform.Find("Timer").GetComponent<TextMesh>();
		boundLeft = bounds.Find("BoundUpLeft").position.x;
		boundUp = bounds.Find("BoundUpLeft").position.y;
		boundRight = bounds.Find("BoundDownRight").position.x;
		boundDown = bounds.Find("BoundDownRight").position.y;
		//animator = transform.Find("Plane").GetComponent<Animator>();
		firstQuarter = Camera.main.ViewportToWorldPoint(Vector3.one*0.25f).x;
		thirdQuarter = Camera.main.ViewportToWorldPoint(Vector3.one*0.75f).x;
		cameraMin = boundLeft + Camera.main.orthographicSize*Camera.main.aspect;//-2
		cameraMax = boundRight - Camera.main.orthographicSize*Camera.main.aspect;//+2
		planeBoundsLeft = boundLeft + Camera.main.aspect*4;
		planeBoundsRight = boundRight - Camera.main.aspect*4;
		//cameraMin = boundLeft + Camera.main.orthographicSize*0.625f;
		//cameraMax = boundRight - Camera.main.orthographicSize*0.625f;
		fireRateCounter = fireRate;
		//firePosition = planeChild.Find("FirePosition").transform;
		firePosition = transform.GetChild(0).Find("FirePosition").transform;
		wingFirePosition = transform.GetChild(0).Find("WingFirePosition").transform;
		sideFirePosition = transform.GetChild(0).Find("SideFirePosition").transform;
		gameActive = false;
		dontSlowTime = 1;
		Invoke("StartPlaying",0.3f);

		
	}

	void CanSlowTime()
	{
		if(dontSlowTime == 1)
			dontSlowTime = 0;
	}

	public void DetermineGunsAndArmorLevel()
	{
		bulletPool = (Instantiate(Resources.Load("Bullets/BulletPool"+PandaPlane.Instance.mainGunLvl.ToString())) as GameObject).transform;
		if(PandaPlane.Instance.mainGunLvl > 7)
		{
			activeMainGuns = transform.GetChild(0).Find("GunsHolder/MainGuns/Level4");
			activeMainGuns.gameObject.SetActive(true);
		}
		else if(PandaPlane.Instance.mainGunLvl > 3)
		{
			activeMainGuns = transform.GetChild(0).Find("GunsHolder/MainGuns/Level3");
			activeMainGuns.gameObject.SetActive(true);
		}
		else if(PandaPlane.Instance.mainGunLvl > 0)
		{
			activeMainGuns = transform.GetChild(0).Find("GunsHolder/MainGuns/Level2");
			activeMainGuns.gameObject.SetActive(true);
		}
		else
		{
			activeMainGuns = transform.GetChild(0).Find("GunsHolder/MainGuns/Level1");
			activeMainGuns.gameObject.SetActive(true);
		}

		if(PandaPlane.Instance.wingGunLvl > 0)
		{
			wingBulletPool = (Instantiate(Resources.Load("Bullets/WingBulletPool"+PandaPlane.Instance.wingGunLvl.ToString())) as GameObject).transform;
			if(PandaPlane.Instance.wingGunLvl > 7)
			{
				activeWingGuns = transform.GetChild(0).Find("GunsHolder/WingGuns/Level3");
				activeWingGuns.gameObject.SetActive(true);
			}
			else if(PandaPlane.Instance.wingGunLvl > 3)
			{
				activeWingGuns = transform.GetChild(0).Find("GunsHolder/WingGuns/Level2");
				activeWingGuns.gameObject.SetActive(true);
			}
			else
			{
				activeWingGuns = transform.GetChild(0).Find("GunsHolder/WingGuns/Level1");
				activeWingGuns.gameObject.SetActive(true);
			}
		}

		if(PandaPlane.Instance.sideGunLvl > 0)
		{
			sideBulletPool = (Instantiate(Resources.Load("Bullets/SideBulletPool"+PandaPlane.Instance.sideGunLvl.ToString())) as GameObject).transform;
			activeSideGuns = transform.GetChild(0).Find("GunsHolder/SideGuns");
			activeSideGuns.gameObject.SetActive(true);
		}
		

		if(PandaPlane.Instance.armorLvl > 7)
		{
			planeChild.GetComponent<SpriteRenderer>().sprite = GameObject.Find("PlaneReferences/PandaPlaneArmor3Gameplay").GetComponent<SpriteRenderer>().sprite;
			planeChild.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameObject.Find("PlaneReferences/PandaPlaneArmor3HIT").GetComponent<SpriteRenderer>().sprite;
		}
		else if(PandaPlane.Instance.armorLvl > 3)
		{
			planeChild.GetComponent<SpriteRenderer>().sprite = GameObject.Find("PlaneReferences/PandaPlaneArmor2Gameplay").GetComponent<SpriteRenderer>().sprite;
			planeChild.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameObject.Find("PlaneReferences/PandaPlaneArmor2HIT").GetComponent<SpriteRenderer>().sprite;
		}
	}

	public IEnumerator TurnOffWeapon(float time)
	{
		yield return new WaitForSeconds(time);
		if(activeWeapon != null)
		{
			if(activeWeapon.name.Contains("Laser"))
			{
				activeWeapon.transform.GetChild(0).GetComponent<Animation>()["LaserLaunch"].normalizedTime = 1;
				activeWeapon.transform.GetChild(0).GetComponent<Animation>()["LaserLaunch"].speed = -1;
				activeWeapon.transform.GetChild(0).GetComponent<Animation>().Play();
				yield return new WaitForSeconds(0.6f);
				activeWeapon.transform.GetChild(0).GetComponent<Animation>()["LaserLaunch"].normalizedTime = 0;
				activeWeapon.transform.GetChild(0).GetComponent<Animation>()["LaserLaunch"].speed = 1;
			}
			if(PandaPlane.Instance.health > 0)
				isShooting = true;
			yield return new WaitForSeconds(0.25f);
			if(activeWeapon.activeSelf)
				activeWeapon.SetActive(false);
			activeWeapon = null;
			if(dontSlowTime == 1)
				dontSlowTime = 0;
		}
	}
	
	void Update () 
	{
		
		if (bossTimePart)
		{
			// isShooting = false;
			// if(!newPanda.oneShooting)
			// 	newPanda.oneShooting = true;
		}
		else
		{
			// if(newPanda.oneShooting)
			// 	isShooting = true;
		}
		Vector3 movePos = newPanda.PlankInput();
		if(movePos!=Vector3.zero && gameActive)
		{
			StartCoroutine(NormalTime(0.01f));
			transform.Translate(newPanda.Verify(movePos) * Time.deltaTime,Space.Self);
			if (planeChild.eulerAngles.y > 35 && planeChild.eulerAngles.y < 180)
			{
                				planeChild.eulerAngles = new Vector3(0,35,0);
			}
			else if (planeChild.eulerAngles.y > 180 && planeChild.eulerAngles.y < 325)
			{
								planeChild.eulerAngles = new Vector3(0,325,0);
			}

		}
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.J) && isShooting)
		{
			FireBullet();
			FireWingBullet();
			FireSideBullet();
		}
#else
		if(DealCommand.GetKeyDown(1,(AppKeyCode)3) && isShooting)
		{
			FireBullet();
			FireWingBullet();
			FireSideBullet();
		}
#endif		
		// else
		// 	isShooting = false;
		// if(isShooting)
		// {
		// 	if(fireRateCounter >= fireRate)
		// 	{
		// 		FireBullet();
		// 		fireRateCounter = 0;
		// 	}
		// 	else
		// 	{
		// 		//fireRateCounter++;
		// 		fireRateCounter+=Time.deltaTime;
		// 	}
		// 	if(wingBulletPool != null)
		// 	{
		// 		if(wingFireRateCounter >= wingFireRate)
		// 		{
		// 			FireWingBullet();
		// 			wingFireRateCounter = 0;
		// 		}
		// 		else
		// 		{
		// 			wingFireRateCounter += Time.deltaTime;
		// 		}
		// 	}
		// 	if(sideBulletPool != null)
		// 	{
		// 		if(sideFireRateCounter >= sideFireRate)
		// 		{
		// 			FireSideBullet();
		// 			sideFireRateCounter = 0;
		// 		}
		// 		else
		// 		{
		// 			sideFireRateCounter += Time.deltaTime;
		// 		}
		// 	}
		// }
	}

	private void FixedUpdate()
	{
		boundLeft = bounds.Find("BoundUpLeft").position.x;
		boundUp = bounds.Find("BoundUpLeft").position.y;
		boundRight = bounds.Find("BoundDownRight").position.x;
		boundDown = bounds.Find("BoundDownRight").position.y;
	}

	void StartPlay()
	{
		// SoundManager.Instance.Stop_CloudsPassing();
		LevelGenerator.checkpoint = false;
		GameObject circle = GameObject.Find("StartCircleHolder");
		if (circle !=null)
		{
			circle.GetComponent<Collider>().enabled = false;
			circle.GetComponent<Animation>().Play("StartCircleDisappear_Animation");
		}
		PopUpHandler.Instance.ResumeGame();
		Invoke("StartCircleDissapear",1.6f);
	}
	void NormalTime2()
	{
		if(!normalTime)
		{
			normalTime = true;
		}
	}

	void StartCircleDissapear()
	{
		GameObject.Find("StartCircleHolder")?.SetActive(false);
	}

	public void StartPlaying()
	{
		SoundManager.Instance.Play_GameplayMusic();
		GameObject clouds = GameObject.Find("ShopAndClouds");
		gameActive = true;
		MoveBg.canMove = true;
		AircraftBattleGameManager.canMove = true;
		//GameObject.Find("ShopAndClouds").SetActive(false);
		transform.Find("StartCircleHolder")?.gameObject.SetActive(false);
		clouds.transform.GetChild(0).GetComponent<Animation>().Play();
		Destroy(clouds,2f);
		isShooting = true;
		PopUpHandler.popupType = 0;
		Invoke("CanSlowTime",0.5f);
	}

	public IEnumerator NormalTime(float time)
	{
		yield return new WaitForSeconds(time);
		if(pressedAndHold)
		{
			normalTime = true;
			//StopCoroutine(Interface.Instance.SlowTime()); @@@@@@@@
			//StartCoroutine(Interface.Instance.NormalTime()); @@@@@@@@
			//PopUpHandler.Instance.transform.Find("SlowTimeScreen").gameObject.SetActive(true); @@@@@@@@
			PopUpHandler.Instance.ShowMenu();
		}
	}

	public void ForceGameActive()
	{
		gameActive = true;
		normalTime = true;
		menuIsActive = false;
	}

	public bool ReturnGameActive()
	{
		return gameActive;
	}

	public bool ReturnNormalTime()
	{
		return normalTime;
	}

	public bool ReturnMenuIsActive()
	{
		return menuIsActive;
	}

	public int ReturnDontSlowTime()
	{
		return dontSlowTime;
	}

	public void NormalTimeAfterWeaponUse()
	{
		normalTime = true;
		StopCoroutine(Interface.Instance.SlowTime());
		StartCoroutine(Interface.Instance.NormalTime());
		menuIsActive = false;
		PopUpHandler.Instance.ShowMenu();
	}

	void LateUpdate()
	{
		//if(transform.position.x < firstQuarter || offsetX != 0)
		//Debug.Log("Avion: " + transform.position.x + ", ivica kamere: " + (Camera.main.transform.position.x - Camera.main.orthographicSize*Camera.main.aspect + 1));
		//Debug.Log("MRDAAAAK: " + (endX-previousEndX));
		if(offsetX != 0)
		{
			//Debug.Log("111");
			//Debug.Log("ALO BRE: " + cameraMax + " " + cameraMin);
			//Camera.main.transform.position = new Vector3(Mathf.Clamp(Mathf.Lerp(Camera.main.transform.position.x,transform.position.x,Time.deltaTime*2),cameraMin,cameraMax),Camera.main.transform.position.y,Camera.main.transform.position.z);
			//Camera.main.transform.position = new Vector3(Mathf.Clamp(Mathf.Lerp(Camera.main.transform.position.x,Camera.main.transform.position.x+offsetX*5.2f,Time.deltaTime*3f),cameraMin,cameraMax),Camera.main.transform.position.y,Camera.main.transform.position.z);
			cameraParent.position = new Vector3(Mathf.Clamp(Mathf.Lerp(cameraParent.position.x,cameraParent.position.x+offsetX*5.2f,Time.deltaTime*3f),cameraMin,cameraMax),cameraParent.position.y,cameraParent.position.z);
		}
		else if(controlType == 1 && endX - previousEndX != 0)
		{
			cameraParent.position = new Vector3(Mathf.Clamp(Mathf.Lerp(cameraParent.position.x,cameraParent.position.x-(endX-previousEndX)/1.5f,Time.deltaTime*3f),cameraMin,cameraMax),cameraParent.position.y,cameraParent.position.z);
		}
//		else if(transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize*Camera.main.aspect + 3 && controlType == 1)
//		else if(transform.position.x < firstQuarter)
//		{
//			Debug.Log("123123");
//			Camera.main.transform.position = new Vector3(Mathf.Clamp(Mathf.Lerp(Camera.main.transform.position.x,cameraMin,Time.deltaTime*3f),cameraMin,cameraMax),Camera.main.transform.position.y,Camera.main.transform.position.z);
//		}
		//else if(transform.position.x > thirdQuarter)
		//{
		//	Camera.main.transform.position = new Vector3(Mathf.Lerp(Camera.main.transform.position.x,cameraMax,Time.deltaTime*2),Camera.main.transform.position.y,Camera.main.transform.position.z);
		//}
//		else
//		{
//			Camera.main.transform.position = new Vector3(Mathf.Lerp(Camera.main.transform.position.x,0,Time.deltaTime*2),Camera.main.transform.position.y,Camera.main.transform.position.z);
//		}
	}

	public void DisablePlayer()
	{
		isShooting = false;
		gameActive = false;
		//BoxCollider2D[] colls = GetComponents<BoxCollider2D>();
		//colls[0].enabled = false;
		//colls[1].enabled = false;
		GetComponent<Collider2D>().enabled = false;
		transform.GetChild(0).Find("PlaneShadow").GetComponent<Renderer>().enabled = false;
	}

	public void EnablePlayer()
	{
		transform.GetChild(0).Find("PlaneShadow").GetComponent<Renderer>().enabled = true;
		notControlling = true;
		dontSlowTime = 1;
		StartCoroutine(MoveAndBlink());
		SoundManager.Instance.Play_PlaneResurrect();
	}

	IEnumerator MoveAndBlink()
	{
		int i=0;
		bool radi = false;
		transform.position = new Vector3(cameraParent.position.x, cameraParent.position.y - 23, transform.position.z);
		Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z);

		planeChild.gameObject.SetActive(true);
		planeChild.parent.Find("Elipse").gameObject.SetActive(true);
		planeChild.parent.Find("GunsHolder").gameObject.SetActive(true);
		Renderer plane = planeChild.GetComponent<Renderer>();
		GameObject elipse = planeChild.parent.Find("Elipse").gameObject;
		GameObject mainGuns = activeMainGuns.gameObject;
		GameObject wingGuns = null;
		GameObject sideGuns = null;
		if(activeWingGuns != null)
			wingGuns = activeWingGuns.gameObject;
		if(activeSideGuns != null)
			sideGuns = activeSideGuns.gameObject;

		float t = 0;
		float distance = 0;
		bool ok = false;

		while(t < 1)
		{
			yield return null;
			if(transform.position != targetPos && !ok)
			{
				transform.position = Vector3.MoveTowards(transform.position,targetPos,0.2f);
				distance = targetPos.y - transform.position.y;
				if(transform.position == targetPos)
				{
					//targetPos = new Vector3(transform.position.x, cameraParent, transform.position.z);
					//transform.position = new Vector3(transform.position.x, cameraParent.position.y - distance, transform.position.z);
					ok = true;
				}
			}

			plane.enabled = radi;
			elipse.SetActive(radi);
			mainGuns.SetActive(radi);
			if(activeWingGuns != null)
				wingGuns.SetActive(radi);
			if(activeSideGuns != null)
				sideGuns.SetActive(radi);
			if(i==5)
			{
				i=0;
				radi = !radi;
			}
			i++;
			t += Time.deltaTime/3;
		}
		plane.enabled = true;
		elipse.SetActive(true);
		mainGuns.SetActive(true);
		if(activeWingGuns != null)
			wingGuns.SetActive(true);
		if(activeSideGuns != null)
			sideGuns.SetActive(true);

		isShooting = true;
		gameActive = true;
		notControlling = false;
		menuIsActive = false;
		Invoke("CanSlowTime",0.5f);
		normalTime = true;
		if(controlType == 1)
		{
			//startX = endX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
			//startY = endY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
		}
		else
		{
		}
		yield return new WaitForSeconds(1f);
		GetComponent<Collider2D>().enabled = true;
	}

	void FireBullet()
	{
//		for(int i=0; i<bulletPool.childCount; i++)
//		{
//			Bullet tempScript = bulletPool.GetChild(i).GetComponent<Bullet>();
//			if(tempScript.available)
//			{
//				tempScript.initialized = true;
//				break;
//			}
//		}
		if(bulletIndex == bulletPool.childCount)
			bulletIndex = 0;

		Bullet tempScript = bulletPool.GetChild(bulletIndex).GetComponent<Bullet>();
		if(tempScript.available)
		{
			tempScript.initialized = true;
			SoundManager.Instance.Play_FireBullet();
			//break;
		}
		bulletIndex++;
	}

	void FireWingBullet()
	{
		if(wingBulletIndex == wingBulletPool.childCount)
			wingBulletIndex = 0;
		
		Bullet tempScript = wingBulletPool.GetChild(wingBulletIndex).GetComponent<Bullet>();
		if(tempScript.available)
		{
			tempScript.initialized = true;
			//break;
		}
		wingBulletIndex++;
	}

	void FireSideBullet()
	{
		if(sideBulletIndex == sideBulletPool.childCount)
			sideBulletIndex = 0;
		
		SideBullet tempScript1 = sideBulletPool.GetChild(sideBulletIndex).GetComponent<SideBullet>();
		if(tempScript1.available)
		{
			tempScript1.initialized = true;
			//break;
		}
		sideBulletIndex++;
	}

//	void HideBossText()
//	{
//		GameObject.Find("BossTime").renderer.enabled = false;
//	}

	string RaycastFunction(Vector3 pos)
	{
		Ray ray = guiCamera.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 35))
		{
			return hit.collider.name;
		}
		return System.String.Empty;
	}

	public void StartLaser()
	{
		dontSlowTime = 1;
		isShooting = false;
		activeWeapon = weaponLaser;
		activeWeapon.SetActive(true);
		SoundManager.Instance.Play_LaunchLaser();
		StartCoroutine(TurnOffWeapon(PandaPlane.Instance.laserDuration));
	}
	
	public void StartTesla()
	{
		dontSlowTime = 1;
		isShooting = false;
		activeWeapon = weaponTesla;
		activeWeapon.SetActive(true);
		SoundManager.Instance.Play_LaunchTesla();
		StartCoroutine(TurnOffWeapon(PandaPlane.Instance.teslaDuration));
	}
	
	public void StartBlades()
	{
		dontSlowTime = 1;
		activeWeapon = weaponBlades;
		activeWeapon.SetActive(true);
		activeWeapon.SendMessage("Activate");
		SoundManager.Instance.Play_LaunchBlades();
		StartCoroutine(TurnOffWeapon(PandaPlane.Instance.bladesDuration+1.5f));
	}
	
	public void StartBomb()
	{
		dontSlowTime = 1;
		isShooting = false;
		activeWeapon = weaponBomb;
		weaponBomb.transform.position = transform.position;
		activeWeapon.SetActive(true);
		SoundManager.Instance.Play_LaunchBomb();
		StartCoroutine(TurnOffWeapon(1f));
		Camera.main.GetComponent<Animation>().Play();
	}

	public void ResetCoordinates()
	{
		if(controlType == 1)
		{
			//startX = endX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
			//startY = endY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
		}
		else
		{
			
		}
	}

}
