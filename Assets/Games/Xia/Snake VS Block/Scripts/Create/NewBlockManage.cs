using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeVSBlock
{
    public class NewBlockManage : MonoBehaviour
    {
        public Transform snakePos;
        public GameObject[] walls;
        public List<AudioClip> audioClips = new List<AudioClip>();

        private void Awake()
        {
            AudioManager.Instance.playerBGm(audioClips[0]);
        }

        void Start()
        {
            
        }

        void Update()
        {
            try
            {
                if (snakePos != null && snakePos.GetChild(0) != null)
                {
                    
                    transform.position = new Vector3(transform.position.x,
                        snakePos.GetChild(0).position.y-10, transform.position.z);
                }
            }
            catch (Exception e)
            {
            }
        }
        public void PlayAudio(int i,float volume = 1f)
        {
            AudioManager.Instance.SetEff(audioClips[i],volume,Random.Range(0,3));
        }
    }
}