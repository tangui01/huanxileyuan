//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for all tweening operations.
/// </summary>

public abstract class UITweener : MonoBehaviour
{
	/// <summary>
	/// Current tween that triggered the callback function.
	/// </summary>

	static public UITweener current;

	[DoNotObfuscateNGUI] public enum Method
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut,
		BounceIn,
		BounceOut,
		easeInQuad,
		easeOutQuad,
		easeInOutQuad,
		easeInCubic,
		easeOutCubic,
		easeInOutCubic,
		easeInQuart,
		easeOutQuart,
		easeInOutQuart,
		easeInQuint,
		easeOutQuint,
		easeInOutQuint,
		easeInSine,
		easeOutSine,
		easeInOutSine,
		easeInExpo,
		easeOutExpo,
		easeInOutExpo,
		easeInCirc,
		easeOutCirc,
		easeInOutCirc,
		linear,
		spring,
		/* GFX47 MOD START */
		//bounce,
		easeInBounce,
		easeOutBounce,
		easeInOutBounce,
		/* GFX47 MOD END */
		easeInBack,
		easeOutBack,
		easeInOutBack,
		/* GFX47 MOD START */
		//elastic,
		easeInElastic,
		easeOutElastic,
		easeInOutElastic,
		/* GFX47 MOD END */
		punch,
		/* WGM */
		easeOutBackQuart,
	}

	[DoNotObfuscateNGUI] public enum Style
	{
		Once,
		Loop,
		PingPong,
		InOut,
	}

	/// <summary>
	/// Tweening method used.
	/// </summary>

	[HideInInspector]
	public Method method = Method.Linear;

	/// <summary>
	/// Dose it use animation curve or the real value.
	/// </summary>

	[HideInInspector]
	public bool useCurve = true;

	/// <summary>
	/// Does it play once? Does it loop?
	/// </summary>

	[HideInInspector]
	public Style style = Style.Once;

	/// <summary>
	/// Optional curve to apply to the tween's time factor value.
	/// </summary>

	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

	/// <summary>
	/// Whether the tween will ignore the timescale, making it work while the game is paused.
	/// </summary>
	
	[HideInInspector]
	public bool ignoreTimeScale = false;

	/// <summary>
	/// How long will the tweener wait before starting the tween?
	/// </summary>

	[HideInInspector]
	public float delay = 0f;

	/// <summary>
	/// How long is the duration of the tween?
	/// </summary>

	[HideInInspector]
	public float duration = 1f;

	/// <summary>
	/// Whether the tweener will use steeper curves for ease in / out style interpolation.
	/// </summary>

	[HideInInspector]
	public bool steeperCurves = false;

	/// <summary>
	/// Used by buttons and tween sequences. Group of '0' means not in a sequence.
	/// </summary>

	[HideInInspector]
	public int tweenGroup = 0;

	[HideInInspector]
	public float loopDelay = 0;

	/// <summary>
	/// Tween On LateUpdate
	/// </summary>
	/// 
	[HideInInspector]
	public bool lateupdate = false;

	[Tooltip("By default, Update() will be used for tweening. Setting this to 'true' will make the tween happen in FixedUpdate() insted.")]
	public bool useFixedUpdate = false;

	/// <summary>
	/// Event delegates called when the animation finishes.
	/// </summary>

	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Deprecated functionality, kept for backwards compatibility
	[HideInInspector] public GameObject eventReceiver;
	[HideInInspector] public string callWhenFinished;

	/// <summary>
	/// Custom time scale for this tween, if desired. Can be used to slow down or speed up the animation.
	/// </summary>

	[System.NonSerialized] public float timeScale = 1f;

	bool mStarted = false;
	protected float mStartTime = 0f;
	float mDuration = 0f;
	float mAmountPerDelta = 1000f;
	float mFactor = 0f;
	bool mInOutFirst = false;

	/// <summary>
	/// Amount advanced per delta time.
	/// </summary>

	public float amountPerDelta
	{
		get
		{
			if (duration == 0f) return 1000f;

			if (mDuration != duration)
			{
				mDuration = duration;
				mAmountPerDelta = Mathf.Abs(1f / duration) * Mathf.Sign(mAmountPerDelta);
			}
			return mAmountPerDelta;
		}
	}

	/// <summary>
	/// Tween factor, 0-1 range.
	/// </summary>

	public float tweenFactor { get { return mFactor; } set { mFactor = Mathf.Clamp01(value); } }

	/// <summary>
	/// Direction that the tween is currently playing in.
	/// </summary>

	public AnimationOrTween.Direction direction { get { return amountPerDelta < 0f ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward; } }

	/// <summary>
	/// This function is called by Unity when you add a component. Automatically set the starting values for convenience.
	/// </summary>

	void Reset ()
	{
		if (!mStarted)
		{
			SetStartToCurrentValue();
			SetEndToCurrentValue();
		}
	}

	/// <summary>
	/// Update as soon as it's started so that there is no delay.
	/// </summary>

	protected virtual void Start () { DoUpdate(); }
	protected void Update () { if (!useFixedUpdate) DoUpdate(); }
	protected void FixedUpdate () { if (useFixedUpdate) DoUpdate(); }
	protected virtual void LateUpdate() { if(lateupdate) DoUpdate(); }

	/// <summary>
	/// Update the tweening factor and call the virtual update function.
	/// </summary>

	protected void DoUpdate ()
	{
		float delta = ignoreTimeScale && !useFixedUpdate ? Time.unscaledDeltaTime : Time.deltaTime;
		float time = ignoreTimeScale && !useFixedUpdate ? Time.unscaledTime : Time.time;

		if (!mStarted)
		{
			delta = 0;
			mStarted = true;
			mInOutFirst = false;
			mStartTime = time + delay;
		}

		if (time < mStartTime) return;

		// Advance the sampling factor
		mFactor += (duration == 0f) ? 1f : amountPerDelta * delta * timeScale;

		// Loop style simply resets the play factor after it exceeds 1.
		if (style == Style.Loop)
		{
			if (mFactor > 1f)
			{
				mFactor -= Mathf.Floor(mFactor);
				mStartTime = time + loopDelay;
			}
		}
		else if (style == Style.PingPong)
		{
			// Ping-pong style reverses the direction
			if (mFactor > 1f)
			{
				mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
				mAmountPerDelta = -mAmountPerDelta;
				mStartTime = time + loopDelay;
			}
			else if (mFactor < 0f)
			{
				mFactor = -mFactor;
				mFactor -= Mathf.Floor(mFactor);
				mAmountPerDelta = -mAmountPerDelta;
				mStartTime = time + loopDelay;
			}
		} else if(style == Style.InOut) {
			if(mFactor > 1f) {
				if(mInOutFirst == false) {
					mInOutFirst = true;
					mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
					mAmountPerDelta = -mAmountPerDelta;
				}
			}
		}

		// If the factor goes out of range and this is a one-time tweening operation, disable the script
		if ((style == Style.Once || style == Style.InOut) && (duration == 0f || mFactor > 1f || mFactor < 0f))
		{
			mFactor = Mathf.Clamp01(mFactor);
			Sample(mFactor, true);
			enabled = false;

			if (current != this)
			{
				UITweener before = current;
				current = this;

				if (onFinished != null)
				{
					mTemp = onFinished;
					onFinished = new List<EventDelegate>();

					// Notify the listener delegates
					EventDelegate.Execute(mTemp);

					// Re-add the previous persistent delegates
					for (int i = 0; i < mTemp.Count; ++i)
					{
						EventDelegate ed = mTemp[i];
						if (ed != null && !ed.oneShot) EventDelegate.Add(onFinished, ed, ed.oneShot);
					}
					mTemp = null;
				}

				// Deprecated legacy functionality support
				if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
					eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);

				current = before;
			}
		}
		else Sample(mFactor, false);
	}

	List<EventDelegate> mTemp = null;

	/// <summary>
	/// Convenience function -- set a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void SetOnFinished (EventDelegate.Callback del) { EventDelegate.Set(onFinished, del); }

	/// <summary>
	/// Convenience function -- set a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void SetOnFinished (EventDelegate del) { EventDelegate.Set(onFinished, del); }

	/// <summary>
	/// Convenience function -- add a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void AddOnFinished (EventDelegate.Callback del) { EventDelegate.Add(onFinished, del); }

	/// <summary>
	/// Convenience function -- add a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
	/// </summary>

	public void AddOnFinished (EventDelegate del) { EventDelegate.Add(onFinished, del); }

	/// <summary>
	/// Remove an OnFinished delegate. Will work even while iterating through the list when the tweener has finished its operation.
	/// </summary>

	public void RemoveOnFinished (EventDelegate del)
	{
		if (onFinished != null) onFinished.Remove(del);
		if (mTemp != null) mTemp.Remove(del);
	}

	/// <summary>
	/// Mark as not started when finished to enable delay on next play.
	/// </summary>

	protected virtual void OnDisable () { mStarted = false; }

	/// <summary>
	/// Immediately finish the tween animation, if it's active.
	/// </summary>

	public void Finish ()
	{
		if (enabled)
		{
			Sample(mAmountPerDelta > 0f ? 1f : 0f, true);
			enabled = false;
		}
	}

	/// <summary>
	/// Sample the tween at the specified factor.
	/// </summary>

	public void Sample (float factor, bool isFinished)
	{
		// Calculate the sampling value
		float val = Mathf.Clamp01(factor);

		switch(method) {
			case Method.EaseIn:
				val = 1f - Mathf.Sin(0.5f * Mathf.PI * (1f - val));
				if(steeperCurves)
					val *= val;
				break;
			case Method.EaseOut:
				val = Mathf.Sin(0.5f * Mathf.PI * val);

				if(steeperCurves) {
					val = 1f - val;
					val = 1f - val * val;
				}
				break;
			case Method.EaseInOut:
				const float pi2 = Mathf.PI * 2f;
				val = val - Mathf.Sin(val * pi2) / pi2;

				if(steeperCurves) {
					val = val * 2f - 1f;
					float sign = Mathf.Sign(val);
					val = 1f - Mathf.Abs(val);
					val = 1f - val * val;
					val = sign * val * 0.5f + 0.5f;
				}
				break;
			case Method.BounceIn:
				val = BounceLogic(val);
				break;
			case Method.BounceOut:
				val = 1f - BounceLogic(1f - val);
				break;
			case Method.easeInQuad:
				val = easeInQuad(0, 1, val);
				break;
			case Method.easeOutQuad:
				val = easeOutQuad(0, 1, val);
				break;
			case Method.easeInOutQuad:
				val = easeInOutQuad(0, 1, val);
				break;
			case Method.easeInCubic:
				val = easeInCubic(0, 1, val);
				break;
			case Method.easeOutCubic:
				val = easeOutCubic(0, 1, val);
				break;
			case Method.easeInOutCubic:
				val = easeInOutCubic(0, 1, val);
				break;
			case Method.easeInQuart:
				val = easeInQuart(0, 1, val);
				break;
			case Method.easeOutQuart:
				val = easeOutQuart(0, 1, val);
				break;
			case Method.easeInOutQuart:
				val = easeInOutQuart(0, 1, val);
				break;
			case Method.easeInQuint:
				val = easeInQuint(0, 1, val);
				break;
			case Method.easeOutQuint:
				val = easeOutQuint(0, 1, val);
				break;
			case Method.easeInOutQuint:
				val = easeInOutQuint(0, 1, val);
				break;
			case Method.easeInSine:
				val = easeInSine(0, 1, val);
				break;
			case Method.easeOutSine:
				val = easeOutSine(0, 1, val);
				break;
			case Method.easeInOutSine:
				val = easeInOutSine(0, 1, val);
				break;
			case Method.easeInExpo:
				val = easeInExpo(0, 1, val);
				break;
			case Method.easeOutExpo:
				val = easeOutExpo(0, 1, val);
				break;
			case Method.easeInOutExpo:
				val = easeInOutExpo(0, 1, val);
				break;
			case Method.easeInCirc:
				val = easeInCirc(0, 1, val);
				break;
			case Method.easeOutCirc:
				val = easeOutCirc(0, 1, val);
				break;
			case Method.easeInOutCirc:
				val = easeInOutCirc(0, 1, val);
				break;
			case Method.linear:
				val = linear(0, 1, val);
				break;
			case Method.spring:
				val = spring(0, 1, val);
				break;
			case Method.easeInBounce:
				val = easeInBounce(0, 1, val);
				break;
			case Method.easeOutBounce:
				val = easeOutBounce(0, 1, val);
				break;
			case Method.easeInOutBounce:
				val = easeInOutBounce(0, 1, val);
				break;
			case Method.easeInBack:
				val = easeInBack(0, 1, val);
				break;
			case Method.easeOutBack:
				val = easeOutBack(0, 1, val);
				break;
			case Method.easeInOutBack:
				val = easeInOutBack(0, 1, val);
				break;
			case Method.easeInElastic:
				val = easeInElastic(0, 1, val);
				break;
			case Method.easeOutElastic:
				val = easeOutElastic(0, 1, val);
				break;
			case Method.easeInOutElastic:
				val = easeInOutElastic(0, 1, val);
				break;
			case Method.punch:
				val = punch(1, val);
				break;
			case Method.easeOutBackQuart:
				val = easeOutBackQuart(0, 1, val);
				break;
			default:
				break;
		}

		// Call the virtual update
		float v;
		if(useCurve) {
			v = (animationCurve != null) ? animationCurve.Evaluate(val) : val;
		} else {
			v = val;
		}
		OnUpdate(v, isFinished);
	}

	/// <summary>
	/// Main Bounce logic to simplify the Sample function
	/// </summary>
	
	float BounceLogic (float val)
	{
		if (val < 0.363636f) // 0.363636 = (1/ 2.75)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f) // 0.727272 = (2 / 2.75)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f; // 0.545454f = (1.5 / 2.75) 
		}
		else if (val < 0.909090f) // 0.909090 = (2.5 / 2.75) 
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f; // 0.818181 = (2.25 / 2.75) 
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f; // 0.9545454 = (2.625 / 2.75) 
		}
		return val;
	}

	/// <summary>
	/// Play the tween.
	/// </summary>

	[System.Obsolete("Use PlayForward() instead")]
	public void Play () { Play(true); }

	/// <summary>
	/// Play the tween forward.
	/// </summary>

	public void PlayForward () { Play(true); }

	/// <summary>
	/// Play the tween in reverse.
	/// </summary>
	
	public void PlayReverse () { Play(false); }

	/// <summary>
	/// Manually activate the tweening process, reversing it if necessary.
	/// </summary>

	public virtual void Play (bool forward)
	{
		mAmountPerDelta = Mathf.Abs(amountPerDelta);
		if (!forward) mAmountPerDelta = -mAmountPerDelta;

		if (!enabled)
		{
			enabled = true;
			mStarted = false;
		}

		DoUpdate();
	}

	public void Pause()
	{
		enabled = false;
	}

	/// <summary>
	/// Manually reset the tweener's state to the beginning.
	/// If the tween is playing forward, this means the tween's start.
	/// If the tween is playing in reverse, this means the tween's end.
	/// </summary>

	public void ResetToBeginning ()
	{
		mStarted = false;
		mFactor = (amountPerDelta < 0f) ? 1f : 0f;
		Sample(mFactor, false);
	}

	/// <summary>
	/// Manually start the tweening process, reversing its direction.
	/// </summary>

	public void Toggle ()
	{
		if (mFactor > 0f)
		{
			mAmountPerDelta = -amountPerDelta;
		}
		else
		{
			mAmountPerDelta = Mathf.Abs(amountPerDelta);
		}
		enabled = true;
	}

	/// <summary>
	/// Actual tweening logic should go here.
	/// </summary>

	abstract protected void OnUpdate (float factor, bool isFinished);

	/// <summary>
	/// Starts the tweening operation.
	/// </summary>

	static public T Begin<T> (GameObject go, float duration, float delay = 0f) where T : UITweener
	{
		T comp = go.GetComponent<T>();
#if UNITY_FLASH
		if ((object)comp == null) comp = (T)go.AddComponent<T>();
#else
		// Find the tween with an unset group ID (group ID of 0).
		if (comp != null && comp.tweenGroup != 0)
		{
			comp = null;
			T[] comps = go.GetComponents<T>();
			for (int i = 0, imax = comps.Length; i < imax; ++i)
			{
				comp = comps[i];
				if (comp != null && comp.tweenGroup == 0) break;
				comp = null;
			}
		}

		if (comp == null)
		{
			comp = go.AddComponent<T>();

			if (comp == null)
			{
				Debug.LogError("Unable to add " + typeof(T) + " to " + NGUITools.GetHierarchy(go), go);
				return null;
			}
		}
#endif
		comp.mStarted = false;
		comp.mFactor = 0f;
		comp.duration = duration;
		comp.mDuration = duration;
		comp.delay = delay;
		comp.mAmountPerDelta = duration > 0f ? Mathf.Abs(1f / duration) : 1000f;
		comp.style = Style.Once;
		comp.animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		comp.eventReceiver = null;
		comp.callWhenFinished = null;
		comp.onFinished.Clear();
		if (comp.mTemp != null) comp.mTemp.Clear();
		comp.enabled = true;
		return comp;
	}

	/// <summary>
	/// Set the 'from' value to the current one.
	/// </summary>

	public virtual void SetStartToCurrentValue () { }

	/// <summary>
	/// Set the 'to' value to the current one.
	/// </summary>

	public virtual void SetEndToCurrentValue () { }

	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	private float clerp(float start, float end, float value)
	{
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) * 0.5f);
		float retval = 0.0f;
		float diff = 0.0f;
		if((end - start) < -half) {
			diff = ((max - start) + end) * value;
			retval = start + diff;
		} else if((end - start) > half) {
			diff = -((max - end) + start) * value;
			retval = start + diff;
		} else
			retval = start + (end - start) * value;
		return retval;
	}

	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}

	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2) + start;
	}

	private float easeInOutQuad(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if(value < 1)
			return end * 0.5f * value * value + start;
		value--;
		return -end * 0.5f * (value * (value - 2) - 1) + start;
	}

	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	private float easeOutCubic(float start, float end, float value)
	{
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}

	private float easeInOutCubic(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if(value < 1)
			return end * 0.5f * value * value * value + start;
		value -= 2;
		return end * 0.5f * (value * value * value + 2) + start;
	}

	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	private float easeOutQuart(float start, float end, float value)
	{
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
	}

	private float easeInOutQuart(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if(value < 1)
			return end * 0.5f * value * value * value * value + start;
		value -= 2;
		return -end * 0.5f * (value * value * value * value - 2) + start;
	}

	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	private float easeOutQuint(float start, float end, float value)
	{
		value--;
		end -= start;
		return end * (value * value * value * value * value + 1) + start;
	}

	private float easeInOutQuint(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if(value < 1)
			return end * 0.5f * value * value * value * value * value + start;
		value -= 2;
		return end * 0.5f * (value * value * value * value * value + 2) + start;
	}

	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
	}

	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
	}

	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
	}

	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2, 10 * (value - 1)) + start;
	}

	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
	}

	private float easeInOutExpo(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if(value < 1)
			return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
		value--;
		return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
	}

	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
	}

	private float easeOutCirc(float start, float end, float value)
	{
		value--;
		end -= start;
		return end * Mathf.Sqrt(1 - value * value) + start;
	}

	private float easeInOutCirc(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if(value < 1)
			return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
		value -= 2;
		return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
	}

	/* GFX47 MOD START */
	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float d = 1f;
		return end - easeOutBounce(0, end, d - value) + start;
	}
	/* GFX47 MOD END */

	/* GFX47 MOD START */
	//private float bounce(float start, float end, float value){
	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if(value < (1 / 2.75f)) {
			return end * (7.5625f * value * value) + start;
		} else if(value < (2 / 2.75f)) {
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		} else if(value < (2.5 / 2.75)) {
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		} else {
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}
	/* GFX47 MOD END */

	/* GFX47 MOD START */
	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float d = 1f;
		if(value < d * 0.5f)
			return easeInBounce(0, end, value * 2) * 0.5f + start;
		else
			return easeOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
	}
	/* GFX47 MOD END */

	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1;
		float s = 1.70158f;
		return end * (value) * value * ((s + 1) * value - s) + start;
	}

	private float easeOutBack(float start, float end, float value)
	{
		float s = 1.70158f;
		end -= start;
		value = (value) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
	}

	private float easeInOutBack(float start, float end, float value)
	{
		float s = 1.70158f;
		end -= start;
		value /= .5f;
		if((value) < 1) {
			s *= (1.525f);
			return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
		}
		value -= 2;
		s *= (1.525f);
		return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
	}

	private float easeOutBackQuart(float start, float end, float value)
	{
		float s = 5.70158f;
		end -= start;
		value = (value) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
	}

	private float punch(float amplitude, float value)
	{
		float s = 9;
		if(value == 0) {
			return 0;
		} else if(value == 1) {
			return 0;
		}
		float period = 1 * 0.3f;
		s = period / (2 * Mathf.PI) * Mathf.Asin(0);
		return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
	}

	/* GFX47 MOD START */
	private float easeInElastic(float start, float end, float value)
	{
		end -= start;

		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;

		if(value == 0)
			return start;

		if((value /= d) == 1)
			return start + end;

		if(a == 0f || a < Mathf.Abs(end)) {
			a = end;
			s = p / 4;
		} else {
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}

		return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
	}
	/* GFX47 MOD END */

	/* GFX47 MOD START */
	//private float elastic(float start, float end, float value){
	private float easeOutElastic(float start, float end, float value)
	{
		/* GFX47 MOD END */
		//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
		end -= start;

		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;

		if(value == 0)
			return start;

		if((value /= d) == 1)
			return start + end;

		if(a == 0f || a < Mathf.Abs(end)) {
			a = end;
			s = p * 0.25f;
		} else {
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}

		return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
	}

	/* GFX47 MOD START */
	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;

		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;

		if(value == 0)
			return start;

		if((value /= d * 0.5f) == 2)
			return start + end;

		if(a == 0f || a < Mathf.Abs(end)) {
			a = end;
			s = p / 4;
		} else {
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}

		if(value < 1)
			return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
		return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
	}
	/* GFX47 MOD END */

	private float easeInBackQuad(float start, float end, float value)
	{
		end -= start;
		value /= 1;
		return end * (value <= 0.33f ? -3 * value : 3 * value - 2) + start;
	}
}
