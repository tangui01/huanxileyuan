using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace ninjaGame
{
	public class CameraController : MonoBehaviour
	{
		public static CameraController instance;

		private void Awake()
		{
			if (instance==null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void CameraShakeByDead()
		{
			transform.DOShakePosition (0.2f,new Vector3(1,1,0)); // 平面进行一个震动 只改变x,y没有改变z (值是一个震动的强度)
		}
		public void CameraShakeByKill()
		{
			transform.DOShakePosition (0.1f,new Vector3(0.25f,0.25f,0)); // 平面进行一个震动 只改变x,y没有改变z (值是一个震动的强度)
		}
	}
}


