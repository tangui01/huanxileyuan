using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WGM
{
	public class RespBase
	{
		public int Code { get; set; } = 200;
		public string Message { get; set; } = "";

		public RespBase()
		{
			Message = "";
			Code = 200;
		}

		public RespBase(string message = "", int code = 200)
		{
			Code = code;
			Message = message;
		}

		public RespBase SetFailed(string message, int code = 999)
		{
			Code = code;
			Message = message;
			return this;
		}
	}

    public class ReqReceivedAck
    {
        public string MethodName { get; set; }
        public int Code { get; set; } = 200;
        public string Message { get; set; }
    }
	
	public class ReqConfig
	{
		public int HardwareVersion;
		public string AppVersion;
    }


	public class RespConfig : RespBase
	{
		public string Config { get; set; }
        public string Property { get; set; }

        public string ContactNumber { get; set; }
		public bool IsBind { get; set; }
	}


	public class ReqHeartBeat
	{
        public DateTime Time { get; set; }
		/// <summary>
		/// 线上投币数
		/// </summary>
		public int OnlineCoinInCount { get; set; }
		/// <summary>
		/// 线下（投币器）投币数
		/// </summary>
		public int OfflineCoinInCount { get; set; }
		/// <summary>
		/// 线上退票数
		/// </summary>
		public int OnlineTicketOutCount { get; set; }
		/// <summary>
		/// 线下（彩票机）退票数
		/// </summary>
		public int OfflineTicketOutCount { get; set; }
		/// <summary>
		/// 线下退礼1
		/// </summary>
		public int OfflinePrize1OutCount { get; set; }
		/// <summary>
		/// 线下退礼2
		/// </summary>
		public int OfflinePrize2OutCount { get; set; }
		/// <summary>
		/// 线下退礼3
		/// </summary>
		public int OfflinePrize3OutCount { get; set; }
	}

	public class RespHeartBeat : RespBase
	{
		public DateTime Time { get; set; }
	}


	public class RespOnCoinIn : RespBase
	{
		public int Coins { get; set; }
	}

	public class RespOnAgencyBind : RespBase
	{
		public bool IsBind { get; set; }
		public string AddressName { get; set; }
	}

	public class RespOnProperty : RespBase
	{
		public string PropertyJson { get; set; }
	}

	public class RespCompensateCoins : RespBase
	{
		public int Coins { get; set; }
	}

    public class ReqErrorReport : RespBase
    {
       

    }
    public class RespErrorReport : RespBase
    {


    }

    public class ReqRankingList
    {
        /// <summary>
        /// 是否更新排行榜。 true:该请求会更新服务器，false:只查询，不写服务器
        /// </summary>
        public bool IsUpdate { get; set; }
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
    }

	public class ReqNewRankingList
	{
		public bool IsUpdate { get; set; }

		public string Province { get; set; }

		public string City { get; set; }
		public List<ReqNewRankingListItem> Datas { get; set; }
	}

	public class ReqNewRankingListItem
	{
		public int Score { get; set; }
		public int CoinsUsed { get; set; }
		public int TimeUsed { get; set; }
	}
	public class RespRankingList : RespBase
    {
        public Data[] Datas { get; set; }
		public Data2[] TopDatas { get; set; }
		public class Data
        {
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

			
        }
		public class Data2
		{
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

			public bool IsCurrent { get; set; }
		}
	}

    public enum ExtActionCode
	{
		EnterBackground = 0,
		Disconnect = 1,
		Reboot = 2,
		UploadFile = 3,
		AutoTest = 4,
		RefreshStore = 5,
        ClearAisleErrState = 6,
        CoinAdjust=7,
        UpGrade = 8,
        Reset = 9,
    }

	public class RespExtAction : RespBase
	{
		public ExtActionCode Action { get; set; }
		public string Data { get; set; }
	}
}
