﻿using UnityEngine;
using System.Collections;

namespace ninjaGame
{
	public class InAppMenu : MonoBehaviour
	{

		public GameObject InAppMenuParent, MainMenuParent;

		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		//for buttons controll
		public void OnButtonClicked(string ButtonName)
		{
			switch (ButtonName)
			{
				//for starter pack
				case "Buy1":
					Debug.Log("button1 is Clicked");
					break;
				//for Extreme pack
				case "Buy2":
					Debug.Log("button2 is Clicked");
					break;
				//for gamer pack
				case "Buy3":
					Debug.Log("button3 is Clicked");
					break;
				//for back to the main menu
				case "Back":
					InAppMenuParent.SetActive(false); //for inappmenu disables
					MainMenuParent.SetActive(true); //for mainmenu enables
					break;

			}

		}
	}
}
