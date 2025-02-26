using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：PlatesCounterVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


namespace  Crzaykitchen
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform CounterTransform;
        [SerializeField] private Transform plateVisualPrefab;
        
        private List<GameObject> plateVList = new List<GameObject>();
        private void Start()
        {
            platesCounter.OnPlatesSpawn = SpawnPlates;
            platesCounter.OnPlatesRemoved = RemovePlates;
        }

        private void SpawnPlates()
        {
               Transform plat = Instantiate(plateVisualPrefab, CounterTransform);
               plateVList.Add(plat.gameObject);
               plat.localPosition = new Vector3(0,(plateVList.Count-1)*0.1f,0);
        }

        private void RemovePlates()
        {
            GameObject remove= plateVList[plateVList.Count - 1];
            plateVList.RemoveAt(plateVList.Count - 1);
            Destroy(remove);
        }
    } 
}

