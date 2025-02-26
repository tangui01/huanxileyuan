using UnityEngine;
using System.Collections;
using WGM;

public class SetEnemyWave : MonoBehaviour {

	GameObject EnemyWave;
	public static int randomNumber = 0, randomNumberOld=0;
	int randomLevelOfEnemy=0; // 1-Basic, 2-Intermediate, 3-Advanced
	float numberOfEnemiesInWave;
//	int [] numberEnemyType = new int[36];
	int currentStage;
	int currentTerrainInStage;
	int planeHealthLvl, planeArmorLvl, planeMainGunLvl, planeWingGunLvl, planeSideGunLvl;
	Sprite Blue1, Blue2, Blue3, Yellow1, Yellow2, Yellow3, Orange1, Orange2, Orange3, Hit1, Hit2, Hit3;
	private string lastEnemy = "";
	private int whiteCount = 0;
	void Start()
	{
//		for(int i=0;i<36;i++)
//		{
//			numberEnemyType[i]=i+1;
//		}
		Blue1 = GameObject.Find("EnemyBlue1").GetComponent<SpriteRenderer>().sprite;
		Blue2 = GameObject.Find("EnemyBlue2").GetComponent<SpriteRenderer>().sprite;
		Blue3 = GameObject.Find("EnemyBlue3").GetComponent<SpriteRenderer>().sprite;
		Yellow1 = GameObject.Find("EnemyYelow1").GetComponent<SpriteRenderer>().sprite;
		Yellow2 = GameObject.Find("EnemyYelow2").GetComponent<SpriteRenderer>().sprite;
		Yellow3 = GameObject.Find("EnemyYelow3").GetComponent<SpriteRenderer>().sprite;
		Orange1 = GameObject.Find("EnemyOrange1").GetComponent<SpriteRenderer>().sprite;
		Orange2 = GameObject.Find("EnemyOrange2").GetComponent<SpriteRenderer>().sprite;
		Orange3 = GameObject.Find("EnemyOrange3").GetComponent<SpriteRenderer>().sprite;
		Hit1 = GameObject.Find("Enemy1Hit").GetComponent<SpriteRenderer>().sprite;
		Hit2 = GameObject.Find("Enemy2Hit").GetComponent<SpriteRenderer>().sprite;
		Hit3 = GameObject.Find("Enemy3Hit").GetComponent<SpriteRenderer>().sprite;
		
		planeHealthLvl = PandaPlane.Instance.healthLvl;
		planeArmorLvl = PandaPlane.Instance.armorLvl;
		planeMainGunLvl = PandaPlane.Instance.mainGunLvl;
		planeWingGunLvl = PandaPlane.Instance.wingGunLvl;
		planeSideGunLvl = PandaPlane.Instance.sideGunLvl;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag.Equals("Spawn"))
		{
			SetEnemyFromPrefab();
//			Destroy(this.gameObject); //unistava SpawnPlace objekat
		}


		
	}

	void SetEnemyFromPrefab()
	{
		string nameOfWave = SetNameOfEnemy();
		while (lastEnemy.Equals(nameOfWave))
		{
			nameOfWave = SetNameOfEnemy();
			if(whiteCount == 3)
				break;
			whiteCount++;
		}
		lastEnemy = nameOfWave;
		whiteCount = 0;
		EnemyWave = (GameObject)Instantiate(Resources.Load("EnemyWaves/"+nameOfWave));
		
		EnemyWave.transform.position = transform.position;

		string typeAndColorOfPlane = SetTypeAndColor();
		Sprite EnemyLook = null, EnemyHit=null;
		switch (typeAndColorOfPlane)
		{
		case "EnemyBlue1": 
			EnemyLook = Blue1; 
			EnemyHit = Hit1;
			break;

		case "EnemyBlue2": 
			EnemyLook = Blue2; 
			EnemyHit = Hit2;
			break;

		case "EnemyBlue3": 
			EnemyLook = Blue3; 
			EnemyHit = Hit3;
			break;

		case "EnemyYelow1": 
			EnemyLook = Yellow1; 
			EnemyHit = Hit1;
			break;
			
		case "EnemyYelow2": 
			EnemyLook = Yellow2; 
			EnemyHit = Hit2;
			break;
			
		case "EnemyYelow3": 
			EnemyLook = Yellow3; 
			EnemyHit = Hit3;
			break;

		case "EnemyOrange1": 
			EnemyLook = Orange1; 
			EnemyHit = Hit1;
			break;
			
		case "EnemyOrange2": 
			EnemyLook = Orange2; 
			EnemyHit = Hit2;
			break;
			
		case "EnemyOrange3": 
			EnemyLook = Orange3; 
			EnemyHit = Hit3;
			break;

		}


		if(randomLevelOfEnemy==1)
		{
			switch (randomNumber)
			{
			case 1: case 2:  case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 11: case 12: case 13: case 14: case 15: case 16: case 24:
				for(int i=0;i<EnemyWave.transform.childCount;i++)
				{
					EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=EnemyLook;
					EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = EnemyHit;
				}
				break;
			case 0: case 17: case 18: 
				break;
			case 55:
				break;
			}
		}
		else if(randomLevelOfEnemy==2)
		{
			switch (randomNumber)
			{
			case 6: case 7: case 8: case 10: 
				for(int i=0;i<EnemyWave.transform.childCount;i++)
				{
					EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=EnemyLook;
					EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = EnemyHit;
				}
				break;
			case 0: case 1: case 2:  case 3: case 4: case 5: case 9: case 11: case 12: case 13: case 14: 
				break;
			case 55:
				break;
			}
		}
		else
		{
			switch (randomNumber)
			{
			case 2: 
				for(int i=0;i<EnemyWave.transform.childCount;i++)
				{
					EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=EnemyLook;
					EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = EnemyHit;
				}
				break;
			case 0: case 3: case 4: case 5: case 9: case 11: case 12: case 13: case 14: 
				break;
			case 55:
				break;
			}
		}
//		randomNumber=18;
//		switch (randomNumber)
//		{
//		case 1: case 2:  case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 11: case 12: case 13: case 14: case 15: case 16: case 22: case 23: case 24: case 25:  case 26: case 27: case 28: case 29: case 30: case 31: case 32: case 33: case 34: case 35: 
//			for(int i=0;i<EnemyWave.transform.childCount;i++)
//			{
//				EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite=EnemyLook;
//				EnemyWave.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = EnemyHit;
//			}
//			break;
//		case 0: case 17: case 18: case 19: case 20: case 21:
//			break;
//		case 55:
//			break;
//		}

	}

	string SetNameOfEnemy() // pametno bira brojku od 0 do 35, kako bi odabrali tip neprijatelja
	{
		string name;
		
		currentStage = LevelGenerator.currentStage;
		currentTerrainInStage = LevelGenerator.terrainsPassed;
		
		switch (currentTerrainInStage)
		{
		case 0: case 1: case 2:        
			// Do Something
			if(currentStage<3)
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,6);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<5)
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,9);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<7)
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,12);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<9)
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,15);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<11)
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,18);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<13)
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,21);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			else
			{
				randomLevelOfEnemy=1;
				do
				{
					randomNumber = Random.Range(1,25);
				}
				while(randomNumberOld==randomNumber);
				
				randomNumberOld=randomNumber;
			}
			break;
		case 3: case 4: case 5: case 6: 
			// Do Something
			if(currentStage<3)
			{
				randomLevelOfEnemy=Random.Range(1,3);

				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,16);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,6);
					}
					while(randomNumberOld==randomNumber);
				}

				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<5)
			{
				randomLevelOfEnemy=Random.Range(1,3);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,18);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,6);
					}
					while(randomNumberOld==randomNumber);
				}

				randomNumberOld=randomNumber;
			}
			else if(currentStage<7)
			{
				randomLevelOfEnemy=Random.Range(1,3);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,20);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,8);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<9)
			{
				randomLevelOfEnemy=Random.Range(1,3);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,21);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,11);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<11)
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,22);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(1,14);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,3);
					}
					while(randomNumberOld==randomNumber);
				}

				randomNumberOld=randomNumber;
			}
			else if(currentStage<13)
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,23);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(1,9);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,5);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(currentTerrainInStage*2,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(1,21);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,6);
					}
					while(randomNumberOld==randomNumber);
				}

				randomNumberOld=randomNumber;
			}
			break;
		case 7: case 8:
			// Do Something
			if(currentStage<3)
			{
				randomLevelOfEnemy=Random.Range(1,3);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(5,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,6);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<5)
			{
				randomLevelOfEnemy=Random.Range(1,3);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(14,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,9);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<7)
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(17,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(1,12);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,6);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<9)
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(20,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(1,15);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(1,7);
					}
					while(randomNumberOld==randomNumber);
				}

				randomNumberOld=randomNumber;
			}
			else if(currentStage<11)
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(23,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(1,18);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(4,8);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else if(currentStage<13)
			{
				randomLevelOfEnemy=Random.Range(1,3);
				if(randomLevelOfEnemy==1)
				{
					do{
						randomNumber = Random.Range(18,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(4,20);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(5,9);
					}
					while(randomNumberOld==randomNumber);
				}
				
				randomNumberOld=randomNumber;
			}
			else
			{
				randomLevelOfEnemy=Random.Range(1,4);
				if(randomLevelOfEnemy==1)
				{
					do
					{
						randomNumber = Random.Range(19,25);
					}
					while(randomNumberOld==randomNumber);
				}
				else if(randomLevelOfEnemy==2)
				{
					do
					{
						randomNumber = Random.Range(5,23);
					}
					while(randomNumberOld==randomNumber);
				}
				else
				{
					do
					{
						randomNumber = Random.Range(6,11);
					}
					while(randomNumberOld==randomNumber);
				}

				randomNumberOld=randomNumber;
			}
			break;
		default:
			// Do Something
			break;
		}


		if(randomLevelOfEnemy==1)
		{
			name = "Basic/EnemyWaveBasic"+randomNumber;
		}
		else if(randomLevelOfEnemy==2)
		{
			name = "Intermediate/EnemyWaveIntermediate"+randomNumber;
		}
		else
		{
			name = "Advanced/EnemyWaveAdvanced"+randomNumber;
			
		}
		
		return name;
	}
//
//	string SetNameOfEnemy() // pametno bira brojku od 0 do 35, kako bi odabrali tip neprijatelja
//	{
//		string name;
//
//		currentStage = LevelGenerator.currentStage;
//		currentTerrainInStage = LevelGenerator.terrainsPassed;
//
//		switch (currentTerrainInStage)
//		{
//		case 0: case 1: case 2:        
//			// Do Something
//			if(currentStage<3)
//			{
//				do
//				{
//					randomNumber = Random.Range(1,7);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<5)
//			{
//				do
//				{
//					randomNumber = Random.Range(1,12);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<7)
//			{
//				do
//				{
//					randomNumber = Random.Range(1,17);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<9)
//			{
//				do
//				{
//					randomNumber = Random.Range(1,22);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<11)
//			{
//				do
//				{
//					randomNumber = Random.Range(1,27);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<13)
//			{
//				do
//				{
//					randomNumber = Random.Range(1,31);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else
//			{
//				do
//				{
//					randomNumber = Random.Range(1,31);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			break;
//		case 3: case 4: case 5: case 6: 
//			// Do Something
//			if(currentStage<3)
//			{
//				do
//				{
//					randomNumber = Random.Range(currentTerrainInStage*2,19);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<5)
//			{
//				do
//				{
//					randomNumber = Random.Range(currentTerrainInStage*2,22);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<7)
//			{
//				do
//				{
//					randomNumber = Random.Range(currentTerrainInStage*2,25);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<9)
//			{
//				do
//				{
//					randomNumber = Random.Range(currentTerrainInStage*2,28);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<11)
//			{
//				do
//				{
//					randomNumber = Random.Range(currentTerrainInStage*2,31);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<13)
//			{
//				do{
//					randomNumber = Random.Range(currentTerrainInStage*2,34);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else
//			{
//				do{
//					randomNumber = Random.Range(currentTerrainInStage*2,37);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			break;
//		case 7: case 8:
//			// Do Something
//			if(currentStage<3)
//			{
//				do{
//					randomNumber = Random.Range(11,23);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<5)
//			{
//				do{
//					randomNumber = Random.Range(14,26);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<7)
//			{
//				do{
//					randomNumber = Random.Range(17,29);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<9)
//			{
//				do{
//					randomNumber = Random.Range(20,31);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<11)
//			{
//				do{
//					randomNumber = Random.Range(23,33);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else if(currentStage<13)
//			{
//				do{
//					randomNumber = Random.Range(26,35);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			else
//			{
//				do{
//					randomNumber = Random.Range(29,37);
//				}
//				while(randomNumberOld==randomNumber);
//
//				randomNumberOld=randomNumber;
//			}
//			break;
//		default:
//			// Do Something
//			break;
//		}
//		if(randomNumber<17)
//		{
//			name = "Basic/EnemyWave"+randomNumber;
////			name = "Intermediate/EnemyWave"+17;
//		}
//		else if(randomNumber<29)
//		{
//			name = "Intermediate/EnemyWave"+randomNumber;
//		}
//		else
//		{
//			name = "Advanced/EnemyWave"+randomNumber;
//
//		}
//
//		return name;
//	}


	string SetTypeAndColor()
	{
		int number=PandaPlane.Instance.enemyLevelIndex;
		string nameAndColor;
		number+=Random.Range(0,number);
//		"KADA SE ODRADE SVI NEPRIJATELJI ZAVISNO OD randomNumber-a, stage i indexa odrediti koji sprite da ide"

		if(currentStage<3)
		{
		
			if(number<20)
			{
				nameAndColor = "EnemyBlue1";
			}
			else if(number<30)
			{
				nameAndColor = "EnemyYelow1";
			}
			else
			{
				nameAndColor = "EnemyOrange1";
			}

		}
		else if(currentStage<5)
		{
			if(number<17)
			{
				nameAndColor = "EnemyBlue2";
			}
			else if(number<28)
			{
				nameAndColor = "EnemyYelow2";
			}
			else
			{
				nameAndColor = "EnemyOrange2";
			}
		}
		else if(currentStage<7)
		{
			if(number<16)
			{
				nameAndColor = "EnemyBlue3";
			}
			else if(number<28)
			{
				nameAndColor = "EnemyYelow3";
			}
			else
			{
				nameAndColor = "EnemyOrange3";
			}
		}
		else
		{
			if(number<15)
			{
				nameAndColor = "EnemyBlue3";
			}
			else if(number<27)
			{
				nameAndColor = "EnemyYelow3";
			}
			else
			{
				nameAndColor = "EnemyOrange3";
			}
		}

//		return "EnemyBlue3";
		return nameAndColor;
	}



}
