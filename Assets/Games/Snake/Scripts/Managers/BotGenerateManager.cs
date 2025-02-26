using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeGame
{
/****************************************************
    文件：BotGenerateManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：机器人蛇生成器
*****************************************************/
    public class BotGenerateManager : MonoBehaviour
    {
        public static BotGenerateManager instance;
        [Range(0, 1f)] public float NeedGeneratePercent;
        [SerializeField]private List<BotController> bots = new List<BotController>();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            InitBotGenerate();
        }

        /// <summary>
        /// 开始生成的机器人
        /// </summary>
        public void InitBotGenerate()
        {
            for (int i = 0; i < SnakeGameConstant.defaultSnakeCount; i++)
            {
                BotController bot = ResLoadManager.instance.LoadGoObj<BotController>("BotHead",
                    SnakeGameConstant.BotHeadPre, SnakeGameConstant.GetRandomPositionInMap(), Quaternion.identity);
                bot.name = "BotHead-" + i;
                bot.tag = "BotHead";
                bots.Add(bot);
                bot.Init(SnakeGameManager.Instance.playerSkins[
                    Random.Range(0, SnakeGameManager.Instance.playerSkins.Count)]);
            }
        }

        public void RemoveBot(BotController bot)
        {
            bots.Remove(bot);
            if (bots.Count <SnakeGameConstant.defaultSnakeCount * NeedGeneratePercent)
            {
                StartCoroutine("GenerateBot");
            }
        }

        IEnumerator  GenerateBot()
        {
            yield return new WaitForSeconds(10f);
            BotController bot = ResLoadManager.instance.LoadGoObj<BotController>("BotHead",
                SnakeGameConstant.BotHeadPre, SnakeGameConstant.GetRandomPositionInMap(), Quaternion.identity);
            bot.name = "BotHead-" + bots.Count;
            bot.tag = "BotHead";
            bots.Add(bot);
            bot.Init(
                SnakeGameManager.Instance.playerSkins[Random.Range(0, SnakeGameManager.Instance.playerSkins.Count)]);
        }

        private void OnDestroy()
        {
            bots.Clear();
        }
    }
}
