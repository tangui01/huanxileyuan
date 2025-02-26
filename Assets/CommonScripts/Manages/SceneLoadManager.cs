using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WGM;

/****************************************************
    文件：SceneLoadManager.cs
    作者：tg
    邮箱: 1075670319@qq.com
    日期：#CreateTime#
    功能：场景加载管理器
*****************************************************/
public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;
    public Action cb;
    public Action ExitSceneACtion;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartGameLoadScene(string sceneName)
    {
        AsyncOperation ac = SceneManager.LoadSceneAsync(sceneName);
        cb = () =>
        {
            float a = ac.progress;
            if (a == 1)
            {
                CommonUI.instance.SetLoadingPanel(false);
                if (ExitSceneACtion != null)
                {
                    ExitSceneACtion();
                }

                cb = null;
                ExitSceneACtion = null;
            }
        };
    }

    public void LoadSceneAsync(string sceneName)
    {
        AsyncOperation ac = SceneManager.LoadSceneAsync(sceneName);
        CommonUI.instance.SetLoadingPanel(true);
        cb = () =>
        {
            float a = ac.progress;
            CommonUI.instance.Loading.SetProcessText(a);
            if (a == 1)
            {
                CommonUI.instance.SetLoadingPanel(false);
                if (ExitSceneACtion != null)
                {
                    ExitSceneACtion();
                }

                cb = null;
                ExitSceneACtion = null;
            }
        };
    }

    public void LoadSceneAsync(int sceneindex)
    {
        AsyncOperation ac = SceneManager.LoadSceneAsync(sceneindex);
        CommonUI.instance.SetLoadingPanel(true);
        cb = () =>
        {
            float a = ac.progress;
            CommonUI.instance.Loading.SetProcessText(a);
            if (a == 1)
            {
                CommonUI.instance.SetLoadingPanel(false);
                if (ExitSceneACtion != null)
                {
                    ExitSceneACtion();
                }

                cb = null;
            }
        };
    }
    
    /// <summary>
    /// 返回游戏大厅(通过续币失败)
    /// </summary>
    public void BackMainGameByCoin_in()
    {
        LoadABManger.Instance.UnloadAB(SceneManager.GetActiveScene().name);
        LoadABManger.Instance.LoadAB(MainConstant.MainSceneName);
        ExitSceneACtion = BackMainGameByCoin_in_Complted;
    }

    private void BackMainGameByCoin_in_Complted()
    {
        Time.timeScale = 1;
        CommonUI.instance.ExitCouterColdDown();
        GameTimeManager.instance.ClearAction();
    }
    /// <summary>
    /// 获取当前场景是不是游戏大厅
    /// </summary>
    /// <returns></returns>
    public bool GetCurrentScneISMainScene()
    {
        return SceneManager.GetActiveScene().name == MainConstant.MainSceneName;
    }
    private void Update()
    {
        if (cb != null)
        {
            cb();
        }
    }
}