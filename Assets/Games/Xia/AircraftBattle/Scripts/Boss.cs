using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	//public enum terrain {OnlyGround, Everything, OnlyWater, GroundToWater, WaterToGround};
	//public int[] AvailableTerrainTypes;
	public int availableTerrainType;
	public float bgMovingSpeed;
	int health = 400;

	public Transform[] TurretsOrder;

//	void Start () 
//	{
//	
//	}
//	
//	void Update () 
//	{
//	
//	}

	public void SummonBoss()
	{
		GameObject.Find("BossTime").GetComponent<Renderer>().enabled = true;
		Invoke("HideBossText",1.5f);
		Invoke("ShowBoss",2.2f);
	}

	void ShowBoss()
	{
		Camera.main.transform.Find("BossTepach").gameObject.SetActive(true);
		StartCoroutine(AdjustCameraSpeedForBoss());
	}

	void HideBossText()
	{
		GameObject.Find("BossTime").GetComponent<Renderer>().enabled = false;
	}

	IEnumerator AdjustCameraSpeedForBoss()
	{
		while(AircraftBattleGameManager.Instance.speed != bgMovingSpeed)
		{
			yield return null;
			AircraftBattleGameManager.Instance.speed = Mathf.MoveTowards(AircraftBattleGameManager.Instance.speed, bgMovingSpeed, 0.2f);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag.Equals("PlayerBullet"))
		{
			if(health <= 0)
			{
				gameObject.SetActive(false);
				Camera.main.transform.Find("Splendid").GetComponent<Renderer>().enabled = true;
				Transform warpZone = GameObject.Find("WarpZone").transform;
				warpZone.GetComponent<OffsetTexture>().enabled = true;
				warpZone.position = new Vector3(warpZone.position.x,Camera.main.transform.position.y,warpZone.position.z);
				warpZone.parent = Camera.main.transform;
				warpZone.GetComponent<Renderer>().enabled = true;
				Camera.main.transform.position = new Vector3(0,0,-30);
				// Invoke("StartLoadNewStage",1f);
				AircraftBattleGameManager.Instance.bossTime = false;
			}
			else
			{
				health-=5;
				col.gameObject.SetActive(false);
				transform.Find("HealthBar").localScale -= new Vector3(1f/80, 0, 0);
				transform.Find("BossHit").GetComponent<Renderer>().enabled = true;
				StartCoroutine(TurnOffDamageSprite());
			}
		}
	}

	IEnumerator TurnOffDamageSprite()
	{
		yield return new WaitForSeconds(0.15f);
		transform.Find("BossHit").GetComponent<Renderer>().enabled = false;
	}

	void StartLoadNewStage()
	{
		Camera.main.transform.Find("Splendid").GetComponent<Renderer>().enabled = false;
		// LevelGenerator.Instance.DestroyTerrains();
	}
}
