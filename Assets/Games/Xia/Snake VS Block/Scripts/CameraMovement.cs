using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SnakeVSBlock
{
    public class CameraMovement : MonoBehaviour
    {

        [Header("Snake Container")] public Transform SnakeContainer;

        Vector3 initialCameraPos;
        CinemachineVirtualCamera vcam;
        // Use this for initialization
        private void Awake()
        {
            vcam = GetComponent<CinemachineVirtualCamera>();
        }

        void Start()
        {
            initialCameraPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            
            if (SnakeContainer.childCount > 0)
                vcam.Follow = SnakeContainer.GetChild(0);
        }
        
        private void FixedUpdate()
        {
            // if (SnakeContainer.childCount > 0)
            //     transform.position = Vector3.Slerp(transform.position,
            //         (initialCameraPos + new Vector3(0,
            //             SnakeContainer.GetChild(0).position.y - Camera.main.orthographicSize / 8, 0)),
            //         0.25f);
        }
        
    }
}