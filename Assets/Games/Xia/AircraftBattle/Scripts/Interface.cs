using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

	Transform slowTimeScreen;
	[HideInInspector] public bool normalTime;
	static Interface instance;
	[HideInInspector] public SpriteRenderer weaponLaser;
	[HideInInspector] public SpriteRenderer weaponTesla;
	[HideInInspector] public SpriteRenderer weaponBlades;
	[HideInInspector] public SpriteRenderer weaponBomb;
	SpriteRenderer slowTimeScreenSprite;

	public static Interface Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(Interface)) as Interface;
			}
			return instance;
		}
	}

	void Start ()
	{
//		slowTimeScreen = transform.Find("SlowTimeScreen");
//		slowTimeScreenSprite = slowTimeScreen.GetComponent<SpriteRenderer>();
//		weaponLaser = slowTimeScreen.Find("WeaponLaser").GetComponent<SpriteRenderer>();
//		weaponTesla = slowTimeScreen.Find("WeaponTesla").GetComponent<SpriteRenderer>();
//		weaponBlades = slowTimeScreen.Find("WeaponBlades").GetComponent<SpriteRenderer>();
//		weaponBomb = slowTimeScreen.Find("WeaponBomb").GetComponent<SpriteRenderer>();
		//if(!LevelGenerator.checkpoint)
		{
			if(LevelGenerator.currentStage > 1 && !LevelGenerator.checkpoint)
			{
				// Transform shop = GameObject.Find("ShopAndClouds").transform;
				//shop.GetChild(0).Find("New Text").gameObject.SetActive(false);
				// transform.Find("StartCircleHolder").gameObject.SetActive(false);
				// Invoke("StartPlaying",1.5f);
			}
		}
	}

	void StartPlaying()
	{
		PlaneManager.Instance.StartPlaying();
	}

	public IEnumerator SlowTime()
	{
		float t=0;
		float target = 0.1f;
		//weaponLaser.collider.enabled = true;
		//weaponTesla.collider.enabled = true;
		//weaponBlades.collider.enabled = true;
		//weaponBomb.collider.enabled = true;

		while(Time.timeScale >= target + 0.01f && !normalTime)
		{
			
			//slowTimeScreenSprite.color = new Color(1,1,1,Mathf.Lerp(slowTimeScreen.GetComponent<SpriteRenderer>().color.a,0.55f,t));//0.67
			//weaponLaser.color = weaponTesla.color = weaponBlades.color = weaponBomb.color = slowTimeScreenSprite.color*2;
			Time.timeScale = Mathf.Lerp(Time.timeScale,target,t);
			t+=Time.deltaTime/2;
			yield return null;
		}
		//weaponLaser.color = weaponTesla.color = weaponBlades.color = weaponBomb.color = Color.white;
	}
	public IEnumerator NormalTime()
	{
		float t=0;
		//weaponLaser.collider.enabled = false;
		//weaponTesla.collider.enabled = false;
		//weaponBlades.collider.enabled = false;
		//weaponBomb.collider.enabled = false;
		if(normalTime)
		{
//			while(Time.timeScale <= 0.99f && normalTime)
//			{
//				Debug.Log("VRTI");
//				//slowTimeScreenSprite.color = new Color(1,1,1,Mathf.Lerp(slowTimeScreen.GetComponent<SpriteRenderer>().color.a,0,t));
//				//weaponLaser.color = weaponTesla.color = weaponBlades.color = weaponBomb.color = slowTimeScreenSprite.color;
//				Time.timeScale = Mathf.Lerp(Time.timeScale,1,t);
//				t+=Time.deltaTime;
//				yield return null;
//			}
			Time.timeScale = 1;
		}
		yield return null;
	}
	
	public void PauseGame()
	{
		if(Time.timeScale == 0)
		{
			if(slowTimeScreen.GetComponent<SpriteRenderer>().color.a == 0)
				Time.timeScale = 1;
			else
				Time.timeScale = 0.1f;
			PlaneManager.Instance.gameActive = true;
			transform.Find("MenuHolder/Menu").GetComponent<Animator>().Play("UnPauseGame");
		}
		else
		{
			Time.timeScale = 0;
			PlaneManager.Instance.gameActive = false;
			if(PlaneManager.controlType == 1)
			{
				transform.Find("MenuHolder/Menu/ControlsSelection").localPosition = new Vector3(-0.25f,0,-0.1f);
			}
			else
			{
				transform.Find("MenuHolder/Menu/ControlsSelection").localPosition = new Vector3(0.25f,0,-0.1f);
			}
			transform.Find("MenuHolder/Menu").GetComponent<Animator>().Play("PauseGame");
		}
	}
}
