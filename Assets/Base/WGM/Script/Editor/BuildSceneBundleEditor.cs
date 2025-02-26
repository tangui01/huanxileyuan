using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(BuildSceneBundle))]
public class BuildSceneBundleEditor : Editor {

	BuildSceneBundle script;
	static ServerSelect buildServerSelect;

	void OnEnable()
    {
		script = (BuildSceneBundle)target;
    }

	[MenuItem("WGM Editor/OpenPersistentPath")]
	public static void OpenPersistentDataPath()
	{
		System.Diagnostics.Process.Start(Application.persistentDataPath);
	}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
		GUILayout.BeginHorizontal();
		NGUIEditorTools.DrawPaddedProperty("custom", serializedObject, "custom");
		if(GUILayout.Button("立即切换")) {
			SwitchTarget(script.custom.ToString());
		}
		GUILayout.EndHorizontal();

		NGUIEditorTools.DrawProperty("服务器选择", serializedObject, "buildServerSelect");

		buildServerSelect = script.buildServerSelect;

#if UNITY_STANDALONE_WIN
		if(GUILayout.Button("Build exe")) {
#elif UNITY_ANDROID
		if (GUILayout.Button("Build apk")) {
#endif
			Build(script.custom.ToString());
		}
		
        serializedObject.ApplyModifiedProperties();
	}

	static void SwitchTarget(string name)
	{
		string server = "";
		switch(buildServerSelect) {
			case ServerSelect.Normal: server = "huacai"; break;
			case ServerSelect.Local: server = "LOCAL;"; break;
			default: break;
		}

		string prjDefine = server +
			(name + ";");

		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, prjDefine);
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, prjDefine);
	}

	static void Build(string name)
	{
		List<string> buildScenes = new List<string>();
		switch(name) {
			case "kouhongji":
				PlayerSettings.productName = "口红机";
				buildScenes.Add(Application.dataPath + "/WGM/Scene/DaTing.unity");
				break;
			default: break;
		}

		SwitchTarget(name);

		string targetPath = "";
		BuildTargetGroup buildTargetGroup;
		BuildTarget buildTarget;

		targetPath = Application.dataPath.Remove(Application.dataPath.Length - 6) + "Build";
		if(!Directory.Exists(targetPath)) {
			Directory.CreateDirectory(targetPath);
		}

#if UNITY_STANDALONE_WIN
		buildTargetGroup = BuildTargetGroup.Standalone;
		buildTarget = BuildTarget.StandaloneWindows;
		targetPath += "/Win32";
		if(!Directory.Exists(targetPath)) {
			Directory.CreateDirectory(targetPath);
		}
		targetPath += "/" + name + ".exe";
#elif UNITY_ANDROID
		buildTargetGroup = BuildTargetGroup.Android;
		buildTarget = BuildTarget.Android;
		targetPath += "/Android";
		if(!Directory.Exists(targetPath)) {
			Directory.CreateDirectory(targetPath);
		}
		targetPath += "/" + name + ".apk";
#endif

		EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);

        string res = "";
        UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(buildScenes.ToArray(), targetPath, buildTarget, BuildOptions.None);
        if(report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded) {
            res = report.summary.ToString();
        }
        if(res.Length > 0) {
            throw new Exception("BuildPlayer failure: " + res);
        }
	}
}
