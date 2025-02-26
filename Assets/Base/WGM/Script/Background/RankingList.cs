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
        /// ����
        /// </summary>
        [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
        public int RankingListId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// ���ı�
        /// </summary>
        public int CoinsUsed { get; set; }
        /// <summary>
        /// ��ʱ
        /// </summary>
        public int TimeUsed { get; set; }
        /// <summary>
        /// ʡ��
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// ����
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