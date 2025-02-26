using UnityEngine;
using System.Collections;

/**
  * Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...
  * Object:N/A
  * Description: Sample Description
  **/
public class BossInterfaceArrivalAnimEvent : MonoBehaviour {

	public void BossInterfaceShow()
	{
		GameObject.Find("BossHealthArmorScoreHolder/AnimationHolder").GetComponent<Animation>().Play();
	}

	void BossTimeSound()
	{
		SoundManager.Instance.Play_BossTime();
	}
}
