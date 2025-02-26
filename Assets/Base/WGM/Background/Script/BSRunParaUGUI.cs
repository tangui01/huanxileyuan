using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BSRunParaUGUI : MonoBehaviour {

	
	[System.Serializable]
	public struct Item_Other
	{
		public Transform tf;
		public UISprite Background;
		public UILabel Name;
		public UILabel Value;

		public Item_Other(Transform tf)
		{
			this.tf = tf;
			Background = tf.Find("Background").GetComponent<UISprite>();
			Name = tf.Find("Name").GetComponent<UILabel>();
			Value = tf.Find("Value").GetComponent<UILabel>();
			
		}
	}

	public List<Item_Other> Item_OtherList;
	
	// Use this for initialization
	void Start ()
	{

		Item_OtherList = new List<Item_Other>();
		Item_OtherList.Add(new Item_Other(transform.Find("Item_难度调整")));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			Debug.Log("更改难度");
			//throw new NotImplementedException();
			Item_OtherList[0].Value.text = "开始修改";
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Debug.Log("更改难度B");
			//throw new NotImplementedException();
			Item_OtherList[0].Value.text = "开始修改b";
		}
	}
}
