using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using SimpleSQL;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace WGM
{
	#region 按键类型.

	[Flags]
	public enum AppKeyCode
	{
		Bet =  0,   //K押分.
		Flight = 1,   //J射击.
		ExtCh0 = 2,   //R退币.
		UpScore =  3,   //I上分.
		CoinOut = 4,
		CoinIn = 5,
		TicketOut = 6,
		ExtCh1 = 7,   //O下分.
	

	}

	[Flags]
	public enum BgKeyCode
	{
		None=-1,
		Background =  0,
		Left = 1,
		Right = 2,
		Ok = 3,
	}

	public class KeyClass
	{
		protected const int KEY_MAX = 32;
		protected const float DOUBLE_CLICK_INTERVAL = 0.5f;

		public bool[] enable = new bool[KEY_MAX];
		public float[] long_multi_timer = new float[KEY_MAX];
		public float[] long_once_timer = new float[KEY_MAX];
		public float[] long_mul_timer = new float[KEY_MAX];
		public float[] db_click_timer = new float[KEY_MAX];
		public int[] db_click_cnt = new int[KEY_MAX];
		public float[] long_once_time = new float[KEY_MAX];
		public float[] long_multi_time = new float[KEY_MAX];
		public float[] long_continue_interval = new float[KEY_MAX];
		public float[] down_time = new float[KEY_MAX];
		public KeyCode[] remap_code = new KeyCode[KEY_MAX];
		public string[] remap_virtual = new string[KEY_MAX];


		public uint get_key;
		public uint last_key;
		public uint down;
		public uint up;
		public uint trg;
		public uint click;
		public uint db_click;
		public uint long_once;
		public uint long_multi;

		public KeyClass()
		{
			for (int i = 0; i < KEY_MAX; i++)
			{
				enable[i] = true;
				long_once_time[i] = 1.0f;
				long_multi_time[i] = 1.0f;
				long_continue_interval[i] = 0.075f;
				remap_virtual[i] = "";
			}
		}

		public void Init()
		{
			down = 0;
			up = 0;
			click = 0;
			db_click = 0;
			long_once = 0;
		}

		private uint SystemKey()
		{
			int i;
			uint val = 0;

			for (i = 0; i < KEY_MAX; i++)
			{
				if (Input.GetKey(remap_code[i]))
				{
					val |= 1u << i;
				}
			}

			return val;
		}

		public void Analysis(uint app_key)
		{
			uint key_tmp;

			uint en = 0;
			for (int i = 0; i < KEY_MAX; i++)
			{
				en |= (uint)(enable[i] ? 1 : 0) << i;
			}

			get_key = (app_key | SystemKey()) & en;
			down = (last_key ^ get_key) & get_key;
			up = (last_key ^ get_key) & last_key;
			last_key = get_key;
			trg = get_key;

			for (int i = 0; i < KEY_MAX; i++)
			{
				key_tmp = 1u << i;
				if (((int)down & key_tmp) != 0)
				{
					long_once_timer[i] = 0;
					long_multi_timer[i] = 0;
				}
				if (((int)get_key & key_tmp) != 0)
				{
					long_once_timer[i] += long_once_timer[i] >= 0 ? Time.unscaledDeltaTime : 0;
					if (long_once_timer[i] >= long_once_time[i])
					{
						long_once_timer[i] = -1;
						long_once |= key_tmp;
					}

					long_multi_timer[i] += long_multi_timer[i] >= 0 ? Time.unscaledDeltaTime : 0;
					if (long_multi_timer[i] >= long_multi_time[i])
					{
						long_multi_timer[i] = -1;
						long_multi |= key_tmp;
					}
				}
				if (((int)up & key_tmp) != 0)
				{
					long_multi &= ~key_tmp;

					click |= ((long_once_timer[i] > 0) || (long_multi_timer[i] > 0)) ? key_tmp : 0;

					if (((int)click & key_tmp) != 0)
					{
						if (++db_click_cnt[i] % 2 == 0)
						{
							db_click |= key_tmp;
							db_click_timer[i] = 0;
							db_click_cnt[i] = 0;
						}
					}
				}

				if (db_click_cnt[i] > 0)
				{
					db_click_timer[i] += Time.unscaledDeltaTime;
					if (db_click_timer[i] > DOUBLE_CLICK_INTERVAL)
					{
						db_click_timer[i] = 0;
						db_click_cnt[i] = 0;
					}
				}
			}
		}
	}
	#endregion

	public class DealCommand : MonoBehaviour
	{
		public static DealCommand Instance;
		private const int APP_KEY_MAX = 14;
		private const int BS_KEY_MAX = 8;
		private const int LOG_MAX = 20000; //记录太多，只保留最新的20000条

		private readonly string port =
#if UNITY_STANDALONE || UNITY_EDITOR
		"COM3";
#else
		"/dev/ttyS0";
#endif
		readonly string devMouse = "/dev/input/event4";
		readonly string devKeyboard = "/dev/input/event5";

		public static Action<int> onCoinIn;
		public static Action<int, int> onPrizeOut;
		public static Vector2[] aimPosition = new Vector2[Machine.PlayerMax];

		public static Quaternion[] quaternion = new Quaternion[Machine.PlayerMax];
		public static Vector3[] accelerometer = new Vector3[Machine.PlayerMax];
		public static MadgwickAHRS ahrs = new MadgwickAHRS(1f / 50f, 0.1f);

		public static Queue queueDbUpdate = Queue.Synchronized(new Queue());
		public static Queue queueDbInsert = Queue.Synchronized(new Queue());
		public static Queue queueDbDelete = Queue.Synchronized(new Queue());


		private Thread mThreadSave;
		private SimpleSQLManager mDbManager;
		private Machine mMachine = new Machine();
		private PlayerData[] mPlayerDatas = new PlayerData[Machine.PlayerMax];
		private UnityDebugLog mLastUnityDebugLog = new UnityDebugLog();
		private readonly object mDebugLogThreadLock = new object();
		public static int[] handles=new int[] { -1,-2};
		public static bool isBackGround;
		public static Action<int> onStartReturnCoin;
		public LedScreenSerialPort SerialPortManager;
		private string machineID;
		void Awake()
		{
			Time.timeScale = 1;

			Application.targetFrameRate = 30;
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;


			Application.logMessageReceivedThreaded += UnityLogCallback;
			DontDestroyOnLoad(gameObject);

			//Debug.Log("APP Version = " + HallVersion.version + " " + LibUnityPlugin.ReadAllText(Application.streamingAssetsPath + "/build_info.txt"));

			for (int no = 0; no < Machine.PlayerMax; no++)
			{

				LibWGM.playerData[no] = new PlayerData();
				mPlayerDatas[no] = new PlayerData();

			}
			InitDataBase();
#if UNITY_EDITOR
			LibWGM.machine.DeveloperMode = true;
#else
            LibWGM.machine.DeveloperMode = false;
            if (string.IsNullOrEmpty(machineID))
                machineID = LibWGM.Rk3229GetCID();
            if (machineID != "www.huacaizn.com")
            {
                Application.Quit();
            }
#endif
			LibWGM.machine.QrCodeUrl = "";
			LibWGM.machine.showQrCode  = 1;
			mThreadSave = new Thread(BackStageSaveThread);
			mThreadSave.Start();
			handles[0] = LibWGM.Init(port, devMouse, devKeyboard);
            LibWGM.machine.BootCount++;
          
            Localization.language = BSRoot.languages[LibWGM.machine.Language];
			SerialPortManager = new LedScreenSerialPort();
			PortConnect().WrapErrors();
		}
		async Task PortConnect()
		{
			handles[1] = await SerialPortManager.Init(port);
			if(!UINetCheck.nets.ContainsKey(handles[1]))
			UINetCheck.nets.Add(handles[1], false);
			UINetCheck.running = true;
		}
		private void OnEnable()
		{
			if (Instance == this)
				LibWGM.onMotionMsg += OnMotionMsg;
		}

		private void OnDisable()
		{
			if (Instance == this)
			{
				LibWGM.onMotionMsg -= OnMotionMsg;
				Debug.Log("mThreadSave Abort");
				Debug.Log("mThreadListenerKey Abort");
				mThreadSave?.Abort();
				LibWGM.Uninit(handles[0]);
				SerialPortManager.Uninit();
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				DriverManager.UnRegisterKeyEvent(0, AppKeyCode.CoinIn, OnCoinIn);
				DriverManager.UnRegisterKeyEvent(1, AppKeyCode.CoinIn, OnCoinIn);
			}
		}

		void OnCoinIn(AppKeyState state,Driver3229.KeyPara.KeyState keyState )
		{
			if (DealCommand.isBackGround)
				return;
			if (state == AppKeyState.Up)
			{

				LibWGM.playerData[0].coin_in++;
				LibWGM.playerData[0].CoinInCur ++;
				LibWGM.playerData[0].CoinInTotal++;
				onCoinIn?.Invoke(keyState.no);
			}
        }

#if UNITY_EDITOR
		private void OnGUI()
		{
			//GUIStyle  myStyle = new GUIStyle(); 
			//myStyle.fontSize = 20;
			//myStyle.normal.textColor = Color.white;
			//GUI.Label(new Rect(50, 50, 300, 50),  "玩家0：", myStyle);
			//GUI.Label(new Rect(50, 80,300,50),KeyCode.W + ": 开始", myStyle);
			//GUI.Label(new Rect(50, 110, 300, 50), KeyCode.A + ": ←", myStyle);
			//GUI.Label(new Rect(50, 140, 300, 50), KeyCode.D + ": →", myStyle);
			//GUI.Label(new Rect(50, 170, 300, 50), KeyCode.W + ": ↑", myStyle);

			//GUI.Label(new Rect(350, 50, 300, 50), "玩家1：", myStyle);
			//GUI.Label(new Rect(350, 100, 300, 50), KeyCode.Alpha1.ToString() + ": 开始", myStyle);
			//GUI.Label(new Rect(350, 150, 300, 50), KeyCode.LeftArrow + ": ←", myStyle);
			//GUI.Label(new Rect(350, 200, 300, 50), KeyCode.RightArrow + ": →", myStyle);
			//GUI.Label(new Rect(350, 250, 300, 50), KeyCode.UpArrow + ": ↑", myStyle);
		}
#endif
		void Start()
		{
			KeySet(0, AppKeyCode.UpScore, KeyCode.F1);
			KeySet(0, AppKeyCode.Flight, KeyCode.D);
			KeySet(0, AppKeyCode.Bet, KeyCode.W);
			KeySet(0, AppKeyCode.TicketOut, KeyCode.A);
			KeySet(0, AppKeyCode.ExtCh1, KeyCode.J);
			KeySet(0, AppKeyCode.CoinIn, KeyCode.R);
			KeySet(0, AppKeyCode.ExtCh0, KeyCode.S);

			KeySet(1, AppKeyCode.UpScore, KeyCode.F2);
			KeySet(1, AppKeyCode.Flight, KeyCode.D);
			KeySet(1, AppKeyCode.Bet, KeyCode.W);
			KeySet(1, AppKeyCode.TicketOut, KeyCode.A);
			KeySet(1, AppKeyCode.ExtCh1, KeyCode.J);
			KeySet(1, AppKeyCode.CoinIn, KeyCode.R);
			KeySet(1, AppKeyCode.ExtCh0, KeyCode.S);

			KeySet(BgKeyCode.Background, KeyCode.F9);
			KeySet(BgKeyCode.Left, KeyCode.LeftArrow);
			KeySet(BgKeyCode.Right, KeyCode.RightArrow);
			KeySet(BgKeyCode.Ok, KeyCode.KeypadEnter);
			DriverManager.RegisterKeyEvent(1, AppKeyCode.CoinIn, OnCoinIn);

			DriverManager.RegisterKeyEvent(0, AppKeyCode.CoinIn, OnCoinIn);
		}
		public static void ResetAllPosition()
		{


		}
		
		void Update()
		{
			
			for (int no = 0; no < Machine.PlayerMax; no++)
			{
				if (isBackGround)
					break;
			}

			if (GetKeyDown(BgKeyCode.Background))
			{
				//show bg page
				if (SceneManager.GetActiveScene().name != "bg" && SceneManager.GetActiveScene().name != "Ready")
				{
					Time.timeScale = 1;
					GameTimeManager.instance.SetPauseGame(true);
					GameTimeManager.instance.ClearAction();
					AudioManager.Instance.StopAllAudioSound(true);
					LoadABManger.Instance.UnloadAB(SceneManager.GetActiveScene().name);
					SceneLoadManager.instance.LoadSceneAsync("bg");
					CommonUI.instance.EnterBG();
				}
				
			}
			SerialPortManager.Update();
		}

		private void LateUpdate()
		{


		}

		void BackStageSaveThread()
		{

			while (true)
			{
				Thread.Sleep(100);
				SaveData();
			}
		}
		
	

		

		
		public class MKey
		{
			public sbyte upNum;
			public sbyte downNum;
			public sbyte flag;
			public int pla;
			public int IP;
			public int cacheNum;
			public static Action<int, int> onKeyDown;
			public MKey(int p, int ip)
			{
				pla = p;
				IP = ip;
				upNum = 0;
				downNum = 0;
				flag = 0;
				cacheNum = 0;
			}

			public void Update()
			{
				int cur = LibWGM.Rk3229GetKey(pla, IP);

				if (cur == 1)
				{
					upNum = 0;
					downNum++;
					if (downNum >= 2 && flag == 0)
					{
						flag = 1;

					}
				}
				else
				{
					upNum++;
					downNum = 0;
					if (flag == 1)
					{
						cacheNum++;
						onKeyDown?.Invoke(pla, IP);
					}
					flag = 0;
				}
			}
		}
		static float aimX, aimY;
		public static float GetAxisX(int on)
		{

			if (aimPosition[on].x > 0.75f)
			{
				if (aimX < 0)
					aimX = 0;
				aimX += Time.deltaTime;
			}
			else if (aimPosition[on].x < 0.25f)
			{
				if (aimX > 0)
					aimX = 0;
				aimX -= Time.deltaTime;
			}
			else
			{
				aimX = 0;
			}
			aimX = Mathf.Clamp(aimX, -1, 1f);
			return aimX;
		}
		public static float GetAxisY(int on)
		{
			if (aimPosition[on].y > 0.75f)
			{
				if (aimY < 0)
					aimY = 0;
				aimY += Time.deltaTime;
			}
			else if (aimPosition[on].y < 0.25f)
			{
				if (aimY > 0)
					aimY = 0;
				aimY -= Time.deltaTime;
			}
			else
			{
				aimY = 0;
			}
			aimY = Mathf.Clamp(aimY, -1, 1f);
			return aimY;
		}
		public class AsyncMKey
		{

			public MKey[,] keys;
			public List<int[]> listNumber = new List<int[]>();



			public AsyncMKey()
			{
				keys = new MKey[2, 8];
			}
			public bool Init(int p, int key)
			{

				if (p >= keys.GetLength(0) || key >= keys.GetLength(1))
				{
					return false;
				}
				listNumber.Add(new int[] { p, key });
				keys[p, key] = new MKey(p, key);

				return true;
			}
		}


		void OnAdcMsg(int No)
		{
#if UNITY_EDITOR
			//aimPosition[No].x = Mathf.Clamp01((BSAdjust.x - LibWGM.playerData[No].AdcLeft) / (LibWGM.playerData[No].AdcRight - LibWGM.playerData[No].AdcLeft + 0.001f));
			//aimPosition[No].y = Mathf.Clamp01((BSAdjust.y - LibWGM.playerData[No].AdcDown) / (LibWGM.playerData[No].AdcUp - LibWGM.playerData[No].AdcDown + 0.001f));

#else
             aimPosition[No].x = Mathf.Clamp01((LibWGM.Rk3229GetAdc (No, 0) - LibWGM.playerData[No].AdcLeft) / (LibWGM.playerData[No].AdcRight - LibWGM.playerData[No].AdcLeft + 0.001f));
			aimPosition[No].y = Mathf.Clamp01((LibWGM.Rk3229GetAdc(No, 1) - LibWGM.playerData[No].AdcDown) / (LibWGM.playerData[No].AdcUp - LibWGM.playerData[No].AdcDown + 0.001f));
#endif


		}


		void OnMotionMsg(LibWGM.RespMotionMsg msg)
		{
			msg.Gyroscope /= 16.4f;
			msg.Accelerometer /= 16384f;
			msg.Magnetometer *= 0.15f;

			if (msg.Magnetometer == Vector3.zero)
			{
				ahrs.Update(Mathf.Deg2Rad * msg.Gyroscope.x, Mathf.Deg2Rad * msg.Gyroscope.y, Mathf.Deg2Rad * msg.Gyroscope.z, msg.Accelerometer.x, msg.Accelerometer.y, msg.Accelerometer.z);
			}
			else
			{
				ahrs.Update(Mathf.Deg2Rad * msg.Gyroscope.x, Mathf.Deg2Rad * msg.Gyroscope.y, Mathf.Deg2Rad * msg.Gyroscope.z, msg.Accelerometer.x, msg.Accelerometer.y, msg.Accelerometer.z, msg.Magnetometer.x, msg.Magnetometer.y, msg.Magnetometer.z);
			}

			quaternion[msg.No].x = ahrs.Quaternion[0];
			quaternion[msg.No].y = ahrs.Quaternion[1];
			quaternion[msg.No].z = ahrs.Quaternion[2];
			quaternion[msg.No].w = ahrs.Quaternion[3];

			accelerometer[msg.No].x = msg.Accelerometer.x;
			accelerometer[msg.No].y = msg.Accelerometer.y;
			accelerometer[msg.No].z = msg.Accelerometer.z;
		}


		#region 按键处理



		public static bool anyKeyDown
		{
			get
			{
				for (int i = 0; i < DriverManager.keyStates.Count; i++)
				{
					for (int j = 0; j < DriverManager.keyStates[i].Count; j++)
					{
						if (GetKeyDown(i,(AppKeyCode)j))
							return true;
					}

				}
				for (int i = 0; i < DriverManager.bgstate.Length; i++)
				{
					if (GetKeyDown((BgKeyCode)i))
						return true;
				}
				return false;
			}
		}

		private static int GetKeyIndex(uint key)
		{
			int i, tmp;

			tmp = (int)key;
			for (i = 0; i < APP_KEY_MAX; i++)
			{
				tmp >>= 1;
				if (tmp == 0)
				{
					break;
				}
			}
			return i;
		}
		#endregion

		#region 玩家按键
		public static bool GetKey(int no, AppKeyCode key)
		{
			return DriverManager.GetKey(no, (int)key);
		}

		public static bool GetKeyDown(int no, AppKeyCode key)
		{
			return DriverManager.keyStates[no][(int)key].down == 1;
		}

		public static bool GetKeyUp(int no, AppKeyCode key)
		{
			return DriverManager.keyStates[no][(int)key].up == 1;
		}
		static AppKeyCode[] anyKeyCodes = new AppKeyCode[] { AppKeyCode .Bet, AppKeyCode.Flight, 
		AppKeyCode .UpScore,  AppKeyCode.CoinIn,};
		public static int[] anyKeys = new int[6];
		public static bool GetAnyKeyDown()
		{
            for (int j = 0; j < 2; j++)
            {
				for (int i = 0; i < anyKeyCodes.Length; i++)
				{
					if (GetKeyDown(j, anyKeyCodes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}
		public static bool GetAnyKeyUp()
		{
			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < anyKeyCodes.Length; i++)
				{
					if (GetKeyUp(j, anyKeyCodes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}
		public static bool GetKeyLongMulti(int no, AppKeyCode key)
		{
			return DriverManager.keyStates[no][(int)key].longDown  == 1;
		}

		public static void KeySet(int no,AppKeyCode app_key, KeyCode key)
		{
			DriverManager.BindinAppKeyCode(no, app_key, key);

		}

	
		#endregion

		#region 后台按键


		public static bool GetKey(BgKeyCode key)
		{
			return DriverManager. bgstate[(int)key].state==1;
		
		}

		public static bool GetKeyDown(BgKeyCode key)
		{
			return DriverManager.bgstate[(int)key].down == 1;
			
		}

		public static bool GetKeyUp(BgKeyCode key)
		{
			return DriverManager.bgstate[(int)key].up == 1;
		
		}

		
		public static void KeySet(BgKeyCode app_key, KeyCode key)
		{

			DriverManager.BindinBgKeyCode(app_key, key);
		}
		#endregion

		#region 数据保存
		void InitDataBase()
		{
			mDbManager = GetComponent<SimpleSQLManager>();

			mDbManager.CreateTable<Machine>();
			mDbManager.CreateTable<PlayerData>();
			mDbManager.CreateTable<UnityDebugLog>();

			if (mDbManager.Table<Machine>().Count() < 1)
			{
				mDbManager.Insert(LibWGM.machine);
			}

			if (mDbManager.Table<PlayerData>().Count() < 1)
			{
				for (int i = 0; i < LibWGM.playerData.Length; i++)
				{
					mDbManager.Insert(LibWGM.playerData[i]);
				}
			}


			mDbManager.BeginTransaction();
			LibWGM.machine = mDbManager.Table<Machine>().First();
			LibWGM.playerData = mDbManager.Table<PlayerData>().ToArray();

			var unityDebugLogs = mDbManager.Table<UnityDebugLog>().ToList();
			if (unityDebugLogs.Count > LOG_MAX)
			{
				for (int i = 0; i < unityDebugLogs.Count - LOG_MAX; i++)
				{
					mDbManager.Delete(unityDebugLogs[i]);
				}
			}
			mDbManager.Commit();
		}

		public void DropTable<T>()
		{
			string[] strs = typeof(T).ToString().Split(".");
			mDbManager.Execute($"drop table \"{strs[strs.Length - 1]}\"");
		}
		void SaveData()
		{
			if (mDbManager == null)
			{
				return;
			}

		


			mDbManager.BeginTransaction();

			if (!mMachine.EqualTo(LibWGM.machine))
			{
				mMachine = LibWGM.machine.Clone();
				mDbManager.UpdateTable(LibWGM.machine);
			}

			for (int i = 0; i < LibWGM.playerData.Length; i++)
			{
				if (!mPlayerDatas[i].EqualTo(LibWGM.playerData[i]))
				{
					mPlayerDatas[i] = LibWGM.playerData[i].Clone();
					mDbManager.UpdateTable(LibWGM.playerData[i]);
				}
			}

			while (queueDbDelete.Count > 0)
			{
				mDbManager.Delete(queueDbDelete.Dequeue());
			}
			while (queueDbInsert.Count > 0)
			{

				mDbManager.Insert(queueDbInsert.Dequeue());
			}
			while (queueDbUpdate.Count > 0)
			{

				mDbManager.UpdateTable(queueDbUpdate.Dequeue());
			}

			mDbManager.Commit();
		}

		private void UnityLogCallback(string condition, string stackTrace, LogType type)
		{
			lock (mDebugLogThreadLock)
			{
				var prevMessage = mLastUnityDebugLog;
				if (prevMessage != null &&
					prevMessage.LogType == type.ToString() &&
					prevMessage.Message == condition &&
					prevMessage.StackTrace == stackTrace)
				{
					prevMessage.Amount++;
					queueDbUpdate.Enqueue(prevMessage);
				}
				else
				{
					var newLog = new UnityDebugLog
					{
						Time = DateTime.Now,
						Amount = 1,
						LogType = type.ToString(),
						Message = condition,
						StackTrace = stackTrace,
					};
					mLastUnityDebugLog = newLog;
					queueDbInsert.Enqueue(newLog);
				}
			}
		}
		#endregion
		public static Action<bool> OnKeyDisableNormalHandle;
		public static Action<bool> OnKeyEnableNormalHandle;
		public static void KeyDisableAllNormalHandle()
		{

			
			OnKeyDisableNormalHandle?.Invoke(false);
		}
		public static void KeyEnableAllNormalHandle()
		{
			
			OnKeyDisableNormalHandle?.Invoke(true);
		}

		public static void KeyDisableNormalHandle(int i)
		{
			


		}
		public static void KeyEnableNormalHandle(int i)
		{
			


		}
		public static void KeyEnableAll(int no)
		{
			KeyEnableAllNormalHandle();
		}

		public static bool prizeError = false;
		public static bool GetPrize(int no)
		{
			if (prizeError)
			{
				print("---------------礼品功能错误停用---------------");
				return false;
			}

			if (LibWGM.machine.Cl_coin == 0 || LibWGM.machine.Cl_prize == 0 || LibWGM.machine.Cp_coin == 0)
			{
				print("---------------礼品功能没有启用---------------");
				return false;
			}
			print("玩家用币 " + LibWGM.playerData[no].coin_use + "  几币几奖  几币" + LibWGM.machine.Cl_coin);
			return LibWGM.playerData[no].coin_use >= LibWGM.machine.Cl_coin;
		}
		public class ProfitAndLossInfo
		{
			public int state;
			public float level;
			public ProfitAndLossInfo(int state, float level)
			{
				this.state = state;
				this.level = level;
			}
		}

	
		public static bool GetKeyTrg(int i, AppKeyCode key)
		{
			return GetKeyDown(i, key);
		}

		public static void OnPirzeOut()
		{
			LibWGM.playerData[0].CoinOutCur++;//出奖数增加
			LibWGM.playerData[0].CoinOutTotal++;//后台出奖总数增加
		}

		/// <summary>
		/// result 0 不中奖 1 中奖
		/// </summary>
		/// <param name="result"></param>
		public static void SendGameResultToLedScreen(int result)
		{
			Instance.SerialPortManager.SendGameResult(result);
		}
		/// <summary>
		/// 按键数据->LedBox
		/// </summary>
		/// <param name="result"></param>
		public static void SendGameKetStateToLedScreen(int[] list)
		{
			int result = list[0];
			for (int i = 1; i < list.Length; i++)
			{
				result |= list[i] << i;

			}

			Instance.SerialPortManager.SendGameKeyState(result);
		}
	}
}