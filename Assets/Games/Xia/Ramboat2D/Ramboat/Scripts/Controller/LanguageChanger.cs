using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageChanger : MonoBehaviour
{
    [Header("中文")] 
    public Vector3 zh_Scale ;
    [Header("英文")] 
    public Vector3 en_Scale ;
    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLanguageChanged;
    }

    private void Start()
    {
        OnLanguageChanged(null);
    }

    private void OnLanguageChanged(UnityEngine.Localization.Locale locale)
    {
        string currentLanguageCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        switch (currentLanguageCode)
        {
            case "en": // 美式英语
                transform.localScale = en_Scale;
                break;
            case "zh-Hans": // 简体中文
                transform.localScale = zh_Scale;
                break;
            default:
                transform.localScale = Vector3.one;
                break;
        }
    }
}