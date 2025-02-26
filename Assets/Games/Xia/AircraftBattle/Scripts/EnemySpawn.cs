using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

	public float timeBetweenSpawn = 0.35f;
	// Use this for initialization
	void Start () {
//		transform.parent = GameObject.Find("Main Camera").transform;
		StartCoroutine(SpawnWave());
	}
	
	// Update is called once per frame
	void Update()
	{
		if(AircraftBattleGameManager.canMove)
			transform.Translate(0, AircraftBattleGameManager.Instance.speed * Time.deltaTime, 0);
	}

	IEnumerator SpawnWave()
	{
		int numberOfEnemiesInWave = transform.childCount;
		for(int i=0;i<numberOfEnemiesInWave;i++)
		{
			transform.GetChild(i).GetChild(0).GetComponent<Animation>().Play();
			yield return new WaitForSeconds(timeBetweenSpawn);
		}
		yield return new WaitForSeconds(transform.GetChild(0).GetChild(0).GetComponent<Animation>().clip.length+0.5f);
		Destroy(this.gameObject);
	}


}
