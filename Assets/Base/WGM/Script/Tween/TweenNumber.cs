using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UILabel))]
public class TweenNumber : UITweener {

	public float from;
	public float to;

	public Action<float> onUpdate;

	public Action<float> onReach;
	public List<float> reachNumber = new List<float>();

	public enum DisplayType
	{
		Int,
		Float,
		TimeSecond,
		TimeMinute,
	}
	public DisplayType display = DisplayType.Int;

	float mSpeed = 0;
	public float speed
	{
		get { return mSpeed; }
		set { mSpeed = value; duration = Mathf.Min(Mathf.Abs(to - from) / mSpeed, durationMaxOnSpeed); }
	}

	public float durationMaxOnSpeed = float.MaxValue;

	float mNumber = 0;
	float mPreNumber = 0;

	Transform mTrans;
	UILabel mLabel;


	public Transform cachedTransform { get { if(mTrans == null) mTrans = transform; return mTrans; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value { get { return mNumber; }
		set { mNumber = value;
		if(mLabel != null) {
			switch(display) {
				case DisplayType.Int:
					mLabel.text = ((int)mNumber).ToString();
					break;
				case DisplayType.Float:
					mLabel.text = mNumber.ToString("f2");
					break;
				case DisplayType.TimeSecond:
					mLabel.text = mNumber.ToString("f2");
					break;
				case DisplayType.TimeMinute:
					mLabel.text = ((int)mNumber/60).ToString() + "." + ((int)mNumber%60).ToString("D2");
					break;
				default:
					mLabel.text = mNumber.ToString();
					break;
			}
		}
		}
	}

	void Awake() { mLabel = GetComponent<UILabel>(); }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = (from * (1f - factor) + to * factor);

		if(reachNumber != null && onReach != null) {
			for(int i = 0; i < reachNumber.Count; i++) {
				if((Mathf.Abs(to - value) <= Mathf.Abs(to - reachNumber[i])) &&
				   (Mathf.Abs(to - mPreNumber) > Mathf.Abs(to - reachNumber[i])))
				{
					value = reachNumber[i];
					factor = value / (to - from);
					onReach(value);
					break;
				}
			}
		}

		if(onUpdate != null) onUpdate(value);

		mPreNumber = value;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenNumber Begin(GameObject go, float duration, float from, float to)
	{
		TweenNumber comp = UITweener.Begin<TweenNumber>(go, duration);
		comp.from = from;
		comp.to = to;

		if(duration <= 0f) {
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
