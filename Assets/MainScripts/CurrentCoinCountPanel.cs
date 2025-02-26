using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WGM;

/****************************************************
    文件：CurrentCoinCountPanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class CurrentCoinCountPanel : MonoBehaviour
{
    public static CurrentCoinCountPanel instance;
    
   public Text coinText;
    public AudioClip AddCoinSound;
   private void Awake()
   {
       if (instance==null)
       {
           instance = this;
       }
       else
       {
           Destroy(gameObject);
       }
   }

   private void Start()
   {
       Initialize(LibWGM.playerData[1].coin_in, LibWGM.machine.Cp_coin);
       SetGamestateByCoinCount();
   }

   private void Update()
   {
       if (DealCommand.GetKeyDown(1,AppKeyCode.CoinIn))
       {
           AddCoinCount(1);
       }
   }

   public void Initialize(int CoinCount,int palyerNeedCoinCount)
   {
       coinText.text= LibWGM.playerData[1].coin_in+"/"+LibWGM.machine.Cp_coin;
   }

   public void setCoinCount()
   {
       coinText.text= LibWGM.playerData[1].coin_in+"/"+LibWGM.machine.Cp_coin;
   }

   public void AddCoinCount(int addCoinCount)
   {
       LibWGM.playerData[1].coin_in+=addCoinCount;
       coinText.text=LibWGM.playerData[1].coin_in+"/"+LibWGM.machine.Cp_coin;
       SetGamestateByCoinCount();
       AudioManager.Instance.playerEffect3(AddCoinSound);
   }
   public void reduceCoinCount()
   {
       LibWGM.playerData[0].Free_coin_in -= LibWGM.machine.Cp_coin;
       if(LibWGM.playerData[0].Free_coin_in<0)
       {

           LibWGM.playerData[1].coin_in += LibWGM.playerData[0].Free_coin_in;
           LibWGM.playerData[1].coin_use -=LibWGM.playerData[0].Free_coin_in;
          
           LibWGM.playerData[0].Free_coin_in = 0;
       }
       coinText.text = (LibWGM.playerData[1].coin_in+ LibWGM.playerData[0].Free_coin_in) + "/" + LibWGM.machine.Cp_coin;
   }

   public void SetGamestateByCoinCount()
   {
       //一种没有一个币的，显示请投币
       if (LibWGM.playerData[1].coin_in+LibWGM.playerData[0].Free_coin_in==0&&GameTimeManager.instance.GetCurrentTime()<=0)
       {
           GameStateManager.Instance.SwitchState(GameState.Idle);
       }
       //投了一个币以上的，显示还差多少币
       else if (LibWGM.playerData[1].coin_in+LibWGM.playerData[0].Free_coin_in<LibWGM.machine.Cp_coin&&LibWGM.playerData[1].coin_in>=1&&GameTimeManager.instance.GetCurrentTime()<=0)
       {
           GameStateManager.Instance.SwitchState(GameState.NoCoinCount);
       }
       else if (ISStartGame()&&GameTimeManager.instance.GetCurrentTime()<=0)
       {
           GameStateManager.Instance.SwitchState(GameState.Waitpalyer);
       }
       //如果还有游戏时间
       else  if (GameTimeManager.instance.GetCurrentTime()>0)
       {
            if (!GameStateManager.Instance.GetTarGetGameStateIsEqual(GameState.Play))
            {
                GameStateManager.Instance.SwitchState(GameState.Play);
            }
       }
   }

   /// <summary>
/// 是否可以开始游戏或者继续游戏
/// </summary>
/// <returns></returns>
   public bool ISStartGame()
   {
       if (LibWGM.playerData[1].coin_in+LibWGM.playerData[0].Free_coin_in<LibWGM.machine.Cp_coin)
       {
           return false;
       }
       else
       {
           return true;
       }
   }
   
   public int GetNeedCoinCount()
   {
       return Mathf.Clamp(LibWGM.machine.Cp_coin-LibWGM.playerData[1].coin_in,0,10);
   }
}
