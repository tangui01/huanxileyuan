using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Bind Some Delegate Func For Yours.
/// </summary>
public class TTUIBind
{
    static bool isBind = false;

    public static void Bind()
    {
        if (!isBind)
        {
            isBind = true;
            //Debug.LogWarning("Bind For UI Framework.");

            //bind for your loader api to load UI.
            TTUIPage.delegateSyncLoadUI = LoadUI;
			//TTUIPage.delegateAsyncLoadUI = UILoader.Load;
        }
    }

	public static Object LoadUI(string path)
	{
		GameObject go = GameObject.Find("View1");
		string fullPath = "UI/Global/Resources/" + path;
        Transform tran = go.transform.Find(fullPath);
        if(tran != null) {
            return tran.gameObject;
        } else {
			throw new System.Exception("not found " + path);
        }
	}
}