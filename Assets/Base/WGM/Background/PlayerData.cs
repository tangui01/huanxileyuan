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
    public class PlayerData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
        public int PlayerDataId { get; set; }
        /// <summary>
        /// 玩家投币（用于前台显示，显示玩家台面上有多少个币）
        /// </summary>
        public int coin_in { get; set; }
        /// <summary>
        /// 玩家用币
        /// </summary>
        public int coin_use { get; set; }
        /// <summary>
        /// 累积总投币数（后台显示）
        /// </summary>
        public int CoinInTotal { get; set; }
        /// <summary>
        /// 累积当期投币数（后台显示）
        /// </summary>
        public int CoinInCur { get; set; }
        /// <summary>
        /// 累积总退币数（后台显示）
        /// </summary>
        public int CoinOutTotal { get; set; }
        /// <summary>
        /// 累积总退票数
        /// </summary>
        public int CoinOutCur { get; set; }
        /// <summary>
        /// ADC数据-上（不需要理会）
        /// </summary>
        public int AdcUp { get; set; }
        /// <summary>
        /// ADC数据-下（不需要理会）
        /// </summary>
        public int AdcDown { get; set; }
        /// <summary>
        /// ADC数据-左（不需要理会）
        /// </summary>
        public int AdcLeft { get; set; }
        /// <summary>
        /// ADC数据-右（不需要理会）
        /// </summary>
        public int AdcRight { get; set; }
        public int Free_coin_in { get; set; }


        public bool EqualTo(PlayerData before)
		{
			var beforeMembers = GetType().GetProperties();
			var afterMembers = before.GetType().GetProperties();
			for(int i = 0; i < beforeMembers.Length; i++) {
				var beforeVal = beforeMembers[i].GetValue(this, null);
				var afterVal = afterMembers[i].GetValue(before, null);
				var beforeValue = beforeVal?.ToString();
				var afterValue = afterVal?.ToString();
				if(beforeValue != afterValue) {
					return false;
				}
			}

			return true;
		}

        public PlayerData Clone()
		{
			using(MemoryStream stream = new MemoryStream()) {
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, this);
				stream.Seek(0, SeekOrigin.Begin);
				return formatter.Deserialize(stream) as PlayerData;
			}
		}
	}
}
