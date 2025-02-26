using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CustomDefine
{
	huacai,
}

public enum ServerSelect
{
	Normal,
	TestSer,
	Local,
}

public class BuildSceneBundle : MonoBehaviour {

	public static string apiUrlBase =
#if huacai
	"https://www.huacaizn.com/kidstoy/";
#else
	"http://localhost:5030/";
#endif

	public static string assetsUrlBase =
#if huacai
	"https://www.huacaizn.com/";
#else
	"http://localhost:5030/";
#endif

	public CustomDefine custom;
	public bool buildDebug = false;
	public ServerSelect buildServerSelect = ServerSelect.Normal;
}
