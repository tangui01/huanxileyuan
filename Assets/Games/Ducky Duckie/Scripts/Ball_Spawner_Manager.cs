using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Spawner_Manager : MonoBehaviour {

    [Header("Spawning Settings")]
    public GameObject BallPrefab;
    public float delay;
    public Transform SpawnPositionTransform;
    public int initialBalls;

	// Use this for initialization
	void Start () {

        //Spawn the Initial Balls
        SpawnInitialBalls();

	}
	
    void SpawnInitialBalls()
    {
        for (int i = 0; i < initialBalls; i++)
        {
            SpawnBall();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    void SpawnBall()
    {
        float x = Random.Range(-0.5f, 0.5f);

        Vector3 randP = new Vector3(x,0,0);

        Instantiate(BallPrefab, SpawnPositionTransform.position + randP, Quaternion.identity, transform);
    }


}
