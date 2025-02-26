using System.Collections.Generic;
using UnityEngine;

namespace SaucerFlying
{
    public class SaucerFlyingRoadManager : MonoBehaviour
    {
        static public SaucerFlyingRoadManager Instance { get; private set; }

        public GameObject[] partOfRoads;
        [Header("Reference Objects")]
        public int lenghtRoad;

        [Range(2, 5)]
        public float tunnelWidth = 4f;// for move

        [Range(2, 5)]
        public float tunnelHeight = 4f;// for move

        [HideInInspector]
        public float posX;

        [HideInInspector]
        public float posY;

        [HideInInspector]
        public Vector3 posBegin = Vector3.zero;

        private bool flagX = true;
#pragma warning disable 0414
        private bool flagY = true;
        private float timeLast;
#pragma warning restore 0414
        [HideInInspector]
        public int ObsRand = 0;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            ObsRand = Random.Range(0, 4);
        }

        void Start()
        {
            // position of first plane have instance
            posBegin = new Vector3(0, 0, -5);
            CreateRoad();
        }

        // Update is called once per frame
        void Update()
        {
            if (SaucerFlyingGameManager.Instance.GameState == GameState.Playing)
            {
                if (Time.time - timeLast > 0.03f)
                {
                    timeLast = Time.time;
                    
                    // move to Left
                    if (!flagX)
                        MovePosXLeft();
                    
                    // move to Right
                    if (flagX)
                        MovePosXRight();
                }

            }
        }
        void CreateRoad()
        {
            posX = Random.Range(-20, 20);

            posY = 5;
            int rand = Random.Range(0, partOfRoads.Length);
            for (int i = 0; i < lenghtRoad; ++i)
            {
                GameObject obj = Instantiate(partOfRoads[rand], posBegin, Quaternion.identity);
                obj.transform.parent = this.transform;
                obj.name = obj.name + i;

                // Instance Enemy
                if (i > 5)
                {
                    if (Random.Range(0, 1.01f) <= SaucerFlyingGameManager.Instance.coinFrequency)
                        obj.transform.GetChild(0).GetComponent<SaucerFlyingPlaneController>().CreateItem();
                    else
                        obj.transform.GetChild(0).GetComponent<SaucerFlyingPlaneController>().CreateEnemy();
                }

                posBegin.z += 10;
            }
        }
        void MovePosXLeft()
        {
            posX -= Time.deltaTime;

            if (posX < -20)
                flagX = true;// collided with Left
        }
        void MovePosXRight()
        {
            posX += Time.deltaTime;

            if (posX > 20)
                flagX = false;
        }
        void MovePosYTop()
        {
            posY += Time.deltaTime;

            if (posY > 3)
                flagY = true;// collided with Top
        }
        void MovePosYDow()
        {
            posY -= Time.deltaTime;

            if (posY < -4)
                flagY = false;// collided with Top
        }

    }
}
