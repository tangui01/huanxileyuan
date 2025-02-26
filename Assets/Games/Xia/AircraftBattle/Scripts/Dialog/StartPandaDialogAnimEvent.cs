using UnityEngine;
using System.Collections;

/**
  * Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...
  * Object:N/A
  * Description: Sample Description
  **/
public class StartPandaDialogAnimEvent : MonoBehaviour {

	public void StartPandaAnswer()
	{
		if(FindObjectOfType<DialogPanda>().pandaCanSpeak)
		{
			GameObject.Find("PandaDialogHolder/AnimationHolder").GetComponent<Animation>().Play ("DialogArrivalPanda");
			SoundManager.Instance.Play_DialogPopupArrival();
		}
	}
}
