using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Level :MonoBehaviour
{
    public int LevelNumber;
    [Header("Road")]

    public GameObject StartBlock;
    public GameObject Block1;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;
    public GameObject Block5;
    public GameObject Block6;
    public GameObject Block7;
    public GameObject Block8;
    public GameObject Block9;
    public GameObject Block10;



    

    Transform StartLine;
    public GameObject EndTrack;
    GameObject Empty;
    // Update is called once per frame
    public void StartRoad()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(StartBlock) as GameObject;
        StartLine = Track.transform;
        Track.transform.SetParent(Empty.transform);

    }

    public void FinishLine()
    {


        GameObject Track = PrefabUtility.InstantiatePrefab(EndTrack) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }

    public void block1()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block1) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }


    public void block2()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block2) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }


    public void block3() {

    GameObject Track = PrefabUtility.InstantiatePrefab(Block3) as GameObject;
    Track.transform.position = StartLine.transform.position;
    StartLine = Track.transform.GetChild(0).transform;
    Track.transform.SetParent(Empty.transform);
}
        
    

  

    public void block4()
    {
    GameObject Track = PrefabUtility.InstantiatePrefab(Block4) as GameObject;
    Track.transform.position = StartLine.transform.position;
    StartLine = Track.transform.GetChild(0).transform;
    Track.transform.SetParent(Empty.transform);
}

    public void block5()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block5) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }

    public void block6()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block6) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }

    public void block7()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block7) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }
    public void block8()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block8) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }
    public void block9()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block9) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }
    public void block10()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block10) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }

    public void CreateLevelEmpty()
    {
        Empty = new GameObject("Level_"+LevelNumber);
        LevelNumber++;

    }
}
