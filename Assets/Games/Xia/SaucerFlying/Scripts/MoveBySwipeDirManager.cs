using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaucerFlying
{

    public class MoveBySwipeDirManager : MonoBehaviour
    {

        Direction direction;
        private void Update()
        {
            Move(SaucerFlyingPlayerController.Instance.speed);
        }
        public void Move(float speed)
        {
            
            transform.position += transform.forward * speed * Time.deltaTime;
            direction = SwipeDirManager.Instance.direction;
            Vector3 tempPos = transform.position;
            Vector3 tempDirVec = Vector3.zero;
            tempDirVec.x = SwipeDirManager.Instance.directionVec.x;
            tempDirVec.y = SwipeDirManager.Instance.directionVec.y;
            tempDirVec.z = transform.position.z;

            if(direction != Direction.None)
            {
                if(direction == Direction.Left)
                {
                    tempPos.x += tempDirVec.x * speed * Time.deltaTime;
                }
                if(direction == Direction.Right)
                {
                    tempPos.x -= tempDirVec.x * speed * Time.deltaTime;
                }
                if (direction == Direction.Up)
                {
                    tempPos.y += tempDirVec.y * speed * Time.deltaTime;
                }
                if (direction == Direction.Down)
                {
                    tempPos.y -= tempDirVec.y * speed * Time.deltaTime;
                }

                transform.position = tempPos;
            }
        }
    }
}
