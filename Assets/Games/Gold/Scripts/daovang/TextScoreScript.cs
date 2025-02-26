using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreScript : MonoBehaviour 
{
    Vector3 position, scale;
    public Text txtScore;
    public static int score;
    private Vector3 targetPosition = new Vector3(-357f, 332.6f);
	void Start () 
	{
		txtScore.text = "$"+ score.ToString();
        position = transform.localPosition;
        scale = transform.localScale;
        TextMove();
        GoldMinerGameManager.instance.OnStateChange+=NextLevel;
        if (GoldMinerGameManager.instance.CheckScoreISNextLevl(score))
        {
	          GoldMinerGameManager.instance.SetGameState(EnumStateGame.Pause);
        }
	}
    void TextMove()
    {
        transform.DOLocalMove(targetPosition,0.5f).onComplete= () =>
        {
	        transform.DOScale(Vector3.zero, 0.5f).onComplete=()=>
	        {
		        GamePlayScript.instance.ScoreZoomEffect();
		        GoldMinerGameManager.instance.AddScore(score);
		        Destroy(gameObject);
	        }
	        ;
        };
    }

    public void NextLevel()
    {
	    if (GoldMinerGameManager.instance.gameState==EnumStateGame.Pause)
	    {
		   // Destroy(gameObject);
	    }
    }
}
