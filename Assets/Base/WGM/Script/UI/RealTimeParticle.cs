using UnityEngine;

public class RealTimeParticle : MonoBehaviour
{
    private ParticleSystem _particle;
   
    void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_particle == null) return;
     
            _particle.Simulate(Time.unscaledDeltaTime, false, false);
            _particle.Play();

    }
}