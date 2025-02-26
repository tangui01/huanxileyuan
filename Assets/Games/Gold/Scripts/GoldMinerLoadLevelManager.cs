using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：LoadLevelManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：关卡生成
*****************************************************/
[Serializable]
public class GoldMinerLevel
{
    public Sprite bg_Top;
    public Sprite bg_Bottom;
}
[Serializable]
public class GoldMinerLevelmineral
{
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<mineral> minerals = new List<mineral>();
}

[Serializable]
public class mineral
{
    public GameObject prefab;
    public List<Vector3> positions;
    public Transform parent;
}
public class GoldMinerLoadLevelManager : MonoBehaviour
{
    public static GoldMinerLoadLevelManager instance;
    public SpriteRenderer bg_Top;
    public SpriteRenderer bg_Bottom;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<GoldMinerLevel> gold_minerBGimgS;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<GoldMinerLevelmineral> goldMinerLevelminerals;
    
    public List<GameObject> Curentminerals;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Button("下一关的")]
    public void LoadLevel(int level)
    {
        if (Curentminerals.Count>0)
        {
            for (int i = 0; i < Curentminerals.Count; i++)
            {
                Destroy(Curentminerals[i]);
            }
            Curentminerals.Clear();
        }
        if (level<=4)
        {
            bg_Top.sprite=gold_minerBGimgS[level-1].bg_Top;
            bg_Bottom.sprite=gold_minerBGimgS[level-1].bg_Bottom;
            LoadLevelMineral(level);
        }
        else
        {
            int l = Random.Range(0, gold_minerBGimgS.Count);
            bg_Bottom.sprite=gold_minerBGimgS[l].bg_Bottom;
            bg_Top.sprite=gold_minerBGimgS[l].bg_Top;
            LoadLevelMineral(Random.Range(5,9));
        }
    }

    public void LoadLevelMineral(int level)
    {
        for (int i = 0; i < goldMinerLevelminerals[level-1].minerals.Count; i++)
        {
            for (int j = 0; j < goldMinerLevelminerals[level-1].minerals[i].positions.Count; j++)
            {
               GameObject go= Instantiate(goldMinerLevelminerals[level-1].minerals[i].prefab,goldMinerLevelminerals[level-1].minerals[i].positions[j], Quaternion.identity,goldMinerLevelminerals[level-1].minerals[i].parent);
               Curentminerals.Add(go);
            }
        }
    }
}
