using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class VFXPool : MonoBehaviour
{
    private int Index = 0;
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).GetComponent<AudioSource>().time = 0.62f;
        }
    }
    /// <summary>
    /// 播放方块销毁特效和音效
    /// </summary>
    public void Play(Vector3 VFXposition)
    {
        transform.GetChild(Index % 3).position = VFXposition;
        transform.GetChild(Index % 3).GetComponent<ParticleSystem>().Play();
        transform.GetChild(Index % 3).GetComponent<AudioSource>().volume = LibWGM.machine.SeVolume /10;
        transform.GetChild(Index % 3).GetComponent<AudioSource>().Play();
        Index++;
    }
    void Update()
    {
        
    }
}
