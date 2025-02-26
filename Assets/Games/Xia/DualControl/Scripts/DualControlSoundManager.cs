using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualControlSoundManager : MonoBehaviour
{

    public AudioSource SoundM;
    public AudioClip Background, GameOver, ButtonClick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOverSound() {
        SoundM.PlayOneShot(GameOver);
    }


}
