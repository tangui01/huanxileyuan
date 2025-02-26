using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualControlPoolManage : MonoBehaviour
{
    public Transform carsCenter;
    public Transform poolIndex;
    public Transform pools;
    public List<GameObject> Bottlenecks = new List<GameObject>();
    private float distance = 0;
    private Vector3 producePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Abs(carsCenter.position.z - poolIndex.position.z);
        if (distance < 30)
        {
            poolIndex.position += new Vector3(0, 0, 10);
            int ran = Random.Range(0, Bottlenecks.Count);
            for (int i = 0; i <= pools.childCount; i++)
            {
                if (i == pools.childCount)
                {
                    GameObject obj = Instantiate(Bottlenecks[ran], poolIndex.position, Quaternion.identity);
                    obj.transform.parent = pools;
                    if (obj.tag.Equals("Blocker"))
                    {
                        pools.GetChild(i).position +=new Vector3(Random.Range(-1.5f,1.5f), 0, 0); 
                    }
                    break;
                }
                
                if (pools.GetChild(i).gameObject.activeSelf == false && pools.GetChild(i).tag.Equals(Bottlenecks[ran].tag))
                {
                    pools.GetChild(i).gameObject.SetActive(true);
                    pools.GetChild(i).transform.position = poolIndex.position;
                    if (pools.GetChild(i).tag.Equals("Blocker"))
                    {
                        pools.GetChild(i).position +=new Vector3(Random.Range(-1.5f,1.5f), 0, 0); 
                    }
                    break;
                }
            }
        }
    }
}
