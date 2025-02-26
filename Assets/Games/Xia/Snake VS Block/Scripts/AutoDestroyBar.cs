using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeVSBlock
{
    public class AutoDestroyBar : MonoBehaviour
    {

        [Header("Snake Movement")] SnakeMovement SM;

        // Use this for initialization
        void Start()
        {
            //Set the Snake Movement
            SM = GameObject.FindGameObjectWithTag("SnakeManager").GetComponent<SnakeMovement>();
            var ColorID = Random.Range(1, 5);
            switch (ColorID)
            {
                case 1:
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case 3:
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case 4:
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (SM.transform.childCount > 0 && transform.position.y - SM.transform.GetChild(0).position.y < -20)
                Destroy(this.gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.tag == "Bar")
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                other.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        
    }
}
