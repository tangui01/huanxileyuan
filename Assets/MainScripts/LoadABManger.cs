using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/****************************************************
    文件：LoadABManger.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：加载ab包的游戏场景资源
*****************************************************/
public class LoadABManger : SerializedMonoBehaviour
{
     public static LoadABManger  Instance;
     private Action cb;
     private void Awake()
     {
         if (Instance == null)
         {
             Instance = this;
         }
         else
         {
             Destroy(gameObject);
         }
     }
     [DictionaryDrawerSettings]public Dictionary<string,AssetBundle> abDic=new Dictionary<string, AssetBundle>();
     

     public void LoadAB(string sceneName,Action complete=null)
     {
         AssetBundleCreateRequest ab = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/games/" + sceneName.ToLower());
         CommonUI.instance.SetLoadingPanel(true);
         cb = () =>
         {
             float process=ab.progress;
             CommonUI.instance.Loading.SetProcessText(process);
             if (process == 1)
             {
                 SceneLoadManager.instance.StartGameLoadScene(sceneName);
                 SceneLoadManager.instance.ExitSceneACtion += complete;
                 cb = null;
                 abDic.Add(sceneName, ab.assetBundle);
             }
         };
     }

     IEnumerator OnloadAb(string sceneName)
     {
         UnityWebRequest url = UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/games/" + sceneName.ToLower());
         yield return url.SendWebRequest();
         AssetBundle ab = DownloadHandlerAssetBundle.GetContent(url);
         if (ab != null)
         {
             abDic.Add(sceneName, ab);
             SceneLoadManager.instance.LoadSceneAsync(sceneName);
         }
         else
         {
             Debug.LogError("加载场景的路径不正确");
         }
     }

     public void UnloadAB(string sceneName)
     {
         if (abDic.ContainsKey(sceneName))
         {
             abDic[sceneName].Unload(true);
             abDic.Remove(sceneName);
         }
         else
         {
             Debug.LogError("场景名字不存在或者已经被移除");
         }
     }

     private void Update()
     {
         if (cb!=null)
         {
             cb();
         }
     }
}
