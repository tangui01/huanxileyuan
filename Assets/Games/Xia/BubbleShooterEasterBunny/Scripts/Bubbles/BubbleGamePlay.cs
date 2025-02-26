using UnityEngine;
using System.Collections;
using InitScriptName;
using UnityEngine.SceneManagement;


public enum BubbleGameState
{
    Playing,
    Highscore,
    GameOver,
    Pause,
    Win,
    WaitForPopup,
    WaitAfterClose,
    BlockedGame,
    Tutorial,
    PreTutorial,
    WaitForChicken
}


public class BubbleGamePlay : MonoBehaviour {
    public static BubbleGamePlay Instance;
    private BubbleGameState gameStatus;
    bool winStarted;
    public BubbleGameState GameStatus
    {
        get { return BubbleGamePlay.Instance.gameStatus; }
        set 
        {
            if( BubbleGamePlay.Instance.gameStatus != value )
            {
                if( value == BubbleGameState.Win )
                {
                    if( !winStarted )
                        StartCoroutine( WinAction ());
                }
                else if( value == BubbleGameState.GameOver )
                {
                    StartCoroutine( LoseAction() );
                }
                else if( value == BubbleGameState.Tutorial && gameStatus != BubbleGameState.Playing )
                {
                    value = BubbleGameState.Playing;
                    gameStatus = value;
                  //  ShowTutorial();
                }
                else if( value == BubbleGameState.PreTutorial && gameStatus != BubbleGameState.Playing )
                {
                    ShowPreTutorial();
                }

            }
            if( value == BubbleGameState.WaitAfterClose )
                StartCoroutine( WaitAfterClose() );

            if( value == BubbleGameState.Tutorial )
            {
                if( gameStatus != BubbleGameState.Playing )
                    BubbleGamePlay.Instance.gameStatus = value;

            }
          
            BubbleGamePlay.Instance.gameStatus = value;

        }
    }

	// Use this for initialization
	void Start () {
        Instance = this;
	}

    void Update()
    {
        // if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        // {
        //     if( Input.GetKey( KeyCode.W ) ) GamePlay.Instance.GameStatus = GameState.Win;
        //     if( Input.GetKey( KeyCode.E ) ) mainscript.Instance.destroyAllballs() ;
        //     if( Input.GetKey( KeyCode.R ) ) LevelData.LimitAmount = 1 ;
        //     if( Input.GetKey( KeyCode.Q ) ) LevelData.LimitAmount = 50 ;
        // }
    }
	
	// Update is called once per frame
	IEnumerator WinAction () 
    {
        winStarted = true;
        InitScript.Instance.AddLife( 1 );
        GameObject.Find( "BubbleCanvas" ).transform.Find( "LevelCleared" ).gameObject.SetActive( true );
  //       yield return new WaitForSeconds( 1f );
        //if( GameObject.Find( "Music" ) != null)
        //    GameObject.Find( "Music" ).SetActive( false );
        //    GameObject.Find( "CanvasPots" ).transform.Find( "Black" ).gameObject.SetActive( true );
        AudioManager.Instance.playerEffect1(SoundBase.Instance.winSound);
         yield return new WaitForSeconds( 1f );
         if( LevelData.mode == ModeGame.Vertical )
         {
           //  SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.swish[0] );
           //  GameObject.Find( "Canvas" ).transform.Find( "PreComplete" ).gameObject.SetActive( true );
            yield return new WaitForSeconds( 1f );
            GameObject.Find( "CanvasPots" ).transform.Find( "Black" ).gameObject.SetActive( false );
            //     SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.swish[0] );
          //  yield return new WaitForSeconds( 1.5f );
            yield return new WaitForSeconds( 0.5f );
         }

        foreach( GameObject item in GameObject.FindGameObjectsWithTag("Ball") )
        {
            item.GetComponent<ball>().StartFall();
                                   
        }
       // StartCoroutine( PushRestBalls() );
        Transform b = GameObject.Find( "-Ball" ).transform;
        ball[] balls = GameObject.Find( "-Ball" ).GetComponentsInChildren<ball>();
        foreach( ball item in balls )
        {
            item.StartFall();
        }

        while( LevelData.LimitAmount > 0 )
        {
            if( mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy != null )
            {
                LevelData.LimitAmount--;
                ball ball = mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy.GetComponent<ball>();
                mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy = null;
                ball.transform.parent = mainscript.Instance.Balls;
                ball.tag = "Ball";
                ball.PushBallAFterWin();

            }
            yield return new WaitForEndOfFrame();
        }
        foreach( ball item in balls )
        {
            if(item != null)
                item.StartFall();
        }
        yield return new WaitForSeconds( 2f );
        while( GameObject.FindGameObjectsWithTag( "Ball" ).Length > 0  )
        {
            yield return new WaitForSeconds( 0.1f );
        }
        AudioManager.Instance.playerEffect1(SoundBase.Instance.aplauds);
        GameObject.Find( "BubbleCanvas" ).transform.Find( "LevelCleared" ).gameObject.SetActive( false );
        GameObject.Find( "BubbleCanvas" ).transform.Find( "MenuComplete" ).gameObject.SetActive( true );

    }

    //IEnumerator PushRestBalls()
    //{

    //    while( LevelData.limitAmount  > 0)
    //    {
    //        if( mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy != null )
    //        {
    //            LevelData.limitAmount--;
    //            ball b = mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy.GetComponent<ball>();
    //            mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy = null;
    //            b.transform.parent = mainscript.Instance.Balls;
    //            b.tag = "Ball";
    //            b.PushBallAFterWin();

    //        }
    //        yield return new WaitForEndOfFrame();
    //    }

    //}

    void ShowTutorial()
    {
        //GameObject.Find( "Canvas" ).transform.Find( "Tutorial" ).gameObject.SetActive( true );
        

    }
    void ShowPreTutorial()
    {
        GameObject.Find( "BubbleCanvas" ).transform.Find( "PreTutorial" ).gameObject.SetActive( true );

    }

    IEnumerator LoseAction()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.OutOfMoves);
        GameObject.Find( "BubbleCanvas" ).transform.Find( "OutOfMoves" ).gameObject.SetActive( true );
        yield return new WaitForSeconds( 1.5f );
        GameObject.Find( "BubbleCanvas" ).transform.Find( "OutOfMoves" ).gameObject.SetActive( false );
        
        CommonUI.instance.BackMainPanel_OPen();
        yield return new WaitForSeconds( 0.75f );
        SceneManager.LoadScene("BubbleGame");

        

    }

    IEnumerator WaitAfterClose()
    {
        yield return new WaitForSeconds( 1 );
        GameStatus = BubbleGameState.Playing;
    }
}
