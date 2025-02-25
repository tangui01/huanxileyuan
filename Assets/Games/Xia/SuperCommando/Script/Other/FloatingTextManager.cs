﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour {
	public static FloatingTextManager Instance;
	[Header("Floating Text")]
	public GameObject FloatingText;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	
	public void ShowText(FloatingTextParameter para, Vector2 ownerPosition){
		if (FloatingText == null) {
			Debug.LogError ("Need place FloatingText to GameManage object");
			return;
		}

		GameObject floatingText = SpawnSystemHelper.GetNextObject(FloatingText,false);
		var _position = Camera.main.WorldToScreenPoint (para.localTextOffset + ownerPosition);

		floatingText.transform.SetParent (SuperCommandoMenuManager.Instance.transform,false);
		floatingText.transform.position = _position;

		var _FloatingText = floatingText.GetComponent<FloatingText> ();
		_FloatingText.SetText (para.message, para.textColor, para.localTextOffset + ownerPosition);
		floatingText.SetActive (true);
	}
}

[System.Serializable]
public class FloatingTextParameter{
	[Header("Display Text Message")]
	public string message = "MESSAGE";
	public Vector2 localTextOffset = new Vector2(0,1);
	public Color textColor = Color.yellow;
}
