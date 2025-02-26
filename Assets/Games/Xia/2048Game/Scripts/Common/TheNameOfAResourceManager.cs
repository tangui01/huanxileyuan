using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TheNameOfAResourceManager : MonoBehaviour
{
    private static Dictionary<int, Sprite> dic_Sprite;
    private static Dictionary<string, AudioClip> dic_Sound;
    public List<Sprite> spriteArray = new List<Sprite>();

    private void Awake()
    {
        dic_Sprite  = new Dictionary<int, Sprite>();
        dic_Sound = new Dictionary<string, AudioClip>();
        for (int i = 0; i < spriteArray.Count; i++)
        {
            dic_Sprite.Add(int.Parse(spriteArray[i].name), spriteArray[i]);
        }
    }
    //根据数字，加载相应图片
    public static Sprite GetImage(int number)
    {
        if (dic_Sprite.ContainsKey(number))
            return dic_Sprite[number];
        return null;
    }

    public AudioClip GetSound(string name)
    {
        AudioClip clip = null;
        if (dic_Sound.ContainsKey(name))
            clip = dic_Sound[name];
        else
        {
            clip = Resources.Load<AudioClip>("Sounds/" + name);
            dic_Sound.Add(name, clip);
        }
        return clip;
    }
}
