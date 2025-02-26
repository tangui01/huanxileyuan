using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ninjaGame
{
	public class Ace_IngameUiControl : MonoBehaviour
	{

		public GameObject Pausemenu;
		public int inGameCoinCount = 0; //for coins count
		public int inGameScoreCount = 0; //for score count
		public int inGameMultiplierCount = 0; //for multiplier count
		public int inGameDistance; //for distance count
		public Text scoreCountText, coinsCountText, distanceCountText; //for ingameui texts
		public static Ace_IngameUiControl Static;

		void Start()
		{
			Static = this;
			distanceCountText.text = "" + inGameDistance + " M";

		}

		// Update is called once per frame
		void Update()
		{
			//for Distance

			if (GameController.CurrentState == GameController.GameState.GamePlay)
			{
				Distance_IngameCount();
				//for Coins
				Coins_IngameCount();
				// for Score
				Score_IngameCount();
			}
		}

		void AddCoins(int i)
		{

		}
		//for game end and Finalscore card display
		public void GameEnd()
		{
			Invoke("ScoreCardEnabled", 1.6f); //for final score card late display
			Pausemenu.SetActive(false); //for pause menu disable
		}

		// for score card enable
		void ScoreCardEnabled()
		{
			CommonUI.instance.BackMainPanel_OPen(true);
		}
		//for show coins in ingame ui
		void Coins_IngameCount()
		{
			coinsCountText.text = "" + inGameCoinCount;
		}

		//for show distance in ingame ui
		float distanceCounter = 30;

		void Distance_IngameCount()
		{

			inGameDistance +=
				Mathf.RoundToInt(distanceCounter * Time.deltaTime); //for distance calculate Acording to player postion
			distanceCountText.text = "" + inGameDistance + " M"; // for display the distance
		}

		//for show score in ingame ui
		void Score_IngameCount()
		{
			scoreCountText.text = "" + inGameScoreCount;

		}
	}
}
