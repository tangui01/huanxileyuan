using UnityEngine;
using System.Collections;
using System ;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ninjaGame
{
	public class SoundController : MonoBehaviour
	{

		public AudioClip  DeadSound;
		public static SoundController Static;
		public AudioClip[] EFClips; //for audio clips
		
		public string[] ClipsName; //for audio clip names//for audio sources
      
		
		public AudioClip[] BGClips;
		public AudioClip AttackEf;
		void Start()
		{
			Static = this;
			AudioManager.Instance.playerBGm(BGClips[Random.Range(0, BGClips.Length)]);
		}

		//for sound playing acording to clip name
		public void playSoundFromName(string clipName)
		{
			AudioManager.Instance.playerEffect1(EFClips[Array.IndexOf(ClipsName, clipName)]); //for audio clips
		}

		//for player dead sound
		public void PlayDeadSound()
		{
			AudioManager.Instance.playerEffect1(DeadSound);
		}

		public void AttackSound()
		{
			AudioManager.Instance.playerEffect2(AttackEf);
		}
	}

}
