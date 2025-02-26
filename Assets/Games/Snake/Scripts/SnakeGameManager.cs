using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WGM;
using Random = UnityEngine.Random;

/****************************************************
    文件：GameRoot.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：游戏根节点
*****************************************************/
namespace SnakeGame
{
    public class SnakeGameManager : MonoBehaviour
    {
        public static SnakeGameManager Instance;
        public GameConf Conf { get; private set; }
        public GameObject foor;
        public Material[] MapMaterials;
        public List<SnakeSkin> playerSkins = new List<SnakeSkin>();
        public SnakePlayerController snakePlayer;
        public float Score;
        public ScorePanel scorePanel;

        public TimePanel snakeTimerPanel;

        //Game state
        public static bool isGameFinished;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            //游戏资源初始化
            GameConfInit();
            PoolManager.Instance.ClearPoolDic();
            //开始随机背景音乐
            SnakeAudioManger.instance.GameAudioInit();
            //设置时间面版的时间
            snakeTimerPanel.SetTimerText(GameTimeManager.instance.GetCurrentTime());
            GameTimeManager.instance.TimeOverAction = TimeGameOver;
            //开始随机地图
            RandomMap();
            //开始随机玩家蛇皮肤
            RandomSnakeSkin();
            //生成机器人
            BotGenerateManager.instance.StartGame();
            //食物管理器初始化
            FoodsGenerateManager.instance.Init();
            Coin_inPanel.Renew_money_Success += Coin_continue;
            //开始生成道具
            PropManager.Instance.Init();
        }

        private void GameConfInit()
        {
            Conf = Resources.Load<GameConf>(SnakeGameConstant.GameConfPath);
            isGameFinished = false;
        }

        /// <summary>
        /// Gameover logic
        /// </summary>
        /// <param name="delay"></param>
        public void Gameover(float delay = 3f)
        {
            isGameFinished = true;
            SnakeAudioManger.instance.IsPlayUpSpeedSound(false);
            StartCoroutine(DisplayGameoverSequenceCo(delay));
        }

        public void TimeGameOver()
        {
            CommonUI.instance.StartCouterColdDown();
            SnakeAudioManger.instance.IsPlayUpSpeedSound(false);
        }

        public void Coin_continue()
        {
            if (isGameFinished)
            {
                SceneLoadManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
        }

        public IEnumerator DisplayGameoverSequenceCo(float delay)
        {
            CameraController.instance.GameOver(delay);
            yield return new WaitForSeconds(delay);
            if (GameTimeManager.instance.GetCurrentTime() > 0)
            {
                CommonUI.instance.BackMainPanel_OPen(true);
            }

            StopCoroutine("DisplayGameoverSequenceCo");
        }

        /// <summary>
        /// 随机地图
        /// </summary>
        private void RandomMap()
        {
            foor.GetComponent<MeshRenderer>().material = MapMaterials[Random.Range(0, MapMaterials.Length)];
        }

        //随机蛇的皮肤
        public void RandomSnakeSkin()
        {
            if (snakePlayer.gameObject.activeSelf == false)
            {
                snakePlayer.gameObject.SetActive(true);
                snakePlayer.Init(playerSkins[Random.Range(0, playerSkins.Count)]);
            }
        }

        /// <summary>
        /// 得分
        /// </summary>
        /// <param name="x"></param>
        public void AddScore(float addscore, Vector3 bornPoint,bool isDouble)
        {
            Score = isDouble? Score+addscore:Score+2*addscore; 
            AddScoreUI addScoreUI = ResLoadManager.instance.LoadGoObj<AddScoreUI>(SnakeGameConstant.AddScoreUIName,
                SnakeGameConstant.AddScoreUIPath, bornPoint, Quaternion.identity);
            addScoreUI.SetScore((int)addscore,isDouble);
            scorePanel.setScore(Mathf.RoundToInt(Score));
        }
        
        private void OnDestroy()
        {
            Coin_inPanel.Renew_money_Success -= Coin_continue;
        }
    }

    [System.Serializable]
    public class SnakeSkin
    {
        [FormerlySerializedAs("HeadSprite")] public Sprite headSprite;
        [FormerlySerializedAs("BodySprite")] public Sprite bodySprite;
    }
}