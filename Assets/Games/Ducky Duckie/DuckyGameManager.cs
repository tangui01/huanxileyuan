using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DuckyGameManager : MonoBehaviour 
{
   public static DuckyGameManager instance;
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
   [Header(" Player ")]
    public GameObject Player;
    Vector3 ToothBrushInitialScale;
    [Header(" Static Variables ")]
    public static bool gameOn = false;

    [Header(" Balls Settings ")]
    public Container_Manager CM;
    
   
    
	// Use this for initialization
	void Start () 
	{
        ToothBrushInitialScale = Player.transform.localScale;
        Coin_inPanel.Renew_money_Success += Coin_continue;
        StartGame();
	}
    public void StartGame()
    {
        //Set the Game as On
		gameOn = true;
		GameTimeManager.instance.TimeOverAction = TimeOver;
        //Reset the Size of the toothBrush
        Player.transform.localScale = ToothBrushInitialScale;
        //Reset balls
        CM.ResetBallsAmount();
    }
    public void GameOver()
    {
        gameOn = false;
        SceneLoadManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// 时间结束
    /// </summary>
    public void TimeOver()
    {
	    CommonUI.instance.StartCouterColdDown();
    }

    private void Coin_continue()
    {
	    CM.ResetBallsAmount();
    }

    private void OnDestroy()
    {
	    Coin_inPanel.Renew_money_Success -= Coin_continue;
    }
}
