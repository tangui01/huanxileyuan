using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MGP_004CompoundBigWatermelon
{ 
    /// <summary>
    /// 整个游戏入口
    /// </summary>
	public class GameStart : MonoBehaviour
    {

        public AudioClip bgm;
        private void Awake()
        {
            CompoundBigWatermelonGameManager.Instance.Awake(this);
        }

        // Start is called before the first frame update
        void Start()
		{
            CompoundBigWatermelonGameManager.Instance.Start();
            global::AudioManager.Instance.playerBGm(bgm);
        }

		// Update is called once per frame
		void Update()
		{
            CompoundBigWatermelonGameManager.Instance.Update();
        }

        private void OnDestroy()
        {
            CompoundBigWatermelonGameManager.Instance.Destroy();
        }
    }
}
