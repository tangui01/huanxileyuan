//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// Simple script that lets you localize a UIWidget.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	/// <summary>
	/// cache all UILocalize component
	/// </summary>

	static List<UILocalize> mListLocalize = new List<UILocalize>();

	public SpriteAtlas mSpriteAtlas;
	public bool mIsPixelPerfect = true;

	/// <summary>
	/// Localization key.
	/// </summary>

	public string key;

	/// <summary>
	/// Manually change the value of whatever the localization component is attached to.
	/// </summary>

	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				UIWidget w = GetComponent<UIWidget>();
				UILabel lbl = w as UILabel;
				UISprite sp = w as UISprite;
				UI2DSprite u2sp = w as UI2DSprite;

				if (lbl != null)
				{
					// If this is a label used by input, we should localize its default value instead
					UIInput input = NGUITools.FindInParents<UIInput>(lbl.gameObject);
					if (input != null && input.label == lbl) input.defaultText = value;
					else lbl.text = value;
#if UNITY_EDITOR
					if (!Application.isPlaying) NGUITools.SetDirty(lbl);
#endif
				}
				else if (sp != null)
				{
					UIButton btn = NGUITools.FindInParents<UIButton>(sp.gameObject);
					if (btn != null && btn.tweenTarget == sp.gameObject)
						btn.normalSprite = value;

					sp.spriteName = value;
					if(mIsPixelPerfect)
						sp.MakePixelPerfect();
#if UNITY_EDITOR
					if (!Application.isPlaying) NGUITools.SetDirty(sp);
#endif
				} else if(u2sp != null && mSpriteAtlas != null) {
					u2sp.sprite2D = mSpriteAtlas.GetSprite(value);
					if(mIsPixelPerfect)
						u2sp.MakePixelPerfect();
				}
			}
		}
	}

#if UNITY_EDITOR
	void Reset()
	{
		//2018-6-14 兼容图片多语言 yyq remove
		//m_ContentKey = LanguageLabel.text;
		if(!Application.isPlaying) {
			UIWidget widget = GetComponent<UIWidget>();
			UILabel lgeLabel = widget as UILabel;
			UISprite lgeSprite = widget as UISprite;
			UI2DSprite lge2DSprite = widget as UI2DSprite;
			if(lgeLabel != null)
				key = lgeLabel.text;
			else if(lgeSprite != null)
				key = lgeSprite.spriteName.Split('_')[0];
			else if(lge2DSprite != null)
				key = lge2DSprite.sprite2D.name;

		}
	}
#endif

	bool mStarted = false;

	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>

	void OnEnable ()
	{
		mListLocalize.Add(this);
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		if (mStarted) OnLocalize();
	}

	/// <summary>
	/// remove UILocalize from cache when it's been disable.
	/// </summary>

	void OnDisable()
	{
		mListLocalize.Remove(this);
	}

	/// <summary>
	/// Localize the widget on start.
	/// </summary>

	void Start ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		mStarted = true;
		OnLocalize();
	}

	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>

	void OnLocalize ()
	{
		// If no localization key has been specified, use the label's text as the key
		if (string.IsNullOrEmpty(key))
		{
			UILabel lbl = GetComponent<UILabel>();
			if (lbl != null) key = lbl.text;
		}

		// If we still don't have a key, leave the value as blank
		if (!string.IsNullOrEmpty(key)) value = Localization.Get(key);
	}

	/// <summary>
	/// Set the key.
	/// </summary>
	/// <param name="key">localize key</param>

	public void SetKey(string key)
	{
		this.key = key;
		OnLocalize();
	}

	/// <summary>
	/// ReUpdate the localize.
	/// </summary>

	public void ReUpdate()
	{
		if(mStarted) {
			OnLocalize();
		}
	}

	/// <summary>
	/// Set the language which use to localize.
	/// </summary>
	/// <param name="language">language to set</param>

	public static void SetLanguage(string language)
	{
		Localization.language = language;
		foreach(UILocalize l in mListLocalize) {
			l.ReUpdate();
		}
	}
}
