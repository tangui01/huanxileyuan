using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

/**
  * Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...
  * Object:N/A
  * Description: Sample Description
  **/
public class BossStars : MonoBehaviour {


	int turretMinCoins=5, turretMaxCoins=10;
	int laserMinCoins=10, laserMaxCoins=15;
	int bossDeathMinCoins=25, bossDeathMaxCoins=30;
	int tankMainGunMinCoins=12, tankMainGunMaxCoins=17;
	[HideInInspector] public Vector3 spawnPosition;

	static BossStars instance;
	
	public static BossStars Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(BossStars)) as BossStars;
			
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void GenerateCoins(int min, int max)
	{
		
//		int number = Random.Range(min,max);
//		
//		for(int i=0;i<number;i++)
//		{
//			float rotate = Random.Range(-179, 179);
//			GameObject Star = (GameObject) Instantiate(Resources.Load("StarHolder"));
//			Star.transform.rotation = Quaternion.Euler(0,0,rotate);
//			Star.transform.position = new Vector3(spawnPosition.x+Random.Range(-6f,6f), spawnPosition.y+Random.Range(-6f,6f), -45);
//		}
		StartCoroutine(GenerateCoinsWithDelay(min,max));
	}

	IEnumerator GenerateCoinsWithDelay(int min, int max)
	{
		int number = Random.Range(min,max);
		
		for(int i=0;i<number;i++)
		{
			float rotate = Random.Range(-179, 179);
			GameObject Star = (GameObject) Instantiate(Resources.Load("StarHolder"));
			Star.transform.rotation = Quaternion.Euler(0,0,rotate);
			Star.transform.position = new Vector3(spawnPosition.x+Random.Range(-6f,6f), spawnPosition.y+Random.Range(-6f,6f), -45);
			yield return new WaitForSeconds(0.1f);
		}
	}

	void GenerateCoinsOnTurretDestroyed()
	{
		
		int number = Random.Range(turretMinCoins,turretMaxCoins);
		
		for(int i=0;i<number;i++)
		{
			float rotate = Random.Range(-179, 179);
			GameObject Star = (GameObject) Instantiate(Resources.Load("StarHolder"));
			Star.transform.rotation = Quaternion.Euler(0,0,rotate);
			Star.transform.position = new Vector3(transform.position.x+Random.Range(-2f,2f), transform.position.y+Random.Range(-2f,2f), -45);
		}
	}

	void GenerateCoinsOnLaserDestroyed()
	{
		
		int number = Random.Range(laserMinCoins,laserMaxCoins);
		
		for(int i=0;i<number;i++)
		{
			float rotate = Random.Range(-179, 179);
			GameObject Star = (GameObject) Instantiate(Resources.Load("StarHolder"));
			Star.transform.rotation = Quaternion.Euler(0,0,rotate);
			Star.transform.position = new Vector3(transform.position.x+Random.Range(-2f,2f), transform.position.y+Random.Range(-2f,2f), -45);
		}
	}

	void GenerateCoinsOnTankMainGunDestroyed()
	{
		
		int number = Random.Range(tankMainGunMinCoins,tankMainGunMaxCoins);
		
		for(int i=0;i<number;i++)
		{
			float rotate = Random.Range(-179, 179);
			GameObject Star = (GameObject) Instantiate(Resources.Load("StarHolder"));
			Star.transform.rotation = Quaternion.Euler(0,0,rotate);
			Star.transform.position = new Vector3(transform.position.x+Random.Range(-2f,2f), transform.position.y+Random.Range(-2f,2f), -45);
		}
	}

	void GenerateCoinsOnBossDestroyed()
	{
		
		int number = Random.Range(bossDeathMinCoins,bossDeathMaxCoins);
		
		for(int i=0;i<number;i++)
		{
			float rotate = Random.Range(-179, 179);
			GameObject Star = (GameObject) Instantiate(Resources.Load("StarHolder"));
			Star.transform.rotation = Quaternion.Euler(0,0,rotate);
			Star.transform.position = new Vector3(transform.position.x+Random.Range(-3f,3f), transform.position.y+Random.Range(-3f,3f), -45);
		}
	}
}
