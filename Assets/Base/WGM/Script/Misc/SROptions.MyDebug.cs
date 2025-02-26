using UnityEngine;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.IO;
using System;
using WGM;

public partial class SROptions
{
    [Category("Test")]
    public bool DeveloperMode
    {
        get { return WGM.LibWGM.machine.DeveloperMode; }
        set { WGM.LibWGM.machine.DeveloperMode = value; }
    }

	[Category("Test")]
    public void ApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

	[Category("Test")]
	public void OpenLauncher()
	{
		LibUnityPlugin.OpenLauncher();
	}

	[Category("Tools")]
	public void OpenSettingPanel()
	{
		if (WGM.LibWGM.machine.DeveloperMode)
			LibUnityPlugin.OpenSettingPanel();
	}

	bool[,] output = new bool[2, 4] { { true, true, true, true }, { true, true, true, true } };

	[Category("Output")]
	public bool P00 {
		get { return output[0, 0]; }
		set { output[0, 0] = value;
			LibWGM.Rk3229SetGpio(0, 0, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P01 {
		get { return output[0, 1]; }
		set {
			output[0, 1] = value;
			LibWGM.Rk3229SetGpio(0, 1, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P02 {
		get { return output[0, 2]; }
		set {
			output[0, 2] = value;
			LibWGM.Rk3229SetGpio(0, 2, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P03 {
		get { return output[0, 3]; }
		set {
			output[0, 3] = value;
			LibWGM.Rk3229SetGpio(0, 3, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P10 {
		get { return output[1, 0]; }
		set {
			output[1, 0] = value;
			LibWGM.Rk3229SetGpio(1, 0, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P11 {
		get { return output[1, 1]; }
		set {
			output[1, 1] = value;
			LibWGM.Rk3229SetGpio(1, 1, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P12 {
		get { return output[1, 2]; }
		set {
			output[1, 2] = value;
			LibWGM.Rk3229SetGpio(1, 2, value ? 0 : 1);
		}
	}

	[Category("Output")]
	public bool P13 {
		get { return output[1, 3]; }
		set {
			output[1, 3] = value;
			LibWGM.Rk3229SetGpio(1, 3, value ? 0 : 1);
		}
	}

	//adc脉冲信号-----------
	[Category("Adc")] public int Adc00 { get {  return LibWGM.Rk3229GetAdc(0, 0); } }
	[Category("Adc")] public int Adc01 { get { return LibWGM.Rk3229GetAdc(0, 1); } }
	[Category("Adc")] public int Adc10 { get { return LibWGM.Rk3229GetAdc(1, 0); } }
	[Category("Adc")] public int Adc11 { get { return LibWGM.Rk3229GetAdc(1, 1); } }
	[Category("Adc")] public int AdcKey { get { return LibWGM.Rk3229GetGlobalAdc(); } }

	//玩家按键
	[Category("Key0")] public bool Key00 { get { return LibWGM.Rk3229GetKey(0, 0) > 0; } }//切换
	[Category("Key0")] public bool Key01 { get { return LibWGM.Rk3229GetKey(0, 1) > 0; } }//射击
	[Category("Key0")] public bool Key02 { get { return LibWGM.Rk3229GetKey(0, 2) > 0; } }//预留
	[Category("Key0")] public bool Key03 { get { return LibWGM.Rk3229GetKey(0, 3) > 0; } }//开始
	[Category("Key0")] public bool Key04 { get { return LibWGM.Rk3229GetKey(0, 4) > 0; } }//数币 
	[Category("Key0")] public bool Key05 { get { return LibWGM.Rk3229GetKey(0, 5) > 0; } }//投币
	[Category("Key0")] public bool Key06 { get { return LibWGM.Rk3229GetKey(0, 6) > 0; } }//数彩票
	[Category("Key0")] public bool Key07 { get { return LibWGM.Rk3229GetKey(0, 7) > 0; } }//玩家

	//玩家按键
	[Category("Key1")] public bool Key10 { get { return LibWGM.Rk3229GetKey(1, 0) > 0; } }//切换
	[Category("Key1")] public bool Key11 { get { return LibWGM.Rk3229GetKey(1, 1) > 0; } }//射击
	[Category("Key1")] public bool Key12 { get { return LibWGM.Rk3229GetKey(1, 2) > 0; } }//预留
	[Category("Key1")] public bool Key13 { get { return LibWGM.Rk3229GetKey(1, 3) > 0; } }//开始
	[Category("Key1")] public bool Key14 { get { return LibWGM.Rk3229GetKey(1, 4) > 0; } }//数币
	[Category("Key1")] public bool Key15 { get { return LibWGM.Rk3229GetKey(1, 5) > 0; } }//投币
	[Category("Key1")] public bool Key16 { get { return LibWGM.Rk3229GetKey(1, 6) > 0; } }//数彩票
	[Category("Key1")] public bool Key17 { get { return LibWGM.Rk3229GetKey(1, 7) > 0; } }//玩家

    //安卓板最底部的 六个按键
    [Category("KeyGlobal")] public bool KeyGlobal0 { get { return LibWGM.GetGlobalKey(0); } }//取消
    [Category("KeyGlobal")] public bool KeyGlobal1 { get { return LibWGM.GetGlobalKey(1); } }//上移
    [Category("KeyGlobal")] public bool KeyGlobal2 { get { return LibWGM.GetGlobalKey(2); } }//下移
    [Category("KeyGlobal")] public bool KeyGlobal3 { get { return LibWGM.GetGlobalKey(3); } }//左移
    [Category("KeyGlobal")] public bool KeyGlobal4 { get { return LibWGM.GetGlobalKey(4); } }//右移
    [Category("KeyGlobal")] public bool KeyGlobal5 { get { return LibWGM.GetGlobalKey(5); } }//确认


}
