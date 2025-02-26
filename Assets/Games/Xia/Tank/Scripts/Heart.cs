using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {


	[HideInInspector]
    public SpriteRenderer sr;
    public Sprite BrokenSprite;
    public GameObject explosionPrefab;

    public AudioClip dieAudio;

    GameObject explosion;
	[HideInInspector]
	public bool GameOver = false;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Die()
    {
	    GameOver = true;
	    TankPlayerManager.Instance.isDefeat = true;
        sr.sprite = BrokenSprite;
        explosion = Instantiate(explosionPrefab,transform.position,transform.rotation);
        Destroy(explosion,0.5f);
        AudioSource.PlayClipAtPoint(dieAudio, transform.position);
    }




}
