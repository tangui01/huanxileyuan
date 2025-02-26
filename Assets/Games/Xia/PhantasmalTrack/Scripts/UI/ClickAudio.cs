using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhantasmalTrack
{
    public class ClickAudio : MonoBehaviour
    {
        private ManagerVars vars;

        private void Awake()
        {
            vars = FindObjectOfType<ManagerVars>();
            EventCenter.AddListener(EventDefine.PlayClikAudio, PlayAudio);
            EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.PlayClikAudio, PlayAudio);
            EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        }

        private void PlayAudio()
        {
            AudioManager.Instance.playerEffect1(vars.buttonClip);
        }

        /// <summary>
        /// 音效是否开启
        /// </summary>
        /// <param name="value"></param>
        private void IsMusicOn(bool value)
        {
            
        }
    }
}
