using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MGP_004CompoundBigWatermelon
{ 

	public class ScoreManager : MonoBehaviour
	{
		public static ScoreManager Instance;
		public ScorePanel scorePanel;
		private int score;
		private void Awake()
		{
			if (Instance==null)
			{
				Instance=this;
			}
			else
			{
				Destroy(gameObject);
			}
		}
		public void AddScore(int _score)
		{
			if (CompoundBigWatermelonGameManager.Instance.IsGameOver())
			{
				return;
			}
			score += _score;
			scorePanel.setScore(score);
		}
	}
}
