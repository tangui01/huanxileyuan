using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static Action<int> onCoinChange;
    public static Action<Camera> onEnterOtherCamera;
    public static Action<Camera> onExitOtherCamera;
    public static Action onMachineInit;
    public static Action<bool> onEnterGame;
    public static Action onVedioDownloaded;
    public static Action onVolumeChange;
    public static Action onGameOver;
    public static Action onTimeOut;
    public static Action onPlayerFullSkill;
    public static Action<int> onPathEvt;
    public static Action<Vector3> onShowKil;
    public static Action onClearAll;
    public static Action onTriggerFeatureFilmEnter;
    public static Action onTriggerFeatureFilmExit;
    public static Action<int> onPlayerOver;
    public static Action<int> onPlayerPrizeNull;
    public static Action<int> onPrizeTimeOut;
    public static Action<int> onSkill;
    // Use this for initialization

}
