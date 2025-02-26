using UnityEngine;
using System.Collections;

public class FireBallPosition : MonoBehaviour {
	int randomNumber,randomNumberOld=-1;
	// Use this for initialization
	void Start () {
		int numberOfEnemiesInWave = transform.childCount;


		for(int i=0;i<numberOfEnemiesInWave;i++)
		{
			do
			{
				randomNumber = Random.Range(0,5);
			}
			while(randomNumberOld==randomNumber);
			
			randomNumberOld=randomNumber;
			transform.GetChild(i).localPosition = Vector3.right*Random.Range (-25f,25f);
		}
	}
	

}
