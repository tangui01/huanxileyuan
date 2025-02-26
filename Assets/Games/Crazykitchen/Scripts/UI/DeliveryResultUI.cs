using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：DeliveryResultUI.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class DeliveryResultUI : MonoBehaviour
    {
        [SerializeField] private Animator DeliverSuccess;
        [SerializeField] private Animator DeliverFail;
        private void Start()
        {
            CrzayKitchenGameManager.instance.OnRecipeSuccessed += OrderManager_OnRecipeSuccessed;
            CrzayKitchenGameManager.instance.OnRecipeFailed += OrderManager_OnRecipeFailed;
        }
        public void OrderManager_OnRecipeSuccessed()
        {
            DeliverSuccess.gameObject.SetActive(true);
            DeliverSuccess.SetTrigger("IsShow");
            Invoke("DelayHide",0.5f);
        }
        public void OrderManager_OnRecipeFailed()
        {
            DeliverFail.gameObject.SetActive(true);
            DeliverFail.SetTrigger("IsShow");
            Invoke("DelayHide",0.5f);
        }

        public void DelayHide()
        {
            DeliverFail.gameObject.SetActive(false);
            DeliverSuccess.gameObject.SetActive(false);
        }
    }
}

