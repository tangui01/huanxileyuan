using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 上传成绩 / 请求排行榜
/// </summary>
public class ReqMqttRankingList
{
    public bool IsUpdate { get; set; }
    public int Score { get; set; }
    public int CoinsUsed { get; set; }
    public int TimeUsed { get; set; }

    /// <summary>
    /// 排行榜取前几名数据
    /// </summary>
    public int RankTopMax { get; set; }

}
/// <summary>
/// 排行榜结果返回
/// </summary>
//public class RespMqttRankingList
//{
//    /// <summary>
//    /// 本次排名
//    /// </summary>
//    public int CurrentRank { get; set; }
//    public string City { get; set; }
//    /// <summary>
//    /// 排行榜数组
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
