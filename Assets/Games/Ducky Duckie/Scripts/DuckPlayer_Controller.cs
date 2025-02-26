using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class DuckPlayer_Controller : MonoBehaviour {

    private Vector3 touchPosition;
    public float coefDiminish;
    [Header("Min & Max Pos")]
    public Transform MinPlatformPos;
    public Transform MaxPlatformPos;
    public float offset;
    [Header(" Shrinking Management ")]
    public float shrinkingCoef;
    float shrinkDelay = 5;
    float shrinkTime = 0;

    [Header("Particles")]
    public ParticleSystem HitParticles;
    public ScorePanel scoreorePanel;

    private int Score;
    
    
    [SerializeField]private AudioClip SqueakySound; 
    [SerializeField] private AudioClip BGMSound;
    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.playerBGm(BGMSound);
    }
    // Update is called once per frame
    void Update()
    {
        if (DuckyGameManager.gameOn)
        {
            // //Shrink
            Shrink();
            MovePlatform();
        }
        void Shrink()
        {
            if (shrinkTime < shrinkDelay)
            {
                shrinkTime += Time.deltaTime;
            }
            else
            {
                transform.localScale -= new Vector3(coefDiminish, 0, 0);
                shrinkTime = 0;
            }
        }

        void MovePlatform()
        {
            //A健
            if (DealCommand.GetKey(1, AppKeyCode.TicketOut))
            {
                transform.position += Vector3.left * (Time.deltaTime * 3);
                if (transform.position.x < MinPlatformPos.position.x)
                {
                    transform.position = MinPlatformPos.position;
                }
            }
            //J健
            else if (DealCommand.GetKey(1, AppKeyCode.Flight))
            {
                transform.position += Vector3.right * (Time.deltaTime * 3);
                if (transform.position.x > MaxPlatformPos.position.x)
                {
                    transform.position= MaxPlatformPos.position;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ball")
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 0));
            if (Boosters_Manager.instance.ISDouble())
            {
                Score += 2;
            }
            else
            {
                Score += 1;
            }
            PlayerPrefs.SetInt("score",Score);
            scoreorePanel.setScore(Score);
            //Play the Particles
            Vector2 pos = collision.contacts[0].point;
            HitParticles.transform.position = pos;
            HitParticles.Play();

            //Play the squeaky sound
            PlaySqueakySound();
        }
    }


    public void PlaySqueakySound()
    {
        AudioManager.Instance.playerEffect1(SqueakySound);
    }
}
