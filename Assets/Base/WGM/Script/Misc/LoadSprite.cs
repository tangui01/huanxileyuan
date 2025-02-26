using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class LoadSprite {

    public static void Save(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".png";
        
        Texture2D textured = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        textured.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        textured.Apply();

        byte[] bytes = textured.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        bytes = null;
        GC.Collect();
        GameObject.Destroy(textured);
    }

    public static Sprite Load(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".png";
        Debug.Log(path);
        //创建文件读取流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 1024;
        int height = 768;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}
