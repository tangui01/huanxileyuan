using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using WGM;

[System.Serializable]
public class UIOption : MonoBehaviour
{
	
	 
	public UILabel uil_name;
	public UILabel uil_value;
	public UILabel uil_tooltip;
	public UISprite uis_bg;
	public List<string> items = new List<string>();
	public UIOption uio_next_link;
	public UIOption uio_last_link;
	
	public GameObject onUp;
	public GameObject onDown;

	static private GameObject mCurrentSelect = null;

	static public BetterList<UIOption> list = new BetterList<UIOption>();
	public List<EventDelegate> onClick = new List<EventDelegate>();

	public Func<bool> actionSelect;
	public Action<int, string> actionChange;
	
	/// <summary>
	/// 是否开始被选中 Whether the object this script is attached to will get selected as soon as this script is enabled.
	/// </summary>
	public bool startsSelected = false;
	
	public bool onlyDisplay = false;
	/// <summary> 是否已经选择UI子项 </summary>
	public bool isEnterSelected = false;

	private int mFirstLimit = 0;
	public int firstLimit
	{
		get { return mFirstLimit; }
		set
		{
			mFirstLimit = value;
			if(index < mFirstLimit) {
				index = mFirstLimit;
			}
		}
	}

	private int mEndLimit = int.MaxValue;
	public int endLimit
	{
		get { return mEndLimit; }
		set
		{
			mEndLimit = value;
			if(index > mEndLimit) {
				index = mEndLimit;
			}
		}
	}

	Color background_color;
	public int item_idx = 0;

	protected virtual void OnEnable ()
	{
		list.Add(this);

		if(uis_bg != null) {
			background_color = uis_bg.color;
			background_color.a = 1;
			uis_bg.alpha = 0;
		}
		if(uil_tooltip != null) {
			uil_tooltip.alpha = 0;
		}

		if(items.Count > 0 && uil_value != null) {
			for(int i = 0; i < items.Count; i++) {
				if(items[i] != "null") {
					value = items[i];
					break;
				}
			}
		}
		if (startsSelected)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) return;
			#endif

			//if(mCurrentSelect == null) {
			if(UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject)) {
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.selectedObject = gameObject;
				mCurrentSelect = gameObject;
				if(uis_bg != null) {
					uis_bg.alpha = 1;
				}
			}
		}
	}

	protected virtual void OnDisable () { list.Remove(this); }

	// Update is called once per frame
	protected virtual void Update () {
		//防止选择焦点失去而造成的按键无法响应问题
		if( (UICamera.selectedObject == null || UICamera.selectedObject.name == "UI Root")) {
			if(mCurrentSelect != null) {
				UICamera.selectedObject = mCurrentSelect;
				mCurrentSelect.GetComponent<UIOption>().uis_bg.alpha = 1;
			}
		}
	}

	private int FindMin(int a, int b)
	{
		return a < b ? a : b;
	}

	protected GameObject GetUp ()
	{
		if (NGUITools.GetActive(onUp)) return onUp;
		return Get(Vector3.up, false);
	}
	
	protected GameObject GetDown ()
	{
		if (NGUITools.GetActive(onDown)) return onDown;
		return Get(Vector3.down, false);
	}

	
	void OnLeft()
	{
		for(int i = 0; i < items.Count; i++) {
			if(--item_idx < mFirstLimit) {
				item_idx = FindMin(items.Count-1, mEndLimit);
			}
			if(items[item_idx] != "null") {
				value = items[item_idx];
				if(actionChange != null) actionChange(index, value);
				break;
			}
		}
	}
	
	void OnRight()
	{
		for(int i = 0; i < items.Count; i++) {
			if(++item_idx > FindMin(items.Count-1, mEndLimit)) {
				item_idx = mFirstLimit;
			}

			if(items[item_idx] != "null") {
				value = items[item_idx];
				print(value);
				if(actionChange != null) actionChange(index, value);
				break;
			}
		}
	}
	
	
	protected GameObject Get (Vector3 myDir, bool horizontal)
	{
		Transform t = transform;
		myDir = t.TransformDirection(myDir);
		Vector3 myCenter = GetCenter(gameObject);
		float min = float.MaxValue;
		GameObject go = null;
		
		for (int i = 0; i < list.size; ++i)
		{
			UIOption opt = list[i];
			if (opt == this) continue;

			if(opt.onlyDisplay == true) continue;

			// Reject objects that are not within a 45 degree angle of the desired direction
			Vector3 dir = GetCenter(opt.gameObject) - myCenter;
			float dot = Vector3.Dot(myDir, dir.normalized);
			if (dot < 0.1f) continue;
			
			// Exaggerate the movement in the undesired direction
			dir = t.InverseTransformDirection(dir);
			if (horizontal) dir.y *= 2f;
			else dir.x *= 2f;
			
			// Compare the distance
			float mag = dir.sqrMagnitude;
			if (mag > min) continue;
			go = opt.gameObject;
			min = mag;
		}

		if(go) {
			mCurrentSelect = go;
		} else {
			go = mCurrentSelect;
		}

		return go;
	}
	
	static protected Vector3 GetCenter (GameObject go)
	{
		UIWidget w = go.GetComponent<UIWidget>();
		
		if (w != null)
		{
			Vector3[] corners = w.worldCorners;
			return (corners[0] + corners[2]) * 0.5f;
		}
		return go.transform.position;
	}
	
	protected virtual void OnKey (KeyCode key)
	{
		if (!NGUITools.GetActive(this)) return;
		
		GameObject go = null;
        
		switch (key)
		{
		case KeyCode.LeftArrow:
				if (isEnterSelected == false)
				{
					//	Debug.Log("键盘按下");
					go = GetUp();
				}
				if (isEnterSelected == true) {
			//	Debug.Log("键盘按左");
				OnLeft();
			}
			break;
		case KeyCode.RightArrow:
				if (isEnterSelected == false)
				{
					//	Debug.Log("键盘按下");
					go = GetDown();
				}
				if (isEnterSelected == true) {
			//	Debug.Log("键盘按右");
				OnRight();
			}
			break;
		case KeyCode.UpArrow:
			//if(isEnterSelected == false) {
			////	Debug.Log("键盘按上");
			//	go = GetUp();
			//}
			break;
		case KeyCode.DownArrow:
			//if(isEnterSelected == false) {
			////	Debug.Log("键盘按下");
			//	go = GetDown();
			//}
			break;
		case KeyCode.Return:
		case KeyCode.F9:
				break;
			case KeyCode.KeypadEnter :
			if (actionSelect != null) {
				if(!actionSelect()) { StartCoroutine(CShowTooltip()); return; }
			}
			if(uil_value != null) {
				isEnterSelected = !isEnterSelected;
				//uis_bg.color = isEnterSelected ? Color.red : background_color;
				if(isEnterSelected) {
					uil_value.transform.localScale = new Vector3(1.5f, 1.5f);
					uil_value.color = new Color(0, 1, 0);
					uis_bg.color = Color.red;
				} else {
					uil_value.transform.localScale = new Vector3(1f, 1f);
					uil_value.color = new Color(1, 1, 1);
					uis_bg.color = background_color;

					if(uio_next_link != null) {
						go = uio_next_link.gameObject;
						uio_next_link.isEnterSelected = !uio_next_link.isEnterSelected;
						if(uio_next_link.isEnterSelected) {
							uio_next_link.uil_value.transform.localScale = new Vector3(1.5f, 1.5f);
							uio_next_link.uil_value.color = new Color(0, 1, 0);
							uio_next_link.uis_bg.color = Color.red;
						} else {
							uio_next_link.uil_value.transform.localScale = new Vector3(1f, 1f);
							uio_next_link.uil_value.color = new Color(1, 1, 1);
							uio_next_link.uis_bg.color = background_color;
						}
					} else if(uio_last_link != null) {
						go = uio_last_link.gameObject;
					}
				}
			}
				OnClick();
			break;
		}

		if(go != null) {
			for(int i = 0; i < list.size; i++) {
				if(list[i].uis_bg == null) continue;
				if(list[i].gameObject == go) {
					go.transform.GetComponent<UIOption>().uis_bg.alpha = 1;
				} else {
					list[i].uis_bg.alpha = 0;
				}
			}

            mCurrentSelect = go;
			UICamera.selectedObject = go;
		}
	}
	
	protected virtual void OnClick ()
	{
		if(EventDelegate.IsValid(onClick)) {
			EventDelegate.Execute(onClick);
		}
	}

	public int index {
		get {return item_idx;}
		set {
			if(value >= 0 && value < items.Count) item_idx = value;
			if(uil_value == null) return;
			
			if(Localization.Exists(items[item_idx]))
				uil_value.text = Localization.Get(items[item_idx]);
			else
				uil_value.text = items[item_idx];

			ShowTooltip();
		}
	}

	public string value {
		get {return uil_value.text;}
		set {
			int idx = items.FindIndex((s) => { return s == value; });
			if(idx >= 0) {
				index = idx;
			} else {
				items.Add(value);
				index = items.FindIndex((s) => { return s == value; });
			}
			ShowTooltip();
		}
	}

	private int old_index = -1;
	public int oldIndex
	{
		get { return old_index; }
		set
		{
			old_index = value;
			ShowTooltip();
		}
	}

	void ShowTooltip()
	{
		if(uil_tooltip == null) return;

		if(old_index == -1) return;

		uil_tooltip.alpha = index != oldIndex ? 1 : 0;
	}

	IEnumerator CShowTooltip()
	{
		if(uil_tooltip.alpha == 1) yield break;

		uil_tooltip.alpha = 1;
		yield return new WaitForSeconds(2.0f);

		uil_tooltip.alpha = 0;
	}
}
