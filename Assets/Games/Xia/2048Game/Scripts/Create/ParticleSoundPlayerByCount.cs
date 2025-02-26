using System;
using System.Collections;
using UnityEngine;
using WGM;

public class ParticleSoundPlayerByCount : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private AudioSource audioSource;
    private int initialParticleCount;
    public int playCount;
    private bool isPlaying = false;
    private float volume;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
    }
    
    void Update()
    {
        if (particleSystem.particleCount >=playCount && audioSource.isPlaying == false && isPlaying == false)
        {
            StartCoroutine(SoundEnd());
        }
    }
    
    IEnumerator SoundEnd()
    {
        isPlaying = true;
        audioSource.volume = volume *LibWGM.machine.SeVolume / 10f;
        audioSource.Play();
        yield return new WaitForSeconds(4f);
        isPlaying = false;
    }
}