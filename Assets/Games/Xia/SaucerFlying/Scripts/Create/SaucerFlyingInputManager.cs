using UnityEngine;
using WGM;

namespace SaucerFlying
{
    public enum Direction
    {
        Left,
        Right,
        Down,
        Up,
        LeftTop,
        LeftDown,
        RightTop,
        RightDow,
        None
    }
    public class SaucerFlyingInputManager : MonoBehaviour
    {
        public static SaucerFlyingInputManager Instance;

        public Vector2 startPos;
        public Vector2 directionVec;

        public Direction direction;

        public float touchX;
        public float touchY;

        public float sensitivityX;
        public float sensitivityY;

        private bool isPressed;
        private Vector3 prePos;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            direction = Direction.None;
        }
        void Update()
        {
#if UNITY_EDITOR            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                // 检测键盘上的W、S键控制前后（Z轴方向）移动
                if (Input.GetKey(KeyCode.W))
                {
                    touchY = 1.0f;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    touchY = -1.0f;
                }

                // 检测键盘上的A、D键控制左右（X轴方向）移动
                if (Input.GetKey(KeyCode.A))
                {
                    touchX = -1.0f;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    touchX = 1.0f;
                }
            }
#else
        if (DealCommand.GetKey(1,(AppKeyCode)6) || DealCommand.GetKey(1,(AppKeyCode)1) || DealCommand.GetKey(1,(AppKeyCode)0) ||
                DealCommand.GetKey(1,(AppKeyCode)2))
            {
                // 检测键盘上的W、S键控制前后（Z轴方向）移动
                if (DealCommand.GetKey(1,(AppKeyCode)0))
                {
                    touchY = 1.0f;
                }
                else if (DealCommand.GetKey(1,(AppKeyCode)2))
                {
                    touchY = -1.0f;
                }

                // 检测键盘上的A、D键控制左右（X轴方向）移动
                if (DealCommand.GetKey(1,(AppKeyCode)6))
                {
                    touchX = -1.0f;
                }
                else if (DealCommand.GetKey(1,(AppKeyCode)1))
                {
                    touchX = 1.0f;
                }
            }
#endif
            else
            {
                touchY = 0.0f;
                touchX = 0.0f;
            }
            touchX = Mathf.Clamp(touchX, -1.0f, 1.0f);
            touchY = Mathf.Clamp(touchY, -1.0f, 1.0f);
        }
    }
}

