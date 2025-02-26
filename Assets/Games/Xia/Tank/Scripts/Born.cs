using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour {


    public GameObject[] playerPrefab;
    public GameObject[] enemyPrefabs;
    public bool creatPlayer1;
    public bool creatPlayer2;

    // Use this for initialization
    void Start () {
        Invoke("BornTank", 0.8f);
        Destroy(gameObject, 0.8f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
     
    private void BornTank()
    {
        if(creatPlayer1)
        {
            Instantiate(playerPrefab[0], transform.position, Quaternion.identity);
        }
        else if (creatPlayer2)
        {
            Instantiate(playerPrefab[1], transform.position, Quaternion.identity);
        }
        else 
        {
            int num = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[num], transform.position, Quaternion.identity);
        }
    }
}
