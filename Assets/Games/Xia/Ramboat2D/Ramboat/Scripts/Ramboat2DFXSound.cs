using UnityEngine;
using System.Collections;

public class Ramboat2DFXSound : MonoBehaviour {
	public static Ramboat2DFXSound THIS;
	
	[HideInInspector]
	public AudioSource fxSound
	{ 
		get { return AudioManager.Instance.GetEffect(); }
	}
	public AudioClip none;
	public AudioClip[] openCloseDoor;
	public AudioClip[] pistol;
	public AudioClip boatDive, boatJump, boatSplash,boatNavigation;
	public AudioClip[] coinPickup;
	public AudioClip[] enemyGun;
	public AudioClip helicopterLoop,cloudLoop;
	public AudioClip levelUp,dolarCollect,pokerChip,bonus,bling,boxExpolison;
	public AudioClip[] 	missileWhistle,pistolHitFlesh,pistolHitMetal,pistolHitWood,waterCoinDrop;
	public AudioClip[] heavyMachineGun,rocket,shotGun,flameShoot;
	public AudioClip[] setupGun;
	public AudioClip shotHit,enemyGunHit,submarinIn,submarineOut;
	public AudioClip[] rocketLauchHit,flameShotHit,waterSplashBig;
	public AudioClip[] expolision,playerTakeDame,flipCard;
	public AudioClip buttonOpen,buttonClose,menu,buttonBuy;
	//enemy sound
	public AudioClip boatShot,airFlyShot,tictac,scrollClick,fail;
	public AudioClip[] bulletHitWater;
	
	public AudioClip[] music;
	
	void Awake(){
		THIS = this;
	}
}
