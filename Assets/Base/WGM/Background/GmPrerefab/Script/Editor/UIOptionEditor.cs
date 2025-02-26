
//#define DYNAMIC_FONT

//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;

///// <summary>
///// Inspector class used to edit UIPopupLists.
///// </summary>

//[CustomEditor(typeof(UIOption))]
//public class UIOptionEditor : Editor
//{
//	UIOption mList;

//	void OnEnable ()
//	{
//		mList = target as UIOption;
//	}

//	void RegisterUndo ()
//	{
//		NGUIEditorTools.RegisterUndo("Option List Change", mList);
//	}

//	public override void OnInspectorGUI ()
//	{
//		serializedObject.Update();
//		NGUIEditorTools.SetLabelWidth(80f);
		
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(6f);
//		GUILayout.Label("Options");
//		GUILayout.EndHorizontal();
		
//		string text = "";
//		foreach (string s in mList.items) text += s + "\n";
		
//		GUILayout.Space(-14f);
//		GUILayout.BeginHorizontal();
//		GUILayout.Space(84f);
//		string modified = EditorGUILayout.TextArea(text, GUILayout.Height(100f));
//		GUILayout.EndHorizontal();
		
//		if (modified != text)
//		{
//			RegisterUndo();
//			string[] split = modified.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
//			mList.items.Clear();
//			foreach (string s in split) mList.items.Add(s);
//		}

//		NGUIEditorTools.SetLabelWidth(120f);
//		NGUIEditorTools.DrawProperty("Starts Selected", serializedObject, "startsSelected");
//		NGUIEditorTools.DrawProperty("Only Display", serializedObject, "onlyDisplay");
//		NGUIEditorTools.DrawProperty("Tooltip", serializedObject, "uil_tooltip");
//		NGUIEditorTools.DrawProperty("Name", serializedObject, "uil_name");
//		//NGUIEditorTools.DrawProperty("BackGround", serializedObject, "uis_bg");
//		//NGUIEditorTools.DrawProperty("Value", serializedObject, "uil_value");
//		NGUIEditorTools.DrawProperty("NextLink", serializedObject, "uio_next_link");
//		NGUIEditorTools.DrawProperty("LastLink", serializedObject, "uio_last_link");

//		if (NGUIEditorTools.DrawHeader("Override"))
//		{
//			NGUIEditorTools.SetLabelWidth(60f);
//			NGUIEditorTools.BeginContents();
//			NGUIEditorTools.DrawProperty("Up", serializedObject, "onUp");
//			NGUIEditorTools.DrawProperty("Down", serializedObject, "onDown");
//			NGUIEditorTools.EndContents();
//		}

//		NGUIEditorTools.DrawEvents("On Click", mList, mList.onClick);
		
//		serializedObject.ApplyModifiedProperties();
//	}
	

//}