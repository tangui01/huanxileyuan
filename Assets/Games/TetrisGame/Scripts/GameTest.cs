using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
/****************************************************
    文件：GameTest.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：游戏消除测试
*****************************************************/
namespace TetrisGame
{
    public class GameTest : MonoBehaviour
    {
        
        [SerializeField] private Star star;
        
        private Star[] stars;
        [Button("消除单行测试")]
        public void OneLinerTest()
        {
            for (int i = 0; i < 14; i++)
            {
                stars[i]=Instantiate(star,new Vector3(i,0,0), Quaternion.identity);
            }
        }
        [Button("消除多行测试")]
        public void MultiLinerTest(int row)
        {
            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < 14; i++)
                {
                    Instantiate(star.gameObject,new Vector3(i,j,0), Quaternion.identity);
                }
            }
        }

        public void ClearLine()
        {
            
        }
    }
}

