using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
    文件：Scenequickaccess.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

public class sceneinfo
{
    public string name;
    public string path;
}

public enum Modelinfo
{
    SelectOpenScene,
    ManageScene ,
    SelectStyle,
}

public class Scenequickaccess : EditorWindow
{
        [MenuItem("Tan/场景管理工具")]
        public static void ShowWindow()
        {
            GetWindow<Scenequickaccess>("场景管理工具").Show();
        }
        private Modelinfo expression = Modelinfo.SelectOpenScene;
        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                GetWindow<Scenequickaccess>("场景管理工具").Close();
                return;
            }
            GUILayout.BeginVertical();
            {
                expression= (Modelinfo)GUILayout.Toolbar((int)expression, new string[] { "打开场景", "管理场景","选择风格" });
                switch (expression)
                {
                    case Modelinfo.SelectOpenScene:
                        SceneListGUI();
                        break;
                    case Modelinfo.ManageScene:
                        ManageSceneGUI();
                        break;
                    case Modelinfo.SelectStyle:
                        SelectStyleGui();
                        GUI.color=stylecolor;
                        break;
                }
            }
           
            GUILayout.EndVertical();
        }
        #region 打开场景

        private static int index =0;
        private Dictionary<int, sceneinfo> sceneinfoDic = new Dictionary<int, sceneinfo>();
        private Vector2 ScrollPosition = Vector2.zero;
        private void SceneListGUI()
        {
            ScrollPosition= GUILayout.BeginScrollView(ScrollPosition);
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.Label("场景list");
                    index = GUILayout.SelectionGrid(index, GetScenenames().ToArray(), 4);
                    if (!EditorSceneManager.GetActiveScene().path.Equals(sceneinfoDic[index].path))
                    {
                        EditorSceneManager.OpenScene(sceneinfoDic[index].path);
                        Debug.Log("以打开场景:" + sceneinfoDic[index].name + "序列号为:" + index);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }

        #endregion
        #region 管理场景

        private void ManageSceneGUI()
        {
            GUILayout.BeginVertical("box");
            {
               GUILayout.BeginHorizontal();
               {
                   GUILayout.Label("当前打开场景:");
                   GUILayout.Label(EditorSceneManager.GetActiveScene().name);
               }
               GUILayout.EndHorizontal();
               if (GUILayout.Button("管理场景"))
               {
                   SceneManagerWindow.OpenSceneManagerWindow();
                   // AssetDatabase.DeleteAsset(EditorSceneManager.GetActiveScene().path);
                   // sceneinfoDic.Remove(index);
                   // index = 0;
                   // Debug.Log("删除场景:"+sceneinfoDic[index].name+"序列号为:"+index);
               }
            }
            GUILayout.EndVertical();
        }

        #endregion
        #region 选择风格
        private static Color stylecolor = Color.white;
        private void SelectStyleGui()
        {
            stylecolor= EditorGUILayout.ColorField("颜色", stylecolor);
        }

        #endregion
        private List<string> GetScenenames()
        {
            // 查找所有.unity文件
          List<string> scenePaths = AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.StartsWith("Assets/"))
                .ToList();
          for (int i = 0; i < scenePaths.Count; i++)
          {
              if (!sceneinfoDic.ContainsKey(i))
              {
                  sceneinfoDic.Add(i, new sceneinfo() { name = Path.GetFileNameWithoutExtension(scenePaths[i]), path = scenePaths[i] });
              }
          }
          List<string> scenenames = new List<string>();
          for (int i = 0; i < sceneinfoDic.Count; i++)
          {
              scenenames.Add(sceneinfoDic[i].name);
          }
          return scenenames;
        }
    
}
