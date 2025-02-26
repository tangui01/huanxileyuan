using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour {


    public List<Transform> select=new List<Transform>();
    [HideInInspector]
    public static int selectPlayer = 1;//玩家数量
    private int _select = 0;//选项下标

    // Use this for initialization
    void Start () {
        // transform.position = select[_select].position;
    }
    void Exit()
    {
        
    }
    // Update is called once per frame
    // void Update()
    // {
    //     Exit();
    //     if (Input.GetKeyDown(KeyCode.W)&&transform.position!=select[0].position)
    //     {
    //         transform.position = select[--_select].position;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.S) && transform.position != select[select.Count-1].position)
    //     {
    //         transform.position = select[++_select].position;
    //     }
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         for (int i = 0; i < select.Count; i++)
    //         {
    //             if (select[i].position == select[_select].position)
    //             {
    //                 selectPlayer = _select + 1;
    //                 if (selectPlayer == 1 || selectPlayer == 2)
    //                 {
    //                     SceneManager.LoadScene(1);//单人或者双人
    //                 }
    //                 else
    //                 {
    //                     SceneManager.LoadScene(2);
    //                 }
    //             }
    //         }
    //     }
    // }
}
