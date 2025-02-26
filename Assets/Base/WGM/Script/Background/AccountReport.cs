using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace WGM
{
    public class AccountReport
    {
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
}
