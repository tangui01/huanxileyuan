using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：LoolAtCamera.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：让物体看向摄像机
*****************************************************/

namespace  Crzaykitchen
{
    public class LookAtCamera : MonoBehaviour
    {
        private enum LookAtType
        {
           LookAt,
           LookAtInverted,
           CameraForward,
           CameraForwardInverted
        }
        [SerializeField]private LookAtType lookAtType;
        private Camera MainCamera;

        private void Awake()
        {
            MainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            switch (lookAtType)
            {
                case LookAtType.LookAt:
                    transform.LookAt(MainCamera.transform);
                    break;
                case LookAtType.LookAtInverted:
                    Vector3 dirFormCamera=transform.position-MainCamera.transform.position;
                    transform.LookAt(transform.position+dirFormCamera);
                    break;
                case LookAtType.CameraForward:
                    transform.forward = MainCamera.transform.forward;
                    break;
                case LookAtType.CameraForwardInverted:
                    transform.forward = -MainCamera.transform.forward;
                    break;
            }
        }
    }
}

