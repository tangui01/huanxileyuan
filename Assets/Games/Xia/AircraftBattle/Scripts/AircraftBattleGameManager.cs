using UnityEngine;
using System.Collections;

public class AircraftBattleGameManager : MonoBehaviour {

	public Transform slowTimeScreen;
	[HideInInspector] public bool normalTime;
	public static bool canMove = false;
	static AircraftBattleGameManager instance;
	public float speed = 4;
	[HideInInspector] public bool bossTime = false;
	public static AircraftBattleGameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(AircraftBattleGameManager)) as AircraftBattleGameManager;
			}
			return instance;
		}
	}
	// Use this for initialization
	void Start ()
	{
		canMove = false;
		//slowTimeScreen = transform.Find("SlowTimeScreen");
		Input.multiTouchEnabled = false;
		//speed = 30;
		//GameObject.Find("SpawnHolder").SetActive(false);
	}

	void LateUpdate ()
	{
		if(canMove)
			transform.Translate(0, speed * Time.deltaTime, 0);
	}
	
//	public IEnumerator SlowTime()
//	{
//		float t=0;
//		float target = 0.1f;
//		while(Time.timeScale >= target + 0.01f && !normalTime)
//		{
//
//			slowTimeScreen.GetComponent<SpriteRenderer>().color = new Color(1,1,1,Mathf.Lerp(slowTimeScreen.GetComponent<SpriteRenderer>().color.a,0.55f,t));//0.67
//			Time.timeScale = Mathf.Lerp(Time.timeScale,target,t);
//			t+=Time.deltaTime/2;
//			yield return null;
//		}
//	}
//	public IEnumerator NormalTime()
//	{
//		float t=0;
//		while(Time.timeScale <= 0.99f && normalTime)
//		{
//			slowTimeScreen.GetComponent<SpriteRenderer>().color = new Color(1,1,1,Mathf.Lerp(slowTimeScreen.GetComponent<SpriteRenderer>().color.a,0,t));
//			Time.timeScale = Mathf.Lerp(Time.timeScale,1,t);
//			t+=Time.deltaTime;
//			yield return null;
//		}
//		Time.timeScale = 1;
//	}
//
//	public void PauseGame()
//	{
//		if(Time.timeScale == 0)
//		{
//			if(slowTimeScreen.GetComponent<SpriteRenderer>().color.a == 0)
//				Time.timeScale = 1;
//			else
//				Time.timeScale = 0.1f;
//			PlaneManager.Instance.gameActive = true;
//			transform.Find("Menu").GetComponent<Animator>().Play("UnPauseGame");
//		}
//		else
//		{
//			Time.timeScale = 0;
//			PlaneManager.Instance.gameActive = false;
//			if(PlaneManager.Instance.controlType == 1)
//			{
//				transform.Find("Menu/ControlsSelection").localPosition = new Vector3(-0.25f,0,-0.1f);
//			}
//			else
//			{
//				transform.Find("Menu/ControlsSelection").localPosition = new Vector3(0.25f,0,-0.1f);
//			}
//			transform.Find("Menu").GetComponent<Animator>().Play("PauseGame");
//		}
//	}

}
