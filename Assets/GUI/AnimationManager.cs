using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using InitScriptName;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class AnimationManager : MonoBehaviour
{
    public bool PlayOnEnable = true;
    bool WaitForPickupFriends;

    bool WaitForAksFriends;
    System.Collections.Generic.Dictionary<string, string> parameters;

    void OnEnable()
    {
        if( PlayOnEnable )
        {
            AudioManager.Instance.playerEffect1(SoundBase.Instance.swish[0]);
            //if( !GetComponent<SequencePlayer>().sequenceArray[0].isPlaying )
            //    GetComponent<SequencePlayer>().Play();
            if( name == "Fire" )
            {


            }
        }
        if( name == "MenuPlay" )
        {
            for( int i = 1; i <= 3; i++ )
            {
                transform.Find( "Image" ).Find( "Star" + i ).gameObject.SetActive( false );
            }

            int stars = 3;
            if( stars > 0 )
            {
                for( int i = 1; i <= stars; i++ )
                {
                    transform.Find( "Image" ).Find( "Star" + i ).gameObject.SetActive( true );
                }

            }
            else
            {
                for( int i = 1; i <= 3; i++ )
                {
                    transform.Find( "Image" ).Find( "Star" + i ).gameObject.SetActive( false );
                }

            }

        }
        
    }
    void OnDisable()
    {
        //if( PlayOnEnable )
        //{
        //    if( !GetComponent<SequencePlayer>().sequenceArray[0].isPlaying )
        //        GetComponent<SequencePlayer>().sequenceArray[0].Play
        //}
    }

    public void OnFinished()
    {
        if( name == "MenuComplete" )
        {
            StartCoroutine( MenuComplete() );
            StartCoroutine( MenuCompleteScoring() );
        }
        if( name == "MenuPlay" )
        {
            InitScript.Instance.currentTarget = LevelData.GetTarget(NewBall.currTarget);
        }

    }



    IEnumerator MenuComplete()
    {
        for( int i = 1; i <= mainscript.Instance.stars; i++ )
        {
            //  SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoringStar );
            transform.Find( "Image" ).Find( "Star" + i ).gameObject.SetActive( true );
            yield return new WaitForSeconds( 0.5f );
            AudioManager.Instance.playerEffect1(SoundBase.Instance.hit);
        }
        yield return new WaitForSeconds( 2f );
        Next();
    }
    IEnumerator MenuCompleteScoring()
    {
        Text scores = transform.Find( "Image" ).Find( "Scores" ).GetComponent<Text>();
        for( int i = 0; i <= mainscript.Score; i += 500 )
        {
            scores.text = "" + i;
            // SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoring );
            yield return new WaitForSeconds( 0.00001f );
        }
        scores.text = "" + mainscript.Score;
    }


    public void PlaySoundButton()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);

    }

    public IEnumerator Close()
    {
        yield return new WaitForSeconds( 0.5f );
    }

    public void CloseMenu()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( gameObject.name == "MenuPreGameOver" )
        {
            ShowGameOver();
        }
        if( gameObject.name == "MenuComplete" )
        {
            NewBall.currTarget = Random.Range(15,32); 
            Application.LoadLevel( "BubbleGame" );
        }
        if( gameObject.name == "MenuGameOver" )
        {
            NewBall.currTarget = Random.Range(15,32); 
            Application.LoadLevel( "BubbleGame" );
        }

        if( Application.loadedLevelName == "BubbleGame" )
        {
            if( BubbleGamePlay.Instance.GameStatus == BubbleGameState.Pause )
            {
                BubbleGamePlay.Instance.GameStatus = BubbleGameState.WaitAfterClose;

            }
        }
        AudioManager.Instance.playerEffect1(SoundBase.Instance.swish[1]);

        gameObject.SetActive( false );
    }

    public void Play()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click );
        if( gameObject.name == "MenuPreGameOver" )
        {
            if( InitScript.Gems >= 12 )
            {
                InitScript.Instance.SpendGems( 12 );
                LevelData.LimitAmount += 12;
                BubbleGamePlay.Instance.GameStatus = BubbleGameState.WaitAfterClose;
                gameObject.SetActive( false );

            }
        }
        else if( gameObject.name == "MenuPlay" )
        {
            Application.LoadLevel( "BubbleGame" );
        }
    }

    public void PlayTutorial()
    {
//        SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.click );
        BubbleGamePlay.Instance.GameStatus = BubbleGameState.Playing;
    //    mainscript.Instance.dropDownTime = Time.time + 0.5f;
//        CloseMenu();
    }

    public void Next()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click );
        CloseMenu();
    }
    public void BuyGems()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        GameObject.Find( "Canvas" ).transform.Find( "GemsShop" ).gameObject.SetActive( true );
    }

    public void Buy( GameObject pack )
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( pack.name == "Pack1" )
        {
            InitScript.waitedPurchaseGems = int.Parse( pack.transform.Find( "Count" ).GetComponent<Text>().text.Replace( "x ", "" ) );
#if UNITY_WEBPLAYER
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
            // INAPP.Instance.purchaseProduct( "pack1" );
            
            //	INAPP.Instance.purchaseProduct("android.test.refunded");
        }

        if( pack.name == "Pack2" )
        {
            InitScript.waitedPurchaseGems = int.Parse( pack.transform.Find( "Count" ).GetComponent<Text>().text.Replace( "x ", "" ) );
#if UNITY_WEBPLAYER
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
            // INAPP.Instance.purchaseProduct( "pack2" );
            //	INAPP.Instance.purchaseProduct("android.test.refunded");
        }
        if( pack.name == "Pack3" )
        {
            InitScript.waitedPurchaseGems = int.Parse( pack.transform.Find( "Count" ).GetComponent<Text>().text.Replace( "x ", "" ) );
#if UNITY_WEBPLAYER
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
            // INAPP.Instance.purchaseProduct( "pack3" );
            //	INAPP.Instance.purchaseProduct("android.test.refunded");
        }
        if( pack.name == "Pack4" )
        {
            InitScript.waitedPurchaseGems = int.Parse( pack.transform.Find( "Count" ).GetComponent<Text>().text.Replace( "x ", "" ) );
#if UNITY_WEBPLAYER
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
            // INAPP.Instance.purchaseProduct( "pack4" );
            //	INAPP.Instance.purchaseProduct("android.test.refunded");
        }
        CloseMenu();

    }
    public void BuyLifeShop()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( InitScript.Lifes < InitScript.CapOfLife )
            GameObject.Find( "Canvas" ).transform.Find( "LiveShop" ).gameObject.SetActive( true );

    }
    public void BuyLife( GameObject button )
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( InitScript.Gems >= int.Parse( button.transform.Find( "Price" ).GetComponent<Text>().text ) )
        {
            InitScript.Instance.SpendGems( int.Parse( button.transform.Find( "Price" ).GetComponent<Text>().text ) );
            InitScript.Instance.RestoreLifes();
            CloseMenu();
        }
        else
        {
            GameObject.Find( "Canvas" ).transform.Find( "GemsShop" ).gameObject.SetActive( true );
        }

    }



    void ShowGameOver()
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.gameOver);
        GameObject.Find( "Canvas" ).transform.Find( "MenuGameOver" ).gameObject.SetActive( true );
        gameObject.SetActive( false );

    }

    #region Settings
    public void ShowSettings( GameObject menuSettings )
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( !menuSettings.activeSelf )
        {
            menuSettings.SetActive( true );
 //           menuSettings.GetComponent<SequencePlayer>().Play();
        }
        else menuSettings.SetActive( false );
    }

    public void SoundOff( GameObject Off )
    {
        if( !Off.activeSelf )
        {
            InitScript.sound = false;

            Off.SetActive( true );
        }
        else
        {
            InitScript.sound = true;

            Off.SetActive( false );

        }

    }
    public void MusicOff( GameObject Off )
    {
        if( !Off.activeSelf )
        {
            GameObject.Find( "Music" ).GetComponent<AudioSource>().volume = 0;
            InitScript.music = false;

            Off.SetActive( true );
        }
        else
        {
            GameObject.Find( "Music" ).GetComponent<AudioSource>().volume = 1;
            InitScript.music = true;

            Off.SetActive( false );

        }

    }

    public void Info()
    {
        if( Application.loadedLevelName == "map" || Application.loadedLevelName == "menu" )
            GameObject.Find( "Canvas" ).transform.Find( "Tutorial" ).gameObject.SetActive( true );
        else
            GameObject.Find( "Canvas" ).transform.Find( "PreTutorial" ).gameObject.SetActive( true );
    }

    public void Quit()
    {
        if( Application.loadedLevelName == "game" )
            Application.LoadLevel( "map" );
        else
            Application.Quit();
    }



    #endregion

    #region BOOSTS

    public void FiveBallsBoost()
    {
        if( BubbleGamePlay.Instance.GameStatus != BubbleGameState.Playing ) return;
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( InitScript.Instance.FiveBallsBoost > 0 )
        {
            if( BubbleGamePlay.Instance.GameStatus == BubbleGameState.Playing )
                InitScript.Instance.SpendBoost( BoostType.FiveBallsBoost );
        }
        else
        {
            OpenBoostShop( BoostType.FiveBallsBoost );
        }
    }
    public void ColorBallBoost()
    {
        if( BubbleGamePlay.Instance.GameStatus != BubbleGameState.Playing ) return;
        if( BubbleGamePlay.Instance.GameStatus == BubbleGameState.Playing )
            InitScript.Instance.SpendBoost( BoostType.ColorBallBoost );
    }
    public void FireBallBoost()
    {
        if( BubbleGamePlay.Instance.GameStatus != BubbleGameState.Playing ) return;
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click);
        if( InitScript.Instance.FireBallBoost > 0 )
        {
            if( BubbleGamePlay.Instance.GameStatus == BubbleGameState.Playing )
                InitScript.Instance.SpendBoost( BoostType.FireBallBoost );
        }
        else
        {
            OpenBoostShop( BoostType.FireBallBoost );
        }

    }

    public void OpenBoostShop( BoostType boosType )
    {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.click );
        GameObject.Find( "Canvas" ).transform.Find( "BoostShop" ).gameObject.GetComponent<BoostShop>().SetBoost( boosType );
    }

    public void BuyBoost( BoostType boostType, int price )
    {
        
    }

    #endregion


}
