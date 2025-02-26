using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WGM;

namespace SnakeVSBlock
{
    public class AutoDestroy : MonoBehaviour
    {

        [Header("Snake Manager")] SnakeMovement SM;

        [Header("Hits to be destroyed")] public int life = 0;
        public float lifeForColor;
        TextMesh thisTextMesh;

        GameObject[] ToDestroy;
        GameObject[] ToUnparent;

        [Header("Color Management")] int maxLifeForRed = 30;

        //To fix the initial position due to some issues
        Vector3 initialPos;
        public bool dontMove;

        private GameObject bars;
        
        private bool die = false;
        
        public List<Sprite> sprites = new List<Sprite>();
        private int Level = 0;
        void Start()
        {
            //Set the Snake Movement
            bars = GameObject.Find("Bars");
            SM = GameObject.FindGameObjectWithTag("SnakeManager").GetComponent<SnakeMovement>();
            //Initialize the amount of lives
            if (FindObjectOfType<SnakeVSBlockGameController>() .SCORE > 300)
                life = Random.Range(1,31);
            else
                life = Random.Range(3,FindObjectOfType<SnakeVSBlockGameController>() .SCORE / 25);
            if (transform.tag == "SimpleBox")
            {
                life = Random.Range(1,6);
            }
            
            if (life >= maxLifeForRed)
            {
                life = maxLifeForRed;
            }

            //Initialize this text Mesh
            thisTextMesh = GetComponentInChildren<TextMesh>();
            thisTextMesh.text = "" + life;

            //Initialize the 2 arrays
            ToDestroy = new GameObject[transform.childCount];
            ToUnparent = new GameObject[transform.childCount];

            //Randomly spawn some bars
            StartCoroutine(EnableSomeBars());

            //Set the color of the box depending on the life
            lifeForColor = life;
            SetBoxColor();

            //Values to fix the position of the block
            initialPos = transform.position;
            dontMove = false;
        }
        
        // Update is called once per frame
        void Update()
        {

            //Fix the position
            if (dontMove)
                transform.position = initialPos;

            if (SM.transform.childCount > 0 && transform.position.y - SM.transform.GetChild(0).position.y < -25)
                Destroy(this.gameObject);

            //SetBoxColor();

            lifeForColor = life;

            if (life <= 0 && !die)
            {
                die = true;
                
                // transform.GetComponent<BoxCollider2D>().enabled = false;
                //
                // transform.GetComponent<SpriteRenderer>().enabled = false;
                //
                // transform.GetComponentInChildren<MeshRenderer>().enabled = false;
                FindObjectOfType<VFXPool>().Play(transform.position);
                FindObjectOfType<SnakeVSBlockGameController>().SCORE += 10;
                Destroy(gameObject);
            }


        }
        
        public void UpdateText()
        {
            thisTextMesh.text = "" + life;
        }

        public void playAnimation()
        {
            GetComponent<Animator>().SetTrigger("Hit");
        }
        IEnumerator EnableSomeBars()
        {
            int i = 0;

            //Add the GameObjects to the arrays
            while (i < transform.childCount)
            {
                if (transform.GetChild(i).tag.Equals("Bar"))
                {
                    int r = Random.Range(0, 5);
                    if (r == 1)
                    {
                        ToUnparent[i] = transform.GetChild(i).gameObject;
                    }
                    else
                        ToDestroy[i] = transform.GetChild(i).gameObject;

                    i++;
                    yield return new WaitForSeconds(0.01f);
                }
                else
                    i++;
            }

            for (int j = 0; j < transform.childCount; j++)
            {
                if(ToUnparent[j] !=null)
                    ToUnparent[j].transform.parent = bars.transform;
                if (ToDestroy[j] != null)
                {
                    Destroy(ToDestroy[j].gameObject);
                }
            }
            yield return null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.tag == transform.tag)
            {
                Destroy(collision.gameObject);
            }
            else if (collision.transform.tag == "SimpleBox")
            {
                Destroy(collision.gameObject);
            }
            else if (transform.tag == "SimpleBox" && collision.transform.tag == "Box")
            {
                Destroy(gameObject);
            }


        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.tag == transform.tag)
            {
                Destroy(collision.gameObject);
            }
            else if (collision.transform.tag == "SimpleBox")
            {
                Destroy(collision.gameObject);
            }
            else if (transform.tag == "SimpleBox" && collision.transform.tag == "Box")
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.tag == transform.tag)
            {
                Destroy(collision.gameObject);
            }
            else if (collision.transform.tag == "SimpleBox")
            {
                Destroy(collision.gameObject);
            }
            else if (transform.tag == "SimpleBox" && collision.transform.tag == "Box")
            {
                Destroy(gameObject);
            }
        }

        public void SetBoxColor()
        {
            for (int i = sprites.Count; i > 0; i--)
            {
                var count = i * (i + 1) / 2;
                if (lifeForColor >= count)
                {
                    GetComponent<SpriteRenderer>().sprite = sprites[i-1];
                    return;
                }
            }

        }
    }
}
