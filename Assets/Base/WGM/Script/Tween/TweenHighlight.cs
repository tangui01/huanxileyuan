using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UI2DSprite))]
public class TweenHighlight : UITweener {

	[Range(0f, 1f)]public float from = 1f;
	[Range(0f, 1f)]public float to = 1f;

	bool mCached = false;
	Material mMat;
	UI2DSprite mSr;
	[Range(0f, 1f)]float mHighlight = 0;


	void Cache()
	{
		mCached = true;
		mSr = GetComponent<UI2DSprite>();
		mMat = mSr.material;
	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value
	{
		get
		{
			if(!mCached) Cache();
			return mHighlight;
		}
		set
		{
			if(!mCached) Cache();
			mHighlight = Mathf.Clamp01(value);
			mMat.SetFloat("_FlashAmount", mHighlight);
			mSr.RemoveFromPanel();
		}
	}

	

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenHighlight Begin(GameObject go, float duration, float alpha)
	{
		TweenHighlight comp = UITweener.Begin<TweenHighlight>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;

		if(duration <= 0f) {
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void SetStartToCurrentValue() { from = value; }
	public override void SetEndToCurrentValue() { to = value; }
}
