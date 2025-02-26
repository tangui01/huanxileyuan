using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeVSBlock
{
    public class BlocksManager : MonoBehaviour
    {
        [Header("Snake Manager")] public SnakeMovement SM;
        public float distanceSnakeBarrier;

        [Header("Block Prefab")] public GameObject BlockPrefab;

        [Header("Time to spawn Management")] public float minSpawnTime;
        public float maxSpawnTime;
        private float thisTime;
        private float randomTime;

        [Header("Snake Values for Spawning")] public int minSpawnDist;
        Vector2 previousSnakePos;
        public List<Vector3> SimpleBoxPositions = new List<Vector3>();
        [Header("X间距")] public float X;
        [Header("左侧第一个X值")] public float firstLeftBlock;

        // Use this for initialization
        void Start()
        {
            // 初始化当前计时时间
            thisTime = 0;
            // 设置一个随机的生成方块时间间隔
            randomTime = Random.Range(minSpawnTime, maxSpawnTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (SnakeVSBlockGameController.gameState == SnakeVSBlockGameController.GameState.GAME)
            {
                if (thisTime < randomTime)
                {
                    thisTime += Time.deltaTime;
                }
                else
                {
                    SpawnBlock();
                    thisTime = 0;
                    randomTime = Random.Range(minSpawnTime, maxSpawnTime);
                }

                // 阻挡物生成管理逻辑
                if (SM.transform.childCount > 0)
                {
                    if (SM.transform.GetChild(0).position.y - previousSnakePos.y > minSpawnDist)
                    {
                        SpawnBarrier();
                    }
                }
            }
        }

        public void SpawnBarrier()
        {

            for (int i = 0; i < 5; i++)
            {
                float x = firstLeftBlock + i * X;
                float y = 0;

                if (SM.transform.childCount > 0)
                {
                    y = (int)SM.transform.GetChild(0).position.y + distanceSnakeBarrier;
                }

                Vector3 spawnPos = new Vector3(x, y, 0);

                GameObject boxInstance = Instantiate(BlockPrefab, spawnPos, Quaternion.identity, transform);

                // 保存蛇头位置
                if (SM.transform.childCount > 0)
                {
                    previousSnakePos = SM.transform.GetChild(0).position;
                }
            }
        }

        public void SpawnBlock()
        {


            int random;
            random = Random.Range(0, 5);

            float x = firstLeftBlock + random * X;
            float y = 0;

            if (SM.transform.childCount > 0)
            {
                y = (int)SM.transform.GetChild(0).position.y + distanceSnakeBarrier;
            }

            Vector3 spawnPos = new Vector3(x, y, 0);

            // 布尔值，用于判断是否可以在该位置生成方块
            bool canSpawnBlock = true;

            // 如果位置列表为空，则添加当前位置
            if (SimpleBoxPositions.Count == 0)
            {
                SimpleBoxPositions.Add(spawnPos);
            }
            else
            {
                // 检查位置是否已被使用
                for (int k = 0; k < SimpleBoxPositions.Count; k++)
                {
                    if (spawnPos == SimpleBoxPositions[k])
                    {
                        canSpawnBlock = false;
                    }
                }
            }

            GameObject boxInstance;

            if (canSpawnBlock)
            {
                // 添加位置到列表
                SimpleBoxPositions.Add(spawnPos);

                boxInstance = Instantiate(BlockPrefab, spawnPos, Quaternion.identity, transform);

                boxInstance.name = "SimpleBox";
                boxInstance.tag = "SimpleBox";

                // 更改图层以便与普通方块碰撞
                boxInstance.layer = LayerMask.NameToLayer("Default");
                // boxInstance.GetComponent<BoxCollider2D>().isTrigger = true;

                boxInstance.AddComponent<Rigidbody2D>();
                boxInstance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public void SetPreviousSnakePosAfterGameover()
        {
            Invoke("PreviousPosInvoke", 0.5f);
        }

        void PreviousPosInvoke()
        {
            previousSnakePos.y = SM.transform.GetChild(0).position.y;
        }

    }
}