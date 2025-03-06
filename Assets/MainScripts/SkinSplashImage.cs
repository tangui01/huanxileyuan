using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

/****************************************************
    文件：Test.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
[Preserve]
public class SkinSplashImage 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Run()
    {
        Task.Run(() =>
        {
            SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
        });
    }
}