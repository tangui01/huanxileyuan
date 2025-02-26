using System.Collections;
using UnityEngine;
using WGM;

public class ParticleSoundPlayer : MonoBehaviour
{
    private ParticleSystem particleSystem;
    bool isPlaying = false;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (particleSystem.isPlaying)
        {
            StartCoroutine(PlayTime());
        }
    }

    IEnumerator PlayTime()
    {
        if(isPlaying)
            yield break;
        isPlaying = true;
        FindObjectOfType<SubmarineParkourSound>().PlaySound();
        yield return new WaitForSeconds(4f);
        isPlaying = false;
    }
}