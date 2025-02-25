﻿//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2018 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UISpriteAnimations.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UISpriteAnimation))]
public class UISpriteAnimationInspector : Editor
{
	/// <summary>
	/// Draw the inspector widget.
	/// </summary>

	public override void OnInspectorGUI ()
	{
		GUILayout.Space(3f);
		NGUIEditorTools.SetLabelWidth(80f);
		serializedObject.Update();

		//NGUIEditorTools.DrawProperty("Frame Index", serializedObject, "frame");
		NGUIEditorTools.DrawProperty("Framerate", serializedObject, "mFPS");
		NGUIEditorTools.DrawProperty("Name Prefix", serializedObject, "mPrefix");
		NGUIEditorTools.DrawProperty("PlayMode", serializedObject, "mPlayMode");
		NGUIEditorTools.DrawProperty("Pixel Snap", serializedObject, "mSnap");
		NGUIEditorTools.DrawProperty("Ignore timescale", serializedObject, "ignoreTimeScale");
		NGUIEditorTools.DrawProperty("forward", serializedObject, "playForward");

		serializedObject.ApplyModifiedProperties();
	}
}
