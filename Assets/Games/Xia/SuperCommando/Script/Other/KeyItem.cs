﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour {
	public AudioClip sound;
	public GameObject effect;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject == SuperCommandoGameManager.Instance.Player.gameObject) {
			if (effect)
				Instantiate (effect, transform.position, Quaternion.identity);
            SuperCommandoSoundManager.Instance.PlaySfx(sound);
			SuperCommandoGameManager.Instance.isHasKey = true;
            //gameObject.SetActive (false);
            Destroy(gameObject);
        }
	}
}
