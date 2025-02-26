using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGM
{
	public class UnityDebugLog
	{
		/// <summary>
		/// 主键-自增
		/// </summary>
		[SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
		public int UnityDebugLogId { get; set; }
		/// <summary>
		/// log的时间
		/// </summary>
		public DateTime Time { get; set; }
		/// <summary>
		/// 折叠同种类型log的数量
		/// </summary>
		public int Amount { get; set; }
		/// <summary>
		/// log类型
		/// </summary>
		public string LogType { get; set; }
		/// <summary>
		/// 消息
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// 堆栈信息
		/// </summary>
		public string StackTrace { get; set; }
	}
}
