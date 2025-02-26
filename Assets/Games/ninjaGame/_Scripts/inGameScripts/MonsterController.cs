using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/****************************************************
    文件：onster.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace ninjaGame
{
	public class MonsterController : MonoBehaviour
	{
		public Transform thisTrans;
		public static Vector3 thisPosition; //for zombie position
		public Animator MonsterAnim; //for zombie Animation
		private BoxCollider box; //for zombie 
		private Rigidbody rb;
		public GameObject HitEf;
		
		
		
		
		//for zombie States
		public enum MonsterStates
		{
			Alive,
			Dead,
			Null
		}

		public MonsterStates CurrentState;

		void Start()
		{
			thisTrans = transform;
			//thisPosition = thisTrans.localPosition;
			box = GetComponent<BoxCollider>();
			rb = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update()
		{

			switch (CurrentState)
			{
				//for zombie Alive state
				case MonsterStates.Alive:
					//MonsterAnim.SetTrigger("Idle");
					break;
				//for zombie dead state
				case MonsterStates.Dead:
					
					MonsterAnim.SetTrigger("Die");
					break;
				case MonsterStates.Null:

					break;
			}
		}

		//to handle the Obsticles collision information

		private void OnTriggerEnter(Collider col)
		{
			//for Obsticles hit Sword
			if (col.gameObject.CompareTag("sword")&&CurrentState!=MonsterStates.Dead)
			{
				DeathState();
			}
		}

		public void DeathState()
		{
			CurrentState = MonsterStates.Dead; //For zombie dead ;
			box.size = new Vector3(0, 0, 0); //for Zombie collider size
			rb.AddForce((-transform.forward*8 +transform.up*5), ForceMode.Impulse);
			rb.useGravity = true;
			CameraController.instance.CameraShakeByKill();
			GameObject ef= Instantiate(HitEf, transform.position, Quaternion.identity);
			Destroy(ef, 0.5f);
			Invoke("DelayDestroy",0.5f);
			Ace_IngameUiControl.Static.inGameScoreCount++; //for Counting score
			SoundController.Static.AttackSound();
		}

		private void DelayDestroy()
		{
			SoundController.Static.playSoundFromName("Dead2"); //for Zombie dead sound
			
			Destroy(gameObject,0.5f); //for destroy Zombie
		}
	}
}