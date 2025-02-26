using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ComponentCopyWindow : EditorWindow
{
    private GameObject rootGO;
    private List<Component> rootComponents;

    private GameObject targetGO;
    private List<Component> targetComponents;

    [MenuItem("Wonderland6627/ComponentCopyWindow")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindowWithRect<ComponentCopyWindow>(new Rect(0, 0, 700, 350));
    }

    private void OnEnable()
    {
        rootComponents = new List<Component>();
        targetComponents = new List<Component>();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(200, 0, 500, 20), "��'ԭʼ����'����������Ƶ�'����������'��������Transform��Tag��Layer��");
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("ԭʼ���壺");
            rootGO = EditorGUILayout.ObjectField(rootGO, typeof(GameObject)) as GameObject;
            if (rootGO)
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.BeginArea(new Rect(5, 50, 300, 300), GUI.skin.box);
                    {
                        if (GUILayout.Button("����������"))
                        {
                            rootComponents = rootGO.GetComponents<Component>().ToList();
                            rootComponents.RemoveAll((comp) =>
                            {
                                return comp.GetType() == typeof(Transform);
                            });
                        }

                        if (rootComponents != null && rootComponents.Count != 0)
                        {
                            for (int i = 0; i < rootComponents.Count; i++)
                            {
                                GUILayout.Label(i + " " + rootComponents[i].GetType().Name);
                            }
                        }
                    }
                    GUILayout.EndArea();
                }
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(50);
            if (rootGO && targetGO)
            {
                GUI.enabled = (rootComponents.Count != 0);
                if (GUI.Button(new Rect(310, 160, 70, 30), "Copy to =>", EditorStyles.toolbarButton))
                {
                    for (int i = 0; i < rootComponents.Count; i++)
                    {
                        ComponentUtility.CopyComponent(rootComponents[i]);
                        ComponentUtility.PasteComponentAsNew(targetGO);
                    }

                    targetComponents = targetGO.GetComponents<Component>().ToList();
                    targetComponents.RemoveAll((comp) =>
                    {
                        return comp.GetType() == typeof(Transform);
                    });
                }
                GUI.enabled = true;
            }
            GUILayout.Space(50);

            GUILayout.Label("�������壺");
            targetGO = EditorGUILayout.ObjectField(targetGO, typeof(GameObject)) as GameObject;
            if (targetGO)
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.BeginArea(new Rect(5 + 80 + 300, 50, 300, 300), GUI.skin.box);
                    {
                        if (targetComponents != null && targetComponents.Count != 0)
                        {
                            for (int i = 0; i < targetComponents.Count; i++)
                            {
                                GUILayout.Label(i + " " + targetComponents[i].GetType().Name);
                            }
                        }
                    }
                    GUILayout.EndArea();
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}

