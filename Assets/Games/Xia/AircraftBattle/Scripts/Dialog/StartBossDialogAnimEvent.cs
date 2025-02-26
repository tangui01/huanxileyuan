using UnityEngine;
using System.Collections;

/**
  * Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...
  * Object:N/A
  * Description: Sample Description
  **/
public class StartBossDialogAnimEvent : MonoBehaviour {

	public void StartConversationBetweenBossAndPanda()
	{
		GameObject.Find("BossDialogHolder/AnimationHolder").GetComponent<Animation>().Play ("DialogArrivalBoss");
		SoundManager.Instance.Play_DialogPopupArrival();
	}
}
