using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Menu_AskSaveMe : MonoBehaviour
{
    public Text timerTxt;
    public Image timerImage;

    float timer = 3;
    float timerCountDown = 0;

    public Button btnWatchVideoAd;

    float timeStep = 0.02f;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (SuperCommandoGlobalValue.Instance.SaveLives > 0 || (LevelMapType.Instance && LevelMapType.Instance.playerNoLimitLife))
        {
            SuperCommandoGlobalValue.Instance.SaveLives--;
            Continue();
        }
        else
        {
            Time.timeScale = 0;
            btnWatchVideoAd.interactable = false;
            btnWatchVideoAd.gameObject.SetActive(false);

            if (!btnWatchVideoAd.interactable)
                timerCountDown = 0;
            else
                timerCountDown = timer;
        }
    }

    void Update()
    { 
        if (!SuperCommandoGameManager.Instance.isWatchingAd)
        {
            timerCountDown -= timeStep;
            timerTxt.text = (int)timerCountDown + "" ;
            timerImage.fillAmount = Mathf.Clamp01(timerCountDown / timer);

            if (timerCountDown <= 0)
            {
                // if(AdsManager.Instance)
                //     AdsManager.Instance.ShowAdmobBanner(true);
                SuperCommandoGameManager.Instance.GameOver(true);
                Time.timeScale = 1;
                SuperCommandoMenuManager.Instance.OpenSaveMe(false);
                Destroy(this);      //destroy this script
            }
        }
    }

    

    public void SaveByCoin()
    {
        SuperCommandoSoundManager.Instance.Click();
        SuperCommandoGlobalValue.Instance.SavedCoins -= SuperCommandoGameManager.Instance.continueCoinCost;
        Continue();
    }

    public void WatchVideoAd()
    {
        SuperCommandoSoundManager.Instance.Click();

        // AdsManager.AdResult += AdsManager_AdResult;
        // AdsManager.Instance.ShowRewardedAds();
        // AdsManager.Instance.ResetCounter(); 
        //reset to avoid play Unity video ad when finish game
    }

    private void AdsManager_AdResult(bool isSuccess, int rewarded)
    {
        // AdsManager.AdResult -= AdsManager_AdResult;
        if (isSuccess)
        {
            SuperCommandoGlobalValue.Instance.SaveLives += 1;
            Continue();
        }
    }

    void Continue()
    {
        Time.timeScale = 1;
        SuperCommandoGameManager.Instance.Continue();
    }
}
