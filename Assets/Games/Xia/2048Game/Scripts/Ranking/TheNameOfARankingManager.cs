using UnityEngine;
using System.Collections; 

public class TheNameOfARankingManager: MonoBehaviour
{
    public GameObject messagePrefab;
    private TheNameOfARankMessage[] messageArray;
    private int index = 0;
    Transform rankingContentTF;

    Vector3 rankingOldPosition;
    Transform canvasTF,contentTF;
	void Start ()
    {
        var theNameOfAUIManager = FindObjectOfType<TheNameOfAUIManager>();
        rankingContentTF=theNameOfAUIManager.Instance.GetUIByName(this.transform, "PanelContent");

        rankingOldPosition = this.transform.position;
        canvasTF = this.transform.parent;

        messageArray = new TheNameOfARankMessage[100];

        contentTF= theNameOfAUIManager.Instance.GetUIByName(this.transform, "PanelContent");

        LoadMessage();
    }

    void LoadMessage()
    { 
        messageArray[index++] = new TheNameOfARankMessage("猪八戒", 11500);
        messageArray[index++] = new TheNameOfARankMessage("孙悟空", 1520);
        messageArray[index++] = new TheNameOfARankMessage("沙僧", 1120);
        messageArray[index++] = new TheNameOfARankMessage("唐僧", 3200);
        messageArray[index++] = new TheNameOfARankMessage("小白龙", 820);
        messageArray[index++] = new TheNameOfARankMessage("令狐冲", 2900);
        messageArray[index++] = new TheNameOfARankMessage("东方不败", 1000);
        messageArray[index++] = new TheNameOfARankMessage("方丈", 2600);
        messageArray[index++] = new TheNameOfARankMessage("冲虚道长", 1400);
        messageArray[index++] = new TheNameOfARankMessage("任我行", 820); 
    }

    void Sort()
    {
        for (int i = 0; i < messageArray.Length - 1; i++)
        {
            for (int j = 0; j < messageArray.Length - 1 - i; j++)
            {
                TheNameOfARankMessage temp;
                if (messageArray[j + 1] != null && messageArray[j].Score < messageArray[j + 1].Score)
                {
                    temp = messageArray[j];
                    messageArray[j] = messageArray[j + 1];
                    messageArray[j + 1] = temp;
                }
            }
        }
    }

    //每次显示排行榜 都会删除原有数据 
    void DisplayMessage()
    {
        DeleteAllMessage();

        Sort();

        for (int i = 0; i < messageArray.Length; i++)
        {
            if (messageArray[i] == null) return;

            GameObject instance = Instantiate(messagePrefab) as GameObject;
            instance.GetComponent<TheNameOfARankingElement>().Init(
                                                                                                      (i + 1).ToString(),
                                                                                                      messageArray[i].LoginId, 
                                                                                                      messageArray[i].Score.ToString()
                                                                                                  );
      
            instance.transform.SetParent(rankingContentTF);
            instance.transform.localPosition = new Vector3();
            instance.transform.localScale = new Vector3(1,1,1);
        }
    }

    void DeleteAllMessage()
    { 
        for (int i = 0; i < contentTF.childCount; i++)
        {
            Destroy(contentTF.GetChild(i).gameObject);
        } 
    }

    public void AddMessage(TheNameOfARankMessage msg)
    {
        messageArray[index++] = msg;
    }

    public void DisplayRanking()
    {
        DisplayMessage();

        iTween.MoveTo(this.gameObject, iTween.Hash(
      "position", canvasTF.position, "easetype", iTween.EaseType.easeOutExpo, "speed", 500));
    }

    public void HideRanking()
    {
        iTween.MoveTo(this.gameObject, rankingOldPosition, 1);
    }
}
