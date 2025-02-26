using UnityEngine;

namespace SaucerFlying
{
    public class SwipeDirManager : MonoBehaviour
    {
        static public SwipeDirManager Instance;
        private Vector2 startPos;
        public Vector2 directionVec;
        public Direction direction;
        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }
        private void Update()
        {
            //Dir();
        }
        void Dir()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        startPos = touch.position;
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        directionVec = touch.position - startPos;
                        directionVec = Camera.main.ScreenToViewportPoint(directionVec);
                        Debug.Log(directionVec.x);
                        if (directionVec.x > 0)
                            direction = Direction.Right;
                        if (directionVec.x < 0)
                            direction = Direction.Left;
                        if (directionVec.y > 0)
                            direction = Direction.Up;
                        if (directionVec.y < 0)
                            direction = Direction.Down;
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        direction = Direction.None;
                        directionVec = Vector2.zero;
                        break;
                }
            }
        }
    }

}
