using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WGM;

/*
 * This is SoundManager
 * In other script, you just need to call SoundManager.PlaySfx(AudioClip) to play the sound
*/
public class SuperCommandoSoundManager : MonoBehaviour {
    public static SuperCommandoSoundManager Instance;
    //[Header("FORCE PLAY MUSIC")]
    //public AudioClip forcePlayMusic;

    [Header("MAIN MENU")]
    public AudioClip beginSoundInMainMenu;
    [Tooltip("Play music clip when start")]
    public AudioClip musicsMenu;

    [Header("GAMEPLAY")]
    public AudioClip musicsGame;
    [Range(0, 1)]
    public float musicsGameVolume = 1f;

    public AudioClip musicFinishPanel;
    public AudioClip swapGun;
    [Header("Shop")]
    public AudioClip soundPurchased;
    public AudioClip soundUpgrade;
    public AudioClip soundNotEnoughCoin;

    [Tooltip("Place the sound in this to call it in another script by: SoundManager.PlaySfx(soundname);")]
    public AudioClip soundClick;
    public AudioClip soundGamefinish;
    public AudioClip soundGameover;

    private AudioSource musicAudio;
    private AudioSource soundFx;

    private float machineVolume
    {
	    get
	    {
		    return LibWGM.machine.SeVolume / 10;
	    }
    }
    //GET and SET
    public float MusicVolume {
        set { Instance.musicAudio.volume = value * machineVolume; }
        get { return Instance.musicAudio.volume ; }
    }
    public  float SoundVolume {
        set { Instance.soundFx.volume = value * machineVolume; }
        get { return Instance.soundFx.volume ; }
    }

    public  void ResetMusic()
    {
        musicAudio.Stop();
        musicAudio.volume = 1;
        musicAudio.Play();
    }

    public void PauseMusic(bool isPause)
    {
	    if (isPause)
            Instance.musicAudio.mute = true;
	    else
	    {
		    musicAudio.volume = 1;
		    Instance.musicAudio.mute = false;
	    }
            
    }
	
    public  void Click() {
        PlaySfx(Instance.soundClick, 1);
    }

    void Awake() {
        Instance = this;
        musicAudio = gameObject.AddComponent<AudioSource>();
        musicAudio.loop = true;
	    MusicVolume =1f;
        soundFx = gameObject.AddComponent<AudioSource>();
        SoundVolume =1f;
    }

    public  void PlayGameMusic()
    {
        PlayMusic(Instance.musicsGame, Instance.musicsGameVolume);
    }

	public  void PlaySfx(AudioClip clip){
		Instance.PlaySound(clip, Instance.soundFx);
	}

    public  void PlaySfx(AudioClip[] clips)
    {
        if (Instance != null && clips.Length > 0)
            Instance.PlaySound(clips[Random.Range(0, clips.Length)], Instance.soundFx);
    }

    public  void PlaySfx(AudioClip[] clips, float volume)
    {
        if (Instance != null && clips.Length > 0)
            Instance.PlaySound(clips[Random.Range(0, clips.Length)], Instance.soundFx, volume);
    }

    public  void PlaySfx(AudioClip clip, float volume){
		Instance.PlaySound(clip, Instance.soundFx, volume);
	}

	public  void PlayMusic(AudioClip clip, bool loop = true){
        if (Instance != null)
            return;

        Instance.musicAudio.loop = loop;
        Instance.PlaySound (clip, Instance.musicAudio);
	}

	public  void PlayMusic(AudioClip clip, float volume){
		Instance.PlaySound (clip, Instance.musicAudio, volume);
	}

	private void PlaySound(AudioClip clip,AudioSource audioOut){
		if (clip == null) {
			return;
		}

		if (audioOut == musicAudio) {
			audioOut.clip = clip;
			audioOut.Play ();
		} else
			audioOut.PlayOneShot (clip, SoundVolume);
	}

	private void PlaySound(AudioClip clip,AudioSource audioOut, float volume){
		if (clip == null) {
			return;
		}

		if (audioOut == musicAudio) {
			audioOut.clip = clip;
			audioOut.Play ();
		} else
			audioOut.PlayOneShot (clip, SoundVolume * volume );
	}


}
