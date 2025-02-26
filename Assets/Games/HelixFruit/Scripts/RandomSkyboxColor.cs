using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSkyboxColor : MonoBehaviour
{
    [SerializeField] Material[] _skyboxMats;
    [SerializeField] Skybox _skybox;
    private void Start()
    {
        _skybox.material = _skyboxMats[Random.Range(0, _skyboxMats.Length)];
    }
}
