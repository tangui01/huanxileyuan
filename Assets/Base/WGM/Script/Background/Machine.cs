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
    public class Machine
    {

        /// <summary>
        /// 主键
        /// </summary>
        [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
        public int MachineMachineId { get; set; }
        /// <summary>
        /// 6位十进制整数.
        /// </summary>
        public string MachineId { get; set; }
        /// <summary>
        /// 机台秘钥
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// 硬件版本
        /// </summary>
        public int HardwareVersion { get; set; }
		/// <summary>
		/// 硬件灯珠数量
		/// </summary>
		public int HardwareLedBeanMax { get; set; }
        /// <summary>
        /// 背景音量
        /// </summary>
        public float BgmVolume { get; set; }
        /// <summary>
        /// 音效音量
        /// </summary>
        public float SeVolume { get; set; }
        /// <summary>
        /// 开发者模式
        /// </summary>
        [SimpleSQL.Default(0)]
        public bool DeveloperMode { get; set; }
        /// <summary>
        /// 是否绑定代理
        /// </summary>
        [SimpleSQL.Default(0)]
        public bool IsBing { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoPathLocal { get; set; }
        /// <summary>
        /// 视频MD5
        /// </summary>
        public string VideoMd5Local { get; set; }
        /// <summary>
        /// 最大玩家数
        /// </summary>
        public const int PlayerMax = 2;
        public static bool FPS = false;
        /// <summary>
        /// 几币几玩  几币
        /// </summary>
        public int Cp_coin { get; set; }

        public int Difficult { get; set; }


        public string Password { get; set; }


        /// <summary>
        ///    0 扭蛋  1  彩票
        /// </summary>
        public int Prize_type { get; set; }
        /// <summary>
        /// 0 中文   1  繁体   2 英文
        /// </summary>
        public int  Language { get; set; }

        public int Cp_point { get; set; }
        /// <summary>
        /// 几币几礼  几币
        /// </summary>
        public int Cl_coin { get; set; }
        /// <summary>
        /// 几币几礼  几礼
        /// </summary>
        public int Cl_prize { get; set; }
        public int Return_mode { get; set; }

        public float ValveAsyncTime { get; set; }

        /// <summary>
        /// 三分钟游戏时长
        /// </summary>
        public int GameTime { get; set; }


        public float AddSpeed { get; set; }
        /// <summary>
        /// 自动开始时间
        /// </summary>
        public int AutoTime { get; set; }

        public int PlayerCount { get; set; }

        public int BootCount { get; set; }

        public int showQrCode { get; set; }

        public string QrCodeUrl { get; set; }

        public string Message { get; set; }
        /// <summary>
        /// 要上传的币数
        /// </summary>
        public int ULCoinIn { get; set; }
        /// <summary>
        /// 要上传的出礼数
        /// </summary>
        public int ULCoinOut { get; set; }

        public int CostTotal { get; set; }

        public int AutoIncrementCode { get; set; }

        public Machine()
		{
			MachineId = "";
			ApiKey = "";
			HardwareVersion = 0;
			HardwareLedBeanMax = 20;
			BgmVolume = 5f;
			SeVolume = 5f;
            IsBing = false;
            Difficult = 0;
            Cp_coin = 1;
            Cp_point = 1;
            Password = "00000000";
            Language = 0;
            ValveAsyncTime = 0.18f;
            GameTime = 120;
            AutoTime = 0;
            AddSpeed = 2;
            PlayerCount = 1;
            BootCount = 0;
            showQrCode = 1;
            AutoIncrementCode = 0X10;
		}

		public bool EqualTo(Machine before)
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

        public Machine Clone()
		{
			using(MemoryStream stream = new MemoryStream()) {
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, this);
				stream.Seek(0, SeekOrigin.Begin);
				return formatter.Deserialize(stream) as Machine;
			}
		}
	}
}
