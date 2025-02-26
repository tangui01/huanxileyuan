using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using AOT;
using UnityEngine.Assertions;

namespace WGM
{
	public class LibWGM
	{
	 
		public static Machine machine = new Machine();
		public static PlayerData[] playerData = new PlayerData[Machine.PlayerMax];
		public static AccountReport accountReport = new AccountReport();
		public static uint[] plyKey = new uint[Machine.PlayerMax];
		public static uint bgKey;
		public static Queue[] queuePlyKey = new Queue[Machine.PlayerMax];
		public static Queue queueBgKey = Queue.Synchronized(new Queue());
		public static Action<RespLog> onHardwareLog;
		public static Action<RespAdcMsg> onAdcMsg;
		public static Action<RespMotionMsg> onMotionMsg;
		public static Action<RespPrizeMsg> onPrizeOut;
		public static List<RankingList> rankingList = new List<RankingList>();
		public static Action<int, bool> onServerConnect;
		public static Action onBoxTime;
		public static Action<string> onSetNotice;

		static int handle = -1;
		static readonly object handleLocker = new object();
		const int MaxReceive = 2048;
		static readonly byte[] receiveBuf = new byte[MaxReceive];
		static readonly byte[] decodeBuf = new byte[MaxReceive];
		static int decodeCnt = 0;
		static Queue queueCommand = Queue.Synchronized(new Queue());
		static Thread commuThread;
		static byte[] cmdBuf = new byte[2048];
		static Dictionary<CmdType, InvocationDefinition> invocations = new Dictionary<CmdType, InvocationDefinition>();
		public static int LevelMax = 3;
		public static float netTimer;
		public static bool isOpenDebug;
		public static int Init(string port, string mouse, string keyboard)
        {
	        
	        
			//InitDebugLog(DebugLog);
			
			Debug.Log($"chmod {port}: " + LibUnityPlugin.ExecuteShellCmd($"chmod 777 {port}"));
			Debug.Log($"chmod {mouse}: " + LibUnityPlugin.ExecuteShellCmd($"chmod 777 {mouse}"));
			Debug.Log($"chmod {keyboard}: " + LibUnityPlugin.ExecuteShellCmd($"chmod 777 {keyboard}"));

			for(int i = 0; i < Machine.PlayerMax; i++) {
				queuePlyKey[i] = Queue.Synchronized(new Queue());
			}
			
			//InitInputEvents(mouse, keyboard);
			//handle = Open(port);
			//Debug.Log("Open port " + port + " = " + handle);
			//commuThread = new Thread(ReadAsync);
			//commuThread.Start();

			return handle;
		}

		

		public static void Uninit(int myHandle)
		{
			if(handle != myHandle) {
				return;
			}

			commuThread?.Abort();
			UnInitInputEvents();
			handle = Close(handle);
		}

		#region Misc defines
		public enum CmdType
		{
			Null = 0x00,

			HeartBeat = 0x01,

			SystemTime = 0x02,
			/// <summary>
			/// 云支付
			/// </summary>
			CloudPay = 0x03,
			/// <summary>
			/// 机器参数查询
			/// </summary>
			QueryPara = 0x05,
			/// <summary>
			/// 设置机器参数
			/// </summary>
			SetPara = 0x06,
			/// <summary>
			/// 二维码
			/// </summary>
			QrCodeUrl = 0x10,
			/// <summary>
			/// 上传账目增量
			/// </summary>
			UploadAI = 0x13,
			/// <summary>
			/// 网络状态
			/// </summary>
			NetState = 0x19,
			/// <summary>
			/// 重置机器
			/// </summary>
			Reset = 0x63,
			/// <summary>
			/// 我要送
			/// </summary>
			OutCoin = 0x64,
			OpenBG = 0x65,
			OpenDesktop = 0x66,
			DeveloperMode = 0x67,
			QueryNotice = 0x68,
			SetNotice = 0x69,
			QueryHeXiao = 0x6A,
			SetHeXiao = 0x6B,
			GetCfFee = 0x6C,
			SetCfFee = 0x6D,
			/// <summary>
			/// 子命令
			/// </summary>
			SubCmd = 0xC8,
			/// <summary>
			/// 上传分数
			/// </summary>
			UploadScore = 0xCD,

		}

		public enum SubCmdType
		{
			/// <summary>
			/// 游戏机按键状态
			/// </summary>
			GameKeyState = 0x00,
			/// <summary>
			/// 游戏机游戏状态
			/// </summary>
			GameState = 0x01,
			/// <summary>
			/// 游戏机游戏结果
			/// </summary>
			GameResult = 0x02,
			/// <summary>
			/// 进入免费游戏
			/// </summary>
			EnterFreeGame = 0x03,
			/// <summary>
			/// 免费游戏音乐
			/// </summary>
			PlayFreeGameAudio = 0x04,
			/// <summary>
			/// 点阵屏配置 -> 联网状态
			/// </summary>
			GetLEDScreenCnf = 0x05,
			/// <summary>
			/// 免费游戏结果
			/// </summary>
			FreeGameResult = 0x06,
			/// <summary>
			/// 点阵屏音乐
			/// </summary>
			SetVolume = 0x07,
			/// <summary>
			/// 点阵屏返回延时
			/// </summary>
			SetBoxDelay = 0x08,
			/// <summary>
			/// 点阵屏盒子初始化  data 长度 180
			/// </summary>
			LEDScreenInit = 0xFF,
		}

		[DllImport("wgm")]
		static extern int Open(string name);

		[DllImport("wgm")]
		static extern int Close(int handle);

		[DllImport("wgm")]
		static extern int Read(int handle, byte[] buf, int size);

		[DllImport("wgm")]
		static extern int Write(int handle, byte[] buf, int size);

		[DllImport("wgm")]
		static extern void InitDebugLog(DebugLogType pointer);
		
		[DllImport("wgm")]
		static extern int InitInputEvents(string mouse, string keyboard);

		[DllImport("wgm")]
		static extern void UnInitInputEvents();

		[DllImport("wgm")]
		public static extern void SetMouseMove(int x, int y);

		[DllImport("wgm")]
		public static extern void SetMouseDown(KeyCode key);

		[DllImport("wgm")]
		public static extern void SetMouseUp(KeyCode key);
		/// <summary>
		/// RK3229CGetID return "www.huacaizn.com"
		/// </summary>
		/// <returns></returns>
		[DllImport("wgm")]
		public static extern string Rk3229GetCID();
		/// <summary>
		/// 设置管脚输出
		/// </summary>
		/// <param name="no">玩家号 [0~1]</param>
		/// <param name="type">输出类型 [0~3] 0:退币  1:彩票  2:电磁铁  3:预留</param>
		/// <param name="value">电平状态 [0~1] 0:亮（低电平）  1:灭(高电平)</param>
		/// <returns></returns>
		[DllImport("wgm")]
		public static extern int Rk3229SetGpio(int no, int type, int value);

        /// <summary>
        /// 获取玩家按键状态
        /// </summary>
        /// <param name="no">玩家号 [0~1]</param>
        /// <param name="type">按键类型 [0~7] 0:切换  1:射击  2:预留  3:开始  4:数币  5:投币  6:数彩票  7:玩家</param>
        /// <returns></returns>
        [DllImport("wgm")]
		public static extern int Rk3229GetKey(int no, int type);

        /// <summary>
        /// 获取全局按键状态
        /// </summary>
        /// <param name="type">按键类型 [0~5] 0:取消  1:上移  2:下移  3:左移  4:右移  5:确认</param>
        /// <returns></returns>
        [DllImport("wgm")]
		public static extern int Rk3229GetGlobalAdc();

        /// <summary>
        /// 获取玩家ADC值
        /// </summary>
        /// <param name="no">玩家号 [0~1]</param>
        /// <param name="type">adc类型 [0~1] 0:左右x 1:上下y</param>
        /// <returns></returns>
        [DllImport("wgm")]
		public static extern int Rk3229GetAdc(int no, int type);
		static List<int> valueList = new List<int>();
		static List<int> del = new List<int>();
		public static int keyInt=-1;
		static float maxGlobalAdc;
		static float GlobalAdcStep;
		public static void  UpdateGlobalKey()
		{
			
			var value = Rk3229GetGlobalAdc();
			if (maxGlobalAdc < value)
			{
				maxGlobalAdc = value;
				GlobalAdcStep = maxGlobalAdc * 0.25f;
			}
			if (value > maxGlobalAdc- GlobalAdcStep*0.5f)//3500
			{
				keyInt = -1;
				valueList.Clear();
				return ;
			}
			else
			{
		
				valueList.Add(value);
				if (valueList.Count < 3)
				{
					return;
				}
				if (valueList.Count > 20)
				{
					valueList.RemoveAt(0);
				}
				valueList.Sort();
				
				value = valueList[0];
				
				if (value > maxGlobalAdc - GlobalAdcStep * 1.5f)//2500  3000
				{
					keyInt = 0;
					return ;
				}
				else if (value > maxGlobalAdc - GlobalAdcStep * 2.5f)//1500  2000
				{

					keyInt = 1;
					return;
				}
				if (value > maxGlobalAdc - GlobalAdcStep * 3.5f)//500   1000
				{
					keyInt = 2;
					return;
				}
				else if (value < maxGlobalAdc - GlobalAdcStep * 3.75f)//  0  0
				{

					keyInt = 3;
					return;
				}

			}
		}


		public static bool GetGlobalKey(int type)
		{
			if (keyInt == type)
				return true;
			return false;
			
		}

		public delegate void DebugLogType(string info);
		[MonoPInvokeCallback(typeof(DebugLogType))]
		static void DebugLog(string info)
		{
			Debug.Log($"libwgm->{info}");
		}

		public class CallbackDescriptor
		{
			public Action<byte[]> Callback;
			public float TimeEscape;
			public float Timeout;
			public CallbackDescriptor(Action<byte[]> callback, float timeout)
			{
				Callback = callback;
				Timeout = timeout;
				TimeEscape = 0;
			}
		}

		public class InvocationDefinition
		{
			public List<CallbackDescriptor> callbacks = new List<CallbackDescriptor>();
			public Type returnType;
		}

		public class RespPrizeMsg
		{
			public int No { get; set; }
			public int Type { get; set; }
		}

		public class RespAdcMsg
		{
			public int No { get; set; }
			public int Ud { get; set; }
			public int Lr { get; set; }
		}

		public class RespMotionMsg
		{
			public int No { get; set; }
			public Vector3 Gyroscope { get; set; }
			public Vector3 Accelerometer { get; set; }
			public Vector3 Magnetometer { get; set; }
		}

		public class RespDataRead : RespBase
		{
			public int Version { get; set; }
			public int LedBeanMax { get; set; }
		}

		public class RespLog : RespBase
		{
			public int LogType { get; set; }
			public string LogMessage { get; set; }
		}
		#endregion

		#region Invoke

		public static Task<TResult> InvokeAsync<TResult>(CmdType target, byte[] buf, int len, Func<byte[], TResult> convertTo, float timeout = 2.0f) where TResult : RespBase, new()
		{
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();

			SendCommand(target, buf, len);

			if(!invocations.TryGetValue(target, out InvocationDefinition invocation)) {
				invocations.Add(target, invocation = new InvocationDefinition { returnType = typeof(TResult) });
			}

			invocation.callbacks.Add(new CallbackDescriptor((bytes) => {
				if(bytes != null) {
					tcs.TrySetResult(convertTo(bytes));
				} else {
					tcs.TrySetResult(new TResult().SetFailed("请求硬件超时", 408) as TResult);
				}
			}, timeout));

			return tcs.Task;
		}

		/// <summary>
		/// 心跳
		/// </summary>
		/// <returns>通信状态</returns>
		public static async Task<RespBase> HeartBeatAsync()
		{
			byte[] buf = new byte[10];
			byte cnt = 0;

			var resp = await InvokeAsync(CmdType.HeartBeat, buf, cnt, bytes => {
				return new RespBase();
			});

			Debug.Log($"HeartBeat code:{resp.Code}");
			return resp;
		}

		

	

		#endregion

		#region Protocal
		static void ReadAsync()
		{
			while (true)
			{
				int length = Read(handle, receiveBuf, MaxReceive);
				if (length > 0)
				{
					for (int i = 0; i < length; i++)
					{
						DecodeByte(receiveBuf[i]);
					}
				}
				Thread.Sleep(10);
			}
		}

		static void DecodeByte(byte elem)
		{
			//字节加入缓存
			decodeBuf[decodeCnt++] = elem;

			//校验起始位
			do
			{
				if (decodeBuf[0] == 0xAA)
				{
					if (decodeCnt > 3 /*&& decodeBuf[1] < 203*/)
					{
						//长度达到
						if (decodeBuf[1] + 3 <= decodeCnt)
						{
							if (decodeBuf[decodeCnt - 1] == 0xDD && GetCheckSum())
							{
								//正确的指令
								ReceiveProcess(decodeBuf, decodeCnt);
								decodeCnt = 0;
							}
							else
							{
								//校验不合格，找出下一个头
								//如果没有 decodeCnt置 0 ，收到的下个字节从缓存0位压入
								if (CommuDecodeMove())
								{
									continue;
								}
								else
								{
									decodeCnt = 0;
								}
							}
						}
					}
				}
				else
				{
					decodeCnt = 0;
				}
				break;
			} while (true);
		}
		//找出下一个头
		static bool CommuDecodeMove()
		{
			int i, j;

			for (i = 1; i < decodeCnt; i++)
			{
				if (0xAA == decodeBuf[i])
				{
					for (j = 0; j < decodeCnt - i; j++)
					{
						decodeBuf[j] = decodeBuf[i + j];
					}
					decodeCnt -= i;
					return true;
				}
			}
			decodeCnt = 0;
			return false;
		}

		static void ReceiveProcess(byte[] buf, int len)
		{
			byte[] storeBuf = new byte[len];
			Array.Copy(buf, storeBuf, len);
			queueCommand.Enqueue(storeBuf);


		}


		static bool GetCheckSum()
		{
			byte check_sum = 0;
			int i;

			for (i = 1; i < decodeCnt - 2; i++)
			{
				check_sum ^= decodeBuf[i];
			}
			if (check_sum == decodeBuf[decodeCnt - 2])
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 提取Data
		/// </summary>
		/// <param name="res"></param>
		/// <returns></returns>
		public static byte[] GetData(byte[] res)
		{
			int len = (int)res[1] - 3;
			byte[] data = new byte[len];
			if (len > 0)
				Array.Copy(res, 4, data, 0, len);
			return data;
		}

		public static string ByteToString(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		static byte GetCommand(ref byte[] buf)
		{
			if (queueCommand.Count > 0)
			{

				buf = (byte[])queueCommand.Dequeue();
				if (machine.DeveloperMode)
				{
					string str = $"  <color=red> 接收 {(LibWGM.CmdType)buf[3]} </color>   ";
					for (int i = 0; i < buf.Length; i++)
					{
						str += (buf[i].ToString("X2")) + " ";
					}
					if (!string.IsNullOrEmpty(str))
						Debug.Log(str);
				}
				return buf[3];
			}
			else
			{
				return 0;
			}
		}


		static byte[] CommandPack(byte cmd, byte[] buf, int len)
		{
			int i;

			byte[] send_buf = new byte[len + 6];
			int len_t = len + 5;
			int len_d = len + 3;

			send_buf[0] = 0xAA;
			send_buf[1] = (byte)len_d;
			send_buf[2] = 0x02;
			send_buf[3] = cmd;


			if (buf != null && len > 0)
			{
				Array.Copy(buf, 0, send_buf, 4, len);
			}
			byte check_code = 0;
			for (i = 1; i <= len_d; i++)
			{
				check_code ^= send_buf[i];
			}
			send_buf[len + 4] = check_code;
			send_buf[len + 5] = 0xDD;

			return send_buf;
		}

		/// <summary>
		/// 指令 数据 数据长
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="buf"></param>
		/// <param name="len"></param>
		public static void SendCommand(byte cmd, byte[] buf, int len)
		{
			byte[] sbuf = CommandPack(cmd, buf, len);
			lock (handleLocker)
			{
				if (LibWGM.machine.DeveloperMode)
				{
					string str = $"  <color=green> 发送 {(LibWGM.CmdType)sbuf[3]} </color>   ";
					for (int i = 0; i < sbuf.Length; i++)
					{
						str += sbuf[i].ToString("X2") + " ";

					}
					Debug.Log(str);
				}
				Write(handle, sbuf, sbuf.Length);
			}
		}

		public static void SendCommand(CmdType cmd, byte[] buf, int len)
		{
			SendCommand((byte)cmd, buf, len);
		}


		public static ushort UcToU2(byte[] buf, ref int cnt)
		{
			ushort ret;

			ret = (ushort)((buf[cnt]) + (buf[cnt + 1] << 8));
			cnt += 2;
			return ret;
		}



		public static string UcToString(byte[] buf, ref int cnt)
		{
			uint len = 0;

			while (cnt + len < buf.Length)
			{
				if (buf[cnt + len] == 0)
				{
					break;
				}
				len++;
			}

			byte[] byteArray = new byte[len + 1];

			for (int i = 0; i < len; i++)
			{
				byteArray[i] = buf[cnt++];
			}
			byteArray[len] = 0;

			return Encoding.ASCII.GetString(byteArray);
		}

		/// <summary>
		/// 先低后高
		/// </summary>
		/// <param name="num"></param>
		/// <param name="buf"></param>
		/// <param name="cnt"></param>
		public static void U1ToCu(int num, byte[] buf, ref int cnt)
		{
			buf[cnt] = (byte)(num & 0xff);
			cnt += 1;
		}
		public static void U1ToCu(float num, byte[] buf, ref int cnt)
		{
			buf[cnt] = (byte)((int)num & 0xff);
			cnt += 1;
		}
		/// <summary>
		/// 先低后高
		/// </summary>
		/// <param name="num"></param>
		/// <param name="buf"></param>
		/// <param name="cnt"></param>
		public static void U2ToCu(int num, byte[] buf, ref int cnt)
		{
			buf[cnt] = (byte)(num & 0xff);
			buf[cnt + 1] = (byte)((num >> 8) & 0xff);
			cnt += 2;
		}
		/// <summary>
		/// 先低后高
		/// </summary>
		/// <param name="num"></param>
		/// <param name="buf"></param>
		/// <param name="cnt"></param>
		public static ushort CUToU1(byte[] buf, ref int cnt)
		{
			ushort ret;
			ret = (ushort)buf[cnt];
			cnt += 1;
			return ret;
		}
		/// <summary>
		/// byte[2] 转成 ushort
		/// </summary>
		/// <param name="num"></param>
		/// <param name="buf"></param>
		/// <param name="cnt"></param>
		public static ushort CUToU2(byte[] buf, ref int cnt)
		{
			ushort ret;
			ret = (ushort)((buf[cnt]) + (buf[cnt + 1] << 8));
			cnt += 2;
			return ret;
		}

		public static string CUToString(byte[] buf, ref int cnt)
		{

			int len = cnt + 2;
			if (buf.Length >= cnt)
			{
				byte[] bytes = new byte[2];
				for (int i = cnt; cnt < len; cnt++)
				{
					bytes[i] = buf[i];
				}
				cnt = len;
				return Encoding.UTF8.GetString(bytes);
			}
			return null;
		}

		public static void S4ToUc(int num, byte[] buf, ref int cnt)
		{
			buf[cnt] = (byte)((num) & 0xff);
			buf[cnt + 1] = (byte)((num >> 8) & 0xff);
			buf[cnt + 2] = (byte)((num >> 16) & 0xff);
			buf[cnt + 3] = (byte)((num >> 24) & 0xff);
			cnt += 4;
		}
		public static int UcToS4(byte[] buf, ref int cnt)
		{
			int ret;

			ret = ((int)buf[cnt] + (int)(buf[cnt + 1] << 8) + (int)(buf[cnt + 2] << 16) + (int)(buf[cnt + 3] << 24));
			cnt += 4;
			return ret;
		}

		internal static void PeripheralPower()
		{
			throw new NotImplementedException();
		}

		
		
		public static void ClearAllData()
		{
			Debug.Log("ClearAllData");
		}
		#endregion
		
		
		public static PlayerData GetPlayerData(int index)
		{
			return playerData[index];
		}
		/// <summary>
		/// no 玩家  id 0 - 1
		/// </summary>
		/// <param name="no"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static int GetAdcValue(int no, int id)
		{
			return LibWGM.Rk3229GetAdc(no,id) ;
		}

		/// <summary>
		/// 是否可以退奖品
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static bool GetPrize(int n)
		{
			//int no = n == 1 ? 0 : 1;
			//后台设置奖品或币数为零
			if (machine.Cl_prize == 0 || machine.Cl_coin == 0)
				return false;
			//使用的币数未达到出奖所需币数
			if (playerData[n].coin_use < machine.Cl_coin)
				return false;

			return true;
		}



		
		
		public static void WinOneCoin(int no)
		{
			Debug.Log("WinOneCoin");
			playerData[no].coin_in += 1;
		}

		public static bool CheckJoin(int no)
		{
		    if( playerData[0].coin_in >= machine.Cp_coin)
            {
				if (DealCommand.GetKeyDown(no, AppKeyCode.UpScore))
				{
					PlayOneRound(0);

					return true;
				}
			}
			return false;
		}

		public static void PlayOneRound(int no)
		{
			playerData[no].coin_in -= machine.Cp_coin;
			if (!DealCommand.prizeError)
				playerData[no].coin_use += machine.Cp_coin;
			DealCommand.onCoinIn?.Invoke(no);
		}

		/// <summary>
		/// 查网络状态
		/// </summary>
		/// <returns></returns>
		public static async Task<RespBase> RequestNetState(float timeOut)
		{

			var resp = await InvokeAsync(CmdType.NetState, null, 0,
				bytes => {
					return new RespBase();
				},
				timeOut);

			return resp;
		}

	}

}