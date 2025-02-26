using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WGM;

public class MapCreater : MonoBehaviour
{
    //地图组件
    //0.老家 1.墙 2.障碍 3.出生效果 4.河流 5.草 6.空气墙
    public GameObject[] item;
    [HideInInspector]
    public List<GameObject> itemList = new List<GameObject>();
    public List<GameObject> replaceList = new List<GameObject>(); //用来储存钢铁家园
    public static int _scene = 1; //关卡号
    public Text sceneCount;
    public GameObject Scene;
    [HideInInspector]
    public GameObject[] Enemycounts;
    private bool isCheckScene = true;
    private AudioSource audioSource;
    private float volume;
    public static int Score = 0;
    public Text ScoreText;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume * LibWGM.machine.SeVolume / 10;
        itemList.Clear();
        sceneCount.text = _scene.ToString()+"";
        TankPlayerManager.Instance.timeFreeze = true;
        Scene.SetActive(true);
        Invoke("LoadComplate", 2.0f);
        Invoke("InitMap", 2.0f);
    }

    private void Update()
    {
        if (TankPlayerManager.Instance.vestigial <= 0)
        {
            StartCoroutine(WaitCheck_Scene());
        }
        IronHeart();
        Enemycounts = GameObject.FindGameObjectsWithTag("Enemy");
        ScoreText.text = Score.ToString();
    }

    IEnumerator WaitCheck_Scene()
    {
        if(!isCheckScene)
            yield break; 
        isCheckScene = false;
        yield return new WaitForSeconds(1f);
        itemList.Clear();
        SceneManager.LoadScene("TankGame");
        _scene++;
        sceneCount.text = _scene.ToString()+"";
        isCheckScene = true;
    }
    public void PlayAudio()
    {
        audioSource.volume = volume;
        audioSource.Play();
    }
    public void PlayGame()
    {
        StartCoroutine(WaitPlayGame());
    }

    IEnumerator WaitPlayGame()
    {
        itemList.Clear();
        SceneManager.LoadScene("TankGame");
        sceneCount.text = _scene.ToString();
        yield return null;
    }
    
    void IronHeart()
    {
        if (TankPlayerManager.Instance.isIronHeart == true)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                    for (int j = 0; j < 3; j++)
                    {
                        replaceList.Add(Instantiate(item[2], new Vector3(9 + j, 1, 0), Quaternion.identity));
                    }
                else if (i == 0)
                {
                    replaceList.Add(Instantiate(item[2], new Vector3(9, 0, 0), Quaternion.identity));
                    replaceList.Add(Instantiate(item[2], new Vector3(11, 0, 0), Quaternion.identity));
                }
            }

            HeartDel();
            for (int i = 0; i < replaceList.Count; i++)
            {
                Destroy(replaceList[i], 5f);
            }

            Invoke("HeartLively", 5.0f);
        }

        TankPlayerManager.Instance.isIronHeart = false;
    }

    private void HeartDel()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].transform.position == new Vector3(9, 1, 0) ||
                itemList[i].transform.position == new Vector3(10, 1, 0) ||
                itemList[i].transform.position == new Vector3(11, 1, 0)
                || itemList[i].transform.position == new Vector3(9, 0, 0) ||
                itemList[i].transform.position == new Vector3(11, 0, 0))
            {
                itemList[i].gameObject.SetActive(false);
            }
        }
    }

    private void HeartLively()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].transform.position == new Vector3(9, 1, 0) ||
                itemList[i].transform.position == new Vector3(10, 1, 0) ||
                itemList[i].transform.position == new Vector3(11, 1, 0)
                || itemList[i].transform.position == new Vector3(9, 0, 0) ||
                itemList[i].transform.position == new Vector3(11, 0, 0))
            {
                itemList[i].gameObject.SetActive(true);
            }
        }
    }

    private GameObject CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        int j = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            if (createPosition == itemList[i].transform.position)
            {
                j++;
                break;
            }
        }

        if (j == 0)
        {
            GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
            itemGo.transform.SetParent(gameObject.transform);
            return itemGo;
        }

        return null;
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
            for (int i = 0; i < itemList.Count; i++)
            {
                if (createPosition == itemList[i].transform.position)
                    j++;
            }

            if (j == 0) return createPosition;
        }
    }


    //产生敌人的方法
    private void CreateEnemy()
    {
        if (Enemycounts.Length >=10+ _scene * 2)
        {
            return;
        }
        if (TankPlayerManager.Instance.vestigial <= Enemycounts.Length)
        {
            return;
        }
        // X:0~20,Y:14~16;
        Vector3 EnemyPos = new Vector3(Random.Range(0, 21), Random.Range(14, 17), 0);
        // int num = Random.Range(0, 3);
        // if(num==0)
        // {
        //     EnemyPos = new Vector3(0, Random.Range(15,17), 0);
        // }
        // if (num == 1)
        // {
        //     EnemyPos = new Vector3(10, 16, 0);
        // }
        // if (num == 2)
        // {
        //     EnemyPos = new Vector3(20, 16, 0);
        // }
        CreateItem(item[3], EnemyPos, Quaternion.identity);
    }

    private void InitMap()
    {
        //实例化老家
        CreateItem(item[0], new Vector3(10, 0, 0), Quaternion.identity);
        //用墙把老家围起来
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
                for (int j = 0; j < 3; j++)
                {
                    itemList.Add(CreateItem(item[1], new Vector3(10 + (j - 1) * 1, 1, 0), Quaternion.identity));
                }
            else if (i == 0)
            {
                itemList.Add(CreateItem(item[1], new Vector3(9, 0, 0), Quaternion.identity));
                itemList.Add(CreateItem(item[1], new Vector3(11, 0, 0), Quaternion.identity));
            }
        }

        //实例化外围空气墙
        for (int j = 0; j < 23; j++)
        {
            CreateItem(item[6], new Vector3(-1 + j * 1, -1, 0), Quaternion.identity);
        }

        for (int j = 0; j < 18; j++)
        {
            CreateItem(item[6], new Vector3(21, j * 1, 0), Quaternion.identity);
        }

        for (int j = 0; j < 22; j++)
        {
            CreateItem(item[6], new Vector3(j * 1, 17, 0), Quaternion.identity);
        }

        for (int j = 0; j < 18; j++)
        {
            CreateItem(item[6], new Vector3(-1, j * 1, 0), Quaternion.identity);
        }

        //初始化玩家
        if (Select.selectPlayer == 1)
        {
            GameObject go = Instantiate(item[3], new Vector3(8, 0.02f, 0), Quaternion.identity);
            go.GetComponent<Born>().creatPlayer1 = true;
        }

        if (Select.selectPlayer == 2)
        {
            GameObject go = Instantiate(item[3], new Vector3(8, 0.02f, 0), Quaternion.identity);
            go.GetComponent<Born>().creatPlayer1 = true;
            GameObject go2 = Instantiate(item[3], new Vector3(12, 0.02f, 0), Quaternion.identity);
            go2.GetComponent<Born>().creatPlayer2 = true;
        }

        //产生敌人
        CreateItem(item[3], new Vector3(0, 16, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(10, 16, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(20, 16, 0), Quaternion.identity);


        InvokeRepeating("CreateEnemy", 2, Mathf.Max(2.5f-0.2f*_scene+Enemycounts.Length*0.1f,1.5f));

        //实例化地图
        for (int i = 0; i < 60; i++)
        {
            itemList.Add(CreateItem(item[1], CreateRandomPosition(), Quaternion.identity));
        }

        for (int i = 0; i < 20; i++)
        {
            itemList.Add(CreateItem(item[2], CreateRandomPosition(), Quaternion.identity));
        }

        for (int i = 0; i < 20; i++)
        {
            itemList.Add(CreateItem(item[4], CreateRandomPosition(), Quaternion.identity));
        }

        for (int i = 0; i < 20; i++)
        {
            itemList.Add(CreateItem(item[5], CreateRandomPosition(), Quaternion.identity));
        }
    }

    private void LoadComplate()
    {
        Scene.SetActive(false);
        TankPlayerManager.Instance.timeFreeze = false;
    }
}