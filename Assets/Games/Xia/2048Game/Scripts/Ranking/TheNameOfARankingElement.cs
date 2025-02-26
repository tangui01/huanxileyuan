using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TheNameOfARankingElement : MonoBehaviour {

    Text txtNumber, txtLoginId, txtScore;
    void Awake()
    {
        txtNumber = this.transform.Find("TextNumber").GetComponent<Text>();
        txtLoginId = this.transform.Find("TextLoginId").GetComponent<Text>();
        txtScore = this.transform.Find("TextScore").GetComponent<Text>();
    }

    public void Init(string number,string loginId,string score)
    {
        txtNumber.text = number;
        txtLoginId.text = loginId;
        txtScore.text = score;
	} 
}
