using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//小地图摄像机跟随

    public class SimpleFollow : MonoBehaviour
    {
        public GameObject targetToFollow;
        void LateUpdate()
        {
            if (!targetToFollow)
                return;

            transform.position = new Vector3(targetToFollow.transform.position.x, targetToFollow.transform.position.y, transform.position.z);
        }
    }
