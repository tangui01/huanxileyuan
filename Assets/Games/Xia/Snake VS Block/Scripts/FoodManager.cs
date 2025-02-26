using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeVSBlock
{
    public class FoodManager : MonoBehaviour
    {

        [Header("Snake Manager")] SnakeMovement SM;

        [Header("Food Variables")] public GameObject FoodPrefab;
        public int appearanceFrequency;

        [Header("Time to spawn Management")] public float timeBetweenFoodSpawn;
        private float thisTime;

        // Use this for initialization
        void Start()
        {

            //Set the Snake Movement
            SM = GameObject.FindGameObjectWithTag("SnakeManager").GetComponent<SnakeMovement>();

            //Spawn the initial Food
            SpawnFood();
        }

        // Update is called once per frame
        void Update()
        {

            if (SnakeVSBlockGameController.gameState == SnakeVSBlockGameController.GameState.GAME)
            {
                if (thisTime < timeBetweenFoodSpawn)
                {
                    thisTime += Time.deltaTime;
                }
                else
                {
                    SpawnFood();
                    thisTime = 0;
                }
            }
        }

        public void SpawnFood()
        {
            for (int i = 0; i < 5; i++)
            {

                //Set the position to spawn the food
                float x = -5.24f + i * 2.62f;
                float y = 0;

                if (SM.transform.childCount > 0)
                {
                    y = (int)SM.transform.GetChild(0).position.y + 1.3333f * 3 + 10;
                }

                Vector3 spawnPos = new Vector3(x, y, 0);



                //Random Number Management
                int number;

                if (appearanceFrequency < 100)
                    number = Random.Range(0, 100 - appearanceFrequency);
                else
                    number = 1;



                //Actual Spawning step
                GameObject boxInstance;

                if (number == 1)
                {
                    boxInstance = Instantiate(FoodPrefab, spawnPos, Quaternion.identity, transform);
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, 0.3f);
                    for (int j = 0; j < colliders.Length; j++)
                    {
                        if (colliders[j].CompareTag("Box") || colliders[j].CompareTag("SimpleBox"))
                        {
                            Destroy(boxInstance);
                            break;
                        }

                    }
                }

            }
        }
    }
}