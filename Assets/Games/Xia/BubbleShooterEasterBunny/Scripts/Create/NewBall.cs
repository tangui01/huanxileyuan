using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using InitScriptName;
using UnityEngine;
using UnityEngine.SceneManagement;
using WGM;

public class NewBall : MonoBehaviour
{
    private static NewBall instance;

    public static NewBall Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NewBall>();
            }

            return instance;
        }
    }

    public float speed = 0.1f;
    [HideInInspector] public Vector3 rayPosition;
    public static int currTarget = 10;

    private void Awake()
    {
        
    }

    void Start()
    {
        rayPosition = new Vector3(Screen.width * 0.5f,Screen.height,0);
        speed = Screen.width * speed;
        StartCoroutine(ray());
    }

    IEnumerator ray()
    {
        float time = 0;
        var  posy =  rayPosition.y;
        while (time < 1)
        {
            time += Time.deltaTime/2f;
            rayPosition.y = Mathf.Lerp(posy,Screen.height*0.4f, time);
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                rayPosition.y = Screen.height * 0.4f;
                break;
            }
#else
            if (DealCommand.GetKey(1,(AppKeyCode)6) || DealCommand.GetKey(1,(AppKeyCode)1))
            {
                rayPosition.y = Screen.height * 0.4f;
                break;
            }
#endif            
            yield return null;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            var direction = rayPosition;
            if (Input.GetKey(KeyCode.A))
                direction.x -= speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.D))
                direction.x += speed * Time.deltaTime;
            rayPosition = Verify(direction);
        }
#else
        if (DealCommand.GetKey(1,(AppKeyCode)6) || DealCommand.GetKey(1,(AppKeyCode)1))
        {
            var direction = rayPosition;
            if (DealCommand.GetKey(1, (AppKeyCode)6))
                direction.x -= speed * Time.deltaTime;

            if (DealCommand.GetKey(1, (AppKeyCode)1))
                direction.x += speed * Time.deltaTime;
            rayPosition = Verify(direction);
        }
#endif
    }

    public Vector3 Verify(Vector3 movePos)
    {
        if (movePos.x > 0.925f * Screen.width)
            movePos.x = 0.925f * Screen.width;
        if (movePos.x < 0.1f * Screen.width)
            movePos.x = 0.1f * Screen.width;
        return movePos;
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameTimeInadequate()
    {
        LevelData.LimitAmount = 0 ;
        BubbleGamePlay.Instance.GameStatus = BubbleGameState.GameOver;
    }
    IEnumerator ComtinuePlay() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("BubbleGame");
    }
    public void ComtinuePlayGame() {
        StartCoroutine(ComtinuePlay());
    }
}