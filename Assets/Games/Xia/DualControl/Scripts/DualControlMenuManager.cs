using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DualControlMenuManager : MonoBehaviour {


	public string RateUsUrl,MoreGamesURL;
	public Text CurrentLevelText;
    [Header("Share")]
    public string Subject;
    public string Body;
    public GameObject SettingPanel;
    int CurrentLevel;
    int coins;
	

	// Use this for initialization
	void Start () {
      
            CurrentLevel = PlayerPrefs.GetInt("Level", 0);
            CurrentLevelText.text = "MISSION: " + (CurrentLevel+1);
        int first_time = PlayerPrefs.GetInt("FirstPlay", 0);
        if (first_time == 0) {
            SettingPanel.SetActive(true);
            PlayerPrefs.SetInt("FirstPlay", 1);
        }
        
	}


	public void LoadLevel(){

		SceneManager.LoadScene ("DualControlGame");
	}


	public void RateUs ()
	{
        Application.OpenURL(RateUsUrl);

    }


    public void MoreGames(){

		Application.OpenURL (MoreGamesURL);
	}


    public void Restart()
    {

        SceneManager.LoadScene("DualControlGame");
    }
    

    public void ShareClick()
    {
        StartCoroutine(StartShare());
    }

    IEnumerator StartShare()
    {
        yield return new WaitForEndOfFrame();
       // new NativeShare().SetSubject(Subject).SetText(Body).Share();
    }

    public void SetDpad() {

        PlayerPrefs.SetInt("Control", 1);
    }

    public void SetJoystick() {
        PlayerPrefs.SetInt("Control", 2);
    }

    public void SetSteering()
    {
        PlayerPrefs.SetInt("Control", 3);
    }



}
