using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class MagnetCollisionController : MonoBehaviour
    {
        public SnakePlayerController rootParent;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Food"))
            {
                rootParent.GetComponent<SnakePlayerController>().EatFood(other,FoodTypes.NormalFood);
            }
            else if (other.gameObject.CompareTag("GhostFood"))
            {
                rootParent.GetComponent<SnakePlayerController>().EatFood(other,FoodTypes.GhostFood);
            }
        }
    } 
}

