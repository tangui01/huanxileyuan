//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Similar to UIButtonColor, but adds a 'disabled' state based on whether the collider is enabled or not.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	/// <summary>
	/// Current button that sent out the onClick event.
	/// </summary>

	static public UIButton current;

	/// <summary>
	/// Whether the button will highlight when you drag something over it.
	/// </summary>

	public bool dragHighlight = false;

	/// <summary>
	/// Name of the hover state sprite.
	/// </summary>

	public string hoverSprite;

	/// <summary>
	/// Name of the pressed sprite.
	/// </summary>

	public string pressedSprite;

	/// <summary>
	/// Name of the disabled sprite.
	/// </summary>

	public string disabledSprite;

	/// <summary>
	/// Name of the hover state sprite.
	/// </summary>

	public UnityEngine.Sprite hoverSprite2D;

	/// <summary>
	/// Name of the pressed sprite.
	/// </summary>

	public UnityEngine.Sprite pressedSprite2D;

	/// <summary>
	/// Name of the disabled sprite.
	/// </summary>

	public UnityEngine.Sprite disabledSprite2D;

	/// <summary>
	/// Whether the sprite changes will elicit a call to MakePixelPerfect() or not.
	/// </summary>

	public bool pixelSnap = false;

	/// <summary>
	/// Click event listener.
	/// </summary>
    public static Action onStaticClick;
    public List<EventDelegate> onClick = new List<EventDelegate>();

	public List<EventDelegate> onPress = new List<EventDelegate>();

	public Action<Transform> onActionClick;

	public Action<Transform> onActionExcuse;

	public Action<Transform> onActionDown;

	public Action<Transform> onActionUp;


	public BoxCollider boxCollider { get; set; }

	// Cached value
	[System.NonSerialized] UISprite mSprite;
	[System.NonSerialized] UI2DSprite mSprite2D;
	[System.NonSerialized] string mNormalSprite;
	[System.NonSerialized] UnityEngine.Sprite mNormalSprite2D;

	/// <summary>
	/// Whether the button should be enabled.
	/// </summary>

	public override bool isEnabled
	{
		get
		{
			if (!enabled) return false;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
			var col = collider;
#else
			var col = gameObject.GetComponent<Collider>();
#endif
			if (col && col.enabled) return true;
			var c2d = GetComponent<Collider2D>();
			return (c2d && c2d.enabled);
		}
		set
		{
			if (isEnabled != value)
			{
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
				var col = collider;
#else
				var col = gameObject.GetComponent<Collider>();
#endif
				if (col != null)
				{
					col.enabled = value;
					var buttons = GetComponents<UIButton>();
					foreach (UIButton btn in buttons) btn.SetState(value ? State.Normal : State.Disabled, false);
				}
				else
				{
					var c2d = GetComponent<Collider2D>();

					if (c2d != null)
					{
						c2d.enabled = value;
						var buttons = GetComponents<UIButton>();
						foreach (UIButton btn in buttons) btn.SetState(value ? State.Normal : State.Disabled, false);
					}
					else enabled = value;
				}
			}
		}
	}

	/// <summary>
	/// Convenience function that changes the normal sprite.
	/// </summary>

	public string normalSprite
	{
		get
		{
			if (!mInitDone) OnInit();
			return mNormalSprite;
		}
		set
		{
			if (!mInitDone) OnInit();
			if (mSprite != null && !string.IsNullOrEmpty(mNormalSprite) && mNormalSprite == mSprite.spriteName)
			{
				mNormalSprite = value;
				SetSprite(value);
				NGUITools.SetDirty(mSprite);
			}
			else
			{
				mNormalSprite = value;
				if (mState == State.Normal) SetSprite(value);
			}
		}
	}

	/// <summary>
	/// Convenience function that changes the normal sprite.
	/// </summary>

	public UnityEngine.Sprite normalSprite2D
	{
		get
		{
			if (!mInitDone) OnInit();
			return mNormalSprite2D;
		}
		set
		{
			if (!mInitDone) OnInit();
			if (mSprite2D != null && mNormalSprite2D == mSprite2D.sprite2D)
			{
				mNormalSprite2D = value;
				SetSprite(value);
				NGUITools.SetDirty(mSprite);
			}
			else
			{
				mNormalSprite2D = value;
				if (mState == State.Normal) SetSprite(value);
			}
		}
	}
	/// <summary>
	/// Cache the sprite we'll be working with.
	/// </summary>

	protected override void OnInit ()
	{
		base.OnInit();
		mSprite = (mWidget as UISprite);
		mSprite2D = (mWidget as UI2DSprite);
		if (mSprite != null) mNormalSprite = mSprite.spriteName;
		if (mSprite2D != null) mNormalSprite2D = mSprite2D.sprite2D;
		
	}

	/// <summary>
	/// Set the initial state.
	/// </summary>

	protected override void OnEnable ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			mInitDone = false;
			return;
		}
#endif
		boxCollider = GetComponent<BoxCollider>();
		if (isEnabled)
		{
			if (mInitDone) OnHover(UICamera.hoveredObject == gameObject);
		}
		else SetState(State.Disabled, true);
	}

	/// <summary>
	/// Drag over state logic is a bit different for the button.
	/// </summary>

	protected override void OnDragOver ()
	{
		if (isEnabled && (dragHighlight || UICamera.currentTouch.pressed == gameObject))
			base.OnDragOver();
	}

	/// <summary>
	/// Drag out state logic is a bit different for the button.
	/// </summary>

	protected override void OnDragOut ()
	{
		if (isEnabled && (dragHighlight || UICamera.currentTouch.pressed == gameObject))
			base.OnDragOut();
	}

	/// <summary>
	/// Call the listener function.
	/// </summary>

	protected virtual void OnClick ()
	{
		if (current == null && isEnabled && UICamera.currentTouchID != -2 && UICamera.currentTouchID != -3)
		{
			current = this;
			EventDelegate.Execute(onClick);
			current = null;
		}
        onStaticClick?.Invoke();
        onActionClick?.Invoke(transform);
		onActionExcuse?.Invoke(transform);
	}

	protected override void OnPress(bool isPressed)
	{
		base.OnPress(isPressed);
		if(onActionDown != null && isPressed)
			onActionDown(transform);
		if(onActionUp != null && !isPressed)
			onActionUp(transform);
	}

	void Update()
	{
		if(mIsPressed && Time.realtimeSinceStartup > mNextClickTime) {
			mNextClickTime = Time.realtimeSinceStartup + 0.075f;

			if(current == null && isEnabled && gameObject.activeInHierarchy) {
				current = this;
				EventDelegate.Execute(onPress);
				current = null;
			}
			if(onActionExcuse != null)
				onActionExcuse(transform);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		mIsPressed = false;
	}

	/// <summary>
	/// Change the visual state.
	/// </summary>

	public override void SetState (State state, bool immediate)
	{
		base.SetState(state, immediate);

		if (mSprite != null)
		{
			switch (state)
			{
				case State.Normal: SetSprite(mNormalSprite); break;
				case State.Hover: SetSprite(string.IsNullOrEmpty(hoverSprite) ? mNormalSprite : hoverSprite); break;
				case State.Pressed: SetSprite(pressedSprite); break;
				case State.Disabled: SetSprite(disabledSprite); break;
			}
		}
		else if (mSprite2D != null)
		{
			switch (state)
			{
				case State.Normal: SetSprite(mNormalSprite2D); break;
				case State.Hover: SetSprite(hoverSprite2D == null ? mNormalSprite2D : hoverSprite2D); break;
				case State.Pressed: SetSprite(pressedSprite2D); break;
				case State.Disabled: SetSprite(disabledSprite2D); break;
			}
		}
	}

	/// <summary>
	/// Convenience function that changes the sprite.
	/// </summary>

	protected void SetSprite (string sp)
	{
		if (mSprite != null && !string.IsNullOrEmpty(sp) && mSprite.spriteName != sp)
		{
			mSprite.spriteName = sp;
			if (pixelSnap) mSprite.MakePixelPerfect();
		}
	}

	/// <summary>
	/// Convenience function that changes the sprite.
	/// </summary>

	protected void SetSprite (UnityEngine.Sprite sp)
	{
		if (sp != null && mSprite2D != null && mSprite2D.sprite2D != sp)
		{
			mSprite2D.sprite2D = sp;
			if (pixelSnap) mSprite2D.MakePixelPerfect();
		}
	}
}
