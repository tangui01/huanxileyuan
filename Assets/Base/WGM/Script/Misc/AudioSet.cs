using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace WGM
{
	public class AudioSet : MonoBehaviour
	{
		public enum AudioType
		{
			BGM,
			SoundEffect,
		}

		static AudioMixer mAudioMixer;
		public static AudioMixer audioMixer
		{
			get {
				if(mAudioMixer == null) {

                    AudioSource[] sources = FindObjectsOfType<AudioSource>();
					foreach(AudioSource source in sources) {
						if(source.outputAudioMixerGroup != null) {
							mAudioMixer = source.outputAudioMixerGroup.audioMixer;
							break;
						}
					}
				}
				return mAudioMixer;
			}
			set {
				mAudioMixer = value;
			}
		}

		public static bool SetBGM(float value)
		{
            if (audioMixer != null)
            {
                audioMixer.SetFloat("BGM", value == 0 ? -80 : 50 * value - 50);
                return true;
            }
            return false;
		}

		public static bool SetSE(float value)
		{
            if (audioMixer != null)
            {
                audioMixer.SetFloat("SoundEffect", value == 0 ? -80 : 50 * value - 50);
                return true;
            }
            return false;
        }

		public static void Bind(AudioSource audioSource, AudioType audioType)
		{
			if(audioSource != null) {
				audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/VoiceAttenuation/" + audioType)[0];
			}
		}
    }
}