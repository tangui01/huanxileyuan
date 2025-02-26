using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WGM;
using Random = UnityEngine.Random;

namespace SaucerFlying
{
    public class SaucerFlyingPlayerController : MonoBehaviour
    {
        public static event System.Action PlayerDied;
        static public SaucerFlyingPlayerController Instance;

        [HideInInspector]
        public float speed;

        public ParticleSystem particle;

        private int countItemSpeedUpTo;//  
        private float timeLast;
        private Rigidbody rigiBody;
        private Color color;
        private float sizeX;
        private float sizeY;
        private float sizeXRoad;
        private float sizeYRoad;
        private Direction direction;
        private float prePosZ;
        private float originalPosZ;
        private bool isRun = false; 
        AudioSource[] audioSources;
        float[] audioVolume;
        void OnEnable()
        {
            SaucerFlyingGameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            SaucerFlyingGameManager.GameStateChanged -= OnGameStateChanged;
        }
        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }
        void Start()
        {
            var sound = GameObject.Find("Sound");
            audioSources = new AudioSource[sound.transform.childCount];
            audioVolume = new float[sound.transform.childCount];
            for (int i = 0; i < sound.transform.childCount; i++)
            {
                audioSources[i] = sound.transform.GetChild(i).GetComponent<AudioSource>();
                audioVolume[i] = audioSources[i].volume;
            }
            
            VFXeffects = new GameObject[VFXprefabs.Count];
            ShieldVFXeffects = new GameObject[ShieldVFXprefabs.Count];
            ShieldImage = GameObject.Find("Image_Skill").GetComponent<Image>();
            ShieldImage.fillAmount = energy / ShieldConsume;
            
            // Setup
            timeLast = 0;
            speed = 0;
            SaucerFlyingCharacter character = transform.GetComponent<SaucerFlyingCharacter>();
            particle = GameObject.FindGameObjectWithTag("ParticleSystem").GetComponent<ParticleSystem>();
            var main = particle.main;
            main.startColor = new Color(character.charColor.r, character.charColor.g, character.charColor.b, 1);
            //color = gameObject.GetComponent<Renderer>().material.color;

            countItemSpeedUpTo = 0;
            rigiBody = GetComponent<Rigidbody>();

            sizeX = transform.GetChild(0).GetComponent<Renderer>().bounds.size.x;
            sizeY = transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
            sizeXRoad = SaucerFlyingRoadManager.Instance.tunnelWidth;
            sizeYRoad = SaucerFlyingRoadManager.Instance.tunnelHeight;
            speed = SaucerFlyingGameManager.Instance.minMoveSpeed;
            for(int i = 0; i < transform.childCount; i++)
            {
                TrailRenderer lineReneder = transform.GetChild(i).GetComponent<TrailRenderer>();
                if (lineReneder != null)
                {
                    lineReneder.startColor = new Color(character.charColor.r, character.charColor.g, character.charColor.b, 0.7f);
                    lineReneder.endColor = new Color(character.charColor.r, character.charColor.g, character.charColor.b, 0);
                }
            }
            Invoke("DelayRun", 0.1f);
            
        }

        void DelayRun()
        {
            isRun = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (SaucerFlyingGameManager.Instance.GameState == GameState.Playing && isRun)
            {
                if(SaucerFlyingGameManager.Instance.ScoreMode == ScoreMode.Distance &&
                        transform.position.z - prePosZ >= SaucerFlyingGameManager.Instance.DistanceGetScore)
                {
                    SaucerFlyingScoreManager.Instance.AddScore(Random.Range(5,10));
                    prePosZ = transform.position.z;
                }
                MoveByTouch();
            }
#if UNITY_EDITOR
            if (energy >= ShieldConsume && Input.GetKeyDown(KeyCode.J))
#else
            if (energy >= ShieldConsume && DealCommand.GetKeyDown(1, (AppKeyCode)3))
#endif                
            {
                energy = 0;
                ShieldConsume += 2;
                audioSources[1].volume = LibWGM.machine.SeVolume/10 * audioVolume[1];
                audioSources[1].Play();
                ShieldImage.fillAmount = energy / ShieldConsume;
                isShield = true;
                int rand = Random.Range(0, ShieldVFXprefabs.Count);
                while (rand == ShieldIndex)
                {
                    rand = Random.Range(0, ShieldVFXprefabs.Count);
                }
                ShieldIndex = rand;
                if (ShieldVFXeffects[rand] == null)
                {
                    ShieldVFXeffects[rand] = Instantiate(ShieldVFXprefabs[rand], transform.position, Quaternion.identity);
                    ShieldVFXeffects[rand].transform.parent = transform;
                }
                else
                {
                    ShieldVFXeffects[rand].transform.position = transform.position;
                    ShieldVFXeffects[rand].SetActive(true);
                    ShieldVFXeffects[rand].GetComponent<ParticleSystem>().Play();
                }
                StartCoroutine(ShieldVFXEnd(rand));
            }
        }
        IEnumerator ShieldVFXEnd(int rand)
        {
            yield return new WaitForSeconds(5f);
            if (isShield)
            {
                audioSources[2].volume = LibWGM.machine.SeVolume/10 * audioVolume[2];
                audioSources[2].Play();
            }
            isShield = false;
            ShieldVFXeffects[rand].SetActive(false);
        }
        // Listens to changes in game state
        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                // Do whatever necessary when a new game starts
                rigiBody = GetComponent<Rigidbody>();
                originalPosZ = transform.position.z;
                speed = SaucerFlyingGameManager.Instance.minMoveSpeed;
                // rigiBody.useGravity = true;
            }
        }
        // Calls this when the player dies and game over
        public void Die()
        {
            // Fire event
            if (PlayerDied != null)
            {
                PlayerDied();
            }

        }
        void MoveByTouch()
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
            transform.GetChild(1).GetComponent<Renderer>().material.color = Color.white;
            if (SaucerFlyingGameManager.Instance.AccelMode == AccelerationMode.Distance)
            {
                speed = SaucerFlyingGameManager.Instance.minMoveSpeed + (transform.position.z - originalPosZ) * SaucerFlyingGameManager.Instance.accelIndex;
                speed = Mathf.Clamp(speed, SaucerFlyingGameManager.Instance.minMoveSpeed, SaucerFlyingGameManager.Instance.maxMoveSpeed);
            }

            //Direction direction = InputManager.Instance.direction;
            if(SaucerFlyingInputManager.Instance.touchY !=0)
                transform.position += transform.forward * Time.deltaTime * speed;
            else
                transform.position += new Vector3(0,0,1) * Time.deltaTime * speed;
            Vector3 temp = transform.position;

            // move left/ right
            float inputs = SaucerFlyingInputManager.Instance.touchX;
            if (inputs != 0)
                temp += new Vector3(1, 0, 0) * inputs * Time.deltaTime * speed;
            else
                temp += Vector3.zero;

            temp.x = Mathf.Clamp(temp.x, -sizeXRoad / 2 + sizeX / 2, sizeXRoad / 2 - sizeX / 2);
            temp.y = Mathf.Clamp(temp.y, -sizeYRoad / 2 + sizeY, sizeYRoad / 2 - sizeY);
            transform.position = temp;
        }
        
        
        private void OnCollisionEnter(Collision collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.tag == "Enemy")
            {
                if (isShield)
                {
                    GetComponent<BoxCollider>().enabled = false;
                    isShield = false;
                    StartCoroutine(ShieldVFXEnd());
                    for (int i = 0; i < ShieldVFXeffects[ShieldIndex].transform.childCount; i++)
                    {
                        var particleSystem =  ShieldVFXeffects[ShieldIndex].transform.GetChild(i).GetComponent<ParticleSystem>();
                        // 获取粒子数组和粒子数量
                        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
                        int numParticles = particleSystem.GetParticles(particles);
                        for (int j = 0; j < numParticles; j++)
                        {
                            // 减少每个粒子90%的剩余寿命
                            particles[j].remainingLifetime *= 0.1f;
                        }
                        // 将修改后的粒子数组重新设置回粒子系统
                        particleSystem.SetParticles(particles, numParticles);
                        particleSystem.Stop();
                    }
                    return;
                }
                
                PlayerDied();
                this.gameObject.transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
                particle.transform.position = transform.position;
                //particle.GetComponent<Renderer>().material.color = color;
                particle.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject, 0.05f);
                SaucerFlyingCameraController.Instance.ShakeCamera();
            }
        }
        public List<GameObject> VFXprefabs = new List<GameObject>();
        private GameObject[] VFXeffects ;
        private int lastIndex = -1;
        
        public List<GameObject> ShieldVFXprefabs = new List<GameObject>();
        private GameObject[] ShieldVFXeffects;
        public int ShieldConsume = 10;
        private int energy = 0;
        private bool isShield = false;
        private int ShieldIndex = -1;
        private Image ShieldImage;
        private void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.gameObject;
            if (obj.tag == "ItemSpeedUp")
            {
                Destroy(obj);
                ++countItemSpeedUpTo;
                timeLast = Time.time;
            }
            if (obj.tag == "Gold")
            {
                Destroy(obj);
                SaucerFlyingSoundManager.Instance.PlaySound(SaucerFlyingSoundManager.Instance.coin);
                SaucerFlyingScoreManager.Instance.AddScore(100);

            }
            if (obj.tag == "Enemy")
            {
                if (isShield)
                {
                    return;
                }
                if (SaucerFlyingGameManager.Instance.AccelMode == AccelerationMode.Obstacle)
                {
                    speed += SaucerFlyingGameManager.Instance.accelIndex;
                    speed = Mathf.Clamp(speed, SaucerFlyingGameManager.Instance.minMoveSpeed, SaucerFlyingGameManager.Instance.maxMoveSpeed);
                }
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Box")
            {
                if (!isShield && energy < ShieldConsume)
                {
                    energy++;
                    ShieldImage.fillAmount =(float)energy / ShieldConsume;
                }
                else 
                    return;
                
                StartCoroutine(VFXEnd());
            }
        }

        IEnumerator VFXEnd()
        {
            yield return new WaitForSeconds(0.1f);
            int rand = Random.Range(0, VFXprefabs.Count);
            while (lastIndex== rand)
            {
                rand = Random.Range(0, VFXprefabs.Count);
            }
            lastIndex = rand;
            if (rand < VFXeffects.Length && VFXeffects[rand] == null)
            {
                VFXeffects[rand] = Instantiate(VFXprefabs[rand], transform.position, Quaternion.identity);
                VFXeffects[rand].transform.parent = transform;
            }
            else
            {
                VFXeffects[rand].transform.position = transform.position;
                VFXeffects[rand].SetActive(true);
                VFXeffects[rand].GetComponent<ParticleSystem>().Play();
            }
            audioSources[0].volume = LibWGM.machine.SeVolume/10 * audioVolume[0];
            audioSources[0].Play();
            yield return new WaitForSeconds(1.5f);
            VFXeffects[rand].SetActive(false);
        }

        IEnumerator ShieldVFXEnd()
        {
            audioSources[2].volume = LibWGM.machine.SeVolume/10 * audioVolume[2];
            audioSources[2].Play();
            yield return new WaitForSeconds(0.15f);
            GetComponent<BoxCollider>().enabled = true;
        }
    }

}