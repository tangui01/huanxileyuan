using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelCreator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Level level = (Level)target;

        if (GUILayout.Button("Create  Empty Level"))
        {
            level.CreateLevelEmpty();
        }

        if (GUILayout.Button("Start Line")) {
            level.StartRoad();
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Block 1"))
        {
            level.block1();
        }
        if (GUILayout.Button("Block 2"))
        {
            level.block2();
        }
        

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Block 3"))
        {
            level.block3();
        }

        if (GUILayout.Button("Block 4"))
        {
            level.block4();
        }
       
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Block 5"))
        {
            level.block5();
        }

        if (GUILayout.Button("Block 6"))
        {
            level.block6();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Block 7"))
        {
            level.block7();
        }

        if (GUILayout.Button("Block 8"))
        {
            level.block8();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Block 9"))
        {
            level.block9();
        }

        if (GUILayout.Button("Block 10"))
        {
            level.block10();
        }

        GUILayout.EndHorizontal();



        if (GUILayout.Button("Finish Line!"))
        {
            level.FinishLine();
        }
    }
}
