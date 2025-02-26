using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGP_004CompoundBigWatermelon
{ 

	public class AudioManager : IManager
	{

        private AudioClip m_SpawnSound;
        private AudioClip m_BombSound;
        private AudioClip m_collisionFruitsSound;
        private AudioClip m_collisionGrandSound;
        public void Init(Transform worldTrans, Transform uiTrans, params object[] manager)
        {
            m_SpawnSound = Resources.Load<AudioClip>(ResPathDefine.AUDIO_SPAWN_PATH);
            m_BombSound = Resources.Load<AudioClip>(ResPathDefine.AUDIO_BOMB_PATH);
            m_collisionFruitsSound = Resources.Load<AudioClip>(ResPathDefine.AUDIO_Fruits_PATH);
            m_collisionGrandSound = Resources.Load<AudioClip>(ResPathDefine.AUDIO_Grand_PATH);
        }
        public void Update()
        {
        }

        public void Destroy()
        {
            m_SpawnSound = null;
            m_BombSound = null;
        }


        public void PlaySpawnSound()
        {
            global::AudioManager.Instance.playerEffect1(m_SpawnSound);
        }

        public void PlayBombSound()
        {
            global::AudioManager.Instance.playerEffect1(m_BombSound);
        }

        public void PlayerCollisionFruitsSound()
        {
            global::AudioManager.Instance.playerEffect2(m_collisionFruitsSound);
        }
        public  void PlayerCollisionGrandSound()
        {
            global::AudioManager.Instance.playerEffect2(m_collisionGrandSound);
        }
    }
}
