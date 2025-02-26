using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour {
	
	int appStartedNumber;
	float progress = 0;
	Image progressBar;
	string sceneToLoad;
	// Use this for initialization
	private void Awake()
	{
		Application.LoadLevel(1);
	}

	// void Start ()
	// {
	//
	// 	if(PlayerPrefs.HasKey("TutorialCompleted"))
	// 	{
	// 		sceneToLoad = "MainScene";
	// 	}
	// 	else
	// 		sceneToLoad = "TutorialLevel";
	//
	// 	progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
	// 	if(PlayerPrefs.HasKey("appStartedNumber"))
	// 	{
	// 		appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
	// 	}
	// 	else
	// 	{
	// 		appStartedNumber = 0;
	// 	}
	// 	appStartedNumber++;
	// 	PlayerPrefs.SetInt("appStartedNumber",appStartedNumber);
	// 	//Invoke("LoadFirstScene",4f);
	// 	StartCoroutine(LoadScene());
	// 	//SyncLoadLevel("MainScene");
	// }
	
	
	void LoadFirstScene()
	{
		Application.LoadLevel(sceneToLoad);
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(2f);
		while(progress < 1)
		{
			progress += 0.05f;
			progressBar.fillAmount=progress;
			yield return new WaitForSeconds(0.05f);
		}
		Application.LoadLevel(sceneToLoad);

	}

	IEnumerator Load ()
	{
		yield return progress;
	}


}
