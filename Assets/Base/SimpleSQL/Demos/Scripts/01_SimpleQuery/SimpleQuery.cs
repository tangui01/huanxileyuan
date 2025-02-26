using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// This script shows how to call a simple SQL query from a database using the class definition of the
/// database to format the results.
/// 
/// In this example we overwrite the working database since no data is being changed. This is set in the 
/// SimpleSQLManager gameobject in the scene.
/// </summary>
public class Weapon
{ }
public class SimpleQuery : MonoBehaviour {

	// reference to our database manager object in the scene
	public SimpleSQL.SimpleSQLManager dbManager;
	
	// reference to the gui text object in our scene that will be used for output
	public Text outputText;
	
	void Start () 
	{
		// Gather a list of weapons and their type names pulled from the weapontype table		
		List<Weapon> weaponList = dbManager.Query<Weapon>(
														"SELECT " + 
															"W.WeaponID, " + 
															"W.WeaponName, " + 
															"W.Damage, " + 
															"W.Cost, " + 
															"W.Weight, " + 
															"W.WeaponTypeID, " + 
															"T.Description AS WeaponTypeDescription " + 
														"FROM " + 
															"Weapon W " + 
															"JOIN WeaponType T " + 
																"ON W.WeaponTypeID = T.WeaponTypeID " + 
														"ORDER BY " + 
															"W.WeaponID "
														);
		
		// output the list of weapons
		outputText.text = "Weapons\n\n";
	


        // get the first weapon record that has a WeaponID > 4
		outputText.text += "\nFirst weapon record where the WeaponID > 4: ";
        bool recordExists;
        Weapon firstWeaponRecord = dbManager.QueryFirstRecord<Weapon>(out recordExists, "SELECT WeaponName FROM Weapon WHERE WeaponID > 4");
       

	}
}
