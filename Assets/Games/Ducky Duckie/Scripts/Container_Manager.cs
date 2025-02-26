using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container_Manager : MonoBehaviour {

    [Header("Some Values")]
    public int initialBalls;
    public TextMesh T_Balls_Amount;
    public static int ActualBallsAmount;

    [Header("Balls Prefabs")]
    public GameObject BallPrefab;
    public Transform SpawnPos;
    public Transform RealBallContainer;

    [Header("Spawning Settings")]
    public int maxSpawnedBalls;
    public float minXForce;
    public float minYForce;
    public float maxXForce;
    public float maxYForce;

    [Header("Time Variables")]
    public float spawnDelay;
    float time = 0;

    [SerializeField]private ParticleSystem ActualBallsAmountEF;
    
	void Start () 
    {
        //Set the text of the textmesh
        SetBallsAmountText(initialBalls);
        //Set the actual Balls Amount
        ActualBallsAmount = initialBalls;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (DuckyGameManager.gameOn)
        {

            if (time < spawnDelay)
                time += Time.deltaTime;
            else
            {
                //Spawn some ducks / balls
                SpawnBalls();

                //Reset time
                time = 0;
            }
        }
        if(ActualBallsAmount == 0 && RealBallContainer.childCount == 0)
        {
             DuckyGameManager.instance.GameOver();
        }
    }

    public void ResetBallsAmount(){
        ActualBallsAmount = initialBalls;
        SetBallsAmountText(ActualBallsAmount);
    }

    void SpawnBalls()
    {
        if(ActualBallsAmount > 0)
        { 
        int spawnAmount = Random.Range(1, maxSpawnedBalls+1);

            if (spawnAmount > 1)
                StartCoroutine(SpawnWithDelay(Mathf.Min(spawnAmount, ActualBallsAmount)));
            else
            {
                //Spawn a ball
                GameObject ballInstance = Instantiate(BallPrefab, SpawnPos.position, Quaternion.identity, RealBallContainer);

                //Add a random force
                float x = Random.Range(minXForce, maxXForce);
                float y = Random.Range(minYForce, maxYForce);

                //Set the vector
                Vector2 dir = new Vector2(x, y);

                //Apply the force
                ballInstance.GetComponent<Rigidbody2D>().AddForce(dir);

                //Update the balls amount
                ActualBallsAmount--;
                SetBallsAmountText(ActualBallsAmount);
            }
        }
    }

    IEnumerator SpawnWithDelay(int amount)
    {

        //Add a random force
        float x = Random.Range(minXForce, maxXForce);
        float y = Random.Range(minYForce, maxYForce);

        //Set the vector
        Vector2 dir = new Vector2(x, y);
        
        for (int i = 0; i < amount; i++)
        {
            //Spawn a ball
            GameObject ballInstance = Instantiate(BallPrefab, SpawnPos.position, Quaternion.identity, RealBallContainer);

            //Apply the force
            ballInstance.GetComponent<Rigidbody2D>().AddForce(dir);

            //Update the balls amount
            ActualBallsAmount--;
            SetBallsAmountText(ActualBallsAmount);

            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            //Add one to the balls text
            ActualBallsAmount++;

            //Update the text
            SetBallsAmountText(ActualBallsAmount);
            ActualBallsAmountEF.Play();
            //Destroy the ball Allah yar7amlek lwalidin
            Destroy(collision.gameObject);

        }
    }
    // UTILITIES
    void SetBallsAmountText(int amount)
    {
        //Set the text mesh
        T_Balls_Amount.text = "x" + amount;
    }

}
