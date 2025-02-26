using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WGM;
/****************************************************
    文件：MainGameSelectView.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：游戏大厅选择游戏
*****************************************************/
public class MainGameSelectView : MonoBehaviour
{
    public GameObject Item;
    public Transform Content;

    public MainMenuGamesConf gc;

    public int CurrentGameindex;

    [SerializeField] private Image GameIcon;
    [SerializeField] private GameNameVisual GameName;
    public List<GameNameitem> GameNameitems;
    public ScrollRect scrollRect;

    public static MainGameSelectView Instance;
    
    private float NextMoveLeftTimer;
    private float NextMoveRightTimer;
    private float NextMoveUpTimer;
    private float NextMoveDownTimer;
    
    
    [SerializeField] private AudioClip SelectSound;
    [SerializeField] private AudioClip clickSound;

    [SerializeField] private float MoveNexttimer;
    private bool ISStartGame=false;
    
    public StartSelectPanel StartSelectTipPanel;
    
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
        for (int i = 0; i < gc.games.Count; i++)
        {
            GameNameitem gn = Instantiate(Item, Vector3.zero, Quaternion.identity, Content)
                .GetComponent<GameNameitem>();
            gn.Init(gc.games[i],scrollRect);
            GameNameitems.Add(gn);
        }
    }

    private void Start()
    {
        CurrentGameindex = GameSelectManger.Instance.GetSelectGame();
        LocalizationManager.Instance.SwitchLanguage(LibWGM.machine.Language);
        UpdateGameConf();
        ISStartGame = false;
    }

    public void Update()
    {
        GetGameKeyInput();
    }

    private void GetGameKeyInput()
    {
        #region 检测单次按下

        //A健 左移
        if (DealCommand.GetKeyDown(1, AppKeyCode.TicketOut))
        {
            if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
            {
                GameNameitems[CurrentGameindex].ExitSelect();
                CurrentGameindex--;
                if (CurrentGameindex < 0)
                {
                    CurrentGameindex = gc.games.Count - 1;
                    scrollRect.verticalNormalizedPosition = 0;
                }
                UpdateGameConf();
            }
        }
        else if (DealCommand.GetKeyDown(1, AppKeyCode.Flight))
        {
            if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
            {
                GameNameitems[CurrentGameindex].ExitSelect();
                CurrentGameindex += 1;
                if (CurrentGameindex > gc.games.Count - 1)
                {
                    CurrentGameindex = 0;
                    scrollRect.verticalNormalizedPosition = 1;
                }

                UpdateGameConf();
            }
        }
        //W健 上移
        else if (DealCommand.GetKeyDown(1, AppKeyCode.Bet))
        {
            if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
            {
                GameNameitems[CurrentGameindex].ExitSelect();
                CurrentGameindex -= 4;
                if (CurrentGameindex < 0)
                {
                    CurrentGameindex = gc.games.Count+CurrentGameindex;
                    scrollRect.verticalNormalizedPosition = 0;
                }
                UpdateGameConf();
            }
        }
        else if (DealCommand.GetKeyDown(1, AppKeyCode.ExtCh0))
        {
            if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
            {
                GameNameitems[CurrentGameindex].ExitSelect();
                CurrentGameindex += 4;
                if (CurrentGameindex > gc.games.Count - 1)
                {
                    CurrentGameindex = CurrentGameindex%4;
                    scrollRect.verticalNormalizedPosition = 1;
                }
                UpdateGameConf();
            }  
        }

        #endregion
        #region  检测长按
        else if (DealCommand.GetKey(1, AppKeyCode.TicketOut))
        {
            NextMoveLeftTimer+=Time.deltaTime;
            if (NextMoveLeftTimer>MoveNexttimer)
            {
                NextMoveLeftTimer = 0;
                if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
                {
                    GameNameitems[CurrentGameindex].ExitSelect();
                    CurrentGameindex--;
                    if (CurrentGameindex < 0)
                    {
                        CurrentGameindex = gc.games.Count - 1;
                        scrollRect.verticalNormalizedPosition = 0;
                    }
                    UpdateGameConf();
                }
            }
        }
        else if (DealCommand.GetKeyUp(1, AppKeyCode.TicketOut))
        {
            NextMoveLeftTimer = 0;
        }
        //J健 右移
        else if (DealCommand.GetKey(1, AppKeyCode.Flight))
        {
            NextMoveRightTimer += Time.deltaTime;
            if (NextMoveRightTimer>MoveNexttimer)
            {
                NextMoveRightTimer = 0;
                if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
                {
                    GameNameitems[CurrentGameindex].ExitSelect();
                    CurrentGameindex += 1;
                    if (CurrentGameindex > gc.games.Count - 1)
                    {
                        CurrentGameindex = 0;
                        scrollRect.verticalNormalizedPosition = 1;
                    }

                    UpdateGameConf();
                }  
            }
        }
        else if (DealCommand.GetKeyUp(1, AppKeyCode.Flight))
        {
            NextMoveRightTimer = 0;
        }
        //W健 上移
        else if (DealCommand.GetKey(1, AppKeyCode.Bet))
        {
            NextMoveUpTimer+=Time.deltaTime;
            if (NextMoveUpTimer>MoveNexttimer)
            {
                NextMoveUpTimer = 0;
                if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
                {
                    GameNameitems[CurrentGameindex].ExitSelect();
                    CurrentGameindex -= 4;
                    if (CurrentGameindex < 0)
                    {
                        CurrentGameindex = gc.games.Count+CurrentGameindex;
                        scrollRect.verticalNormalizedPosition = 0;
                    }
                    UpdateGameConf();
                }
            }
        }
        else if (DealCommand.GetKeyUp(1, AppKeyCode.Bet))
        {
            NextMoveUpTimer = 0;
        }
        //S健 下移
        else if (DealCommand.GetKey(1, AppKeyCode.ExtCh0))
        {
            NextMoveDownTimer+=Time.deltaTime;
            if (NextMoveDownTimer>MoveNexttimer)
            {
                NextMoveDownTimer = 0;
                if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
                {
                    GameNameitems[CurrentGameindex].ExitSelect();
                    CurrentGameindex += 4;
                    if (CurrentGameindex > gc.games.Count - 1)
                    {
                        CurrentGameindex = CurrentGameindex%4;
                        scrollRect.verticalNormalizedPosition = 1;
                    }
                    UpdateGameConf();
                }  
            }
        }
        else if (DealCommand.GetKeyUp(1, AppKeyCode.ExtCh0))
        {
            NextMoveDownTimer = 0;
        }

        #endregion
        #region 开始游戏按键检测

        if (DealCommand.GetKeyDown(1, AppKeyCode.UpScore))
        {
            if (GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
            {
                //开始游戏的两种方式(一种是有时间，可以继续游戏)
                //一种是没有时间，需要投币后才能游戏
                //先检测有没有游戏时间
                if (GameTimeManager.instance.GetCurrentTime() > 0&&!ISStartGame)
                {
                    ISStartGame=true;
                    StartGame();
                    GameSelectManger.Instance.SelectGame(CurrentGameindex);
                }
            }
            else
            {
                if (CurrentCoinCountPanel.instance.ISStartGame())
                {
                    GameStateManager.Instance.SwitchState(GameState.Play);
                    StartSelectTipPanel.UpdateAni();
                    GameTimeManager.instance.StartColdDown(LibWGM.machine.GameTime);
                    AudioManager.Instance.playerEffect1(clickSound);
                    GameSelectManger.Instance.SelectGame(CurrentGameindex);
                    CommonUI.instance.mainTimePanel.Enter();
                    CurrentCoinCountPanel.instance.reduceCoinCount();
                }
            }
        }

        #endregion
    }

    private void StartGame()
    {
        CommonUI.instance.mainTimePanel.Exit();
        AudioManager.Instance.playerEffect1(clickSound);
        AudioManager.Instance.StopBGm();
        LoadABManger.Instance.UnloadAB(MainConstant.MainSceneName);
        LoadABManger.Instance.LoadAB(GameNameitems[CurrentGameindex].GetGameSceneName(),
        ()=>{
            if (LocalizationManager.Instance.GetCurrentLanguage()==Language.Chinese)
            {
                CommonUI.instance.AddTips("长按开始键可返回游戏大厅");
            }
            else
            {
                CommonUI.instance.AddTips("Long press the Start button to return to the game hall");
            }
        }
        );
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("level", 1);
    }

    public void UpdateGameConf()
    {
        AudioManager.Instance.playerEffect1(SelectSound);
        Updatelanguage();
        GameNameitems[CurrentGameindex].SelectGame();
    }

    public void Updatelanguage()
    {
        GameIcon.sprite = gc.games[CurrentGameindex].icon;
        GameName.setGameName(LocalizationManager.Instance.GetCurrentLanguage(), gc.games[CurrentGameindex]);
    }
}