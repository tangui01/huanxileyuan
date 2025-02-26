using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildAssets : MonoBehaviour {

    [MenuItem("WGM/Clear PC PersistentDataPath")]
    public static void ClearPersistentDataPath()
    {
        foreach (string dir in Directory.GetDirectories(Application.persistentDataPath))
        {
            Directory.Delete(dir, true);
        }
        foreach (string file in Directory.GetFiles(Application.persistentDataPath))
        {
            File.Delete(file);
        }
    }

    [MenuItem("WGM/Open PC PersistentDataPath Folder")]
    public static void OpenPersistentDataPath()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }
}
