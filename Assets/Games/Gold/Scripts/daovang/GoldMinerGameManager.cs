using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum TypeAction {Nghi, ThaCau, KeoCau};
public enum EnumStateGame {Play, Pause,GameOver};

public delegate void OnStateChangeHandler();

public class GoldMinerGameManager : MonoBehaviour {
    public static GoldMinerGameManager instance;
	public event OnStateChangeHandler OnStateChange;
	public EnumStateGame gameState { get; private set; }

	public int typeLuoiCau { get; private set; }

    public bool power, bookStone, clover, diamond, clock, powerCurrent;
    
    public PlayerPanel playerPanel;
    public NextLevelPanel nextLevelPanel;
    public int score { get; private set; }
    public int level { get; private set; }
	private void Awake() {
        if (instance == null)
        {
	        instance = this;
        }
        else
        {
	        Destroy(gameObject);
        }
        DisableItems();
    }

	private void Start()
	{
		InitScore();
	}

	public int GetScoreTarget(int level)
    {
        if (level == 1)
        {
            return 800;
        }
        else if (level == 2)
        {
            return 2050;
        }
        else
        {
            return 800 + (level - 1) * 1250 + (level - 2) * 500;
        }
    }
    public void DisableItems()
    {
        power = false;
        bookStone = false;
        clover = false;
        diamond = false;
        clock = false;
        powerCurrent = false;
    }
    public void SetGameState(EnumStateGame gameState) {
		this.gameState = gameState;
		if(OnStateChange!=null) {
			OnStateChange();
		}
	}

	public void InitScore()
	{
		score = PlayerPrefs.GetInt("score");
		level = PlayerPrefs.GetInt("level");
		SetGameState(EnumStateGame.Play);
		GoldMinerLoadLevelManager.instance.LoadLevel(level);
		GameTimeManager.instance.TimeOverAction = TimeOver;
		SceneLoadManager.instance.ExitSceneACtion = ExitGame;
		playerPanel.SetScore(score);
		playerPanel.SetTargetScore(GetScoreTarget(level));
		playerPanel.SetLevel(level);
	}

	public void TimeOver()
	{
		PlayerPrefs.SetInt("level",level);
		PlayerPrefs.SetInt("score",score);
		powerCurrent = false;
		CommonUI.instance.StartCouterColdDown();
	}

	public void ExitGame()
	{
		PlayerPrefs.SetInt("level",1);
		PlayerPrefs.SetInt("score",0);
	}
	public void AddScore(int addScore)
	{
		score += addScore;
		if (score >=GetScoreTarget(level))
		{
			NextLevlAni();
		}
		playerPanel.SetScore(score);
	}
//检查当前分数是否足够前往下一关
	public bool CheckScoreISNextLevl(int addScore)
	{
		if (score + addScore>=GetScoreTarget(level))
		{
			return  true;
		}
		return false;
	}

	public void SetTypeLuoiCau(int type) {
		this.typeLuoiCau = type;
	}

	IEnumerator OnNextLevelAni()
	{
		nextLevelPanel.Enter();
		SetGameState(EnumStateGame.Pause);
		yield return new WaitForSeconds(0.5f);
		GoldMinerLoadLevelManager.instance.LoadLevel(level);
		playerPanel.SetTargetScore(GetScoreTarget(level));
		playerPanel.SetLevel(level);
		yield return new WaitForSeconds(2.3f);
		nextLevelPanel.Exit();
		SetGameState(EnumStateGame.Play);
	}

	public void NextLevlAni()
	{
		level += 1;
		GamePlayScript.instance.StopAllSound();
		PlayerPrefs.SetInt("level",level);
		PlayerPrefs.SetInt("score",score);
		powerCurrent = false;
		StartCoroutine("OnNextLevelAni");
	}
}
