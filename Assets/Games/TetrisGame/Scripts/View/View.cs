using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：View.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public class View : MonoBehaviour
    {
        [SerializeField] private Text ScoreText;
        [SerializeField] private Text SpeedText;
        [SerializeField] private ParticleSystem ClearLineFx;

        [SerializeField] private Animator ScorePanel;
        
        
        [SerializeField] private PortraitGirl _portraitGirl;
        private int score = 0;
        public void AddScore(int _score, int y)
        {
            //PlayerClearLineFx(y);
            score = _score;
            Invoke("OnaddScore",0.5f);
        }

        private void OnaddScore()
        {
            ScoreText.text = score.ToString();
            ScorePanel.SetTrigger("AddScore");
            _portraitGirl.Smile();
        }

        public void SetSpeed(int value)
        {
            SpeedText.text=value.ToString();
        }

        public void CharFail()
        {
            _portraitGirl.Fail();
        }

        /// <summary>
/// 播放消除特效
/// </summary>
        private void PlayerClearLineFx(int y)
        {
           GameObject fx = PoolManager.Instance.GetObj("ClearLineFx",ClearLineFx.gameObject, new Vector3(ClearLineFx.gameObject.transform.position.x, y, ClearLineFx.gameObject.transform.position.z), Quaternion.identity);
           StartCoroutine(DealyDestory("ClearLineFx", fx));
        }

        IEnumerator  DealyDestory(string name,GameObject obj)
        {
            yield return new WaitForSeconds(0.5f);
            PoolManager.Instance.PushObj(name,obj);
        }

        private void OnDestroy()
        {
            PoolManager.Instance.ClearPoolDic();
        }
    }
}