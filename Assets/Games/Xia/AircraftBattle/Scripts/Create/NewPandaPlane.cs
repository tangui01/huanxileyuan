using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WGM;

public class NewPandaPlane : MonoBehaviour
{
    static NewPandaPlane instance;

    public static NewPandaPlane Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NewPandaPlane>();
            }

            return instance;
        }
    }

    public float SpeedX =30;
    public float SpeedY =30;
    private Vector3 viewportPos; //飞机屏幕坐标
    GameObject player;
    [HideInInspector]
    public bool oneShooting = false;
    [HideInInspector]
    public bool isprop = false;
    
    
    private void Awake()
    {
        player = GameObject.Find("PandaPlaneGameplayHOLDER");
        GameObject BlueColorCameraFilter = GameObject.Find("BlueColorCameraFilter");
        if ((BlueColorCameraFilter && UnityEngine.Random.Range(0, 2) == 0)|| LevelGenerator.currentStage ==1)
        {
            BlueColorCameraFilter.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null)
            viewportPos = Camera.main.WorldToViewportPoint(player.transform.position);
    }
    
    

    public void ReStartPlane()
    {
        StartCoroutine(ReStartPlaneGame());
    }

    IEnumerator ReStartPlaneGame()
    {
        if ( PandaPlane.Instance.numberOfKills <= PandaPlane.Instance.allowedKeepPlayingNumber)
        {
            StartCoroutine(PandaPlane.Instance.MoveCameraSlowly(0,0.5f));
            yield return new WaitForSeconds(2f);
            PandaPlane.Instance.NewPlane();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            CommonUI.instance.BackMainPanel_OPen();
            yield return new WaitForSeconds(1.25f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public Vector3 PlankInput()
    {
        Vector3 mousePos = Vector3.zero;
        
        if (DealCommand.GetKey(1, (AppKeyCode)6))
            mousePos.x = -1;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.D))
            mousePos.x = 1;
        if (Input.GetKey(KeyCode.D) && DealCommand.GetKey(1, (AppKeyCode)6))
            mousePos.x = 0;
#else
        if (DealCommand.GetKey(1, (AppKeyCode)1))
            mousePos.x = 1;
        if (DealCommand.GetKey(1, (AppKeyCode)1) && DealCommand.GetKey(1, (AppKeyCode)6))
            mousePos.x = 0;
#endif        


        if (DealCommand.GetKey(1, (AppKeyCode)0))
            mousePos.y = 1;
        if (DealCommand.GetKey(1, (AppKeyCode)2))
            mousePos.y = -1;
        if (DealCommand.GetKey(1, (AppKeyCode)0) && DealCommand.GetKey(1, (AppKeyCode)2))
            mousePos.y = 0;
        return mousePos;
    }

    public Vector3 Verify(Vector3 movePos)
    {
        if (viewportPos.x <= 0.05 && movePos.x <= 0) movePos.x = 0;
        if (viewportPos.x >= 0.95 && movePos.x >= 0) movePos.x = 0;
        if (viewportPos.y <= 0.075 && movePos.y < 0) movePos.y = 0;
        if (viewportPos.y >= 0.75 && movePos.y >= 0) movePos.y = 0;
        movePos.x *= SpeedX;
        movePos.y *= SpeedY;
        return movePos;
    }
    
}