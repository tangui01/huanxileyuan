using UnityEngine;
using System.Collections;

public class TheNameOfARankMessage {
    //记录成绩和ID，为排行榜做铺垫
    public string LoginId{ get; set; }

    public int Score { get; set; }

    public TheNameOfARankMessage(string loginId, int score)
    {
        this.LoginId = loginId;
        this.Score = score;
    }
}
