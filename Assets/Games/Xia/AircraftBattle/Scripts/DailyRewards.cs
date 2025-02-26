using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class DailyRewards : MonoBehaviour {

	public static int [] DailyRewardAmount = new int[]{0,100, 200, 300, 400, 500, 1000};
	int OneDayTime=60*60*24;
	public static int LevelReward;
	bool rewardCompleted = false;
	List<int> availableSixthReward=new List<int>();
	int sixDayCount, typeOfSixReward; // typeOfSixReward 0-stars, 1-blades, 2-bomb, 3-laser, 4-tesla
	Text moneyText;
	System.Globalization.DateTimeFormatInfo format;
	private  DateTime quitTime;
	string lastPlayDate,timeQuitString;
	string enterDay, enterMonth, enterYear, quitDay, quitMonth, quitYear;
	public static bool dailyRewardActive = false;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey("SixDayCount"))
		{
			sixDayCount=PlayerPrefs.GetInt("SixDayCount");
			if(sixDayCount<4)
			{
				sixDayCount++;
			}
		}
		else
		{
			sixDayCount=1;
		}
		GameObject.Find("Day6Holder/AnimationHolder/RewardText").GetComponent<Text>().text = sixDayCount+"x";
		moneyText = GameObject.Find("StarsNumberText").GetComponent<Text>();
		moneyText.text = Shop.stars.ToString();
		DateTime currentTime = DateTime.Now;
//		enterDay="1";
//		enterMonth="1";
//		enterYear="2015";
		enterDay = currentTime.Day.ToString();
		enterMonth = currentTime.Month.ToString();
		enterYear = currentTime.Year.ToString();

//		//TODOR ovo je neko testisiranje
//		Debug.Log("ENter day pre: " + enterDay);
//		enterDay = currentTime.AddDays(4).Day.ToString();
//		Debug.Log("ENter day posle: " + enterDay);

		if(PlayerPrefs.HasKey("LevelReward"))
		{
			LevelReward=PlayerPrefs.GetInt("LevelReward");
		}
		else
		{
			LevelReward=0;
		}

		if(PlayerPrefs.HasKey("VremeQuit"))
		{
			lastPlayDate=PlayerPrefs.GetString("VremeQuit");
			quitTime=DateTime.Parse(lastPlayDate);
//			danIzlaska="31";
//			mesecIzlaska="12";
//			godinaIzlaska="2014";
			quitDay=quitTime.Day.ToString();
			quitMonth = quitTime.Month.ToString();
			quitYear = quitTime.Year.ToString();
			if((int.Parse(enterYear)-int.Parse(quitYear))<1)
			{

				if((int.Parse(enterMonth)-int.Parse(quitMonth))==0)
				{

					if((int.Parse(enterDay)-int.Parse(quitDay)) > 1)
					{
						LevelReward=1;
						GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
						SetActiveDay(LevelReward);
						GameObject.Find("Day1Holder/AnimationHolder").GetComponent<Animation>().Play();
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						ShowDailyReward(LevelReward);
					}
					else if ((int.Parse(enterDay)-int.Parse(quitDay)) > 0)
					{


						if(LevelReward<6)
						{
							GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
							LevelReward++;
							SetActiveDay(LevelReward);
							GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
							PlayerPrefs.SetInt("LevelReward",LevelReward);
							PlayerPrefs.Save();
							ShowDailyReward(LevelReward);
						}
						else
						{
						
							LevelReward=1;
							GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
							SetActiveDay(LevelReward);
							GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
							PlayerPrefs.SetInt("LevelReward",LevelReward);
							PlayerPrefs.Save();
							ShowDailyReward(LevelReward);
						}

						//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
					}
					else
					{
						Invoke("CheckForRate",1f);
					}
				}
				else
				{

					if(int.Parse(enterDay)==1)
					{
						if(int.Parse(quitMonth)==1 || int.Parse(quitMonth)==3 || int.Parse(quitMonth)==5 || int.Parse(quitMonth)==7 || int.Parse(quitMonth)==8 || int.Parse(quitMonth)==10 || int.Parse(quitMonth)==12)
						{
							if(int.Parse(quitDay)==31)
							{

								if(LevelReward<6)
								{
									GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
									LevelReward++;
									SetActiveDay(LevelReward);
									GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									ShowDailyReward(LevelReward);
								}
								else
								{
									LevelReward=1;
									GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
									SetActiveDay(LevelReward);
									GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									ShowDailyReward(LevelReward);
								}

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
							else
							{
								LevelReward=1;
								GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
								SetActiveDay(LevelReward);
								GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
								PlayerPrefs.SetInt("LevelReward",LevelReward);
								PlayerPrefs.Save();
								ShowDailyReward(LevelReward);

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
						}
						else if(int.Parse(quitMonth)==4 || int.Parse(quitMonth)==6 || int.Parse(quitMonth)==9 || int.Parse(quitMonth)==11)
						{
							if(int.Parse(quitDay)==30)
							{

								if(LevelReward<6)
								{
									GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
									LevelReward++;
									SetActiveDay(LevelReward);
									GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									ShowDailyReward(LevelReward);
								}
								else
								{
									LevelReward=1;
									GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
									GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									ShowDailyReward(LevelReward);
								}

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
							else
							{
								LevelReward=1;
								GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
								SetActiveDay(LevelReward);
								GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
								PlayerPrefs.SetInt("LevelReward",LevelReward);
								PlayerPrefs.Save();
								ShowDailyReward(LevelReward);
								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
						}
						else
						{
							if(int.Parse(quitDay)>27)
							{

								if(LevelReward<6)
								{
									GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
									LevelReward++;
									SetActiveDay(LevelReward);
									GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									ShowDailyReward(LevelReward);
								}
								else
								{
									LevelReward=1;
									GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
									SetActiveDay(LevelReward);
									GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									ShowDailyReward(LevelReward);
								}

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
							else
							{
								LevelReward=1;
								GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
								SetActiveDay(LevelReward);
								GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
								PlayerPrefs.SetInt("LevelReward",LevelReward);
								PlayerPrefs.Save();
								ShowDailyReward(LevelReward);
								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
						}
					}
					else
					{
						LevelReward=1;
						GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
						SetActiveDay(LevelReward);
						GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						ShowDailyReward(LevelReward);
						//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
					}

				}


			}
			else
			{
				if(int.Parse(quitDay)==31 && int.Parse(enterDay)==1)
				{

					if(LevelReward<6)
					{
						GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
						LevelReward++;
						SetActiveDay(LevelReward);
						GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						ShowDailyReward(LevelReward);
					}
					else
					{
						LevelReward=1;
						GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
						SetActiveDay(LevelReward);
						GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						ShowDailyReward(LevelReward);
					}
					
					//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
				}
				else
				{

					LevelReward=1;
					GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardArrival");
					SetActiveDay(LevelReward);
					GameObject.Find("Day"+LevelReward+"Holder/AnimationHolder").GetComponent<Animation>().Play();
					PlayerPrefs.SetInt("LevelReward",LevelReward);
					PlayerPrefs.Save();
					ShowDailyReward(LevelReward);
					//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
				}
			}


		}
		else
		{
//			Collect();
			LevelReward=0;
			PlayerPrefs.SetInt("LevelReward",LevelReward);
			PlayerPrefs.Save();

			//Pokreni Notifikaciju za DailyReward na 24h
		}


	}



	void OnApplicationPause(bool pauseStatus) { //vraca false kad je aktivna app
		if(pauseStatus)
		{
			//izasao iz aplikacuje
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeQuit", timeQuitString);
			PlayerPrefs.Save();
			
		}
		else
		{
			//usao u aplikacuju
			
		}
		
		
	}

	void ShowDailyReward(int currentDayReward)
	{
		dailyRewardActive = true;
//		GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward"+TrenutniDan);
		GameObject currentDay;
		currentDay = GameObject.Find("Day " + currentDayReward.ToString());

//		currentDay.transform.GetChild(0).GetComponent<Animator>().Play("CollectDailyRewardTab");
//		currentDay.transform.GetChild(0).Find("DailyRewardParticlesIdle").particleSystem.Play();
//		switch(TrenutniDan)
//		{
//		case 1:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward1");
//			break;
//
//		case 2:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward2");
//			break;
//
//		case 3:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward3");
//			break;
//
//		case 4:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward4");
//			break;
//
//		case 5:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward5");
//			break;
//
//		case 6:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward6");
//			break;
//		}
	}

	public IEnumerator moneyCounter(int kolicina)
	{
//		if(kolicina>0)
//		{
//			if(PlaySounds.soundOn)
//				PlaySounds.Play_CoinsSpent();
//		}

		int current = int.Parse(moneyText.text);
		int suma = current + kolicina;
		int korak = (suma - current)/10;
		while(current != suma)
		{
			current += korak;
			moneyText.text = current.ToString();
			yield return new WaitForSeconds(0.07f);
		}
		moneyText.text = Shop.stars.ToString();
		yield return new WaitForSeconds(0.2f);
		ShopNotificationClass.numberNotification = GameObject.Find("AircraftBattleCanvas").transform.Find("ShopMenu").GetComponent<Shop>().ShopNotification();
		GameObject.Find("AvailablePurchases").GetComponent<Text>().text = ShopNotificationClass.numberNotification.ToString();
		GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardDeparting");
		//UPDATE!!!!!
		//GameObject.Find("NativeAds/MainScreen/NativeAdHolder").GetComponent<FacebookNativeAd>().LoadAdWithDelay(0.5f);
	}

	void SetActiveDay(int dayNumber)
	{
		GameObject.Find("Day"+dayNumber+"Holder/AnimationHolder/DailyRewardImage").GetComponent<Image>().color = Color.white;
	}

	void OnApplicationQuit() {
		timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.Save();

		//Pokreni Notifikaciju za DailyReward na 24h
	}

	public void TakeReward()
	{
		if(!rewardCompleted)
		{
			if(LevelReward!=6)
			{
				StartCoroutine("moneyCounter",DailyRewardAmount[LevelReward]);
				Shop.stars += DailyRewardAmount[LevelReward];
			}

			string uzengije = (Shop.stars+10) + "#" + (Shop.highScore-20) + "#" + (Shop.laserNumber+5) + "#" + (Shop.teslaNumber+5) + "#" + (Shop.bladesNumber+5) + "#" + (Shop.bombNumber+5);
			PlayerPrefs.SetString("Uzengije",uzengije);
			PlayerPrefs.Save();
			rewardCompleted=true;
		}

	}

	public void TakeSixthReward()
	{
		if(!rewardCompleted)
		{
			rewardCompleted=true;

			// typeOfSixReward 0-stars, 1-blades, 2-bomb, 3-laser, 4-tesla
			if(Shop.currentStateBlades==0 && Shop.currentStateBomb==0 && Shop.currentStateLaser==0 && Shop.currentStateTesla==0)
			{
				availableSixthReward.Add(0);
			}
			else
			{
				if(Shop.currentStateBlades>0)
					availableSixthReward.Add(1);
				if(Shop.currentStateBomb>0)
					availableSixthReward.Add(2);
				if(Shop.currentStateLaser>0)
					availableSixthReward.Add(3);
				if(Shop.currentStateTesla>0)
					availableSixthReward.Add(4);
			}
			
			
			
			int randomReward = UnityEngine.Random.Range(0, availableSixthReward.Count);
			switch(availableSixthReward[randomReward])
			{
			case 0:  //-stars
				GameObject.Find("PowerUp").GetComponent<Image>().sprite = GameObject.Find("ReferenceStars").GetComponent<Image>().sprite;
				break;
			case 1:  //-blades
				GameObject.Find("PowerUp").GetComponent<Image>().sprite = GameObject.Find("ReferenceBlades").GetComponent<Image>().sprite;
				Shop.bladesNumber+=sixDayCount;
				break;
			case 2:  //-bomb
				GameObject.Find("PowerUp").GetComponent<Image>().sprite = GameObject.Find("ReferenceBomb").GetComponent<Image>().sprite;
				Shop.bombNumber+=sixDayCount;
				break;
			case 3:  //-laser
				GameObject.Find("PowerUp").GetComponent<Image>().sprite = GameObject.Find("ReferenceLaser").GetComponent<Image>().sprite;
				Shop.laserNumber+=sixDayCount;
				break;
			case 4:  //-tesla
				GameObject.Find("PowerUp").GetComponent<Image>().sprite = GameObject.Find("ReferenceTesla").GetComponent<Image>().sprite;
				Shop.teslaNumber+=sixDayCount;
				break;
			}
			
			if(Shop.currentStateBlades==0 && Shop.currentStateBomb==0 && Shop.currentStateLaser==0 && Shop.currentStateTesla==0)
			{
				GameObject.Find("SixDayCountText").GetComponent<Text>().text = "1000x";
				StartCoroutine("moneyCounter",DailyRewardAmount[LevelReward]);
				Shop.stars += DailyRewardAmount[LevelReward];
			}
			else
			{
				switch(sixDayCount)
				{
				case 1:
					GameObject.Find("SixDayCountText").GetComponent<Text>().text = "1x";
					break;
				case 2:
					GameObject.Find("SixDayCountText").GetComponent<Text>().text = "2x";
					break;
				case 3:
					GameObject.Find("SixDayCountText").GetComponent<Text>().text = "3x";
					break;
				case 4:
					GameObject.Find("SixDayCountText").GetComponent<Text>().text = "4x";
					break;
				}
				Invoke("HideAfterSixtDay",2f);
			}
			string uzengije = (Shop.stars+10) + "#" + (Shop.highScore-20) + "#" + (Shop.laserNumber+5) + "#" + (Shop.teslaNumber+5) + "#" + (Shop.bladesNumber+5) + "#" + (Shop.bombNumber+5);
			PlayerPrefs.SetString("Uzengije",uzengije);
			PlayerPrefs.SetInt("SixDayCount",sixDayCount);
			PlayerPrefs.Save();
			GameObject.Find("PowerUpCollect/AnimationHolder").GetComponent<Animation>().Play();

		}

	}

	public void Collect()
	{
		if(LevelReward<6)
		{
			TakeReward();
		}
		else
		{
			TakeSixthReward();
		}
	}

	void CheckForRate()
	{
		
	}


	void HideAfterSixtDay()
	{
		GameObject.Find("DailyRewardHolder/AnimationHolder").GetComponent<Animation>().Play("DailyRewardDeparting");
		//UPDATE!!!!!
		Debug.Log("ULAZIVAC 3");

	}

}
