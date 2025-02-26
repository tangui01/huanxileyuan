using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TankPlayerManager : MonoBehaviour
{
    public static int lifeValue1 = 3; 
    public  int lifeValue2 = 3;
    public int vestigial = 25; //敌人剩余数
    [HideInInspector] public bool isDead1;
    [HideInInspector] public bool isDead2;
    [HideInInspector] public bool isDefeat;
    [HideInInspector] public bool isIronHeart;
    [HideInInspector] public bool timeFreeze;
    [HideInInspector] public bool isDestoryAll;
    public static int tankLevel1 = 0; //子弹等级
    public  int tankLevel2 = 0;
    public GameObject born;
    public GameObject isDefeatUI;
    public Text vestigialText;
    public Text PlayerLifeValueText;
    public Text Player2LifeValueText;
    public GameObject player2;


    private float freezeTime = 4.0f;

    private static TankPlayerManager instance;

    public static TankPlayerManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private Heart heart;


    private void Awake()
    {
        if (Select.selectPlayer == 2)
        {
            player2.SetActive(true);
        }

        vestigial += MapCreater._scene * 5; //怪物数量根据关卡提升
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
    }

    IEnumerator ResetPlayGame()
    {
        isDefeat = false;
        yield return new WaitForSeconds(0.5f);
        CommonUI.instance.BackMainPanel_OPen();
        yield return new WaitForSeconds(1f);
        MapCreater.Score = 0;
        FindObjectOfType<MapCreater>().PlayGame();
    }

    // Update is called once per frame
    void Update()
    {
        DestoryAll();
        if (lifeValue1 <= 0 && lifeValue2 <= 0)
        {
            //游戏失败
            isDefeat = true;
            FindObjectOfType<MapCreater>().itemList.Clear();
            MapCreater._scene = 1;
        }

        if (isDefeat)
        {
            isDefeatUI.SetActive(true);
            StartCoroutine(ResetPlayGame());
            tankLevel1 = 0;
            tankLevel2 = 0;
            return;
        }

        if (heart == null)
            heart = FindObjectOfType<Heart>();
        if (isDead1 && !heart.GameOver)
        {
            Recover1();
        }

        if (isDead2 && !heart.GameOver)
        {
            Recover2();
        }

        vestigialText.text = vestigial.ToString();
        PlayerLifeValueText.text = lifeValue1.ToString();
        Player2LifeValueText.text = lifeValue2.ToString();
    }

    private void FixedUpdate()
    {
        FreezeEnemy();
    }

    //复活玩家1
    private void Recover1()
    {
        ;
        if (lifeValue1 - 1 < 0)
        {
            return;
        }

        lifeValue1--;
        GameObject go = Instantiate(born, new Vector3(8, 0, 0), Quaternion.identity);
        go.GetComponent<Born>().creatPlayer1 = true;
        isDead1 = false;
    }

    //复活玩家2
    private void Recover2()
    {
        if (lifeValue2 - 1 < 0)
        {
            return;
        }

        lifeValue2--;
        GameObject go = Instantiate(born, new Vector3(12, 0, 0), Quaternion.identity);
        go.GetComponent<Born>().creatPlayer2 = true;
        isDead2 = false;
    }


    private void FreezeEnemy()
    {
        if (timeFreeze)
        {
            freezeTime -= Time.fixedDeltaTime;
            if (freezeTime <= 0)
            {
                timeFreeze = false;
                freezeTime = 4.0f;
            }
        }
    }

    //清除所有敌人
    private void DestoryAll()
    {
        if (isDestoryAll)
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("Enemy");
            if (tanks.Length > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    tanks[i].GetComponent<TankEnemy>().SendMessage("Die");
                }
            }
            else
            {
                for (int i = 0; i < tanks.Length; i++)
                {
                    tanks[i].GetComponent<TankEnemy>().SendMessage("Die");
                }
            }
            isDestoryAll = false;
        }
    }
}