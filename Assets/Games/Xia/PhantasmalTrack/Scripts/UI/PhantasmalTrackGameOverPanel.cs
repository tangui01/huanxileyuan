using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PhantasmalTrack
{
    public class PhantasmalTrackGameOverPanel : MonoBehaviour
    {
        public Text txt_Score;

        private void Awake()
        {
            EventCenter.AddListener(EventDefine.ShowGameOverPanel, Show);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, Show);
        }

        private void Show()
        {

            GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());
            txt_Score.text = GameManager.Instance.GetGameScore().ToString();
            //更新总的钻石数量
            GameManager.Instance.UpdateAllDiamond(GameManager.Instance.GetGameDiamond());
            gameObject.SetActive(true);
            
            StartCoroutine(WaitReStart());
        }

        IEnumerator WaitReStart()
        {
            yield return new WaitForSeconds(1.75f);
            CommonUI.instance.BackMainPanel_OPen();
            yield return new WaitForSeconds(0.75f);
            OnRestartButtonclick();

        }
        
        /// <summary>
        /// 再来一局按钮点击
        /// </summary>
        private void OnRestartButtonclick()
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameData.IsAgainGame = true;
        }

        /// <summary>
        /// 排行榜按钮点击
        /// </summary>
        private void OnRankButtonClick()
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            EventCenter.Broadcast(EventDefine.ShowRankPanel);
        }

        /// <summary>
        /// 主界面按钮点击
        /// </summary>
        private void OnHomeButtonClick()
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameData.IsAgainGame = false;
        }
    }
}
