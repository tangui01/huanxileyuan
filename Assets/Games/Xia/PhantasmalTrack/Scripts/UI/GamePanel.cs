using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PhantasmalTrack
{
    public class GamePanel : MonoBehaviour
    {
        private Text txt_Score;

        private void Awake()
        {
            EventCenter.AddListener(EventDefine.ShowGamePanel, Show);
            EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
            EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
            Init();
        }

        private void Init()
        {
            txt_Score = GameObject.Find("txt_Score").GetComponent<Text>();
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
            EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
            EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 更新成绩显示
        /// </summary>
        /// <param name="score"></param>
        private void UpdateScoreText(int score)
        {
            txt_Score.text = score.ToString();
        }

        /// <summary>
        /// 更新钻石数量显示
        /// </summary>
        /// <param name="diamond"></param>
        private void UpdateDiamondText(int diamond)
        {
            
        }

        /// <summary>
        /// 暂停按钮点击
        /// </summary>
        public void OnPauseButtonClick()
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            //游戏暂停
            Time.timeScale = 0;
            GameManager.Instance.IsPause = true;
        }

        /// <summary>
        /// 开始按钮点击
        /// </summary>
        public void OnPlayButtonClick()
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            //继续游戏
            Time.timeScale = 1;
            GameManager.Instance.IsPause = false;
        }
       
        
    }
}
