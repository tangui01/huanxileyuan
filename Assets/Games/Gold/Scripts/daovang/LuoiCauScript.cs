using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using WGM;

public class LuoiCauScript : MonoBehaviour
{
    public static LuoiCauScript instance;
	public float speed;
	public float speedBegin;
	public Vector2 velocity;
	public float maxY;
	public Transform target;
	Vector3 prePosition;
	public bool isUpSpeed;
	public float timeUpSpeed;

    public GameObject hook, halfHook;
    private Vector3 positionHalfHook, scaleHalfHook;

    public bool cameraOut;

    private Rigidbody2D rb;
    private void Awake()
    {
	    rb = GetComponent<Rigidbody2D>();
    }

    void Start () {
        MakeInstance();
		isUpSpeed = false;
		prePosition = transform.localPosition;
	}
    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
	public IEnumerator TimeUpSpeed ()
	{
		while(true){
			if(isUpSpeed)
			{
				timeUpSpeed = timeUpSpeed - 1;
				if(timeUpSpeed <= 0)
					isUpSpeed = false;
			}
			yield return new WaitForSeconds (1);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.timeScale==0)
		{
			return;
		}
		///如果发射健按下
		if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore)&&GoldMinerGameManager.instance.gameState==EnumStateGame.Play)
		{
			checkTouchScene();
		}
		checkKeoCauXong();
		checkMoveOutCameraView();
        if (GoldMinerGameManager.instance.power || GoldMinerGameManager.instance.powerCurrent)
        {
            speed = 4;
        }
        
        positionHalfHook = halfHook.gameObject.transform.position;
        scaleHalfHook = halfHook.gameObject.transform.localScale;
        if (DayCauScript.instance.typeAction == TypeAction.KeoCau && !cameraOut)
        {
            GamePlayScript.instance.PlaySound(5);
            hook.SetActive(false);
            halfHook.SetActive(true);
            if (positionHalfHook.x > 0)
            {
                scaleHalfHook.x = -0.2f;
            }
            else
            {
                scaleHalfHook.x = 0.2f;
            }
            halfHook.transform.localScale = scaleHalfHook;
            cameraOut = false;
        }
        else if (DayCauScript.instance.typeAction == TypeAction.Nghi)
        {
            hook.SetActive(true);
            halfHook.SetActive(false);
        }
    }

	private float currentSpeed;
	void FixedUpdate() {
		{
			if(DayCauScript.instance.typeAction == TypeAction.ThaCau)
			{
					rb.velocity = velocity * speed;
            }
				
            else if(DayCauScript.instance.typeAction == TypeAction.KeoCau)
            {
                if (cameraOut)
                {
		                rb.velocity = velocity * speed * 3;
                }
                else
                {
	                if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore))
	                {
		                rb.velocity = velocity * speed*5;
	                }
	                else
	                {
		                rb.velocity = velocity * speed;
	                }
                }
            }
		}
	}
	bool checkPositionOutBound() {
		return  gameObject.GetComponent<Renderer>().isVisible ;
	}

	public void checkTouchScene() { 	
		if(DayCauScript.instance.typeAction == TypeAction.Nghi)
		{
			DayCauScript.instance.typeAction = TypeAction.ThaCau;
			velocity = new Vector2(transform.position.x - target.position.x, 
			                       transform.position.y - target.position.y);
			velocity.Normalize();
			speed = speedBegin;
		}
	}
	//kiem tra khi luoi cau ra ngoai tam nhin cua camera
	void checkMoveOutCameraView() {
		if(DayCauScript.instance.typeAction == TypeAction.ThaCau) {
			if(!checkPositionOutBound()) {
                cameraOut = true;
                DayCauScript.instance.typeAction = TypeAction.KeoCau;
				velocity = -velocity;
			}
		}
	}

	//kiem tra khi luoi ca keo len mat nuoc
	void checkKeoCauXong() {
		if(transform.localPosition.y > maxY && DayCauScript.instance.typeAction == TypeAction.KeoCau) {
			rb.velocity = Vector2.zero;
			DayCauScript.instance.typeAction = TypeAction.Nghi;
			transform.localPosition = prePosition;
		}
	}

	public void SetSpeed(int _speedstage)
	{
		switch (_speedstage)
		{
			case 1:
				speed = 0.5f;
				break;
			case 2:
				speed =1.5f;
				break;
			case 3:
				speed = 2f;
				break;
			case 4:
				speed = 3f;
				break;
			case 5:
				speed = 4f;
				break;
		}
	}
}
