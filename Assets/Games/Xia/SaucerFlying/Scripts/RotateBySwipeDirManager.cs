using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaucerFlying
{

    public class RotateBySwipeDirManager : MonoBehaviour
    {
        static public RotateBySwipeDirManager Instance;
        private void Awake()
        {
            if (!Instance) Instance = this;
        }
        public void Rotate()
        {
            float smooth = 5.0f;
            float tiltAngle = 60.0f;
            float tiltAroundZ = -SwipeDirManager.Instance.directionVec.x * tiltAngle;
            float tiltAroundX = -SwipeDirManager.Instance.directionVec.y * tiltAngle / 2;

            Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }
    }

}