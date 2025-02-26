using UnityEngine;
using System.Collections;

public class TweenAround : UITweener {

	public float from;
	public float to;
	public float radius;

	

	public bool lookAt = false;

	[HideInInspector]
	public bool worldSpace = false;

	Transform mTrans;
	UIRect mRect;

	public Transform cachedTransform { get { if(mTrans == null) mTrans = transform; return mTrans; } }

	float mLocalAngle = 0;
	float mStartAngle = 0;

	Vector3 mCenter = Vector3.zero;
	public Vector3 center
	{
		get { return mCenter; }
		set
		{
			mCenter = value;
			if(worldSpace) {
				Vector3 dirVec = cachedTransform.position - center;
				mStartAngle = Vector3.Angle(dirVec, Vector3.up);
				mStartAngle = Vector3.Cross(dirVec, Vector3.up).z < 0 ? -mStartAngle : mStartAngle;
				radius = Vector3.Distance(center, cachedTransform.position);
				mLocalAngle = cachedTransform.eulerAngles.z;
			} else {
				Vector3 dirVec = cachedTransform.localPosition - center;
				mStartAngle = Vector3.Angle(dirVec, Vector3.up);
				mStartAngle = Vector3.Cross(dirVec, Vector3.up).z < 0 ? -mStartAngle : mStartAngle;
				radius = Vector3.Distance(center, cachedTransform.localPosition);
				mLocalAngle = cachedTransform.localEulerAngles.z;
			}
		}
	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Vector3 value
	{
		get
		{
			return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
		}
		set
		{
			if(mRect == null || !mRect.isAnchored || worldSpace) {
				if(worldSpace) cachedTransform.position = value;
				else cachedTransform.localPosition = value;
			} else {
				value -= cachedTransform.localPosition;
				NGUIMath.MoveRect(mRect, value.x, value.y);
			}
		}
	}

	public Vector3 angle
	{
		get
		{
			return worldSpace ? cachedTransform.eulerAngles : cachedTransform.localEulerAngles;
		}

		set
		{
			if(worldSpace) {
				cachedTransform.eulerAngles = value;
			} else {
				cachedTransform.localEulerAngles = value;
			}
		}
	}

	void Awake() { mRect = GetComponent<UIRect>(); }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished)
	{
		float a = from * (1f - factor) + to * factor + mStartAngle;

		value = new Vector3(-Mathf.Sin(a * Mathf.Deg2Rad) * radius, Mathf.Cos(a * Mathf.Deg2Rad) * radius, 0) + center;
		
		if(lookAt) {
			angle = new Vector3(0, 0, a + mLocalAngle - mStartAngle);
		}
	}

	protected override void OnDisable()
	{
		if(style == Style.Once || style == Style.InOut) {
			enabled = false;
		}
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAround Begin(GameObject go, float duration, float angle)
	{
		TweenAround comp = UITweener.Begin<TweenAround>(go, duration);
		comp.from = 0;
		comp.to = angle;

		if(duration <= 0f) {
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	static public TweenAround Begin(GameObject go, float duration, float angle, bool worldSpace, Vector3 center)
	{
		TweenAround comp = UITweener.Begin<TweenAround>(go, duration);
		comp.worldSpace = worldSpace;
		comp.from = 0;
		comp.to = angle;
		comp.center = center;

		if(duration <= 0f) {
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAround Begin(GameObject go, float duration, float angle, bool worldSpace)
	{
		TweenAround comp = UITweener.Begin<TweenAround>(go, duration);
		comp.worldSpace = worldSpace;
		comp.from = 0;
		comp.to = angle;

		if(duration <= 0f) {
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
