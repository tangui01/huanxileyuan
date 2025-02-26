using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：LengthPanel.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：蛇的长度显示ui
*****************************************************/
namespace SnakeGame
{
    public class LengthPanel : MonoBehaviour
    {
        [SerializeField] private Text lengthText;

        public void SetLength(int length,int maxlength)
        {
            lengthText.text = length+"/"+maxlength;
        }
    }
}

