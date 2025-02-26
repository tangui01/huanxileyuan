using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ϴ��ɼ� / �������а�
/// </summary>
public class ReqMqttRankingList
{
    public bool IsUpdate { get; set; }
    public int Score { get; set; }
    public int CoinsUsed { get; set; }
    public int TimeUsed { get; set; }

    /// <summary>
    /// ���а�ȡǰ��������
    /// </summary>
    public int RankTopMax { get; set; }

}
/// <summary>
/// ���а�������
/// </summary>
//public class RespMqttRankingList
//{
//    /// <summary>
//    /// ��������
//    /// </summary>
//    public int CurrentRank { get; set; }
//    public string City { get; set; }
//    /// <summary>
//    /// ���а�����
//    /// </summary>
//    public List<MqttRankingListItem> RankLists { get; set; }

    public class MqttRankingListItem
    {
        public int Rank { get; set; }
        public int Score { get; set; }
       // public int CoinsUsed { get; set; }
        //public int TimeUsed { get; set; }
        public string City { get; set; }
       // public bool IsCurrent { get; set; }
    }

//}
