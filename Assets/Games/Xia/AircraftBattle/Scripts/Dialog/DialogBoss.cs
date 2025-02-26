using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WGM;

/**
  * Scene:N/A
  * Object:N/A
  * Description: Ova skripta se koristi da docara efekat kucanja teksta kao na kucacoj masini. Potrebno je skriptu nakaciti na GameObject koji ima Componentu Text novog UI sistema(od 4.6 verzija Unity-a, pa na dalje.
  * 
  **/
public class DialogBoss : MonoBehaviour {
	[HideInInspector]
	public  int versionDialog;
	[HideInInspector]
	public int randomNumber;
	string[] bossName = new string[] {"","Brazut","Zerunx","Gezbek","Spitspaz","Nazgel","Mornuk","Trugon","Kerugm","Gixemuek","Wizmaz"};
	string[] bossDialogStage1 = new string[]   {"Brazut: How dare you come to Brazut?! Squash panda! Waaargh!", 
											    "Brazut: You wanna beat Brazut with that little toy? Tee hee har har hur ur!!!", 
											    "Brazut: Wanna fight Brazut?! Who you think you arrrrr?!"};

	string[] bossDialogStage2 = new string[]   {"Zerunx: Hoy, you funny panda! You can't beat the mighty Zerunx!", 
												"Zerunx: This is war! Ghee'Haaw! Zerunx smash you!", 
												"Zerunx: Grrrrr!!! Boring panda! Feel the anger of Zerunx!"};

	string[] bossDialogStage3 = new string[]   {"Gezbek: G'Roat!!! Gezbek crunch pandas!!! Dinner time!", 
												"Gezbek: Gezbek hungry! Gezbek eat panda! Nom nom nom!!!", 
												"Gezbek: Gezbek crush your funny plane! Goblins, we eat panda for dinner!"};

	string[] bossDialogStage4 = new string[]   {"Spitspaz: Ironpaw, we finally meet! Spitspaz make panda burgers! Yum yum!", 
												"Spitspaz: Grrrrr, you really have the guts! Too bad Spitspaz must smash your toy plane!", 
												"Spitspaz: Spitspaz hurt panda! Prepare for pain! Waaargh!"};

	string[] bossDialogStage5 = new string[]   {"Nazgel: Ironpaw, I was waiting for you! Now you feel my rage! Aarghh!", 
		"Nazgel: Nobody mess with Nazgel! Now you gonna pay!", 
		"Nazgel: Nazgel destroy pandas for fun! You be next! Grrrrr!!!"};

	string bossDialogStage6 = "Mornuk: Panda beat Mornuk in that toy?! Tee hee har har hur ur!";
	string bossDialogStage7 = "Trugon: Do you know who is I? Trugon smash you in pieces!!! Waaargh!";
	string bossDialogStage8 = "Kerugm: Mighty Kerugm crunch you in no time! Foolish little panda!";
	string bossDialogStage9 = "Gixemuek: RAWR!!!! This is your end! Gixemux smash panda!!!";
	string bossDialogStage10 = "Wizmas: You destroyed my best warriors! Now you gonna pay Ironpaw!!! GRRRRR!!!";

//	public static bool dialogPressed = false;

	float letterPause = 0.005f;
	Text BossMessageText;
	string message;
	public List<Sprite> bossSprites = new List<Sprite>();
	// Use this for initialization
	void Awake () {
		versionDialog = Random.Range(0,3);
		if (LevelGenerator.currentStage >= 1 && LevelGenerator.currentStage <= 9)
		{
			GameObject.Find("BossNameText").GetComponent<Text>().text = bossName[LevelGenerator.currentStage];
			GameObject.Find("BossFace").GetComponent<Image>().sprite = bossSprites[LevelGenerator.currentStage-1];
			switch (LevelGenerator.currentStage)
			{
				case 1: message = bossDialogStage1[versionDialog]; break;
				case 2: message = bossDialogStage2[versionDialog]; break;
				case 3: message = bossDialogStage3[versionDialog]; break;
				case 4: message = bossDialogStage4[versionDialog]; break;
				case 5: message = bossDialogStage5[versionDialog]; break;
				case 6: message = bossDialogStage6; break;
				case 7: message = bossDialogStage7; break;
				case 8: message = bossDialogStage8; break;
				case 9: message = bossDialogStage9; break;
			}
		}
		else
		{
			randomNumber = Random.Range(1,10);
			GameObject.Find("BossNameText").GetComponent<Text>().text = bossName[randomNumber];
			message = bossName[randomNumber] +": Do you know who is I? "+ bossName[randomNumber] +" smash you in pieces!!! Waaargh!";
			GameObject.Find("BossFace").GetComponent<Image>().sprite = bossSprites[randomNumber];
		}
		BossMessageText = GameObject.Find("BossMessageText").GetComponent<Text>();
		BossMessageText.text = "";
	}

	IEnumerator TypeTextBoss () 
	{
		// int playSound = 0;
		// for(int i=0; i<message.Length;i++)
		// {
		// 	if(!DialogPanda.dialogPressed)
		// 	{
		// 		if(playSound % 3 == 0)
		// 			SoundManager.Instance.Play_DialogTextTyping();
		// 		playSound++;
		// 		BossMessageText.text += message[i];
		// 		yield return new WaitForSeconds (letterPause);
		// 	}
		// 	else
		// 	{
		// 		BossMessageText.text += message[i];
		// 		yield return new WaitForSeconds (0.005f);
		// 	}
		// }
		
		// if (LibWGM.machine.Language == 1)
		// {
		// 	
		// }
		BossMessageText.text = message.ToString();
		yield return new WaitForSeconds(1f);
		GameObject.Find("BossDialogHolder/AnimationHolder").GetComponent<Animation>().Play ("DialogDepartingBoss");
	}
	



}
