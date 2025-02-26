using UnityEngine;
using System.Collections;

public class GroundEnemiesActivation : MonoBehaviour {

	int currentStage;
	int currentTerrainInStage;
	public int randomNumber = 0;
	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag.Equals("Spawn"))
		{
			currentStage = LevelGenerator.currentStage;
			currentTerrainInStage = LevelGenerator.terrainsPassed;

			switch (currentTerrainInStage)
			{
			case 0: case 1: case 2:        
				// Do Something
//				if(currentStage<3)
//				{
					randomNumber = Random.Range(2,transform.parent.childCount/2);
//				}
//				else if(currentStage<5)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<7)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<9)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<11)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else
//				{
//					randomNumber = Random.Range(1,7);
//				}
				break;
			case 3: case 4: case 5: case 6: 
				// Do Something
//				if(currentStage<3)
//				{
				randomNumber = Random.Range(3,transform.parent.childCount/2);
//				}
//				else if(currentStage<5)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<7)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<9)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<11)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else
//				{
//					randomNumber = Random.Range(1,7);
//				}
				break;
			case 7: case 8:
				// Do Something
//				if(currentStage<3)
//				{
				randomNumber = Random.Range(2,transform.parent.childCount);
//				}
//				else if(currentStage<5)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<7)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<9)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else if(currentStage<11)
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				else
//				{
//					randomNumber = Random.Range(1,7);
//				}
				break;
			default:
				// Do Something
				break;
			}
//			Debug.Log("Postavi neprijatelje na "+transform.parent.parent.name+" ima ukupo neprijatelja na terenu "+transform.parent.childCount);
			for(int i=0;i<randomNumber;i++)
			{
				transform.parent.GetChild(i).gameObject.SetActive(true);
			}
		}
		else if(col.tag.Equals("EnemyKill"))
		{
//			Debug.Log("KILL THEM ALL");
			transform.parent.gameObject.SetActive(false);
		}
		
	}
}
