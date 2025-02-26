using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesPlay : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public float speed=0.1f;
    public Transform SnakeHead;
    private float playSpritesTimer = 0;
    private int spriteIndex = 0;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        transform.position = SnakeHead.GetChild(0).position;
        playSpritesTimer += Time.deltaTime;
        if (playSpritesTimer >= speed)
        {
            playSpritesTimer = 0;
            spriteRenderer.sprite = sprites[spriteIndex % sprites.Count];
            spriteIndex++;
        }

    }
}
