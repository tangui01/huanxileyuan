using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeVSBlock
{
    public class HitBoxBehavior : MonoBehaviour
    {

        SnakeMovement SM;

        public List<Sprite> sprites = new List<Sprite>();

        private SpriteRenderer _spriteRenderer;
        // Use this for initialization
        void Start()
        {
            SM = transform.GetComponentInParent<SnakeMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            if (transform == SM.BodyParts[0] && _spriteRenderer.sprite != sprites[0])
            {
                _spriteRenderer.sprite = sprites[0];
                _spriteRenderer.sortingOrder = 1;
            }
            else if(transform != SM.BodyParts[0] && _spriteRenderer.sprite != sprites[1])
                _spriteRenderer.sprite = sprites[1];
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f) * (70+SM.BodyParts.Count * 2);
            if (transform == SM.BodyParts[0])
                transform.GetChild(0).localPosition = new Vector3(0, 0.666f + SM.BodyParts.Count * 0.0015f, 0);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Also need to check if this is the first snake part
            if (collision.transform.tag == "Box" && transform == SM.BodyParts[0])
            {
                
                var autoDestroy = collision.transform.GetComponent<AutoDestroy>(); 
                if(autoDestroy.life <= 0)
                    return;
                autoDestroy.life -= 1;
                SM.SnakeParticle.transform.position = collision.contacts[0].point;
                SM.SnakeParticle.Play();
                autoDestroy.UpdateText();
                autoDestroy.SetBoxColor();
                autoDestroy.playAnimation();
                //Reset the parent of the text Mesh
                if (SM.BodyParts.Count > 1 && SM.BodyParts[1] != null)
                {
                    SM.PartsAmountTextMesh.transform.parent = SM.BodyParts[1];
                    SM.PartsAmountTextMesh.transform.position = SM.BodyParts[1].position +
                                                                new Vector3(0, 0.5f, 0);
                }
                else if (SM.BodyParts.Count == 1)
                {
                    SM.PartsAmountTextMesh.transform.parent = null;
                }
                

                //Destroy the Part of the snake that hit the box
                Destroy(this.gameObject);
                
                SM.BodyParts.Remove(SM.BodyParts[0]);


            }


            else if (collision.transform.tag == "SimpleBox" && transform == SM.BodyParts[0])
            {
                var autoDestroy = collision.transform.GetComponent<AutoDestroy>(); 
                if(autoDestroy.life <= 0)
                    return;
                
                autoDestroy.life -= 1;
                SM.SnakeParticle.transform.position = collision.contacts[0].point;
                SM.SnakeParticle.Play();
                autoDestroy.UpdateText();
                autoDestroy.SetBoxColor();
                autoDestroy.playAnimation();

                // Reset the parent of the text Mesh
                if (SM.BodyParts.Count > 1 && SM.BodyParts[1] != null)
                {
                    SM.PartsAmountTextMesh.transform.parent = SM.BodyParts[1];
                    SM.PartsAmountTextMesh.transform.position = SM.BodyParts[1].position +
                                                                new Vector3(0, 0.5f, 0);
                }
                else if (SM.BodyParts.Count == 1)
                {
                    SM.PartsAmountTextMesh.transform.parent = null;
                }
                SM.BodyParts.Remove(SM.BodyParts[0]);
                //Destroy the Part of the snake that hit the box
                Destroy(this.gameObject,0.05f);
            }
            else if (collision.transform.tag == "Snake" && transform != SM.BodyParts[0])
            {
                Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            //If we collide with the food
            if (SM.BodyParts.Count > 0)
            {
                if (collision.transform.tag == "Food" && transform == SM.BodyParts[0])
                {
                    if(collision.transform.GetComponent<FoodBehavior>().foodAmount>1)
                        FindObjectOfType<NewBlockManage>().PlayAudio(1);
                    else
                        FindObjectOfType<NewBlockManage>().PlayAudio(3);
                    for (int i = 0; i < collision.transform.GetComponent<FoodBehavior>().foodAmount; i++)
                    {
                        SM.AddBodyPart();
                    }
                    FindObjectOfType<SnakeVSBlockGameController>().SCORE += 2;
                    //Destroy the food
                    Destroy(collision.transform.gameObject);
                }
            }

        }


    }
}