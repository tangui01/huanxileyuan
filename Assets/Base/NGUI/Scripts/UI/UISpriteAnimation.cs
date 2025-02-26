//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Very simple sprite animation. Attach to a sprite and specify a common prefix such as "idle" and it will cycle through them.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
    public enum ANIM_MODE
    {
        NGUI_ANIM_LOOP,
        NGUI_ANIM_PINGPONG,
        NGUI_ANIM_ONCE
    }

    [HideInInspector]
    [SerializeField]
    protected int mFPS = 30;
    [HideInInspector]
    [SerializeField]
    protected string mPrefix = "";
    [HideInInspector]
    [SerializeField]
    protected bool mSnap = true;

    protected UISprite mSprite;
    protected float mDelta = 0f;
    protected int mIndex = 0;
    protected bool mActive = true;
    protected List<string> mSpriteNames = new List<string>();

    /// <summary>
    /// Number of frames in the animation.
    /// </summary>

    public int frames { get { return mSpriteNames.Count; } }

    /// <summary>
    /// Animation framerate.
    /// </summary>

    public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

    /// <summary>
    /// Set the name prefix used to filter sprites from the atlas.
    /// </summary>

    public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

    /// <summary>
    /// Returns is the animation is still playing or not
    /// </summary>

    public bool isPlaying { get { return mActive; } }

    /// <summary>
    /// is Snap size
    /// </summary>
    public bool snap
    {
        get { return mSnap; }
        set { this.mSnap = value; }
    }

    [HideInInspector]
	[SerializeField]
    public bool ignoreTimeScale = false;

	[HideInInspector]
    [SerializeField]
    protected ANIM_MODE mPlayMode = ANIM_MODE.NGUI_ANIM_LOOP;

	[HideInInspector]
    [SerializeField]
	public bool playForward = true;

    public ANIM_MODE playMode { get { return mPlayMode; } set { mPlayMode = value; } }
    public int curFrame { get { return mIndex; } }

	private float mDelayDelta = 0;
	public float delay = 0;

    /// <summary>
    /// Rebuild the sprite list first thing.
    /// </summary>

    protected virtual void Start() { RebuildSpriteList(); }

    /// <summary>
    /// Advance the sprite animation process.
    /// </summary>

    protected virtual void Update()
    {
        if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0)
        {
            mDelta += ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;

            float rate = 1f / mFPS;

            if (rate < mDelta)
            {
                mDelta = (rate > 0f) ? mDelta - rate : 0f;

                mIndex += playForward ? 1 : -1;
                if (mIndex >= mSpriteNames.Count)
                {
                    if (playMode == ANIM_MODE.NGUI_ANIM_LOOP)
                    {
						mDelayDelta += ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
						mIndex = mSpriteNames.Count - 1;
						if(mDelayDelta >= delay) {
							mDelayDelta = 0;
							mIndex = 0;
						}
                    }
                    else if (playMode == ANIM_MODE.NGUI_ANIM_PINGPONG)
                    {
                        mIndex = (mSpriteNames.Count - 2 >= 0) ? mSpriteNames.Count - 2 : 0;
                        playForward = !playForward;
                    }
                    else if (playMode == ANIM_MODE.NGUI_ANIM_ONCE)
                    {
                        mActive = false;
                    }
                }
                else if (mIndex < 0)
                {
                    if (playMode == ANIM_MODE.NGUI_ANIM_LOOP)
                    {
                        mIndex = mSpriteNames.Count - 1;
                    }
                    else if (playMode == ANIM_MODE.NGUI_ANIM_PINGPONG)
                    {
                        mIndex = mSpriteNames.Count > 1 ? 1 : 0;
                        playForward = !playForward;
                    }
                    else if (playMode == ANIM_MODE.NGUI_ANIM_ONCE)
                    {
                        mActive = false;
                    }
                }

                if (mActive)
                {
                    mSprite.spriteName = mSpriteNames[mIndex];
                    if (mSnap) mSprite.MakePixelPerfect();
                }
            }
        }
    }

    /// <summary>
    /// Rebuild the sprite list after changing the sprite name.
    /// </summary>

    public void RebuildSpriteList()
    {
        if (mSprite == null) mSprite = GetComponent<UISprite>();
        mSpriteNames.Clear();

        if (mSprite != null && mSprite.atlas != null)
        {
            List<UISpriteData> sprites = mSprite.atlas.spriteList;

            for (int i = 0, imax = sprites.Count; i < imax; ++i)
            {
                UISpriteData sprite = sprites[i];

                if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
                {
                    mSpriteNames.Add(sprite.name);
                }
            }
            mSpriteNames.Sort();
        }
    }

    /// <summary>
    /// Reset the animation to the beginning.
    /// </summary>

    public void Play() { mActive = true; }

    /// <summary>
    /// Pause the animation.
    /// </summary>

    public void Pause() { mActive = false; }

    /// <summary>
    /// Reset the animation to frame 0 and activate it.
    /// </summary>

    public void ResetToBeginning()
    {
        mActive = true;
        mIndex = 0;

        if (mSprite != null && mSpriteNames.Count > 0)
        {
            mSprite.spriteName = mSpriteNames[mIndex];
            if (mSnap) mSprite.MakePixelPerfect();
        }
    }

	public float Play(string prefix)
	{
		mPrefix = prefix;
		RebuildSpriteList();
        mSpriteNames.Sort(Sort);
		ResetToBeginning();
		return (float)mSpriteNames.Count / mFPS;
	}

	public int Sort(string a, string b)
	{
		char[] arr1 = a.ToCharArray();
		char[] arr2 = b.ToCharArray();
		int i = 0, j =0;

		while(i < arr1.Length && j < arr2.Length) {
			if(char.IsDigit(arr1[i]) && char.IsDigit(arr2[j])) {
				string s1 = "",s2 = "";
				while (i < arr1.Length && char.IsDigit(arr1[i])) {
					s1 += arr1[i];
					i++;
				}
				while(j < arr2.Length && char.IsDigit(arr2[j])) {
					s2 += arr2[j];
					j++;
				}
				if(int.Parse(s1) > int.Parse(s2)) {
					return 1;
				}
				if(int.Parse(s1) < int.Parse(s2)) {
					return -1;
				}
			} else {
				if(arr1[i] > arr2[j]) {
					return 1;
				}
				if(arr1[i] < arr2[j]) {
					return -1;
				}
				i++;
				j++;
			}
		}
		if(arr1.Length == arr2.Length) {
			return 0;
		} else {
			return arr1.Length > arr2.Length? 1: -1;
		}
	}
}
