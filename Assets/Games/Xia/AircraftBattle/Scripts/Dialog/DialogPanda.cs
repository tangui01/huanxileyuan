using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/**
  * Scene:N/A
  * Object:N/A
  * Description: Ova skripta se koristi da docara efekat kucanja teksta kao na kucacoj masini. Potrebno je skriptu nakaciti na GameObject koji ima Componentu Text novog UI sistema(od 4.6 verzija Unity-a, pa na dalje.
  * 
  **/
public class DialogPanda : MonoBehaviour {

//	string[] pandaDialogStage1 = new string[]   {"Ironpaw: This is our territory and I'm taking it back!", 
//		"Ironpaw: You can laugh now Brazut, but I will have the last laugh!", 
//		"Ironpaw: I am Commander Ironpaw! Remember my name, it is the last you will ever hear!"};
//	
//	string[] pandaDialogStage2 = new string[]   {"Ironpaw: You may laugh at me, but the victory will be mine!", 
//		"Ironpaw: I am not affraid of you Zerunx! Let's start the battle!", 
//		"Ironpaw: Your words don't threaten me! I will win this fight!"};
//	
//	string[] pandaDialogStage3 = new string[]   {"Ironpaw: I'm not afraid of you Gezbek! It's time for revenge!", 
//		"Ironpaw: Gezbek, you make me sick! You're going down!", 
//		"Ironpaw: You're skipping dinner this time! I will destroy you and your wicked minions!"};
//	
//	string[] pandaDialogStage4 = new string[]   {"Ironpaw: Spitspaz, the only thing you'll be tasting tonight are my bullets!", 
//		"Ironpaw: Bring it on Spitspaz! You may have a bigger plane, but I will defeat you!", 
//		"Ironpaw: I'm not scared, your words mean nothing! Ironpaw will never surrender!"};
//
//	string[] pandaDialogStage5 = new string[]   {"Ironpaw: Save your words Nazgel! We'll see your skills on the battlefield!", 
//		"Ironpaw: No, I will make you pay for all the damage you caused to my friends!", 
//		"Ironpaw: You will never hurt a single panda again! I will save my homeland!"};
//
//	string pandaDialogStage6 = "Ironpaw: I will beat you with my bare paws to save my friends!";
//	string pandaDialogStage7 = "Ironpaw: Less talking, more fighting! I'll show you my strength on the battlefield!";
//	string pandaDialogStage8 = "Ironpaw: My mission is to save my homeland and I will never give up!";
//	string pandaDialogStage9 = "Ironpaw: I will free my friends from your bloodthirsty reign! There's no surrender!";
//	string pandaDialogStage10 = "Ironpaw: I didn't come so far to be defeated! Let's dance Wizmaz!";

	string[] pandaDialogStage1 = new string[]   {"Ironpaw: One does not simply threathen a panda.", 
		"Ironpaw: I sting like a bee. Can you feel it?", 
		"Ironpaw: I'm widely known as goblin extreminator. Let me show you why."};
	
	string[] pandaDialogStage2 = new string[]   {"Ironpaw: This isn't the panda you're looking for.", 
		"Ironpaw: Your feet smell bad, I guarantee it!", 
		"Ironpaw: You dialed wrong number, there's no panda here."};
	
	string[] pandaDialogStage3 = new string[]   {"Ironpaw: I will serve you yourself for dinner.", 
		"Ironpaw: Holy macaroni, a talking goblin!", 
		"Ironpaw: Do you know what roast goblin tastes like?"};
	
	string[] pandaDialogStage4 = new string[]   {"Ironpaw: You cannot scare me cuz you're funny looking.", 
		"Ironpaw: I will digitally remove your head from your body.", 
		"Ironpaw: Hohoho, Spitspaz...You have a...beautiful name, man."};
	
	string[] pandaDialogStage5 = new string[]   {"Ironpaw: Ever looked yourself in a mirror or too afraid to buy one?", 
		"Ironpaw: I will destroy you. Then I will hurt you.", 
		"Ironpaw: Why don't you just return to wherever you came from?"};
	
	string pandaDialogStage6 = "Ironpaw: I'll deal with you quickly, I have better things to do.";
	string pandaDialogStage7 = "Ironpaw: This is getting ridiculous.";
	string pandaDialogStage8 = "Ironpaw: What's with these names? Kerugm, really?";
	string pandaDialogStage9 = "Ironpaw: You have no power here.";
	string pandaDialogStage10 = "Ironpaw: I didn't come so far to be defeated! Let's dance Wizmaz!";

	// public static bool dialogPressed = false;
	[HideInInspector]
	public  bool pandaCanSpeak = true;
	[HideInInspector]
	public  bool noTutorial = true;

	float letterPause = 0.005f;
	Text PandaMessageText;
	string message;
	DialogBoss dialog;
	// Use this for initialization
	void Start ()
	{
		dialog = FindObjectOfType<DialogBoss>();
		switch(LevelGenerator.currentStage)
		{
		case 1:
			message = pandaDialogStage1[dialog.versionDialog];
			break;
		case 2:
			message = pandaDialogStage2[dialog.versionDialog];
			break;
		case 3:
			message = pandaDialogStage3[dialog.versionDialog];
			break;
		case 4:
			message = pandaDialogStage4[dialog.versionDialog];
			break;
		case 5:
			message = pandaDialogStage5[dialog.versionDialog];
			break;
		case 6:
			message = pandaDialogStage6;
			break;
		case 7:
			message = pandaDialogStage7;
			break;
		case 8:
			message = pandaDialogStage8;
			break;
		case 9:
			message = pandaDialogStage9;
			break;
		
		default:
			message = "Ironpaw: Less talking, more fighting! I'll show you my strength on the battlefield!";
			break;
		}
		PandaMessageText = GameObject.Find("PandaMessageText").GetComponent<Text>();
		PandaMessageText.text = "";
	}

	/// <summary>
	/// Koorutina koja oponasa prikaz teksta kao da se kuca na kucacoj masini.
	/// </summary>
	/// <param name="msg">String koji zelite da se prikaze.</param>
	IEnumerator TypeTextPanda () 
	{
		// for(int i=0; i<message.Length;i++)
		// {
		// 	if(!dialogPressed)
		// 	{
		// 		if(i%2 == 0)
		// 			SoundManager.Instance.Play_DialogTextTyping();
		// 		PandaMessageText.text += message[i];
		// 		yield return new WaitForSeconds (letterPause);
		// 	}
		// 	else
		// 	{
		// 		PandaMessageText.text += message[i];
		// 		yield return new WaitForSeconds (0.01f);
		// 	}
		// }
		PandaMessageText.text = message;
		yield return new WaitForSeconds(0.5f);
		GameObject.Find("PandaDialogHolder/AnimationHolder").GetComponent<Animation>().Play ("DialogDepartingPanda");
	}

//	void ChangeLetterPause()
//	{
//		letterPause = 0.035f;
//	}
	



}
