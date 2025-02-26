using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace WGM
{
    [Serializable]
    public class RankingList
    {

        /// <summary>
        /// 主键
        /// </summary>
        [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
        public int RankingListId { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 消耗币
        /// </summary>
        public int CoinsUsed { get; set; }
        /// <summary>
        /// 用时
        /// </summary>
        public int TimeUsed { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }


      
        public RankingList()
        {
           
        }

        public bool EqualTo(Machine before)
        {
            var beforeMembers = GetType().GetProperties();
            var afterMembers = before.GetType().GetProperties();
            for (int i = 0; i < beforeMembers.Length; i++)
            {
                var beforeVal = beforeMembers[i].GetValue(this, null);
                var afterVal = afterMembers[i].GetValue(before, null);
                var beforeValue = beforeVal?.ToString();
                var afterValue = afterVal?.ToString();
                if (beforeValue != afterValue)
                {
                    return false;
                }
            }

            return true;
        }

        public RankingList Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream) as RankingList;
            }
        }
    }
}