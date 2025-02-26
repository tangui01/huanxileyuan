using System;
using UnityEngine;

public enum AudioType
{
    Button,
    BallBounce,
    ObstacleDestroy,
    Die,
    Win
}

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
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
    [Space(10)]
    [SerializeField] AudioClip _bgMusicClip;

    [Header("Sound Effect Clip :")]
    [SerializeField] AudioClip _ballBounceClip;
    [SerializeField] AudioClip _obstacleDestroy;
    [SerializeField] AudioClip _dieClip;
    [SerializeField] AudioClip _winClip;
    

    #region UNITY_METHOD
    private void Start()
    {
       AudioManager.Instance.playerBGm(_bgMusicClip);
    }
    #endregion
    #region PUBLIC_METHOD

    public AudioClip GetClip(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.ObstacleDestroy:
                return _obstacleDestroy;
            case AudioType.BallBounce:
                return _ballBounceClip;
            case AudioType.Die:
                return _dieClip;
            case AudioType.Win:
                return _winClip;
            default:
                return null;
        }
    }

    public void Destroy() {
        Destroy(gameObject);
    }
    #endregion
}