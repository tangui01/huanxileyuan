using System;
using UnityEngine;
using System.Collections;
using WGM;
using Random = UnityEngine.Random;

public class SubmarineParkourTorpedo : MonoBehaviour 
{
	public SubmarineParkourTorpedoMain parent;								//A link to the torpedo manager
	
	public GameObject indicator;							//The torpedo's indicator
	public GameObject torpedo;								//The torpedo object
	public ParticleSystem explosion;						//The torpedo's explosion
	
	bool canMove 					= false;				//Torpedo movement enabled/disabled
	bool paused 					= false;				//Game paused switch

    float originalSpeed             = 0;                    //The original speed, before the modification
	float speed						= 0;					//Speed of the torpedo
	Vector3 originalPos				= new Vector3();		//The orogonal position of the torpedo
	Vector3 originalExpPos			= new Vector3();		//The original position of the explosion
	
	bool explosionPlaying			= false;				//Explosion playing/not playing

	private Transform MoveGObj;

	private bool isAudio = true;
	private AudioSource audioSource;
	private float volume;
	//Called at the beginning of the game
	private void Awake()
	{
		MoveGObj = GameObject.Find("Move").transform;
		audioSource = GetComponent<AudioSource>();
		volume = audioSource.volume;
	}

	void Start()
	{
		//Records original position
		originalPos = torpedo.transform.position;
		originalExpPos = explosion.transform.position;
	}
	//Called at every frame
	void Update () 
	{
		//If the torpedo is enabled, and the game is not paused
		if (canMove && !paused)
		{
			torpedo.transform.position -= Vector3.right * speed * Time.deltaTime;
			if(isAudio)
				StartCoroutine(isPlayingSound());
		}

	}

	IEnumerator isPlayingSound()
	{
		Debug.Log("isPlayingSound");
		isAudio = false;
		audioSource.volume = volume * LibWGM.machine.SeVolume / 10;
		audioSource.PlayOneShot(audioSource.clip);
		yield return new WaitForSeconds(5f);
		isAudio = true;
	}
    //Enables/disables the object with childs based on platform
    void EnableDisable(GameObject what, bool childs)
    {
        #if UNITY_3_5
            what.SetActiveRecursively(childs);
        #else
            what.SetActive(childs);
        #endif
    }
	//Place torpedo indicator
	IEnumerator PlaceIndicator()
	{
		//Get a random y position
		int yPos = Random.Range(-23, 23);
		
		float indPos = 41 + MoveGObj.position.x;
			// SubmarineParkourResolutionManager.Instance.RightPosition();
			
		if (indPos == 0)
		{
			indPos = 41 + MoveGObj.position.x;
		}
			
		
		//Place the indicator
		indicator.transform.position = new Vector3 (indPos, yPos, -4.9f);

		//Activates the indicator
        EnableDisable(indicator, true);
		
		//Wait for 3 seconds
		double waited = 0;
		while (waited <= 3)
		{
			//If the game is not paused
			if (!paused)
				//Increase waited time
				waited += Time.deltaTime;
			
			//Wait for the end of frame
			yield return 0;
		}
		
		//Deactivate and reset indicator
        EnableDisable(indicator, false);
		indicator.transform.position = new Vector3 (41+GameObject.Find("Move").transform.position.x, 0, -4.9f);
		
		//Place torpedo at yPos
		Vector3 pos = torpedo.transform.position;
		pos.y = yPos;
		pos.x = 60 + GameObject.Find("Move").transform.position.x;
		torpedo.transform.position = pos;
		
        //Set torpedo speed
        this.speed = originalSpeed * SubmarineParkourLevelGenerator.Instance.SpeedMultiplier();

		//Activate torpedo
        EnableDisable(torpedo, true);
		
		//Enable movement
		canMove = true;
	}
	//Place explosion at a given location
	IEnumerator PlaceExplosion(float x, float y)
	{
		//Place the explosion at the given position, and play it
		explosion.transform.position = new Vector3(x - 6 +GameObject.Find("Move").transform.position.x, y, originalExpPos.z);
		explosion.Play ();
		
		//Set explosionPlaying variable and add the explosion to the level generator to scroll it
		explosionPlaying = true;
        SubmarineParkourLevelGenerator.Instance.AddExplosion(explosion.gameObject);
		
		//Wait for 2 seconds
		double waited = 0;
		while (waited <= 2)
		{
			//If the game is not paused
			if (!paused)
				//Increase waited time
				waited += Time.deltaTime;
			
			//Wait for the end of frame
			yield return 0;
		}
		
		//Remove the explosion from the level generator, and modify explosionPlaying variable
        SubmarineParkourLevelGenerator.Instance.RemoveExplosion(explosion.gameObject);
		explosionPlaying = false;
		
		//Reset explosion position
		explosion.transform.position = originalExpPos + new Vector3 (GameObject.Find("Move").transform.position.x, 0, 0);
	}
	//Launch a torpedo, Called from level generator
	public void Launch (float s) 
	{	
		//Set original speed, and place indicator
        originalSpeed = s;
		StartCoroutine(PlaceIndicator());
	}
	//Reset the torpedo
	public void ResetThis()
	{
		//Stop the coroutines
		StopAllCoroutines();
		
		//If the explosion is playing, reset it
		if (explosionPlaying)
            SubmarineParkourLevelGenerator.Instance.RemoveExplosion(explosion.gameObject);
		
		//Modify variables
		canMove = false;
		paused = false;
		
		//Reset torpedo
		torpedo.transform.position = originalPos + new Vector3 (GameObject.Find("Move").transform.position.x, 0, 0);
        EnableDisable(torpedo, false);
		
		//Deactivate and reset indicator
        EnableDisable(indicator, false);
		indicator.transform.position = new Vector3 (41 + GameObject.Find("Move").transform.position.x, 0, -4.9f);
		
		//Notify torpedo manager
		parent.ResetTorpedo(this);
	}
	//Called when the torpedo collides with the player, or with the sonic wave
	public void TargetHit(bool playExplosion)
	{
		//Modify variables
		canMove = false;
		paused = false;
		
		//Play explosion, if set
		if (playExplosion)
			StartCoroutine (PlaceExplosion(torpedo.transform.position.x, torpedo.transform.position.y));
		
		//Reset torpedo
		torpedo.transform.position = originalPos + new Vector3 (GameObject.Find("Move").transform.position.x, 0, 0);
        EnableDisable(torpedo, false);
		
		//Notify torpedo manager
		parent.ResetTorpedo(this);
	}
	//Pause the torpedo
	public void Pause()
	{
		paused = true;
	}
	//Resume the torpedo
	public void Resume()
	{
		paused = false;
	}
}
