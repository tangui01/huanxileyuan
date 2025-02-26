using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：PipeController.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace FlyBird
{
    public class PipeController : MonoBehaviour
    {
        
        [SerializeField] private float pipegenerateSpeed = 0.1f;
        [SerializeField] private GameObject pipePrefab;
        [SerializeField] private float PipemaxY = 0.1f;
        [SerializeField] private float PipeminY = 0.1f;
        [SerializeField] private float PipeminX = 0.1f;
        [SerializeField] private float PipeMoveSpeed = 0.1f;
        private enum GeneratePipestage
        {
            stage1,
            stage2,
            stage3
        }
        private float timer;

        private GeneratePipestage stage = GeneratePipestage.stage1;

        private void Start()
        {
            GameController.Instance.AddAcoreAction = SetGeneratePipeSpeed;
        }

        private void Update()
        {
            if (timer < pipegenerateSpeed)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                GeneratePipe();
            }
        }

        private void GeneratePipe()
        {
            Pipe pipe = PoolManager.Instance.GetObj("Pipe", pipePrefab,
                    new Vector3(transform.position.x, GeneratePipePositiony(), transform.position.z),
                    Quaternion.identity)
                .GetComponent<Pipe>();
            pipe.gameObject.SetActive(true);
            pipe.Init(PipeMoveSpeed, PipeminX);
            
        }

        private float GeneratePipePositiony()
        {
            float pipepositiony = UnityEngine.Random.Range(PipeminY, PipemaxY);
            return pipepositiony;
        }

        //随着时间的增加，产生管道时间变短
        public void SetGeneratePipeSpeed(int count)
        {
            if (count<=10)
            {
                stage = GeneratePipestage.stage1;
            }
            else if (count<=20)
            {
                stage = GeneratePipestage.stage2;
            }
            else
            {
                stage = GeneratePipestage.stage3;
            }
            switch (stage)
            {
                case GeneratePipestage.stage1: 
                     pipegenerateSpeed =1.5f;
                    break;
                case GeneratePipestage.stage2:
                    pipegenerateSpeed =1f;
                    break;
                case GeneratePipestage.stage3:
                    pipegenerateSpeed =0.5f;
                    break;
            }
        }
    }
}