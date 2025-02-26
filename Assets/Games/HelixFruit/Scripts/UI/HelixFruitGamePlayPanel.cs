using System;
using System.Collections;
using System.Collections.Generic;
using Sans.Manager;
using UnityEngine;
using UnityEngine.UI;

/****************************************************
    文件：GamePlayPanel.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
public class HelixFruitGamePlayPanel : MonoBehaviour
{
    [Header("UI References :")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private Text _currentStageText;
    [SerializeField] private Text _nextStageText;
    [SerializeField] private Text _scoreText;

    private void Awake()
    {
        UpdateScoreText(ScoreHandler.Instance.GetCurrentScore());
    }

    private void Start()
    {
        UpdateProgressText();
        HelixFruitGameManager.OnUpdateProgressValue += UpdateProgressFill;
        ScoreHandler.OnScoreUpdated += UpdateScoreText;
    }

    private void OnDisable()
    {
        HelixFruitGameManager.OnUpdateProgressValue -= UpdateProgressFill;
        ScoreHandler.OnScoreUpdated -= UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }

    private void UpdateProgressText() {
        int currentStage = PlayerPrefs.GetInt("level", 1);
        _currentStageText.text = currentStage.ToString();
        _nextStageText.text = (currentStage + 1).ToString();
    }

    private void UpdateProgressFill(float value) {
        _fillImage.fillAmount = value;
    }
}
