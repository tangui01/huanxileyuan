using UnityEngine;
using UnityEditor;
using System.Collections;

public class DebugHack : MonoBehaviour {

	//[MenuItem("WGM/Debug/EnableDebug")]
	//public static void EnableDebug()
	//{
	//	Enemy.debugEnable = true;
	//}

	//[MenuItem("WGM/Debug/DisableDebug")]
	//public static void DisableDebug()
	//{
	//	Enemy.debugEnable = false;
	//}

	[MenuItem("WGM/Debug/SpeedUp")]
	public static void SpeedUp()
	{
		Time.timeScale *= 1.5f;
	}

	[MenuItem("WGM/Debug/SpeedNormal")]
	public static void SpeedNormal()
	{
		Time.timeScale = 1.0f;
	}
}
