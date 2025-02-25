//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright ?2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using System;
using UnityEngine;

/// <summary>
/// Very basic script that will activate or deactivate an object (and all of its children) when clicked.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
    public GameObject target;
    public bool state = true;
    public Action<bool> onChange;

    void OnClick()
    {
        if(target != null)
            NGUITools.SetActive(target, state);
        onChange?.Invoke(state);
    }
}