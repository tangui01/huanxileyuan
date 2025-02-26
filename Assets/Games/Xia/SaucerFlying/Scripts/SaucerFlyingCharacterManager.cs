using UnityEngine;
using System.Collections;

namespace SaucerFlying
{
    public class SaucerFlyingCharacterManager : MonoBehaviour
    {
        public static SaucerFlyingCharacterManager Instance;

        public int CurrentCharacterIndex;


        public GameObject[] characters;

        void Awake()
        {
            CurrentCharacterIndex =  Random.Range(0,characters.Length);
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}