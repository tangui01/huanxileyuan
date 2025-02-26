using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
public enum BoostType
{
    FiveBallsBoost = 0,
    ColorBallBoost,
    FireBallBoost
}
public enum Target
{
    Top = 0,
    Chicken
}

namespace InitScriptName
{


    public class InitScript : MonoBehaviour
    {
        public static InitScript Instance;
        private int _levelNumber = 1;
        private int _starsCount = 1;
        private bool _isShow;
        public static int openLevel;

        public static bool boostJelly;
        public static bool boostMix;
        public static bool boostChoco;

        public static bool sound = false;
        public static bool music = false;

        public static int waitedPurchaseGems;

        public static List<string> selectedFriends;
        public static bool Lauched;
        public static int scoresForLeadboardSharing;
        public static int lastPlace;
        public static int savelastPlace;
        public static bool beaten;
        public static List<string> Beatedfriends;
        int messCount;
        public static bool loggedIn;
        //	public GameObject LoginButton;
        //	public GameObject InviteButton;
        public GameObject EMAIL;
        public GameObject MessagesBox;


        public static bool FirstTime;
        public static int Lifes;
        public static int CapOfLife = 5;
        public static int Gems;

        public static float RestLifeTimer;
        public static string DateOfExit;
        public static DateTime today;
        public static DateTime DateOfRestLife;
        public static string timeForReps;

        public static bool openNext;
        public static bool openAgain;

        public int FiveBallsBoost;
        public int ColorBallBoost;
        public int FireBallBoost;
        public bool BoostActivated;

        Hashtable mapFriends = new Hashtable();

        public Target currentTarget;

        public void Awake()
        {
            Instance = this;
            if (Application.loadedLevelName == "map")
            {
                if (GameObject.Find("Canvas").transform.Find("MenuPlay").gameObject.activeSelf) GameObject.Find("Canvas").transform.Find("MenuPlay").gameObject.SetActive(false);

            }
            RestLifeTimer = 9999;
            DateOfExit = "2024/12/2 11:41:45";
            Gems = 0;
            Lifes = 99;
            
            
            ReloadBoosts();

            boostPurchased = false;

            // if (INAPP.Instance == null)
            // {
            //     Instantiate(Resources.Load("OpenIAB/OpenIABEventManager"));
            //     Instantiate(Resources.Load("OpenIAB/OpenIABEventListener"));
            // }
        }

        void Start()
        {

        }


        public static bool boostPurchased;


        void Update()
        {

        }



        public void AddGems(int count)
        {
            Gems += count;
        }

        public void SpendGems(int count)
        {
            Gems -= count;
        }

        public void RestoreLifes()
        {
            Lifes = CapOfLife;
        }


        public void AddLife(int count)
        {
            Lifes += count;
            if (Lifes > CapOfLife) Lifes = CapOfLife;
        }

        public int GetLife()
        {
            if (Lifes > CapOfLife)
            {
                Lifes = CapOfLife;
            }
            return Lifes;
        }

        public void PurchaseSucceded()
        {
            AddGems(waitedPurchaseGems);
            waitedPurchaseGems = 0;
        }
        public void SpendLife(int count)
        {
            if (Lifes > 0)
            {
                Lifes -= count;
            }

        }

        public void BuyBoost(BoostType boostType, int count, int price)
        {
            SpendGems(price);
            if (boostType != BoostType.FiveBallsBoost)
            {
                
            }
            else
            {
                //                LevelData.LimitAmount += 5;
            }
            ReloadBoosts();
        }

        public void SpendBoost(BoostType boostType)
        {
            InitScript.Instance.BoostActivated = true;
            if (boostType != BoostType.FiveBallsBoost)
                mainscript.Instance.boxCatapult.GetComponent<Grid>().Busy.GetComponent<ball>().SetBoost(boostType);
        }

        public void ReloadBoosts()
        {

        }


        #region selectlevel
        public int LoadLevelStarsCount(int level)
        {
            return level > 10 ? 0 : (level % 3 + 1);
        }

        public void SaveLevelStarsCount(int level, int starsCount)
        {
            Debug.Log(string.Format("Stars count {0} of level {1} saved.", starsCount, level));
        }

        public void ClearLevelProgress(int level)
        {

        }


        public void OnLevelClicked(int number)
        {
            if (!GameObject.Find("Canvas").transform.Find("MenuPlay").gameObject.activeSelf)
            {
                // PlayerPrefs.SetInt("OpenLevel", number);
                // PlayerPrefs.Save();
                openLevel = number;
                currentTarget = LevelData.GetTarget(number);
                GameObject.Find("Canvas").transform.Find("MenuPlay").gameObject.SetActive(true);
            }
        }



        void OnApplicationPause(bool pauseStatus)
        {
            
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {
            //		if(RestLifeTimer>0){
            //		}
 

            //		FacebookSNSAgent.OnUserInfoArrived -= OnUserInfoArrived;
            //		FacebookSNSAgent.OnUserFriendsArrived -= OnUserFriendsArrived;
        }


        #endregion

    }


}