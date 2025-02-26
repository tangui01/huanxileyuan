using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
    文件：Controller.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：俄罗斯方块控制层
*****************************************************/
namespace TetrisGame
{
    public class Controller : MonoBehaviour
    {
        public View view { get;private set; }
        public Model model { get;private set; }
        
        public AudioManager audioManager { get;private set; }
        private bool ISGameover;

        private void Awake()
        {
            view = GameObject.FindGameObjectWithTag("View").GetComponent<View>(); 
            model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
            audioManager = GetComponent<AudioManager>();
        }

        private void Start()
        {
            model.OnScoreChanged += view.AddScore;
            model.OnClearLine += audioManager.PlayLineClear;
            model.OnShapeDownSpeedChanged = view.SetSpeed;
            model.GameoverAction = view.CharFail;
            GameTimeManager.instance.TimeOverAction = TimeOver;
            ISGameover = false;
        }

        public void GameOver()
        {
            if (ISGameover)
            {
                return; 
            }
            ISGameover = true;
            CommonUI.instance.BackMainPanel_OPen(ISGameover);
        }

        private void TimeOver()
        {
            CommonUI.instance.StartCouterColdDown();
        }
    }
}

