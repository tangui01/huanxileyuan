using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WGM;

public class SuperCommandoControllerInput : MonoBehaviour, IListener {
    public static SuperCommandoControllerInput Instance;
    public GameObject rangeAttack;
	SuperCommandoPlayer Player;
    [Header("Button")]
    public GameObject btnJump;
    public GameObject btnRange;

    public float Vertical, Horizontak;

    private void OnEnable()
    {
        if (SuperCommandoGameManager.Instance != null)
            StopMove();
    }

    public void TurnJump(bool isOn)
    {
        btnJump.SetActive(isOn);
    }

    public void TurnMelee(bool isOn)
    {
 
    }

    public void TurnRange(bool isOn)
    {
        btnRange.SetActive(isOn);
    }

    public void TurnDash(bool isOn)
    {
   
    }
    bool shooting;
    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        
        Player = FindObjectOfType<SuperCommandoPlayer> ();
		if(Player==null)
			Debug.LogError("There are no Player character on scene");
    }

	void Update(){

        // if (DealCommand.GetKeyDown(1,(AppKeyCode)1))
        //     SuperCommandoMenuManager.Instance.Pause();
        if (isMovingRight)
            MoveRight();
        else if (isMovingLeft)
            MoveLeft();

        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        //     SuperCommandoMenuManager.Instance.RestartGame();
        if (FindObjectOfType<NewSuperCommando>().isPaused)
        {
            Shot(false);
            return;
        }
        SuperCommandoGameManager.Instance.Player.Shoot(shooting);
        Shot(false);
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J))
            Shot(true);
#else
        if (DealCommand.GetKeyDown(1,(AppKeyCode)3))
            Shot(true);
#endif

        if (DealCommand.GetKeyDown(1,(AppKeyCode)0))
            Jump();
        else if (DealCommand.GetKeyUp(1,(AppKeyCode)0))
            JumpOff();

        // if (DealCommand.GetKeyDown(1,(AppKeyCode)0))
        //     ThrowGrenade();
    }

    bool isMovingLeft, isMovingRight;
	
	public void MoveLeft(){
        if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
        {
            Player.MoveLeft();
            isMovingLeft = true;
        }
	}

	public void MoveRight(){
        if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
        {
            Player.MoveRight();
            isMovingRight = true;
        }
	}

	public void FallDown(){
		if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
			Player.FallDown ();
	}


	public void StopMove(){
        if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
        {
            Player.StopMove();
            isMovingLeft = false;
            isMovingRight = false;
        }
	}

	public void Jump (){
		if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
			Player.Jump ();
	}

	public void JumpOff(){
		if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
			Player.JumpOff ();
	}

    public void Shot(bool hold)
    {
        shooting = hold;
    }

    private void OnDisable()
    {
        Player.StopMove();
        isMovingLeft = false;
        isMovingRight = false;
    }

    public void ThrowGrenade()
    {
        SuperCommandoGameManager.Instance.Player.ThrowGrenade();
    }

    public void IPlay()
    {

    }

    public void ISuccess()
    {

    }

    public void IPause()
    {

    }

    public void IUnPause()
    {

    }

    public void IGameOver()
    {
       
    }

    public void IOnRespawn()
    {

    }

    public void IOnStopMovingOn()
    {

    }

    public void IOnStopMovingOff()
    {

    }
}
