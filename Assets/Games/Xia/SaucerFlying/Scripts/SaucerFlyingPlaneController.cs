using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SaucerFlying
{
    public class SaucerFlyingPlaneController : MonoBehaviour
    {
        private float timeLast;
        public GameObject[] listEnemys;
        public GameObject coin;

        public int maxInstanceEnemys;
        public List<Material> materials = new List<Material>();
        // Use this for initialization
        private GameObject[] allEnemys;

        private void Awake()
        {
            allEnemys = new GameObject[5];
        }

        public void CreateEnemy()
        {
            int indexEnemy = Random.Range(0, listEnemys.Length);
            if (allEnemys[indexEnemy] == null)
            {
                Vector3 posIns = new Vector3(transform.position.x, 0.2f, transform.position.z);
                posIns.x = Random.Range(-SaucerFlyingRoadManager.Instance.tunnelWidth / 2 + 0.5f, SaucerFlyingRoadManager.Instance.tunnelWidth / 2 - 0.5f);
                posIns.y = Random.Range(-SaucerFlyingRoadManager.Instance.tunnelHeight / 2 + 0.5f, SaucerFlyingRoadManager.Instance.tunnelHeight / 2 - 0.5f);
                allEnemys[indexEnemy] = Instantiate(listEnemys[indexEnemy], posIns, Quaternion.identity);
                allEnemys[indexEnemy].name = "Enemy_" + indexEnemy;
                allEnemys[indexEnemy].transform.parent = this.transform;
                for (int i = 0; i < allEnemys[indexEnemy].transform.childCount; i++)
                {
                    if( !allEnemys[indexEnemy].transform.GetChild(i).tag.Equals("Enemy") )
                        continue;
                    allEnemys[indexEnemy].transform.GetChild(i).GetComponent<MeshRenderer>().material = 
                        materials[SaucerFlyingRoadManager.Instance.ObsRand % materials.Count];
                }
            }
            else
            {
                // allEnemys[indexEnemy].transform.position = posIns;
                allEnemys[indexEnemy].SetActive(true);
            }
        }

        public void CreateItem()
        {
            Vector3 posIns = new Vector3(transform.position.x, 0.1f, transform.position.z);
            posIns.y = Random.Range(-0.2f, 0.5f);
            posIns.x = Random.Range(-1.0f, 1.0f);
            GameObject obj = Instantiate(coin, posIns, Quaternion.identity);
            obj.name += "object";
            obj.transform.parent = this.transform;
        }
    }

}