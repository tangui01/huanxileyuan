using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/****************************************************
    文件：SceneManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class SceneManagerWindow : EditorWindow
{
     public static void OpenSceneManagerWindow()
     {
         GetWindow<SceneManagerWindow>().Show();
     }
}
