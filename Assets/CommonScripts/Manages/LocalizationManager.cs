using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Settings;
/****************************************************
    文件：LocalizationManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：多语言本地管理器
*****************************************************/
public enum Language
{
    Chinese,
    Chinese_Traditional,
    English
}

public class LocalizationManager : MonoBehaviour
{
    private bool Active=false;
    public static LocalizationManager Instance;
    private Language currentLanguage = Language.Chinese;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SwitchLanguage(int localId)
    {
        if (Active)
        {
            return;
        }

        if (localId==1)
        {
            localId = 0;
        }
        StartCoroutine(SetLocale(localId));
    }

    IEnumerator SetLocale(int _localeId)
    {
        Active=true;
        currentLanguage=(Language)_localeId;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeId];
        MainGameSelectView.Instance?.Updatelanguage();
        Active=false;
    }

    public Language GetCurrentLanguage()
    {
        return currentLanguage;
    }
}
