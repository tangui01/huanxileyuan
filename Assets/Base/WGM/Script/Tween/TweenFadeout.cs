using UnityEngine;
using System.Collections;

public class TweenFadeout : TweenAlpha {

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = Mathf.Lerp(from, to, factor);

		if(isFinished) {
			value = from;
			gameObject.SetActive(false);
		}
	}

	 static public TweenFadeout Begin(GameObject go, float duration, float alpha)
	{
		TweenFadeout comp = UITweener.Begin<TweenFadeout>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;

		if(duration <= 0f) {
			comp.Sample(1f, true);
			comp.enabled = false;
		}

		if(!comp.gameObject.activeInHierarchy) {
			comp.enabled = false;
		}

		return comp;
	}
}
