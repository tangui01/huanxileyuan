using UnityEngine;
using System.Collections;

public class CollisionWater : MonoBehaviour {
	
	// Use his for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Water" && name=="FlyCheck") {
			Ramboat2DPlayerController.Intance.canFly = false;
			Ramboat2DPlayerController.Intance.isShoot = false;
		}
		if (other.tag == "Water" && name == "SwimCheck") {
			
			Ramboat2DPlayerController.Intance.canSwim = true;
			if (Ramboat2DPlayerController.Intance.waterEffectJump.activeInHierarchy)
				Ramboat2DPlayerController.Intance.waterEffectJump.SetActive (false);
			if (Ramboat2DPlayerController.Intance.waterEffectJump2.activeInHierarchy)
				Ramboat2DPlayerController.Intance.waterEffectJump2.SetActive (false);
			Ramboat2DPlayerController.Intance.waterEffectJump2.SetActive (true);
			Ramboat2DPlayerController.Intance.player.velocity = Vector2.zero;
		}

	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Water" && name=="FlyCheck") {
			Ramboat2DFXSound.THIS.fxSound.Stop ();
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.boatSplash);
			Ramboat2DPlayerController.Intance.canFly = true;
			Ramboat2DPlayerController.Intance.allowShoot = true;
			Ramboat2DPlayerController.Intance.isShoot = true;
			Ramboat2DPlayerController.Intance.flashShoot.gameObject.SetActive (true);
			Ramboat2DPlayerController.Intance.player.velocity = new Vector3(1.5f,Ramboat2DPlayerController.Intance.player.velocity.y,0);
		}
		if (other.tag == "Water" && name == "SwimCheck") {
			Ramboat2DPlayerController.Intance.canSwim = false;
			Ramboat2DPlayerController.Intance.effectAfterSwim = true;
			if (Ramboat2DPlayerController.Intance.waterEffect.activeInHierarchy)
				Ramboat2DPlayerController.Intance.waterEffect.SetActive (false);
			if (!Ramboat2DPlayerController.Intance.waterEffectJump.activeInHierarchy)
				Ramboat2DPlayerController.Intance.waterEffectJump.SetActive (true);
			if (!Ramboat2DPlayerController.Intance.waterEffectJump2.activeInHierarchy)
				Ramboat2DPlayerController.Intance.waterEffectJump2.SetActive (true);
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Water" && name=="SwimCheck") {
			if (!Ramboat2DPlayerController.Intance.playerDead && !Ramboat2DFXSound.THIS.fxSound.isPlaying) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.boatNavigation);
			}
			if (Ramboat2DPlayerController.Intance.player.velocity.x > -0.5f && !Ramboat2DPlayerController.Intance.playerDead) {
				if (!Ramboat2DPlayerController.Intance.waterEffect.activeInHierarchy)
					Ramboat2DPlayerController.Intance.waterEffect.SetActive (true);
			} else {
				Ramboat2DPlayerController.Intance.waterEffect.SetActive (false);
			}
		}

	}
}
