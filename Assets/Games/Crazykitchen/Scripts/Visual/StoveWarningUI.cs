using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：StoveWarningUI.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房灶台警告UI
*****************************************************/
namespace Crzaykitchen
{
    public class StoveWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private bool show = false;
        private AudioSource audioSource;
        private float warningTimer;
        private float warningTimerMAx=0.2f;
        private void Start()
        {
            stoveCounter.OnPrecessChanged += OnStoveStateChanged;
            show = false;
            Hide();
        }

        private void Update()
        {
            warningTimer+=Time.deltaTime;
            if (warningTimer>=warningTimerMAx)
            {
                warningTimer = 0;
                SoundManager.Instance.PlayWarningSound();
            }
        }
        public void OnStoveStateChanged(object sender, IHasPrecess.OnPrecessChangedEventArgs e)
        {
            show = stoveCounter.IsFried() && e.Precess > 0.5f;
            if (show)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

