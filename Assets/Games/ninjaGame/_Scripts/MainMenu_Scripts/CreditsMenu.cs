using UnityEngine;
using System.Collections;

namespace ninjaGame
{
	public class CreditsMenu : MonoBehaviour
	{

		public GameObject CreditsMenuParent, MainMenuParent;

		void Start()
		{

		}


		void Update()
		{

		}

		//for button control
		public void OnButtonClick(string ButtonName)
		{
			switch (ButtonName)
			{
				//for back button
				case "Back":
					CreditsMenuParent.SetActive(false); //for credits menu disables
					MainMenuParent.SetActive(true); //for mainmenu enables
					break;

			}

		}
	}
}
