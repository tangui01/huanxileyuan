using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChangeNGUILayer : MonoBehaviour {

	public int layer = 0;

	// Use this for initialization
	void Start () {
		Change(layer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Change(int layer)
	{
		UIWidget widget;
		foreach(Transform tran in GetComponentsInChildren<Transform>(true)) {
			widget = tran.GetComponent<UIWidget>();
			if(widget == null) continue;
			widget.depth += layer;
		}
	}
}
