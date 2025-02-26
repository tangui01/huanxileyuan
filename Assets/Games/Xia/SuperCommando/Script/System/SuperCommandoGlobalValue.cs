using System;
using UnityEngine;
using System.Collections;

public enum BulletFeature { Normal, Explosion, Shocking }

public class SuperCommandoGlobalValue : MonoBehaviour
{
    public static SuperCommandoGlobalValue Instance;
    [HideInInspector]
    public  GunTypeID currentGunTypeID;
    public  Transform currentHighestLevelObj;
    public  bool isFirstOpenMainMenu = true;
    public  int levelPlaying = 1;
    public  int totalLevel = 6;
    public  string WorldReached = "WorldReached";
    public  string Coins = "Coins";
    [HideInInspector]
    public  string Character = "Character";
    [HideInInspector]
    public  string ChoosenCharacterID = "choosenCharacterID";
    public  string ChoosenCharacterInstanceID = "ChoosenCharacterInstanceID";
    [HideInInspector]
    public  GameObject CharacterPrefab;
    public  int normalBulletLimited = 6;

    public  bool isSound = true;
    public  bool isMusic = true;

    private void OnEnable()
    {
        Instance = this;
    }

    public  int getDartLimited()
    {
       return normalBulletLimited + (int)UpgradeItemPower(UPGRADE_ITEM_TYPE.dartHoler.ToString());
    }
    

    // public static int lastDayShowNativeAd1
    // {
    //     get { return PlayerPrefs.GetInt("lastDayShowNativeAd1", 0); }
    //     set { PlayerPrefs.SetInt("lastDayShowNativeAd1", value); }
    // }
    //
    // public static int lastDayShowNativeAd2
    // {
    //     get { return PlayerPrefs.GetInt("lastDayShowNativeAd2", 0); }
    //     set { PlayerPrefs.SetInt("lastDayShowNativeAd2", value); }
    // }
    //
    // public static int lastDayShowNativeAd3
    // {
    //     get { return PlayerPrefs.GetInt("lastDayShowNativeAd3", 0); }
    //     set { PlayerPrefs.SetInt("lastDayShowNativeAd3", value); }
    // }

    public  bool RemoveAds = true;

    public  bool isSetDefaultValue
    {
        get { return PlayerPrefs.GetInt("isSetDefaultValue", 0) == 1 ? true : false; }
        set { PlayerPrefs.SetInt("isSetDefaultValue", value ? 1 : 0); }
    }

    [HideInInspector]
    public  int Attempt = 0;

    public  int SaveLives = 3;
    

    public  void ResetLives()
    {
        SaveLives = 3;
        currentGunTypeID = null;
    }

    public  int SavedCoins
    {
        get { return PlayerPrefs.GetInt(Coins, DefaultValue.Instance != null ? DefaultValue.Instance.defaultCoin : 99999); }
        set { PlayerPrefs.SetInt(Coins, value); }
    }

    public  int powerBullet
    {
        get { return PlayerPrefs.GetInt("powerBullet", 0); }
        set { PlayerPrefs.SetInt("powerBullet", value); }
    }

    public  int Bullets
    {
        get {
            int bullets = PlayerPrefs.GetInt("Bullets", 3);
            bullets = Mathf.Clamp(bullets, 0, getDartLimited());
            return bullets; }
        set { PlayerPrefs.SetInt("Bullets", value); }
    }
    //
    // public static int storeGod
    // {
    //     get { return PlayerPrefs.GetInt("storeGod", 0); }
    //     set { PlayerPrefs.SetInt("storeGod", value); }
    // }
    //
    // public static void SetScrollLevelAte(int scrollID)
    // {
    //     //Debug.LogError("EAT: " + scrollID);
    //     PlayerPrefs.SetInt("AteScroll" + levelPlaying + scrollID, 1);
    // }
    //
    // public static bool IsScrollLevelAte(int scrollID)
    // {
    //     //Debug.LogError(scrollID + ":" + (PlayerPrefs.GetInt("AteScroll" + levelPlaying + scrollID, 0) == 1));
    //     return PlayerPrefs.GetInt("AteScroll" + levelPlaying + scrollID, 0) == 1 ? true : false;
    // }
    //
    // public static bool IsScrollLevelAte(int level, int scrollID)
    // {
    //     return PlayerPrefs.GetInt("AteScroll" + level + scrollID, 0) == 1 ? true : false;
    // }
    //
    //
    // public static int Scroll
    // {
    //     get { return PlayerPrefs.GetInt("Scroll", 0); }
    //     set { PlayerPrefs.SetInt("Scroll", value); }
    // }

    public  int LevelHighest
    {
        get { return PlayerPrefs.GetInt("LevelHighest", 1); }
        set { PlayerPrefs.SetInt("LevelHighest", value); }
    }

    public  void UpgradedItem(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }

    public  int UpgradedItem(string name)
    {
        return PlayerPrefs.GetInt(name, 0);
    }


    public  void UpgradeItemPower(string name, float value)
    {
        PlayerPrefs.SetFloat(name + "UpgradeItemPower", value);
    }

    public  float UpgradeItemPower(string name)
    {
        return PlayerPrefs.GetFloat(name + "UpgradeItemPower", 0f);
    }

    public  bool isPicked(GunTypeID gunID)
    {
        return PlayerPrefs.GetString("GUNTYPE" + gunID.gunType, "") == gunID.gunID;
    }

    // public static void pickGun(GunTypeID gunID)
    // {
    //     PlayerPrefs.SetString("GUNTYPE" + gunID.gunType, gunID.gunID);
    // }
}
