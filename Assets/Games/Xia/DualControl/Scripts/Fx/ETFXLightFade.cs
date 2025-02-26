using UnityEngine;
using System.Collections;
using WGM;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private Light li;
        private float initIntensity;
        
        void Awake()
        {
            GetComponent<AudioSource>().volume *= LibWGM.machine.SeVolume /10;
            GetComponent<AudioSource>().Play();
        }
        
        void Update()
        {
            
        }
    }
}