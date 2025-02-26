using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TheNameOfAGameOverController : MonoBehaviour {
     
    Vector3 canvasVector;
    Vector3 oldPosition;
    Text  overScore;
    TextMeshProUGUI mainScore;
    TheNameOfAUserHandler uhScript;
    Button saveScoreButton;
    void Start()
    { 
        canvasVector = this.transform.parent.position;
        oldPosition = this.transform.position;

        mainScore = GameObject.Find("Score")?.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        overScore = FindObjectOfType<TheNameOfAUIManager>().Instance.GetUIByName(TheNameOfANameManager.PANEL_OVER, TheNameOfANameManager.TXT_SCORE).GetComponent<Text>();
        // uhScript = UIManager.Instance.GetUIByName(NameManager.PANEL_LOGIN, NameManager.PANEL_LOGIN_DIALOG).GetComponent<UserHandler>();
        
        //获取保存按钮，注册事件
        // saveScoreButton = UIManager.Instance.GetUIByName(this.transform, NameManager.BTN_SAVESCORE).GetComponent<Button>();
        // saveScoreButton.onClick.AddListener(OnClick_SaveScore); 
    }
    

    public void Display()
    {
        // saveScoreButton.interactable = true;
        overScore.text = mainScore.text;
        GameObject.Find("Score")?.SetActive(false);
        GameObject.Find("Timer")?.SetActive(false);
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", canvasVector, "easetype", iTween.EaseType.easeInOutBounce));
        StartCoroutine(AllowRestart());
    }

    IEnumerator AllowRestart()
    {
        yield return new WaitForSeconds(1f);
        CommonUI.instance.BackMainPanel_OPen();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Hide()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
                "position", oldPosition, "easetype", iTween.EaseType.easeInOutBounce));
    }

    //重新开始
    public void Restart()
    {
        //Application.LoadLevel(Application.loadedLevel);
        Hide();
        GameObject.Find("TheNameOfACanvas")?.transform.GetChild(3).gameObject.SetActive(true);
        GameObject.FindObjectOfType<TheNameOfAGameController>().Restart();
    }

    public void OnClick_SaveScore()
    {
        uhScript.DisplayLogin(mainScore.text);
        saveScoreButton.interactable = false;//取消交互功能
    }
}
