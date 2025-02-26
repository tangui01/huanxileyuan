using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SnakeVSBlock
{
    public class SnakeVSBlockGameController : MonoBehaviour
    {

        public enum GameState
        {
            MENU,
            GAME,
            GAMEOVER
        }

        public static GameState gameState;

        [Header("Managers")] public SnakeMovement SM;
        public BlocksManager BM;

        [Header("Canvas Groups")] public CanvasGroup MENU_CG;
        public CanvasGroup GAME_CG;
        public CanvasGroup GAMEOVER_CG;

        [Header("Score Management")] public Text ScoreText;
        public Text MenuScoreText;
        public Text BestScoreText;
        public int SCORE;
        public static int BESTSCORE;

        [Header("Some Bool")] bool speedAdded;

        private float addSpeed = 100;
        // Use this for initialization
        void Start()
        {

            //Initially, set the menu and Score is null
            SetMenu();
            SCORE = 0;

            //Initialize some booleans
            speedAdded = false;

            //Load the best score
            BESTSCORE = 0;
            Invoke("SetGame", 0.5f);

        }

        // Update is called once per frame
        void Update()
        {

            //Update the score text
            ScoreText.text = SCORE + "";
            MenuScoreText.text = SCORE + "";

            //Update the Best Score and the text
            if (SCORE > BESTSCORE)
                BESTSCORE = SCORE;

            BestScoreText.text = BESTSCORE + "";

            if ( SCORE > addSpeed)
            {
                addSpeed += 100;
                SM.speed += 0.5f;
            }

        }

        public void SetMenu()
        {
            //Set the GameState
            gameState = GameState.MENU;

            //Manage Canvas Groups
            EnableCG(MENU_CG);
            DisableCG(GAME_CG);
            DisableCG(GAMEOVER_CG);
        }

        public void SetGame()
        {
            //Set the GameState
            gameState = GameState.GAME;

            //Manage Canvas Groups
            EnableCG(GAME_CG);
            DisableCG(MENU_CG);
            DisableCG(GAMEOVER_CG);

            //Reset score
            SCORE = 0;
        }

        public void SetGameover()
        {
            //Set the GameState
            gameState = GameState.GAMEOVER;

            //Manage Canvas Groups
            EnableCG(MENU_CG);
            DisableCG(GAME_CG);
            DisableCG(GAMEOVER_CG);

            //Delete all the objects
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Box"))
            {
                Destroy(g);
            }

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Snake"))
            {
                Destroy(g);
            }

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("SimpleBox"))
            {
                Destroy(g);
            }

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Bar"))
            {
                Destroy(g);
            }
            
            // SM.SpawnBodyParts();
            
            // BM.SetPreviousSnakePosAfterGameover();

            //Reset the Speed
            speedAdded = false;

            //Save the Best Score

            //Reset the Simple Blocks List
            BM.SimpleBoxPositions.Clear();

            //Increase AdMob Counter
            
            StartCoroutine(WaitStart());
        }

        IEnumerator WaitStart()
        {
            yield return new WaitForSeconds(0.5f);
            CommonUI.instance.BackMainPanel_OPen();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void EnableCG(CanvasGroup cg)
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }

        public void DisableCG(CanvasGroup cg)
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
}