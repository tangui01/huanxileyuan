using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using WGM;
using static WGM.LibWGM;
public enum DeviceType
{
	Null,
	Box,
	Led,
}
public class LedScreenSerialPort
{
	[DllImport("wgm")]
	static extern int Open(string name);

	[DllImport("wgm")]
	static extern int Close(int handle);

	[DllImport("wgm")]
	static extern int Read(int handle, byte[] buf, int size);

	[DllImport("wgm")]
	static extern int Write(int handle, byte[] buf, int size);

	[DllImport("wgm")]
	static extern int Write485(int handle, byte[] buf, int size);

	[DllImport("wgm")]
	static extern int WaitForWriteEnd(int handle);
	/// <summary>
	/// #define B0          0   /** Hang up */
	//#define B50         1
	//#define B75         2
	//#define B110        3
	//#define B134        4
	//#define B150        5
	//#define B200        6
	//#define B300        7
	//#define B600        8
	//#define B1200       9
	//#define B1800      10
	//#define B2400      11
	//#define B4800      12
	//#define B9600      13
	//#define B19200     14
	//#define B38400     15
	//		// CBAUDEX range B57600 - B4000000
	//#define B57600     16
	//#define B115200    17
	//#define B230400    18
	//#define B460800    19
	//#define B500000    20
	//#define B576000    21
	//#define B921600    22
	//#define B1000000   23
	//#define B1152000   24
	//#define B1500000   25
	//#define B2000000   26
	//#define B2500000   27
	//#define B3000000   28
	//#define B3500000   29
	//#define B4000000   30
	/// </summary>
	/// <param name="handle"></param>
	/// <param name="baudrate"></param>
	/// <returns></returns>
	[DllImport("wgm")]
	static extern int SetRate(int handle, int baudrate);

	public int handle = -1;
	const int MaxReceive = 1024;
	byte[] receiveBuf = new byte[MaxReceive];
	byte[] decodeBuf = new byte[MaxReceive];
	int decodeCnt = 0;
	Queue queueCommand = Queue.Synchronized(new Queue());
	Queue<Byte[]> queueWrite = new Queue<Byte[]>();
	Thread commuThread;
	Thread sendThread;
	byte[] cmdBuf = new byte[1024];
	Dictionary<CmdType, InvocationDefinition> invocations = new Dictionary<CmdType, InvocationDefinition>();

	public static Action<int> onFreeGame;
	public static Action<int> onFreeGameWinCoin;
	public static Action<int> onFreeGameWinGift;
	public static Action<bool, MqttRankingListItem> onRankingUpdate;


	public static float netTimer = 30;
	int cloudPayCheckCode = -1;
	/// <summary>
	/// 是否打开游戏状态发送
	/// </summary>
	public bool iSOpenSendGameState = false;
	public DeviceType deviceType;


	public async Task<int> Init(string port)
	{
		handle = Open(port);
		Debug.Log("Open port " + port + " = " + handle);
		if (handle > -1)
		{

			//machine.QrCodeUrl = null;
#if UNITY_EDITOR
			Debug.Log("handle:" + handle + "  SetRate =" + SetRate(handle, 38400));
#else
            Debug.Log(SetRate(handle, 15));
#endif
			LibWGM.machine.QrCodeUrl = "";
			commuThread = new Thread(ReadAsync);
			commuThread.Start();
			sendThread = new Thread(WriteAsync);
			sendThread.Start();
			LibWGM.Rk3229SetGpio(0, 3, 0);
			SendSetBoxDelay(10);
			AutoUploadAI().WrapErrors();
			var a = await SendGetLedScreencnf(1);
			await Task.Delay(1000);
			if (a.Code == 200)
			{
				Debug.Log("-----检测到了 LED");
				deviceType = DeviceType.Led;
			}
			else
			{
				Debug.Log("----检测到了 BOX");
				deviceType = DeviceType.Box;

			}

		}
		else
		{
			handle = -2;

			return handle;
		}

		return handle;
	}
	
	public void Uninit()
	{
		sendThread?.Abort();
		commuThread?.Abort();
		handle = Close(handle);
	}
	/// <summary>
	/// 串口读数据
	/// </summary>
	void ReadAsync()
	{
		Debug.Log("<color=green>ReadAsync</color>");
		while (true)
		{

			int length = Read(handle, receiveBuf, MaxReceive);
			//if (DealCommand.monihoutaishuju)
			//{
			//	if (CMDbuf != null)
			//	{
			//		receiveBuf = CMDbuf;
			//		length = CMDbuf.Length;
			//		DealCommand.monihoutaishuju = false;
			//	}
			//}
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

	void WriteAsync()
	{

		while (true)
		{
			Thread.Sleep(10);
			if (queueWrite.Count > 0)
			{
				byte[] sbuf = queueWrite.Dequeue();
				if (LibWGM.machine.DeveloperMode)
				{

					string str = $"  <color=green> 发送 {(LibWGM.CmdType)sbuf[3]} </color>   ";
					for (int i = 0; i < sbuf.Length; i++)
					{
						str += sbuf[i].ToString("X2") + " ";

					}
					Debug.Log(str);
				}
				LibWGM.Rk3229SetGpio(0, 3, 1);
				Write(handle, sbuf, sbuf.Length);
				WaitForWriteEnd(handle);
				LibWGM.Rk3229SetGpio(0, 3, 0);

			}

		}
	}
	void DecodeByte(byte elem)
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
	bool CommuDecodeMove()
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

	void ReceiveProcess(byte[] buf, int len)
	{
		byte[] storeBuf = new byte[len];
		Array.Copy(buf, storeBuf, len);
		queueCommand.Enqueue(storeBuf);


	}

	bool GetCheckSum()
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

	byte GetCommand(ref byte[] buf)
	{
		if (queueCommand.Count > 0)
		{

			buf = (byte[])queueCommand.Dequeue();
			if (LibWGM.machine.DeveloperMode)
			{
				string str = $"  <color=red> 接收 {(LibWGM.CmdType)buf[3]} </color>   ";
				for (int i = 0; i < buf.Length; i++)
				{
					str += buf[i].ToString("X2") + " ";
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


	static byte[] CMDbuf;


	public void SendCommand(CmdType cmd, byte[] buf, int len)
	{
		if (handle < 0)
			return;
		//Debug.LogWarning("Send cmd:"+ cmd);
		SendCommand((byte)cmd, buf, len);
	}

	/// <summary>
	/// 指令 数据 数据长
	/// </summary>
	/// <param name="cmd"></param>
	/// <param name="buf"></param>
	/// <param name="len"></param>
	public void SendCommand(byte cmd, byte[] buf, int len)
	{
		byte[] sbuf = CommandPack(cmd, buf, len);

		if ((CmdType)cmd == CmdType.QueryPara)
			CMDbuf = sbuf;

		queueWrite.Enqueue(sbuf);
		//if (LibWGM.machine.DeveloperMode)
		//	Debug.Log("设置高电位 可发送");
		//LibWGM.Rk3229SetGpio(1, 3, 1);
		//Write(handle, sbuf, sbuf.Length);
		//if (LibWGM.machine.DeveloperMode)
		//	Debug.Log("设置高电位 可接收");
		//LibWGM.Rk3229SetGpio(1, 3, 0);
	}

	public void Update()
	{
		

		netTimer -= Time.deltaTime;
		if (netTimer < -10)
		{
			netTimer = 60;
			Debug.LogError("网络超时");
			LibWGM.onServerConnect?.Invoke(handle, false);
		}
		if (handle < 0)
			return;
		GetPayQrcode();

		CmdType type = (CmdType)GetCommand(ref cmdBuf);
		if (type != CmdType.Null)
		{
			if (LibWGM.machine.DeveloperMode)
				Debug.LogWarning("Receive CmdType = " + type.ToString() + "   " + (int)type);
		}

		switch (type)
		{
			case CmdType.HeartBeat:
				byte[] heartBeatdata = GetData(cmdBuf);
				HeartBeat(heartBeatdata, -1);
				break;
			case CmdType.SystemTime:

				break;
			case CmdType.NetState:
				//0 信号值  1服务器连接状态
				//netTimer = Time.time + 32;
				byte[] netStatedata = GetData(cmdBuf);
				Debug.Log("信号：" + (int)netStatedata[0]);
				netTimer = 30;
				onServerConnect?.Invoke(handle,netStatedata[1] ==1 ? false : true);
				break;
			case CmdType.QrCodeUrl:

				byte[] qrCodeUrldata = GetData(cmdBuf);

				string url = ByteToString(qrCodeUrldata);

				machine.QrCodeUrl = url;
				Uri uri = new Uri(url);
				string fragment = uri.Fragment;
				string id = "";
				int cnt = 0;
				for (int i = 0; i < fragment.Length; i++)
				{
					if (cnt == 1 && (fragment[i] != '&' && fragment[i] != '%'))
						id += fragment[i];
					if (fragment[i] == '=' || fragment[i] == '&' || fragment[i] == '%')
					{
						cnt++;
						if (cnt == 2)
							break;
					}

				}
				id = id.Replace("ABCD", "");
				LibWGM.machine.MachineId = id;

				Debug.Log("收到的二维码：" + url);

				break;
			case CmdType.CloudPay:

				///响应盒子
				InvokeAsync(CmdType.CloudPay, new byte[] { 0x01 }, 1, bytes => { return new RespBase(); });
				byte[] cloudPaydata = GetData(cmdBuf);
				int code = (int)cloudPaydata[0];
				if (cloudPayCheckCode == code)
				{
					Debug.LogWarning("重复的指令");
					return;
				}
				cloudPayCheckCode = code;
				int paycnt = 1;
				int coinCount = LibWGM.UcToU2(cloudPaydata, ref paycnt);
				int fee = LibWGM.UcToU2(cloudPaydata, ref paycnt);
				LibWGM.playerData[1].coin_in += coinCount;
				LibWGM.playerData[1].CoinInCur += coinCount;
				LibWGM.playerData[1].CoinInTotal += coinCount;
				DealCommand.onCoinIn?.Invoke(1);
				CommonUI.instance.CoinCountPanel.setCoinCount();
				break;
			case CmdType.UploadAI:
				//Debug .Log ("UploadAI 成功");
				//machine.ULCoinIn -= coinIn;
				//machine.ULCoinOut -= coinOut;
				//if (queueErrorCode.Contains(errorCode))
				//{
				//	queueErrorCode.Remove(errorCode);
				//}
				break;

			case CmdType.QueryPara:
				UploadPara(machine).WrapErrors();
				break;
			case CmdType.SetPara:
				UpdatePara(GetData(cmdBuf)).WrapErrors();
				break;
			case CmdType.Reset:
				LibWGM.machine.BgmVolume = 5;
				LibWGM.machine.SeVolume = 5;

				InvokeAsync(CmdType.Reset, new byte[] { 0x01 }, 1, bytes => { return new RespBase(); });
				break;
			case CmdType.OutCoin:

				var outCoinData = GetData(cmdBuf);
				
				SendCommand(CmdType.OutCoin, new byte[] { 0x01 }, 1);
				
				break;
			case CmdType.OpenBG:

				if (SceneManager.GetActiveScene().name != "bg")
					SceneManager.LoadScene("bg");
				SendCommand(CmdType.OpenBG, new byte[] { 0x01 }, 1);
				//InvokeAsync(CmdType.OpenBG, new byte[] { 0x01 }, 1, bytes => { return new RespBase(); });



				break;
			case CmdType.OpenDesktop:

				LibUnityPlugin.OpenSettingPanel();
				SendCommand(CmdType.OpenDesktop, new byte[] { 0x01 }, 1);
				//InvokeAsync(CmdType.OpenDesktop, new byte[] { 0x01 }, 1, bytes => { return new RespBase(); });
				break;
			case CmdType.DeveloperMode:
				if (!isOpenDebug)
				{
					SRDebug.Instance.ShowDebugPanel(SRDebugger.DefaultTabs.Console);
				}
				else
				{
					SRDebug.Instance.HideDebugPanel();
				}
				isOpenDebug = !isOpenDebug;
				SendCommand(CmdType.DeveloperMode, new byte[] { 0x01 }, 1);

				break;
			case CmdType.QueryNotice:
				int num = 60;
				string str = machine.Message == null ? "输入要显示的内容" : machine.Message;
				byte[] b = Encoding.UTF8.GetBytes("_");
				byte[] bytes = Encoding.UTF8.GetBytes(str);
				byte[] buf = new byte[num];
				bytes.CopyTo(buf, 0);

				for (int i = bytes.Length; i < num; i++)
				{
					buf[i] = b[0];
				}
				SendCommand(CmdType.QueryNotice, buf, buf.Length);

				break;
			case CmdType.SetNotice:
				var noticeData = GetData(cmdBuf);
				string notice = Encoding.UTF8.GetString(noticeData);
				List<string> notice2 = new List<string>();
				string temp = "";
				for (int i = 0; i < notice.Length; i++)
				{
					if (notice[i] != '_')
						temp += notice[i];
					else
					{
						if (temp.Length > 0)
						{
							notice2.Add(temp);
							temp = "";
						}
					}

				}
				Debug.Log(notice2[0]);
				machine.Message = notice2[0];
				onSetNotice?.Invoke(LibWGM.machine.Message);
				SendCommand(CmdType.SetNotice, new byte[] { 0x01 }, 1);
				//InvokeAsync(CmdType.SetNotice, new byte[] { 0x01 }, 1, bytes => { return new RespBase(); });
				break;
			case CmdType.QueryHeXiao:

				//InvokeAsync(CmdType.QueryNotice, LibWGM.machine.IsHeXiao?new byte[] { 0x01 }: new byte[] { 0x00 }, 1, bytes => { return new RespBase(); });
				break;
			case CmdType.SetHeXiao:

				//LibWGM.machine.IsHeXiao = setHeXiaoData[0]!=0;

				break;
			case CmdType.GetCfFee:
				int p = 0;
				byte[] para = new byte[4];
				//S4ToUc(LibWGM.machine.CfFee, para, ref p);
				SendCommand(CmdType.GetCfFee, para, para.Length);

				break;
			case CmdType.SetCfFee:
				var CfFee = GetData(cmdBuf);
				int v = 0;
				v = UcToS4(CfFee, ref v);
				//LibWGM.machine.CfFee = v;
				Debug.Log(v);
				SendCommand(CmdType.SetCfFee, new byte[] { 0x01 }, 1);

				break;
			case CmdType.SubCmd:
				var subCmdPara = GetData(cmdBuf);
				SubCmdType subCmdType = (SubCmdType)subCmdPara[0];
				byte[] subPara = new byte[subCmdPara.Length - 1];
				Array.Copy(subCmdPara, 1, subPara, 0, subCmdPara.Length - 1);
				Debug.Log("subCmdType" + subCmdType);
				switch (subCmdType)
				{
					case SubCmdType.GameResult:
						//InvokeAsync(CmdType.SubCmd, new byte[] { (byte)SubCmdType.GameResult }, 1, bytes => { return new RespBase(); });
						break;
					case SubCmdType.GameState:
						//InvokeAsync(CmdType.SubCmd, new byte[] { (byte)SubCmdType.GameState }, 1, bytes => { return new RespBase(); });
						break;
					case SubCmdType.PlayFreeGameAudio:
						//InvokeAsync(CmdType.SubCmd, new byte[] { (byte)SubCmdType.PlayFreeGameAudio }, 1, bytes => { return new RespBase(); });
						break;
					case SubCmdType.GetLEDScreenCnf:
						OnLEDScreenCnf(subPara);
						//InvokeAsync(CmdType.SubCmd, new byte[] { (byte)SubCmdType.GetLEDScreenCnf }, 1, bytes => { return new RespBase(); });
						break;
					case SubCmdType.EnterFreeGame:
						OnEnterFreeGame(subPara);
						//InvokeAsync(CmdType.SubCmd, new byte[] { (byte)SubCmdType.EnterFreeGame }, 1, bytes => { return new RespBase(); });
						break;
					case SubCmdType.FreeGameResult:
						OnFreeGameResult(subPara);
						//InvokeAsync(CmdType.SubCmd, new byte[] { (byte)SubCmdType.EnterFreeGame }, 1, bytes => { return new RespBase(); });
						break;
				}
				break;

			case CmdType.UploadScore:
				var subCmd = GetData(cmdBuf);
				byte subCmdtybe = subCmd[0];
				byte[] data = new byte[subCmd.Length - 1];
				switch (subCmdtybe)
				{
					case 0x01:

						Array.Copy(subCmd, 1, data, 0, subCmd.Length - 1);
						if (data.Length == 0)
						{
							Debug.Log("                   self Ranking reture json :");
							onRankingUpdate?.Invoke(true, null);
							break;
						}
						string json = Encoding.UTF8.GetString(data);
						Debug.Log("                   self Ranking reture json :" + json);
						MqttRankingListItem list = JsonConvert.DeserializeObject<MqttRankingListItem>(json);
						onRankingUpdate?.Invoke(true, list);
						break;
					case 0x02:

						Array.Copy(subCmd, 1, data, 0, subCmd.Length - 1);
						if (data.Length == 0)
						{
							Debug.Log("                   self Ranking reture json :");
							onRankingUpdate?.Invoke(false, null);
							break;
						}
						string json2 = Encoding.UTF8.GetString(data);
						Debug.Log("                 other Ranking reture json :" + json2);
						MqttRankingListItem list2 = JsonConvert.DeserializeObject<MqttRankingListItem>(json2);
						onRankingUpdate?.Invoke(false, list2);
						break;
				}

				break;
			default:
				break;
		}
		//received
		InvocationDefinition invocation;
		if (invocations.TryGetValue(type, out invocation))
		{
			var callbacks = invocation.callbacks;
			if (callbacks.Count > 0)
			{
				var callback = callbacks[0];
				callback.Callback?.Invoke(cmdBuf);
				callbacks.Remove(callback);
			}
		}

		//timeout
		foreach (var invocat in invocations)
		{
			var callbacks = invocat.Value.callbacks;
			for (int i = callbacks.Count - 1; i >= 0; i--)
			{
				var callback = callbacks[i];
				callback.TimeEscape += Time.unscaledDeltaTime;
				if (callback.TimeEscape >= callback.Timeout)
				{
					callback.Callback?.Invoke(null);
					callbacks.Remove(callback);
				}
			}
		}
	}


	float getPayQrcodeTimer;
	/// <summary>
	/// 调用一次，直至成功取得二维码
	/// </summary>
	/// <param name="timeOut"></param>
	/// <returns></returns>
	public void GetPayQrcode()
	{
		if (string.IsNullOrEmpty(LibWGM.machine.QrCodeUrl) && getPayQrcodeTimer < Time.time)

		{
			getPayQrcodeTimer += 5;
			SendCommand(CmdType.QrCodeUrl, null, 0);
		}


	}

	public List<int> queueErrorCode = new List<int>();

	static float time;
	/// <summary>
	/// 更新上传数据,上传由 AutoUploadAI 自行处理
	/// </summary>
	public  void UpdateULdata(int pay, int outCoin = 0, int errorCode = 0)
	{
		//return;
		if (pay != 0 || outCoin != 0)
		{
			LibWGM.machine.ULCoinIn += pay;
			LibWGM.machine.ULCoinOut += outCoin;
			time = Time.time + 10;
			//Debug.LogWarning("将上传>>>>>>>> in " + pay + "  out " + machine.ULCoinOut);
		}
		if (errorCode != 0)
		{
			queueErrorCode.Add(errorCode);
			time = Time.time + 10;
			//Debug.LogWarning("将上传 >>>>>>>> in " + pay + "  out " + machine.ULCoinOut+ " errorCode"+ errorCode);
		}

	}


	 async Task AutoUploadAI()
	{
		while (Application.isPlaying)
		{
			await Task.Delay(TimeSpan.FromSeconds(10.0f));
			if (CanUpload())
			{
				int coinIn = machine.ULCoinIn;
				int coinOut = machine.ULCoinOut;
				//最大255
				if (coinIn > 255)
					coinIn = 255;
				if (coinOut > 255)
					coinOut = 255;
				int errorCode = queueErrorCode.Count > 0 ? queueErrorCode[0] : 0;
				//Debug.LogWarning("上传>>>>>>>> in " + coinIn + "  out " + coinOut+ " errorCode" + errorCode);
				var resp = await UploadAI(machine.ULCoinIn, machine.ULCoinOut, errorCode, 2);
				if (resp.Code == 200)
				{
					auto_increment_code++;
					if (auto_increment_code >= 256)
						auto_increment_code = 0x10;
					machine.ULCoinIn -= coinIn;
					machine.ULCoinOut -= coinOut;
					if (errorCode != 0)
						queueErrorCode.RemoveAt(0);
					Debug.LogWarning($"AutoUploadAI 上传>>>>>>>> 成功 coinIn{coinIn} coinOut{coinOut} errorCode{errorCode}");
				}
				else
				{
					Debug.LogError($"AutoUploadAI 失败 coinIn{machine.ULCoinIn} coinOut{machine.ULCoinOut} errorCode{queueErrorCode}");
				}
			}
			else
			{
				//Debug.Log("AutoUploadAI 没有数据上传");
			}
		}
	}

	public  bool CanUpload()
	{
		if (time > Time.time)
			return false;
		if (machine.ULCoinOut == 0 && machine.ULCoinIn == 0 && queueErrorCode.Count == 0)
			return false;
		return true;
	}

	public static int auto_increment_code = 0x10;
	/// <summary>
	/// 上传账目增量
	/// </summary>
	/// <returns></returns>
	public  async Task<RespBase> UploadAI(int pay, int outCoin, int errorCode, float timeOut)
	{
		
		byte[] buf = new byte[7];
		buf[0] = (byte)auto_increment_code;
		buf[1] = (byte)errorCode;
		buf[2] = 0;
		buf[3] = (byte)outCoin;
		buf[4] = (byte)pay;
		byte cnt = 7;

		var resp = await InvokeAsync(CmdType.UploadAI, buf, cnt,
			bytes => {
				return new RespBase();
			},
			timeOut);

		return resp;
	}

	public  async Task<RespBase> UploadPara(Machine machine, float timeOut = 2)
	{
		byte[] buf = new byte[256];
		int cnt = 0;

		U1ToCu(machine.Cp_coin, buf, ref cnt);
		U2ToCu(machine.Cl_coin, buf, ref cnt);
		U2ToCu(machine.Cl_prize, buf, ref cnt);
		U1ToCu(machine.BgmVolume*10, buf, ref cnt);
		U1ToCu(machine.SeVolume*10, buf, ref cnt);
		U2ToCu(machine.GameTime, buf, ref cnt);
		U1ToCu(machine.Language , buf, ref cnt);
		U1ToCu(machine.AutoTime, buf, ref cnt);
		U1ToCu(machine.showQrCode, buf, ref cnt);
		
		foreach (UIOption l in UIOption.list)
		{
			l.index = l.index;
		}

		var resp = await InvokeAsync(CmdType.QueryPara, buf, cnt,
			bytes => {
				return new RespBase();
			},
			timeOut);

		return resp;
	}

	public  async Task<RespBase> UpdatePara(byte[] buf)
	{
		int cnt = 0;
		int a = machine.Cp_coin;
		int b = machine.Cl_coin;
		int c = machine.Cl_prize;
		machine.Cp_coin = CUToU1(buf, ref cnt);
		machine.Cl_coin = CUToU2(buf, ref cnt);
		machine.Cl_prize = CUToU2(buf, ref cnt);
		if (a != machine.Cp_coin || b != machine.Cl_coin || c != machine.Cl_prize)
		{
			Debug.Log("清除数据");
		}
		machine.BgmVolume = CUToU1(buf, ref cnt) / 10f;
		machine.SeVolume = CUToU1(buf, ref cnt) / 10f;
		EventManager.onVolumeChange?.Invoke();
		machine.GameTime = CUToU2(buf, ref cnt);
		machine.showQrCode = CUToU1(buf, ref cnt);
		byte[] buf2 = new byte[1] { 1};
		machine.Language = CUToU1(buf, ref cnt);
		string[] language = new string[] { "简体中文", "繁体中文", "English" };
		Localization.language = language[machine.Language];
		
		//更新语言
		LocalizationManager.Instance?.SwitchLanguage(machine.Language);
		//更新几币一局
		CurrentCoinCountPanel.instance?.setCoinCount();
		//更新背景音乐和音效
		AudioManager.Instance?.SetBGmVolume(machine.BgmVolume/10);
		AudioManager.Instance?.SetEfVolume(machine.SeVolume/10);
		
		machine.AutoTime = CUToU1(buf, ref cnt);
		
		var resp = await InvokeAsync(CmdType.SetPara, buf2, 1,
			bytes => {
				return new RespBase();
			}
		);
		return resp;
		
	}

	
	public Task<TResult> InvokeAsync<TResult>(CmdType target, byte[] buf, int len, Func<byte[], TResult> convertTo, float timeout = 2.0f) where TResult : RespBase, new()
	{
		TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
		if (handle < 0)
		{
			tcs.TrySetResult(new TResult().SetFailed("硬件未连接", 408) as TResult);
			return tcs.Task;
		}

		SendCommand(target, buf, len);

		if (timeout > 0)
		{
			InvocationDefinition invocation = null;

			if (!invocations.TryGetValue(target, out invocation))
			{
				invocations.Add(target, invocation = new InvocationDefinition { returnType = typeof(TResult) });
			}

			invocation.callbacks.Add(new CallbackDescriptor((bytes) =>
			{
				if (bytes != null)
				{
					tcs.TrySetResult(convertTo(bytes));
				}
				else
				{
					tcs.TrySetResult(new TResult().SetFailed("请求硬件超时", 408) as TResult);
				}
			}, timeout));
		}
		else
		{
			tcs.TrySetResult(new TResult().SetFailed($"发送 CmdType: {target}", 666) as TResult);
		}

		return tcs.Task;
	}

	
	public async Task<RespBase> RequestNetState(float timeOut)
	{
		if (deviceType == DeviceType.Box)
		{
			var resp = await InvokeAsync(CmdType.NetState, null, 0,
			  bytes =>
			  {
				  return new RespBase();
			  },
			  timeOut);

			return resp;
		}
		if (deviceType == DeviceType.Led)
		{
			return await SendGetLedScreencnf(timeOut);
		}
		return await SendGetLedScreencnf(timeOut);
	}

	/// <summary>
	/// state 0 idle 1 start 2 end
	/// </summary>
	/// <param name="state"></param>
	public async Task<RespBase> SendGetLedScreencnf(float timeOut)
	{
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (int)SubCmdType.GetLEDScreenCnf;
		LibWGM.U2ToCu(LibWGM.machine.CostTotal, data, ref cnt);
		var resp = await InvokeAsync(CmdType.SubCmd, data, 1,
				bytes => {
					return new RespBase();
				},
				timeOut);

		return resp;

	}
	/// <summary>
	/// state 0 idle 1 start 2 end
	/// </summary>
	/// <param name="state"></param>
	public void SendGameState(int state)
	{
	
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (int)SubCmdType.GameState;
		data[cnt++] = (byte)state;
		LibWGM.U2ToCu(LibWGM.machine.CostTotal, data, ref cnt);
		SendCommand(CmdType.SubCmd, data, cnt);

	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="keystate"></param>
	public void SendSubCmd(SubCmdType subCmdType, Byte[] bytes)
	{
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (byte)subCmdType;
		for (int i = 0; i < bytes.Length; i++)
		{
			data[cnt++] = bytes[i];

		}

		SendCommand(CmdType.SubCmd, data, cnt);
	}

	/// <summary>
	/// keystate 是一个位数据
	/// </summary>
	/// <param name="keystate"></param>
	public void SendGameKeyState(int keystate)
	{
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (int)SubCmdType.GameKeyState;
		U2ToCu(keystate, data, ref cnt);
		SendCommand(CmdType.SubCmd, data, cnt);
	}

	/// <summary>
	/// result 0 不中奖 1 中奖
	/// </summary>
	/// <param name="result"></param>
	public void SendGameResult(int result)
	{
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (int)SubCmdType.GameResult;
		data[cnt++] = (byte)result;
		SendCommand(CmdType.SubCmd, data, cnt);
	}

	/// <summary>
	/// 设置点阵屏音量大小
	/// </summary>
	/// <param name="value"></param>
	public void SendSetVolume(int value)
	{
		Debug.Log("SendSetVolume" + value);
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (int)SubCmdType.SetVolume;
		data[cnt++] = (byte)value;
		SendCommand(CmdType.SubCmd, data, cnt);
	}

	public void SendSetBoxDelay(int value)
	{
		Debug.Log("SendSetBoxDelay" + value);
		int cnt = 0;
		byte[] data = new byte[128];
		data[cnt++] = (int)SubCmdType.SetBoxDelay;
		data[cnt++] = (byte)value;
		SendCommand(CmdType.SubCmd, data, cnt);
	}

	void OnLEDScreenCnf(byte[] para)
	{
		int freeGameState = para[0];
		int box = para[1];
		int led = para[2];
		netTimer = 30;

		Debug.Log("0组序号 1盒子网状态 2点阵屏网状态 " + "byte[0]:" + freeGameState + "  byte[1]:" + box + "  byte[2]:" + led);
		onServerConnect?.Invoke(handle, led >= 1);
	}

	void OnEnterFreeGame(byte[] para)
	{
		int freeGameState = para[0];
		onFreeGame?.Invoke(freeGameState);


	}

	void OnFreeGameResult(byte[] para)
	{
		int cnt = 0;
		int resultTybe = CUToU1(para, ref cnt);
		int winCnt = CUToU2(para, ref cnt);
		if (winCnt <= 0)
		{
			Debug.Log("免费出礼数为0，不发放");
		}
		if (resultTybe == 0)
		{
			Debug.Log("免费出礼为送币+" + winCnt);

			LibWGM.playerData[0].Free_coin_in += winCnt;
			onFreeGameWinCoin?.Invoke(winCnt);
			DealCommand.onCoinIn?.Invoke(0);
		}
		else if (resultTybe == 1)
		{
			Debug.Log("免费出礼为出礼+" + winCnt);
			onFreeGameWinGift?.Invoke(winCnt);
			//LibWGM.WinOnePrize(winCnt);
		}

	}
	public void HeartBeat(byte[] data, float timeOut)
	{
		byte[] buf = new byte[1024];
		int cnt = 0;
		U2ToCu(66, buf, ref cnt);
		SendCommand(CmdType.HeartBeat, buf, cnt);

	}
}
