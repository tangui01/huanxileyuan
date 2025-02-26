using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollShop : MonoBehaviour {

	Scrollbar SC;
	float currentValue;
	// Use this for initialization
	void Start () {
		SC = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
	}
	
	public void ScrollUp()
	{
		SC.value = SC.value+0.05f;
	}

	public void ScrollDown()
	{
		SC.value = SC.value-0.05f;
	}
}
