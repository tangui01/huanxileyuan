using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MGP_004CompoundBigWatermelon
{

    public class CompoundBigWatermelonGameManager : IManager
    {

        private FruitManager m_FruitManager;
        private LineManager m_LineManager;
        private EffectManager m_EffectManager;
        private AudioManager m_AudioManager;
        private ScoreManager m_ScoreManager;

        private Transform m_WorldTrans;
        private Transform m_UITrans;
        private Transform m_Warningline;
        private Transform m_SpawnFruitPosTrans;

        private bool m_IsGameOverWarning;
        public bool GameOverWarning => m_IsGameOverWarning;
        private bool m_IsGameOVer ;
        public bool GameOver=> m_IsGameOVer;
 
        private float m_OverTimer = 0;
        private float m_WarningTimer = 0;
        private MonoBehaviour m_Mono;
        

        private static CompoundBigWatermelonGameManager m_Instance;
        public static CompoundBigWatermelonGameManager Instance
        {
            get {
                if (m_Instance==null)
                {
                    m_Instance = new CompoundBigWatermelonGameManager();
                }

                return m_Instance;
            }
        }
        

        public void Awake(MonoBehaviour mono) {
            m_Mono = mono;
            m_FruitManager = new FruitManager();
            m_LineManager = new LineManager();
            m_EffectManager = new EffectManager();
            m_AudioManager = new AudioManager();
        }

		public void Start()
		{
            FindGameObjectInScene();
            Init(m_WorldTrans, m_UITrans,this);
        }

        public void Init(Transform worldTrans, Transform uiTrans, params object[] manager)
        {
            m_IsGameOverWarning = false;
            m_IsGameOVer = false;
            m_EffectManager.Init(worldTrans, uiTrans);
            m_AudioManager.Init(worldTrans, uiTrans);
            m_FruitManager.Init(worldTrans, uiTrans, m_Mono, m_EffectManager,m_AudioManager, m_ScoreManager);
            m_LineManager.Init(worldTrans, uiTrans, m_FruitManager);
            GameTimeManager.instance.TimeOverAction = timeOver;
        }

        public void Update()
        {
            if (m_IsGameOVer==true)
            {
                return;
            }
            m_FruitManager.Update();
            m_LineManager.Update();
            m_EffectManager.Update();
            m_AudioManager.Update();

            UpdateJudgeGaveOverAndWarning();
        }

        public void Destroy()
        {
            m_FruitManager.Destroy();
            m_LineManager.Destroy();
            m_EffectManager.Destroy();
            m_AudioManager.Destroy();

            m_WorldTrans = null;
            m_UITrans = null;

            m_Warningline = null;
            m_SpawnFruitPosTrans = null;

            m_IsGameOverWarning = false;
            m_IsGameOVer = false;
        }

        void FindGameObjectInScene() {
            m_WorldTrans = GameObject.Find(GameObjectPathInSceneDefine.WORLD_PATH).transform;
            m_UITrans = GameObject.Find(GameObjectPathInSceneDefine.UI_PATH).transform;

            m_Warningline = m_WorldTrans.Find(GameObjectPathInSceneDefine.WARNING_LINE_PATH);
            m_SpawnFruitPosTrans = m_WorldTrans.Find(GameObjectPathInSceneDefine.SPAWN_FRUIT_POS_TRANS_PATH);
        }

        void UpdateJudgeGaveOverAndWarning() {
            if (IsGameOverWarning() == true)
            {
                m_WarningTimer += Time.deltaTime;
                if (m_WarningTimer >= GameConfig.JUDGE_GAME_OVER_WARNING_TIME_LENGHT)
                {
                    m_Warningline.gameObject.SetActive(true);

                }

                if (IsJudgeGameOver() == true)
                {
                    m_OverTimer += Time.deltaTime;
                    if (m_OverTimer >= GameConfig.JUDGE_GAME_OVER_TIME_LENGHT)
                    {
                        m_IsGameOVer = true;

                        if (m_FruitManager.CurFruit != null)
                        {
                            m_FruitManager.CurFruit.DisableFruit();
                        }

                        OnGameOver();
                    }
                }
                else
                {
                    m_OverTimer = 0;
                }
            }
            else
            {
                m_Warningline.gameObject.SetActive(false);
                m_OverTimer = 0;
                m_WarningTimer = 0;
            }
        }

        bool IsGameOverWarning()
        {
            Fruit fruit;
            foreach (Transform item in m_SpawnFruitPosTrans)
            {
                if (item.gameObject.activeSelf == true)
                {
                    fruit = item.GetComponent<Fruit>();
                    if (fruit != null)
                    {
                        if (fruit.CircleCollider2D.enabled == true && fruit != m_FruitManager.CurFruit)
                        {
                            if (m_Warningline.transform.position.y - (fruit.transform.position.y + fruit.CircleCollider2D.radius) < GameConfig.GAME_OVER_WARNING_LINE_DISTANCE)
                            {
                                return true;
                            }
                        }
                    }
                }

            }

            return false;
        }

        bool IsJudgeGameOver()
        {
            Fruit fruit;
            foreach (Transform item in m_SpawnFruitPosTrans)
            {
                if (item.gameObject.activeSelf == true)
                {
                    fruit = item.GetComponent<Fruit>();
                    if (fruit != null)
                    {
                        if (fruit.CircleCollider2D.enabled == true)
                        {
                            if (m_Warningline.transform.position.y - (fruit.transform.position.y + fruit.CircleCollider2D.radius) <= 0)
                            {
                                return true;
                            }
                        }
                    }
                }

            }

            return false;
        }

        public bool IsGameOver()
        {
            return  m_IsGameOVer;
        }

        void OnGameOver()
        {
            m_FruitManager.OnGameOver();
            m_IsGameOVer = true;
            SceneLoadManager.instance.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        void timeOver()
        {
            CommonUI.instance.StartCouterColdDown();
        }
    }
}
