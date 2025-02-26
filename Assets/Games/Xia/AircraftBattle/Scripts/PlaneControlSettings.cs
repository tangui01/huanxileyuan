using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaneControlSettings : MonoBehaviour {


	Image FingerControl, ParallelControl;
	Color FullColor = new Color(1f,1f,1f,1f), HalfTransparentColor = new Color(1f,1f,1f,0.5f);
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey("ControlType"))
			PlaneManager.controlType = PlayerPrefs.GetInt("ControlType");

		FingerControl = GameObject.Find("ButtonFingerControl").GetComponent<Image>();
		ParallelControl = GameObject.Find("ButtonParallelControl").GetComponent<Image>();

		RefreshState();
		if(Application.loadedLevelName.Equals("MainScene"))
			InitialiseSoundSettings();

	}

	public void ActivateFingerControl()
	{
		PlaneManager.controlType = 1;
		FingerControl.color = FullColor;
		ParallelControl.color = HalfTransparentColor;
		PlayerPrefs.SetInt("ControlType",PlaneManager.controlType);
		PlayerPrefs.Save();
	}

	public void ActivateParallelControl()
	{
		PlaneManager.controlType = 2;
		FingerControl.color = HalfTransparentColor;
		ParallelControl.color = FullColor;
		PlayerPrefs.SetInt("ControlType",PlaneManager.controlType);
		PlayerPrefs.Save();
	}

	public void RefreshState()
	{
		if(PlaneManager.controlType == 1)
		{
			FingerControl.color = FullColor;
			ParallelControl.color = HalfTransparentColor;
		}
		else
		{
			FingerControl.color = HalfTransparentColor;
			ParallelControl.color = FullColor;
		}
	}

	public void InitialiseSoundSettings()
	{
		//ovde proveriti playerprefs za zvuk i muziku
		//SoundOnOff();
		//MusicOnOff();
		if(SoundManager.soundOn == 0)
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
		if(SoundManager.musicOn == 0)
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
	}

	public void SoundOnOff()
	{
		if(SoundManager.soundOn == 1)
		{
			SoundManager.soundOn = 0;
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			SoundManager.soundOn = 1;
			SoundManager.Instance.Play_ButtonClick();
			GameObject.Find("SoundOnOff").GetComponent<Image>().enabled = false;
		}
		PlayerPrefs.SetInt("SoundOn",SoundManager.soundOn);
		PlayerPrefs.SetInt("MusicOn",SoundManager.musicOn);
		PlayerPrefs.Save();
	}

	public void MusicOnOff()
	{
		if(SoundManager.musicOn == 1)
		{
			SoundManager.Instance.Stop_MenuMusic();
			SoundManager.musicOn = 0;
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = true;
		}
		else
		{
			SoundManager.musicOn = 1;
			SoundManager.Instance.Play_MenuMusic();
			GameObject.Find("MusicOnOff").GetComponent<Image>().enabled = false;
		}
		PlayerPrefs.SetInt("SoundOn",SoundManager.soundOn);
		PlayerPrefs.SetInt("MusicOn",SoundManager.musicOn);
		PlayerPrefs.Save();
	}
	
}
