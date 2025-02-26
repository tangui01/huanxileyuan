using System.Collections.Generic;
using UnityEngine;

namespace SaucerFlying
{
    public class PartOfRoadManager : MonoBehaviour
    {
        public GameObject left, right, top, bot;
        private void Start()
        {
            // set up bound of path
            left.transform.localPosition = new Vector3(-SaucerFlyingRoadManager.Instance.tunnelWidth / 2, 0, 0);
            right.transform.localPosition = new Vector3(SaucerFlyingRoadManager.Instance.tunnelWidth / 2, 0, 0);
            top.transform.localPosition = new Vector3(0, SaucerFlyingRoadManager.Instance.tunnelHeight / 2, 0);
            bot.transform.localPosition = new Vector3(0, -SaucerFlyingRoadManager.Instance.tunnelHeight / 2, 0);
        }
        private void FixedUpdate()
        {
            if (SaucerFlyingGameManager.Instance.GameState == GameState.Playing && SaucerFlyingGameManager.Instance.playerController != null)
            {
                if (SaucerFlyingGameManager.Instance.playerController.transform.position.z - transform.position.z > 20)
                {
                    Vector3 temp = transform.position;
                    temp.z = SaucerFlyingRoadManager.Instance.posBegin.z;
                    transform.position = temp;
                    if (transform.GetChild(0).childCount != 0)
                    {
                        for (int i = 0; i < transform.GetChild(0).childCount; i++)
                        {
                            if(transform.GetChild(0).GetChild(i).gameObject.activeSelf)
                                transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                        }
                        
                    }

                    if (Random.Range(0, 0.99f) <= SaucerFlyingGameManager.Instance.coinFrequency)
                    {
                        transform.GetChild(0).gameObject.GetComponent<SaucerFlyingPlaneController>().CreateItem();
                    }
                    else
                    {
                        transform.GetChild(0).gameObject.GetComponent<SaucerFlyingPlaneController>().CreateEnemy();
                    }

                    SaucerFlyingRoadManager.Instance.posBegin.z += 9.97f;
                }
            }
        }
    }
}
