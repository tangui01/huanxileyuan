using UnityEngine;
using System.Collections;

public class SubmarineParkourLevelEdge : MonoBehaviour 
{
	//Called when something triggerer the object
	void OnTriggerEnter(Collider other) 
	{
		//If a spawn triggerer is collided with this
		if (other.name == "SpawnTriggerer")
		{
			//Spawn a proper object
			switch (other.tag)
			{
				case "SecondLayer":
                    SubmarineParkourLevelGenerator.Instance.GenerateSecondLayer(0);
					break;
				
				case "ThirdLayer":
					SubmarineParkourLevelGenerator.Instance.GenerateThirdLayer(0);
					break;
					
				case "FourthLayer":
                    SubmarineParkourLevelGenerator.Instance.GenerateFourthLayer(0);
					break;
				
				case "Obstacles":
                    SubmarineParkourLevelGenerator.Instance.GenerateObstacles();
					break;
			}
		}
		//If a reset triggerer is collided with this
		else if (other.name == "ResetTriggerer")
		{
			//Reset the proper object
			switch (other.tag)
			{
				case "SecondLayer":
				case "ThirdLayer":
				case "FourthLayer":
                    SubmarineParkourLevelGenerator.Instance.SleepGameObject(other.transform.parent.gameObject);
					break;
				
				case "Obstacles":
					other.transform.parent.GetComponent<SubmarineParkourObstacles>().DeactivateChild();
                    SubmarineParkourLevelGenerator.Instance.SleepGameObject(other.transform.parent.gameObject);
					break;
			}
		}
		//If a power up is collided with this
		else if (other.tag == "PowerUps")
		{
			//Reset the power up
			other.GetComponent<SubmarineParkourPowerUp>().ResetThis();
		}
		//If a torpedo is collided with this
		else if (other.name == "Torpedo")
		{
			//Reset the torpedo
			other.transform.parent.gameObject.GetComponent<SubmarineParkourTorpedo>().ResetThis();
		}
	}
}
