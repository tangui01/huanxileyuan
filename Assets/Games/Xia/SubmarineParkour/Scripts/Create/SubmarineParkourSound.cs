using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class SubmarineParkourSound : MonoBehaviour
{
    public List<AudioSource> audioSources = new List<AudioSource>();
    private int SourceIndex = 0;
    float volume = 0.75f;
    void Start()
    {
        volume = audioSources[0].volume;
    }
    
    void Update()
    {
        
    }

    public void PlaySound()
    {
        audioSources[SourceIndex % audioSources.Count].volume = volume * LibWGM.machine.SeVolume / 10f;
        audioSources[SourceIndex % audioSources.Count].PlayOneShot(audioSources[SourceIndex % audioSources.Count].clip);
        SourceIndex++;
    }
}
