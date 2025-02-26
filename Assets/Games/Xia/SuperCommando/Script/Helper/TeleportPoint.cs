using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour {
	public Teleport Teleport;

	void OnTriggerEnter2D(Collider2D other){
		if (!SuperCommandoGameManager.Instance.Player.isPlaying)
			return;
		
		if (other.GetComponent<SuperCommandoPlayer> ()) {
			Teleport.TeleportPlayer (transform.position);
			return;
		}
	}
}
