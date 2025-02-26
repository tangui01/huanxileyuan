using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：CuttingProcessUI.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：切割柜台进度UI显示
*****************************************************/


namespace   Crzaykitchen
{
    public class ProcessUI : MonoBehaviour
    {
        [SerializeField]private Image processImage;
        [SerializeField] private GameObject hasPrecessGameObject;
        
         private IHasPrecess hasPrecess;
        private void Start()
        {
            hasPrecess=hasPrecessGameObject.GetComponent<IHasPrecess>();
            hasPrecess.OnPrecessChanged += SetPrecessImg;
            processImage.fillAmount = 0;
            Hide();
        }
        public void SetPrecessImg(object sender,IHasPrecess.OnPrecessChangedEventArgs e)
        {
            if (e.Precess>=1||e.Precess==0)
            {
                Hide();
            }
            else
            {
                Show();
            }
            processImage.fillAmount = e.Precess;
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

