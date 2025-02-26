using UnityEngine;
using System.Collections;

public class LongBullets : MonoBehaviour {

	public float timeBetweenSpawn = 0.35f;
	// Use this for initialization
	void Start () {
		//		transform.parent = GameObject.Find("Main Camera").transform;
		StartCoroutine(SpawnWave());
	}
	
	// Update is called once per frame
//	void Update()
//	{
//		if(GameManager.canMove)
//			transform.Translate(0, GameManager.Instance.speed * Time.deltaTime, 0);
//	}
	
	IEnumerator SpawnWave()
	{
		int numberOfBulletsInWave = transform.childCount;
		for(int i=0;i<numberOfBulletsInWave;i++)
		{

				if(transform.parent.parent.parent.GetChild(0).gameObject.activeSelf)
				{
					transform.GetChild(i).GetChild(0).GetComponent<Animation>().Play();
					SoundManager.Instance.Play_BossMainGunFire();
				}
				


			yield return new WaitForSeconds(timeBetweenSpawn);
		}
		transform.parent=null;
		yield return new WaitForSeconds(transform.GetChild(0).GetChild(0).GetComponent<Animation>().clip.length+0.5f);
		Destroy(this.gameObject);
	}
}
