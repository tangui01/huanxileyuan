using UnityEngine;

namespace SaucerFlying
{
    public class SaucerFlyingRotatePlayer : MonoBehaviour
    {

        public float forwardSpeed = 5.0f;
        public float reverseSpeed = 2.0f;
        public float turnRate = 80.0f;

        // Update is called once per frame
        void Update()
        {
            if (SaucerFlyingGameManager.Instance.GameState == GameState.Playing)
                RotateTouch_();
        }
       
        void RotateTouch_()
        {
            float smooth = 5.0f;
            float tiltAngle = 60.0f;
            float tiltAroundZ = -SaucerFlyingInputManager.Instance.touchX * tiltAngle;
            

            // move up/ dow
            float tiltAroundX = -SaucerFlyingInputManager.Instance.touchY * tiltAngle / 1.5f;

            Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth * 2);
        }
    }
}


