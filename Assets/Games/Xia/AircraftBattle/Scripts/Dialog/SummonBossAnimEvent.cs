using UnityEngine;
using System.Collections;

/**
  * Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...
  * Object:N/A
  * Description: Sample Description
  **/
public class SummonBossAnimEvent : MonoBehaviour {

	public void SummonBoss()
	{
		if(FindObjectOfType<DialogPanda>().noTutorial)
			LevelGenerator.Instance.CallBoss();
	}
}
