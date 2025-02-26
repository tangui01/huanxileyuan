using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TheNameOfAUserHandler : MonoBehaviour
{
    InputField inputLoginId;
    Text txtMessage;
    GameObject panelLoginDialog;
    TheNameOfARankingManager rmScript;
    public string userName; 
    void Start()
    {
        var theNameOfAUIManager= FindObjectOfType<TheNameOfAUIManager>();
        inputLoginId = theNameOfAUIManager.Instance.GetUIByName(this.transform, "InputName").GetComponent<InputField>();
        txtMessage = theNameOfAUIManager.Instance.GetUIByName(this.transform, "TextMessage").GetComponent<Text>();
        panelLoginDialog = theNameOfAUIManager.Instance.GetPanel("PanelLogin").gameObject;
        rmScript=theNameOfAUIManager.Instance.GetPanel("PanelRanking").GetComponent<TheNameOfARankingManager>();

        panelLoginDialog.SetActive(false);
    }
 
    public void Login()
    { 
        string name = inputLoginId.text.Trim();

        if (string.IsNullOrEmpty(name) == false)
        {
            userName = name;
            txtMessage.color = Color.blue;
            txtMessage.text = "登陆成功";
            Invoke("HideLoginDialog", 0.5f);
        }
        else
        {
            txtMessage.color = Color.red;
            txtMessage.text = "请输入昵称";
        }
    }

    public void HideLoginDialog()
    {
        panelLoginDialog.SetActive(false);

        rmScript.AddMessage(new TheNameOfARankMessage(userName, score));
        rmScript.DisplayRanking();
    }

    int score;
    public void DisplayLogin(string strScore)
    {
        score = int.Parse(strScore); 
        panelLoginDialog.SetActive(true);
        iTween.ScaleFrom(panelLoginDialog, new Vector3(0, 0, 0), 1);
    }

    public Vector2 velocity;
    public void ChangeValue()
    {
        ScrollRect sr = GameObject.Find("GameObject").GetComponent<ScrollRect>();
        velocity = sr.velocity;
    } 
}
