using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class LanguageManager : MonoBehaviour {
	/*
	 * Scene:All
	 * Object:LanguageManager
	 * Sadrzi tekstove za menije za izabrani jezik, kao i
	 * informaciju koji je jezik izabran ( u formatu _SkracenicaZaJezik e.g. _en )
	 * u statikom stringu chosenLanguage.  U okviru awake funkcije je neophodno da stoji
	 * DontDestroyOnLoad(gameObject) da bi manager u svim scenema
	 * Funkcije 
	 * void Awake()
	 * public static void RefreshTexts()
	 * public static string SplitTextIntoRows(string, int) 
	 * 
	 * Promena tekstova se vrsi promenom izabranog
	 * jezika i pozivanje funkcije RefreshTexts()
	 * RefreshTexts cita xmlove sa lokacije Resources/xmls/inGameText/SpaceDefence+chosenLanguage
	 * 
	 * SplitTextIntoRows se koristi za prilagodjavanje duzine tekstova redu.
	 * */
	
	public static string chosenLanguage="_en";


	//////////////////////////////////////////////////////
	//
	// kolekcija stringova koji ce se koristiti u toku igre
	//
	//////////////////////////////////////////////////////
	
	public static string AppName="Panda Plane";
	public static string FacebookInviteMsg="Invite Your Friend";


	/////////////////////////////////////////////////////
	//
	// kraj kolekcije stringova 
	//
	//////////////////////////////////////////////////

	//Poziva se jednom kad se startuje igra
	void Awake()
	{
		transform.name="LanguageManager";
		DontDestroyOnLoad(gameObject);

		if(PlayerPrefs.HasKey("choosenLanguage"))
			chosenLanguage = PlayerPrefs.GetString("choosenLanguage");
		RefreshTexts();
		Debug.Log("Chosen Languafe iz Language Mangaer: " + chosenLanguage);
	}
	
	//Potrebno je pozvati se kad se promeni izabrani jezik.
	//Menja kolekciju stringova na vrednosti za odgovarajuci jezik
	public static void RefreshTexts()
	{
		TextAsset aset =(TextAsset)Resources.Load("xmls/inGameText/Language"+chosenLanguage);
		XElement xmlNov= XElement.Parse(aset.ToString());
		IEnumerable<XElement> xmls = xmlNov.Elements();	
		int number=xmls.Count();
		Debug.Log ("Ukupno ima "+number+" xml elemenata");
		AppName=xmls.ElementAt(0).Value;
		FacebookInviteMsg=xmls.ElementAt(0).Value;

	}
	

}


