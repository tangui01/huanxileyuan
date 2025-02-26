using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombIgnoreCollision : MonoBehaviour
{
    private Collider2D _collider2D;
    bool isTakeDamage = false;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "RightPrevent" || other.gameObject.name == "LeftPrevent")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(),_collider2D);
        }

        if (other.gameObject.tag == "Boat" && !isTakeDamage)
        {
            isTakeDamage = true;
            other.gameObject.GetComponent<ShootAndCollisionPlayer>().TakeDameInBomb(gameObject);
        }
    }
    
}
