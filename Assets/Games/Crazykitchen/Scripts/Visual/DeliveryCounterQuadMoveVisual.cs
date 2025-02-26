using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：DeliveryCounterQuadMoveVisual.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class DeliveryCounterQuadMoveVisual : MonoBehaviour
    {
        [SerializeField]private float MoveVisualSpeed = 1.5f;
        private MeshRenderer rend;
        private float preces;
        private void Awake()
        {
            rend = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (preces<-1)
            {
                preces = 1;
            }
            else
            {
                preces -= Time.deltaTime;
            }
            rend.material.SetTextureOffset("_MainTex", new Vector2(preces*MoveVisualSpeed, 0));
        }
    }
   
}
