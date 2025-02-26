using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/****************************************************
    文件：GameController.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace  FlyBird
{
    public class GameController : MonoBehaviour
    {
           public static GameController Instance;
           private bool IsGameover=false;
           private int Score=0;
           [SerializeField] private ScorePanel ScorePanel;
           [FormerlySerializedAs("audioManager")] public FlyBirdAudioManager flyBirdAudioManager;
           public Action<int> AddAcoreAction;

           private int Count = 0;
           
           public AddScoreEF AddScoreef;
           
           
           [SerializeField]private Bird[] birds;

           private Bird bird;
           private void Awake()
           {
               if (Instance == null)
               {
                   Instance = this;
               }
               else
               {
                   Destroy(gameObject);
               }
           }

           private void Start()
           {
               IsGameover = false;
               Count = 0;
               GameTimeManager.instance.TimeOverAction = TimeOver;
               bird=Instantiate(birds[Random.Range(0,birds.Length)],Vector3.up,Quaternion.identity);
               AddScoreef = bird.addScoreEF;
           }

           public void TimeOver()
           {
               CommonUI.instance.StartCouterColdDown();
               PoolManager.Instance.ClearPoolDic();
           }
           public void AddScore(int addScore=1)
           {
               Score+=addScore;
               Count++;
               flyBirdAudioManager.PlayerAddScoreSound();
               ScorePanel.setScore(Score);
               AddAcoreAction?.Invoke(Count);
           }

           public void GameOver()
           {
               IsGameover = true;
               flyBirdAudioManager.PlayerDieSound();
               PoolManager.Instance.ClearPoolDic();
               Invoke("LoadScene",2);
           }

           public void LoadScene()
           {
               CommonUI.instance.BackMainPanel_OPen(true);
           }

           public bool GetIsGameover()
           {
               return IsGameover;
           }

           private void OnDestroy()
           {
               PoolManager.Instance.ClearPoolDic();
           }
    }
    
}

