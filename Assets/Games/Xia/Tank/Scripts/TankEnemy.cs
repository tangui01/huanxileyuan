using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WGM;

public class TankEnemy : MonoBehaviour
{

    public float moveSpeed = 3;
    public GameObject bullectPrefab;
    public GameObject explosionPrefab;



    private float v = 0;
    private float h = 0;
    private Vector3 bullectEulerAngles;

    //计时器
    private float timeValChangeDirection=2;
    private float timeVal;



    //控制移动
    private float dir = 1;
    // Use this for initialization
    void Start()
    {
        moveSpeed += 0.2f * MapCreater._scene;
        moveSpeed = Mathf.Min(moveSpeed, 5f);
    }


    private void Update()
    {

        
    }




    //坦克的攻击方法
    private void Attack()
    {

            //子弹产生的角度==当前坦克的角度,加上子弹应该旋转的角度.
            Instantiate(bullectPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bullectEulerAngles));
            timeVal = 0;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(TankPlayerManager.Instance.timeFreeze==true || Time.timeScale <= 0)
        {
            return;
        }
        Move();
        //攻击的时间间隔
        if (timeVal >= 3)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
    private void Move()
    {
        if (h != 0)
            GetComponent<Animator>().SetFloat("DirX", 0);
        if (v != 0)
            GetComponent<Animator>().SetFloat("DirY", 0);
        if (timeValChangeDirection>=2-MapCreater._scene *0.1f)
        {
            int num = Random.Range(0, 8);
            if(num>5)
            {
                h = 0;
                v = -1;

            }
            else if(num==0)
            {

                h = 0;
                v = 1;
            }
            else if(num>0&&num<=2)
            {
                v = 0;
                h = 1;
            }
            else if(num>2&&num<=4)
            {
                v = 0;
                h = -1;
            }
            timeValChangeDirection = 0;
        }
        else
        {
            timeValChangeDirection += Time.fixedDeltaTime;
        }

        if (v != 0&&h!=0)
        {
            v = 0;
            GetComponent<Animator>().SetFloat("DirY", 0);
        }
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
        if (h != 0&&v!=0)
        {
            h = 0;
            GetComponent<Animator>().SetFloat("DirX", 0);
        }
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
    }

    //坦克的死亡方法
    private void Die()
    {
        TankPlayerManager.Instance.vestigial--;
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.GetComponent<AudioSource>().volume = LibWGM.machine.SeVolume / 10;
        explosion.GetComponent<AudioSource>().Play();
        MapCreater.Score += 10;
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            timeValChangeDirection = 4;
        }
    }

}