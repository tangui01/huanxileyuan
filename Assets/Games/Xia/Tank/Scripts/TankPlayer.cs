using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class TankPlayer : MonoBehaviour {

    public float moveSpeed = 3;
    public GameObject[] bullectPrefabs;//子弹
    public GameObject explosionPrefab;//死亡
    public GameObject defendEffectPrefab;//无敌

    private AudioSource _moveAudio;

    private AudioSource moveAudio
    {
        get
        {
            if (_moveAudio == null)
            {
                _moveAudio = GetComponent<AudioSource>();
            }
            _moveAudio.volume = LibWGM.machine.SeVolume / 10;
            return _moveAudio;
        }
        set
        {
            _moveAudio = value;
        }
    }

    public AudioClip[] tankAudio;
    public AnimatorOverrideController[] tankatate1;
    public AnimatorOverrideController[] tankatate2;

    private float v = 0;
    private float h = 0;
    private Vector3 bullectEulerAngles;
    private float timeVal1 = 0.4f;
    private float timeVal2 = 0.4f;
    private float defendedTime=3.0f;
    /// <summary>
    /// 是否处于无敌状态
    /// </summary>
    public bool isDefended=true;

    //控制移动
    private float dir = 1;
    private AudioSource TrAudio;
    private float TrAudioVolume = 0;
    NewTank tankInput;
    private void Awake()
    {
        moveAudio = GetComponent<AudioSource>();
    }

    void Start ()
    {
        tankInput = FindObjectOfType<NewTank>();
        TrAudio = transform.GetChild(1).GetComponent<AudioSource>();
        TrAudioVolume = TrAudio.volume * LibWGM.machine.SeVolume/10;
        if(gameObject.tag=="Tank")
        {
            // v = NewTank.Instance.TankInput(1).y;
            // if (v != 0)
            //     return;
            // h = NewTank.Instance.TankInput(1).x;
            // if (h != 0)
            //     return;
        }
        if (gameObject.tag == "Tank2")
        {
            // v = NewTank.Instance.TankInput(0).y;
            // if (v != 0)
            //     return;
            // h = NewTank.Instance.TankInput(0).x;
            // if (h != 0)
                return;
        }
    }


    private void Update()
    {
        if(Time.timeScale <= 0 )
            return;
        //是否处于无敌状态
        if(isDefended)
        {
            defendEffectPrefab.SetActive(true);
            defendedTime -= Time.deltaTime;
            if(defendedTime<=0)
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }
        
        if (gameObject.tag=="Tank")
        {
            if (timeVal1 >= 0.4f)
            {
                Attack();
            }
            else
            {
                timeVal1 += Time.deltaTime;
            }
        }
        // if (gameObject.tag == "Tank2")
        // {
        //     Move2();
        //     if (timeVal2>= 0.4f)
        //     {
        //         Attack();
        //     }
        //     else
        //     {
        //         timeVal2 += Time.deltaTime;
        //     }
        // }
      
    }




    //坦克的攻击方法
    private void Attack()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.J) && gameObject.tag=="Tank")
        {
            //子弹产生的角度==当前坦克的角度,加上子弹应该旋转的角度.
            Instantiate(bullectPrefabs[TankPlayerManager.tankLevel1], transform.position,Quaternion.Euler(transform.eulerAngles+bullectEulerAngles));
            timeVal1 = 0;
        }
#else
        if(DealCommand.GetKeyDown(1, (AppKeyCode)3)&&gameObject.tag=="Tank")
        {
            //子弹产生的角度==当前坦克的角度,加上子弹应该旋转的角度.
            Instantiate(bullectPrefabs[TankPlayerManager.tankLevel1], transform.position,Quaternion.Euler(transform.eulerAngles+bullectEulerAngles));
            timeVal1 = 0;
        }
#endif        
        if (DealCommand.GetKeyDown(0, (AppKeyCode)1) && gameObject.tag == "Tank2")
        {
            //子弹产生的角度==当前坦克的角度,加上子弹应该旋转的角度.
            Instantiate(bullectPrefabs[TankPlayerManager.Instance.tankLevel2], transform.position, Quaternion.Euler(transform.eulerAngles + bullectEulerAngles));
            timeVal2 = 0;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        // if(TankPlayerManager.tankLevel1>=0&&gameObject.tag=="Tank")
        // {
        //     Animator animator = GetComponent<Animator>();
        //     animator.runtimeAnimatorController = tankatate1[TankPlayerManager.tankLevel1];
        // }
        
        if(Time.timeScale <= 0 )
            return;
        if (TankPlayerManager.Instance.tankLevel2 >= 0 && gameObject.tag == "Tank2")
        {
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = tankatate2[TankPlayerManager.Instance.tankLevel2];
        }
        if (TankPlayerManager.Instance.isDefeat)
        {
            return;
        }
        
        
        
        if (gameObject.tag=="Tank")
        {
            Move1();
            // if (timeVal1 >= 0.4f)
            // {
            //     Attack();
            // }
            // else
            // {
            //     timeVal1 += Time.fixedDeltaTime;
            // }
        }
        if (gameObject.tag == "Tank2")
        {
            Move2();
            if (timeVal2>= 0.4f)
            {
                Attack();
            }
            else
            {
                timeVal2 += Time.fixedDeltaTime;
            }
        }

    }


    private void Move1()
    {
        if (h != 0)
            GetComponent<Animator>().SetFloat("DirX", 0);
        if (v != 0)
            GetComponent<Animator>().SetFloat("DirY", 0);

        h = tankInput.TankInput(1).x;
        
        transform.Translate(Vector2.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (h > 0)
        {
            
            GetComponent<Animator>().SetFloat("DirX", dir * h);
            bullectEulerAngles = new Vector3(0, 0, -90);
        }

        else if (h < 0)
        {
            GetComponent<Animator>().SetFloat("DirX", dir * h);
            bullectEulerAngles = new Vector3(0, 0, 90);
        }




        if (h != 0)
        {
            moveAudio.clip = tankAudio[1];
            if(!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
            return;
        }
        v = tankInput.TankInput(1).y;
        transform.Translate(Vector2.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (v > 0)
        {
            GetComponent<Animator>().SetFloat("DirY", dir * v);
            bullectEulerAngles = new Vector3(0, 0, 0);
        }

        else if (v < 0)
        {
            GetComponent<Animator>().SetFloat("DirY", dir * v);
            bullectEulerAngles = new Vector3(0, 0, -180);
        }
        if(v!=0)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

    }
    private void Move2()
    {
        if (h != 0)
            GetComponent<Animator>().SetFloat("DirX", 0);
        if (v != 0)
            GetComponent<Animator>().SetFloat("DirY", 0);
        h = tankInput.TankInput(0).x;
        transform.Translate(Vector2.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (h > 0)
        {

            GetComponent<Animator>().SetFloat("DirX", dir * h);
            bullectEulerAngles = new Vector3(0, 0, -90);
        }

        else if (h < 0)
        {
            GetComponent<Animator>().SetFloat("DirX", dir * h);
            bullectEulerAngles = new Vector3(0, 0, 90);
        }




        if (h != 0)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
            return;
        }
        v = tankInput.TankInput(0).y;
        transform.Translate(Vector2.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (v > 0)
        {
            GetComponent<Animator>().SetFloat("DirY", dir * v);
            bullectEulerAngles = new Vector3(0, 0, 0);
        }

        else if (v < 0)
        {
            GetComponent<Animator>().SetFloat("DirY", dir * v);
            bullectEulerAngles = new Vector3(0, 0, -180);
        }
        if (v != 0)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

    }
    //坦克的死亡方法
    private void Die()
    {
        if (isDefended) return;
        if(gameObject.tag=="Tank")
        {
            TankPlayerManager.Instance.isDead1 = true;
        }
        if (gameObject.tag == "Tank2")
        {
            TankPlayerManager.Instance.isDead2 = true;
        }
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.GetComponent<AudioSource>().volume = LibWGM.machine.SeVolume / 10;
        explosion.GetComponent<AudioSource>().Play();
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }

    public void PropSound()
    {
        TrAudio.volume = TrAudioVolume;
        TrAudio.PlayOneShot(tankAudio[2]);
        MapCreater.Score += 10;
    }
    //碰到道具
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "StrongHeart":
                Destroy(collision.gameObject);
                TankPlayerManager.Instance.isIronHeart = true;
                PropSound();
                break;
            case "Up":
                Destroy(collision.gameObject);
                if(TankPlayerManager.tankLevel1<3 && gameObject.tag=="Tank")
                {
                    TankPlayerManager.tankLevel1++;
                }
                if (TankPlayerManager.Instance.tankLevel2 < 3 && gameObject.tag == "Tank2")
                {
                    TankPlayerManager.Instance.tankLevel2++;
                }
                PropSound();
                break;
            case "God":
                Destroy(collision.gameObject);
                isDefended = true;
                defendedTime = 3.0f;
                PropSound();
                break;
            case "Distory":
                Destroy(collision.gameObject);
                TankPlayerManager.Instance.isDestoryAll = true;
                PropSound();
                break;
            case "Hp":
                Destroy(collision.gameObject);
                if (gameObject.tag == "Tank")
                {
                    TankPlayerManager.lifeValue1++;
                }
                if (gameObject.tag == "Tank2")
                {
                    TankPlayerManager.Instance.lifeValue2++;
                }
                PropSound();
                break;
            case "Time":
                Destroy(collision.gameObject);
                TankPlayerManager.Instance.timeFreeze = true;
                PropSound();
                break;

        }

         
    }





}
