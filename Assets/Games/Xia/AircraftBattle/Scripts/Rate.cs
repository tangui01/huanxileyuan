using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
  * Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...
  * Object:N/A
  * Description: Sample Description
  **/
public class Rate : MonoBehaviour {

	int appStartedNumber,alreadyRated;
	bool rateClicked = false;
	public static bool ratePresent = false;

	// Use this for initialization
	void Start () {

		if(PlayerPrefs.HasKey("alreadyRated"))
		{
			alreadyRated = PlayerPrefs.GetInt("appStartedNumber");
		}
		else
		{
			alreadyRated = 0;
		}

		if(alreadyRated==0)
		{
			appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
			if(appStartedNumber>=6)
			{
				appStartedNumber=0;
				PlayerPrefs.SetInt("appStartedNumber",appStartedNumber);
				PlayerPrefs.Save();
				ShowRateMenu();
				ratePresent = true;
			}
			else
			{
//				if(!DailyRewards.dailyRewardActive)
//				{
//					Debug.Log("ULAZIVAC 4");
//					FacebookNativeAd.showMainScreen = true;
//					GameObject.Find("NativeAds/MainScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().LoadAdWithDelay(0.5f);
//				}
			}
		}

	}

	public void RateClicked(int number)
	{
		if(!rateClicked)
		{
			alreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated",alreadyRated);
			PlayerPrefs.Save();
			rateClicked=true;
			StartCoroutine("ActivateStars",number);
		}
	}

	IEnumerator ActivateStars(int number)
	{
		switch(number)
		{
		case 1: case 2: case 3:
			for(int i=1;i<=number;i++)
			{
				GameObject.Find("RateStarsHolder/StarsHolder/Star"+i).GetComponent<Image>().enabled = true;
			}
			yield return new WaitForSeconds(0.5f);
			HideRateMenu();
			break;
		case 4: case 5:
			for(int i=1;i<=number;i++)
			{
				GameObject.Find("RateStarsHolder/StarsHolder/Star"+i).GetComponent<Image>().enabled = true;
			}
			yield return new WaitForSeconds(0.5f);
			HideRateMenu();
			yield return new WaitForSeconds(0.5f);
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.Flying.Panda.Strike.Jet.Force.Attack");
			break;
		}
		yield return null;
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();

	}

	public void ShowRateMenu()
	{
		transform.GetChild(0).GetComponent<Animation>().Play("DailyRewardArrival");
	}

	public void HideRateMenu()
	{
		ratePresent = false;
		SoundManager.Instance.Play_ButtonClick();
		transform.GetChild(0).GetComponent<Animation>().Play("DailyRewardDeparting");

	}

	public void NoThanks()
	{
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();
		HideRateMenu();
	}
}
