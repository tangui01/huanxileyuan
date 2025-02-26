using UnityEngine;
using System.Collections;

public class BossReady : MonoBehaviour {

	void Ready()
	{
		transform.parent.SendMessage("FireAway",SendMessageOptions.DontRequireReceiver);
		SoundManager.Instance.Play_BossMusic();
	}

	void BossPlaneArrival()
	{
		if(transform.parent.name.Contains("Plane"))
			SoundManager.Instance.Play_BossPlaneArrival();
	}

	void BossPlaneMovement()
	{
		if(transform.parent.name.Contains("Plane"))
			SoundManager.Instance.Play_BossPlaneMovement();
	}

	void StopPlaneMovementSound()
	{
		if(transform.parent.name.Contains("Plane"))
			SoundManager.Instance.Stop_BossPlaneMovement();
	}

	public void SenseiExplanation()
	{
		GameObject.Find("BossDialogHolder/AnimationHolder").GetComponent<Animation>().Play ("DialogArrivalSenseiShop");
		SoundManager.Instance.Play_DialogPopupArrival();
	}
}
