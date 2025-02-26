using UnityEngine;

namespace SaucerFlying
{
    public class TargetController : MonoBehaviour
    {
        private int countItemSpeedUpTo;
        private int speed;
        private float timeLast;
        public int speedDefault;

        // Use this for initialization
        void Start()
        {
            timeLast = 0;
            countItemSpeedUpTo = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (SaucerFlyingGameManager.Instance.GameState == GameState.Playing)
            {
                if (countItemSpeedUpTo >= 1)
                {
                    speed = 10;
                    if (Time.time - timeLast > 1)
                    {
                        --countItemSpeedUpTo;
                        timeLast = Time.time;
                    }
                }
                else
                {
                    speed = speedDefault;
                }

                transform.position += transform.forward * Time.deltaTime * speed;

                Vector3 temp = transform.position;

                float inputs = SaucerFlyingInputManager.Instance.touchX;

                if (inputs != 0)
                    temp += new Vector3(1, 0, 0) * inputs * Time.deltaTime * speed;
                else
                    temp += Vector3.zero;

                transform.position = temp;
            }
        }
    }

}