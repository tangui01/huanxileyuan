using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using WGM;

public class LibUnityPlugin
{

    public static Action<bool, string> onUDiskChange;
    /// <summary>
    /// 0--270  1--0  2--90 3--180;
    /// </summary>

    public static float[] angles = new float[] { 0, 90, 180, 270 };
    public static string GetRotationCmd(float angle)
    {
        string cmd = $"setprop persist.sys.rotation  {angle}\nmount -o rw, remount /system\nbusybox sed -i \"s/persist.sys.rotation=[0-9]*/persist.sys.rotation={angle}/g\" /system/build.prop\nsync";
        Debug.Log($"旋转屏幕到{cmd}");
        byte[] array = System.Text.Encoding.ASCII.GetBytes(cmd);
        cmd = System.Text.Encoding.ASCII.GetString(array);
        Debug.Log(cmd);
        return cmd;
    }


    public static void Play(string path, int x, int y, int width, int height)
    {
        LibUnityPlugin.RK3229PlayVideo($"videoplay:///{path}", x, y, width, height);
    }






    static LibUnityPlugin()
    {
        PluginsInit();
    }

    public static bool Exists(string path)
    {
        return IsAssetExists(path);
    }

    public static byte[] ReadAllBytes(string path)
    {
        return GetAssetBytes(path);
    }

    public static string ReadAllText(string path)
    {
        return GetAssetString(path);
    }

    public static bool ExecuteShellCmd(string cmd)
    {
        return ExecShellCmd(cmd);
    }

    public static void InstallApp(string path)
    {
        InstallAppFromPath(path);
    }

    public static void RestartApp()
    {
        RestartApplication();
    }

    public static void SimulateTap(int x, int y)
    {
        SimulateTapScreen(x, y);
    }

    public static void ClearApplicationData()
    {
        ClearAllData();
    }

    public static bool IsUDiskExit(string path)
    {
        return IsUDiskMounted(path);
    }

    public static void OpenLauncher()
    {
        Debug.Log("OpenLauncher");
        A33OpenLauncher();
    }
    public static void OpenWifiNet()
    {
        OpenWIFI();
    }
    public static void OpenSettingPanel()
    {
        OpenSetting();
    }
    /// <summary>
    /// 旋转屏幕 0--270  1--0  2--90 3--180;
    /// </summary>
    public static void RodationSrceen(int index)
    {
        //return;
        if (Rotation(angles[index]))
            Debug.Log($"旋转屏幕到{angles[index]} 成功");
        else
            Debug.Log($"旋转屏幕到{angles[index]} 失败");
        // DelayReboot();
    }

    static async void DelayReboot()
    {
        await new WaitForSeconds(5.0f);
        LibUnityPlugin.Reboot();
    }

    public static void InstallApk(string path)
    {
        //path: "/storage/usbhost1/test.apk" U盘根目录下的test.apk
        A33InstallApk(path);
    }

    public static void Reboot()
    {
        Debug.Log("Reboot");
        ExecShellCmd("reboot");
        //A33Reboot();
    }



    public static void SetDateTime(long milliseconds)
    {
        SetTime(milliseconds);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    static AndroidJavaObject javaCall;

	static void PluginsInit()
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        javaCall = new AndroidJavaObject("com.arslly.main.UnityPlugin");
        javaCall.CallStatic("PluginsInit",currentActivity);
		RegistCallback();
	}

	static bool IsAssetExists(string path)
	{
		path = path.Remove(0, Application.streamingAssetsPath.Length + 1);
		return javaCall.Call<bool>("IsAssetExists", path);
	}

	public static byte[] GetAssetBytes(string path)
	{
		path = path.Remove(0, Application.streamingAssetsPath.Length + 1);
		return javaCall.Call<byte[]>("GetAssetBytes", path);
	}

	static string GetAssetString(string path)
	{
		path = path.Remove(0, Application.streamingAssetsPath.Length + 1);
		return javaCall.Call<string>("GetAssetString", path);
	}

	static bool ExecShellCmd(string cmd)
	{
		return javaCall.Call<bool>("ExecShellCmd", cmd);
	}

	static void InstallAppFromPath(string path)
	{
		javaCall.Call("InstallApk", path);
	}

	static void RestartApplication()
	{
		javaCall.Call("RestartApplication");
	}

	static void SimulateTapScreen(int x, int y)
	{
		javaCall.Call("SimulateTap", x, y);
	}

	static void ClearAllData()
	{
		javaCall.Call("ClearAllData");
	}

	static bool IsUDiskMounted(string path)
	{
		return javaCall.Call<bool>("IsUDiskMounted", path);
	}

	static void RegistCallback()
	{
		javaCall.Call("RegistUnityPluginCallback", new UnityPluginCallback());
	}

	class UnityPluginCallback : AndroidJavaProxy
    {
        public UnityPluginCallback() : base("com.arslly.main.UnityPluginCallback") { }

        public void OnUDiskChange(bool mount, string path) {
			Debug.Log("UnityPluginCallback.OnUDiskChange = " + mount + " path = " + path);
			if(onUDiskChange != null) onUDiskChange(mount, path);
        }
    }

	static void A33OpenLauncher()
	{
		javaCall.Call("A33OpenLauncher");
	}
     static void OpenWIFI()
    {
        ExecShellCmd("am start com.android.settings/.wifi.WifiSettings");
    }
    static void OpenSetting()
    {
       ExecShellCmd("am start -n com.android.settings/.Settings");
    }
    static bool Rotation(float angle)
    {
        return ExecShellCmd(GetRotationCmd (angle));
    }
	static void A33InstallApk(string path)
	{
		javaCall.Call("A33InstallApk", path);
	}

	static void A33Reboot()
	{
		javaCall.Call("A33Reboot");
	}

    
    static void RK3229PlayVideo(string path, int x, int y, int width, int height)
    {
        javaCall.Call("RK3229PlayVideo", path, x, y, width, height);
    }

	static void SetTime(long milliseconds)
	{
		DateTime timeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		long ticks = timeStart.Ticks + milliseconds * 10000;
		DateTime dt = new DateTime(ticks);
		string time = dt.ToString("yyyyMMdd.HHmmss");
		LibUnityPlugin.ExecuteShellCmd("date -s " + time);
		LibUnityPlugin.ExecuteShellCmd("clock -w");
	}
#else
    static void PluginsInit()
    {

    }

    static bool IsAssetExists(string path)
    {
        return File.Exists(path);
    }

    public static byte[] GetAssetBytes(string path)
    {
        return File.ReadAllBytes(path);
    }

    static string GetAssetString(string path)
    {
        return File.ReadAllText(path);
    }

    static bool ExecShellCmd(string cmd)
    {
        return false;
    }

    static void InstallAppFromPath(string path)
    {
        Application.OpenURL(path);
        Application.Quit();
    }

    static void RestartApplication()
    {
        Application.Quit();
        Application.OpenURL(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
    }

    static void SimulateTapScreen(int x, int y)
    {

    }

    static void ClearAllData()
    {

    }

    static bool IsUDiskMounted(string path)
    {
        return false;
    }

    static void A33OpenLauncher()
    {

    }
    static void OpenWIFI()
    {

    }

    static void OpenSetting()
    {

    }
    static bool Rotation(float angle)
    {
        GetRotationCmd(angle);
        return true;
    }
    static void A33InstallApk(string path)
    {

    }

    static void A33Reboot()
    {

    }

    static void RK3229PlayVideo(string path, int x, int y, int width, int height)
    {

    }



    static void SetTime(long milliseconds)
    {
        DateTime timeStart = new DateTime(1970, 1, 1, 0, 0, 0);
        long ticks = timeStart.Ticks + milliseconds * 10000;
        DateTime dt = new DateTime(ticks);
        string time = dt.ToString("yyyyMMdd.HHmmss");
        if (LibWGM.machine.DeveloperMode)
            Debug.Log(time);
    }
#endif
}
