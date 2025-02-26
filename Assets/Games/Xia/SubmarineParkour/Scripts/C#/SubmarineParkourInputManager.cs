using UnityEngine;
using System.Collections;
using WGM;

public class SubmarineParkourInputManager : MonoBehaviour 
{
	public bool useTouch = false;				//Use touch based controls
	
	public LayerMask mask = -1;					//Set input layer mask
	
	Ray ray;									//The hit ray
	RaycastHit hit;								//The hit raycast
	
	Transform button;							//The triggered button
	
	//Called at every frame
	void Update () 
	{
		if (useTouch)
			GetTouches();
		else
			GetClicks();
	}
	//If playing with mouse
	void GetClicks()
	{
		//If we pressed the mouse
		
		// if(Input.GetMouseButtonDown(0))
		// {
		// 	//Cast a ray
		// 	ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// 	
		// 	//If the ray hit something in the set layer
		// 	if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
		// 	{
		// 		//Register it, and send it to the GUI manager
		// 		button = hit.transform;
		// 		SubmarineParkourGUIManager.Instance.ButtonDown(button);
		// 	}
		// 	//If the ray didn't hit a GUI object
		// 	else
		// 	{
		// 		button = null;
		// 	}
		// }
		// //If the click was released
		// else if (Input.GetMouseButtonUp(0))
		// {
		// 	//If there is no button registered previousely
		// 	if (button != null)
		// 		SubmarineParkourGUIManager.Instance.ButtonUp(button);
		// 	
		// }

		if (DealCommand.GetKey(1,(AppKeyCode)0))
		{
			SubmarineParkourPlayerManager.Instance.MoveUp();
		}
		
		if (DealCommand.GetKey(1,(AppKeyCode)2))
		{
			SubmarineParkourPlayerManager.Instance.MoveDown();
		}

		if (DealCommand.GetKey(1,(AppKeyCode)6))
		{
			SubmarineParkourPlayerManager.Instance.MoveLeft();
		}
#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.D))
		{
			SubmarineParkourPlayerManager.Instance.MoveRight();
		}
#else
		if (DealCommand.GetKey(1,(AppKeyCode)1))
		{
			SubmarineParkourPlayerManager.Instance.MoveRight();
		}
#endif		
		
	}
	//If playing with touch screen
	void GetTouches()
	{
		//Loop through the touches
		foreach (Touch touch in Input.touches) 
		{
			//If a touch has happened
            if (touch.phase == TouchPhase.Began && touch.phase != TouchPhase.Canceled)
			{
				//Cast a ray
				ray = Camera.main.ScreenPointToRay(touch.position);
				
				//If the ray hit something in the set layer
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
				{
					//Register it, and send it to the GUI manager
					button = hit.transform;
                    SubmarineParkourGUIManager.Instance.ButtonDown(button);
				}
				//If the ray didn't hit a GUI object
				else
				{
					//Set the button to null, and move the sub up
					button = null;
                    SubmarineParkourPlayerManager.Instance.MoveUp();
				}
			}
			//If a touch has ended
			else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				//If there is no button registered previousely
				if (button == null)
					//Move the sub down
                    SubmarineParkourPlayerManager.Instance.MoveDown();
				//If there is a button registered
				else
					//Send it to the GUI manager
                    SubmarineParkourGUIManager.Instance.ButtonUp(button);
			}
		}
	}
}
