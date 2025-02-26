using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class Bullect : MonoBehaviour {

    public float moveSpeed = 10;
    public bool isPlayerBullect=true;
    public List<Sprite> bullectSprites = new List<Sprite>();
    public AudioClip bullectSound;
    public List<Sprite> bombSprites = new List<Sprite>();
    public bool isBomb = false;
    public float bombSpeed = 0.1f;
    private SpriteRenderer sr;
    private int currentBullet=0;
    private bool BombScale = false;
    public AudioClip bombAudioClip;
    MapCreater mapCreater;
    private void Awake()
    {
        if(gameObject.tag == "PlayerBullect")
            AudioManager.Instance.SetEff(bullectSound,1,Random.Range(0,2));
        mapCreater = FindObjectOfType<MapCreater>();
    }
    // Use this for initialization
	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        // if (TankPlayerManager.tankLevel1 >= 2 && gameObject.tag == "PlayerBullect")
        // {
        //     gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        //     if (TankPlayerManager.tankLevel1 == 3)
        //         gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        // }

        if (TankPlayerManager.Instance.tankLevel2 >= 2 && gameObject.tag == "Player2bullet")
        {
            gameObject.transform.localScale = new Vector3(4, 4, 4);
            if (TankPlayerManager.Instance.tankLevel2 == 3)
                gameObject.transform.localScale = new Vector3(5, 5, 5);

        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.up * Time.deltaTime*moveSpeed, Space.World);
        
    }

    private void FixedUpdate()
    {
        if (bullectSprites.Count > 0)
        {
            sr.sprite = bullectSprites[currentBullet % bullectSprites.Count];
            currentBullet++;
        }
    }
    IEnumerator PlayBombSprites()
    {
        if (isBomb)
        {
            if (BombScale)
                yield break;
            BombScale = true;
            moveSpeed = 0;
            AudioManager.Instance.SetEff(bombAudioClip,1,2);
            for (int i = 0; i < bombSprites.Count; i++)
            {
                sr.sprite = bombSprites[i];
                GetComponent<BoxCollider2D>().size = new Vector2(sr.sprite.texture.width/166,sr.sprite.texture.width/166);
                GetComponent<BoxCollider2D>().offset = new Vector2(0,0.05f);
                yield return new WaitForSeconds(bombSpeed);
            }
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Tank":
                
                if (!isPlayerBullect)
                {
                    if (collision.GetComponent<TankPlayer>().isDefended)
                    {
                        return;
                    }

                    StartCoroutine(PlayBombSprites());
                    TankPlayerManager.tankLevel1 -= 2;
                    if (TankPlayerManager.tankLevel1 < 0)
                    {
                        collision.SendMessage("Die");
                        TankPlayerManager.tankLevel1 = 0;
                    }
                    
                }
                break;
            case "Tank2":

                if (!isPlayerBullect)
                {
                    if(collision.GetComponent<TankPlayer>().isDefended)
                    {
                        return;
                    }
                    StartCoroutine(PlayBombSprites());
                    TankPlayerManager.Instance.tankLevel2 -= 2;
                    if (TankPlayerManager.Instance.tankLevel2 < 0)
                    {
                        collision.SendMessage("Die");
                        TankPlayerManager.Instance.tankLevel2 = 0;
                    }

                }
                break;
            case "Heart":
                StartCoroutine(PlayBombSprites());
                collision.SendMessage("Die");
        
                break;
            case "Enemy":
                if(isPlayerBullect)
                {
                    collision.SendMessage("Die");
                    StartCoroutine(PlayBombSprites());
                }
                break;
            case "Wall":

                for (int i = 0; i < mapCreater.itemList.Count; i++)
                {
                    if (mapCreater.itemList[i].transform.position == collision.gameObject.transform.position)
                    {
                        mapCreater.itemList.RemoveAt(i);
                        break;
                    }
                }
                if(isPlayerBullect && (gameObject.tag == "PlayerBullect"|| gameObject.tag == "Player2bullet"))
                {
                    Destroy(collision.gameObject);
                }
                else if(gameObject.tag == "EnemyBullect")
                    Destroy(collision.gameObject);
                StartCoroutine(PlayBombSprites());
                break;
            case "Barriar":
                
                if (isPlayerBullect)
                {
                    if(((TankPlayerManager.tankLevel1 >= 3 && gameObject.tag == "PlayerBullect") ||
                    (TankPlayerManager.Instance.tankLevel2 >= 3 && gameObject.tag == "Player2bullet")))
                    {
                        for (int i = 0; i < mapCreater.itemList.Count; i++)
                        {
                            if (mapCreater.itemList[i].transform.position == collision.gameObject.transform.position)
                            {
                                mapCreater.itemList.RemoveAt(i);
                                break;
                            }
                        }
                        Destroy(collision.gameObject);
                        StartCoroutine(PlayBombSprites());
                    }
                    else
                    {
                        StartCoroutine(PlayBombSprites());
                    }
                    if(!BombScale)
                        mapCreater.PlayAudio();
                    
                }
                else
                {
                    StartCoroutine(PlayBombSprites());
                }
                
                break;
            case "AirBarria":
                StartCoroutine(PlayBombSprites());
                break;
            case "EnemyBullect":
                if(isPlayerBullect)
                {
                    Destroy(collision.gameObject);
                    StartCoroutine(PlayBombSprites());
                }
                break;
            case "PlayerBullect":
                if (!isPlayerBullect)
                {
                    Destroy(collision.gameObject);
                    StartCoroutine(PlayBombSprites());
                }
                break;
        }



    }
    
}
