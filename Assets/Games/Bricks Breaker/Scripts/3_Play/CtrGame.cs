using System.Collections;
using UnityEngine;


public class CtrGame : CtrBase
{
    static CtrGame _instance;

    public static CtrGame instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CtrGame>();
            }

            return _instance;
        }
    }
    [HideInInspector] public bool isStart = false;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public int turnScore;
    [HideInInspector] public int turnCount = 1;
    [HideInInspector] public bool isContinue = false;
    [HideInInspector] public int comboCount = 0;
    [HideInInspector] public bool isAllClear = false;
    [SerializeField] private AudioClip clip;
    public TiltCamera tiltCamera;
    private int shotSoundCount = 0;
    private bool isLock = false;

    //Screen Drag Lock
    public bool IsLock
    {
        get
        {
            if (!isStart)
            {
                return true;
            }

            if (Player.instance.activeBall.Count > 0) return true;
            if (isGameOver) return true;
            return isLock;
        }
        set { isLock = value; }
    }


    private void Awake()
    {
        IsLock = true;

        //Initialize to set play record once
        BricksBreakerPlayManager.Instance.countPlay = 0;
        BricksBreakerPlayManager.Instance.countBreakeBrick = 0;
        BricksBreakerPlayManager.Instance.countAllClear = 0;
        BricksBreakerPlayManager.Instance.countLuckyBonus = 0;
        BricksBreakerPlayManager.Instance.countHighestCombo = 0;

        GameTimeManager.instance.TimeOverAction = TimeOver;
        if (BricksBreakerPlayManager.Instance.isSaveGameStart)
        {
            //When playing from the middle
            BricksBreakerPlayManager.Instance.isSaveGameStart = false;

            turnCount = 0;
            BricksBreakerPlayManager.Instance.score = 0;

            CtrUI.instance.textTurn.text = turnCount.ToString();
            CtrUI.instance.textScore.text = Utility.ChangeThousandsSeparator(0);
            Player.instance.ballCount = turnCount;
            Player.instance.ballMaxCount = turnCount;
        }
        else
        {
            //First
            BricksBreakerPlayManager.Instance.score = 0;
        }
    }

    IEnumerator Start()
    {
        Player.instance.SetData();
        yield return new WaitForSeconds(0.01f);
        CtrBlock.instance.SpwanBlock(0, turnCount);
        yield return new WaitForSeconds(0.5f);
        isStart = true;
        IsLock = false;
    }


    //Next turn
    public void NextTurn()
    {
        isLock = true;
        turnCount += 1;

        CtrUI.instance.SetTurn(turnCount);
        StartCoroutine(NextTurnCo());
    }

    IEnumerator NextTurnCo()
    {
        CtrUI.instance.AddScore(turnScore);
        yield return new WaitForSeconds(0.2f);
        CtrBlock.instance.NextTurn();
    }



    public void NextTurnMoveEnd()
    {
        if (isGameOver) return;

        //All clear check
        if (CtrUI.instance._ComboEffectText.isAllClear)
        {
            isAllClear = true;
            CtrUI.instance._ComboEffectText.isAllClear = false;
        }
        else
        {
            CtrUI.instance._ComboEffectText.allClearCount = 0;
            isAllClear = false;
        }
        
        CtrUI.instance.NextTurnReady();
        turnScore = 0;
        comboCount = 0;
        IsLock = false;
        Player.instance.guideLine.GuidelineOn();
    }
    //GameOver
    public void GameOver()
    {
        BricksBreakerPlayManager.Instance.turn = turnCount;
        CommonUI.instance.BackMainPanel_OPen(true);
    }

    public void TimeOver()
    {
        CommonUI.instance.StartCouterColdDown();
    }

    public void ShotSound()
    {
        if (shotSoundCount > 2) return;
        shotSoundCount++;
        AudioManager.Instance.playerEffect2(clip);
        StartCoroutine(RemoveSoundCo(clip.length));
    }


    IEnumerator RemoveSoundCo(float time)
    {
        yield return new WaitForSeconds(time);
        if (shotSoundCount > 0) shotSoundCount -= 1;
    }
}