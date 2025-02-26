using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WGM;

/****************************************************
    文件：GameSettingWindow.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：游戏配置窗口
*****************************************************/
public class GameSettingWindow : EditorWindow
{
    [MenuItem("Tools/游戏配置窗口")]
    static void OpenGameSettingWindow()
    {
        GetWindow<GameSettingWindow>().Show();
    }
    public static string Path =Application.streamingAssetsPath;
    private void OnGUI()
    {
        GUILayout.BeginVertical("游戏设置","box");
        {
            GUILayout.BeginHorizontal();
            {
                if (GUI.Button(new Rect(10,20,120,20),"打开开发者平台"))
                {
                    Application.OpenURL("https://www.huanxizn.com/developer/");
                }
                
                GUI.Label(new Rect(150,20,121,20),"账号：13560363230");
                if ( GUI.Button(new Rect(280,20,60,20),"复制账号"))
                {
                    GUIUtility.systemCopyBuffer = "13560363230";
                }
                GUI.Label(new Rect(350,20,120,20),"密码：123456");
                if ( GUI.Button(new Rect(440,20,60,20),"复制密码"))
                {
                    GUIUtility.systemCopyBuffer = "123456";
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            {
                if (GUI.Button(new Rect(10,60,120,20),"将所有场景Ab打包"))
                {
                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }
                    BuildPipeline.BuildAssetBundles(Path, BuildAssetBundleOptions.None, BuildTarget.Android);
                }
            }
            GUILayout.EndHorizontal();
            
        }
        GUILayout.EndVertical();
    }
}
