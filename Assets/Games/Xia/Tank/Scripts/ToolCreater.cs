using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToolCreater : MonoBehaviour {

    
    public GameObject[] item;
    MapCreater mapCreater;
    private void Awake()
    {
        Invoke("InitTool",3.0f);
        InvokeRepeating("InitTool", 10, 10);
    }

    private void Start()
    {
        mapCreater = FindObjectOfType<MapCreater>();
    }

    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        Destroy(itemGo, 8f);
    }


    //产生随机位置的方法
    private Vector3 CreateRandomPosition()
    {
        //不生成x=0 20 y=0 16(场景边缘位置)
        while (true)
        {
            Vector3 createPosition = new Vector3(Random.Range(1, 20), Random.Range(1, 16), 0);
            //判定位置列表中是否有这个位置
            int j = 0;
                for (int i = 0; i < mapCreater.itemList.Count; i++)
                {
                    if (createPosition == mapCreater.itemList[i].transform.position)
                        j++;
                }
            if (j == 0) return createPosition;
        }
    }
    void InitTool()
    {
        if (TankPlayerManager.Instance.vestigial<=5)
        {
           return; 
        }
        int i = Random.Range(0, 7);
        if(i>=5)
            CreateItem(item[5], CreateRandomPosition(), Quaternion.identity);
        else
            CreateItem(item[i], CreateRandomPosition(), Quaternion.identity);
        
    }

}
