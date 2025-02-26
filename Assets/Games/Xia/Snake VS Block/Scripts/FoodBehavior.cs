using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeVSBlock
{
    public class FoodBehavior : MonoBehaviour
    {

        [Header("Snake Manager")] SnakeMovement SM;

        [Header("Food Amount")] public int foodAmount;
        public Sprite foodSprite;

        // Use this for initialization
        void Start()
        {
            SM = GameObject.FindGameObjectWithTag("SnakeManager").GetComponent<SnakeMovement>();

            if (Random.Range(0,3) == 0)
            {
                foodAmount = 3;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = foodSprite;
            } 
            else
                foodAmount = 1;
            // GetComponent<TextMesh>().text = "" + foodAmount;
        }

        // Update is called once per frame
        void Update()
        {
            if (SM.transform.childCount > 0 && transform.position.y - SM.transform.GetChild(0).position.y < -10)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject == SM.transform.GetChild(0))
                Destroy(gameObject);
        }
    }
}