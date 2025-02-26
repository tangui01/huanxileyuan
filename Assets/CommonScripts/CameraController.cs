using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Main camera needs to always follow the player's snake. We do the following in this class. We also update camera's FOV (or projection size) based on different criterias.
/// </summary>
public class CameraController : MonoBehaviour
{
     public static  CameraController instance;
     public CinemachineVirtualCamera vcam;
     public CinemachineBasicMultiChannelPerlin noiseProfile;
     private void Awake()
     {
          instance = this;
     }

     private void Start()
     {
          noiseProfile=vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
     }

     public void GameOver(float delay)
     {
          ChangeCameraSize(14,delay);
     }
     /// <summary>
     /// 摄像机振动
     /// </summary>
     /// <param name="duration">持续时间</param>
     /// <param name="amplitude">幅度</param>
     /// <param name="frequency">频率</param>
     public void ShakeCamera(float duration=0.25f, float amplitude=2, float frequency = 1)
     {
          if (noiseProfile != null)
          {
               noiseProfile.m_AmplitudeGain = amplitude;
               noiseProfile.m_FrequencyGain = frequency;
               Invoke(nameof(StopShaking), duration);
          }
     }
     //道具望远镜效果
     [Button]
     public void UnZoom()
     {
          ChangeCameraSize(16,1f);
     }
     [Button]
     public void ExitUnZoom()
     {
          ChangeCameraSize(10,1f);
     }
/// <summary>
/// 改变摄像机视野
/// </summary>
/// <returns></returns>
     private void ChangeCameraSize(float targetSize,float timer)
     {
          // 使用DOTween.To方法来改变myValue的值
          Tween tween = DOTween.To(() => vcam.m_Lens.OrthographicSize, x => vcam.m_Lens.OrthographicSize = x, targetSize, timer);
     }

     private void StopShaking()
     {
          if (noiseProfile != null)
          {
               noiseProfile.m_AmplitudeGain = 0;
               noiseProfile.m_FrequencyGain = 0;
          }
     }
}