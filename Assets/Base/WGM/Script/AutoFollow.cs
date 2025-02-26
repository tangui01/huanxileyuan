using UnityEngine;
using System.Collections;

public class AutoFollow : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public bool worldSpace = false;

	public enum FollowType
	{
		D2FollowD2,
		D2FollowD3,
		D3FollowD2,
		D3FollowD3,
	}

	public FollowType type = FollowType.D2FollowD2;

	[HideInInspector]
	public ToolsCamera cameraThis;

	[HideInInspector]
	public ToolsCamera cameraTarget;

	Transform mTrans;
	Transform mCachedTransform { get { if(mTrans == null) mTrans = transform; return mTrans; } }

	IEnumerator routine;

	// Use this for initialization
	void Start () {
		StartFollow();
	}
	
	// Update is called once per frame
	void Update () {
		/*if(target == null) {
			return;
		}

		if(worldSpace) {
			mCacheTransform.position = target.position + offset;
			switch(type) {
				case FollowType.D2FollowD2:
					break;
				case FollowType.D2FollowD3:
					break;
				case FollowType.D3FollowD2:
					break;
				case FollowType.D3FollowD3:
					break;
				default: break;
			}
		} else {
			mCacheTransform.localPosition = target.localPosition + offset;
		}*/
	}

	public void StartFollow()
	{
		if(worldSpace) {
			routine = StartCoroutine(CFollowWorld(), routine);
		} else {
			routine = StartCoroutine(CFollowLocal(), routine);
		}
	}

	IEnumerator CFollowWorld()
	{
		if(target == null) {
			yield break;
		}

		yield return null;

		while(true) {
			switch(type) {
				case FollowType.D2FollowD2:
					break;
				case FollowType.D2FollowD3:
					break;
				case FollowType.D3FollowD2:
					mCachedTransform.position = cameraTarget.WorldToCameraWorld(target.position,
												cameraThis,
												cameraThis.transform.position.y - transform.position.y);
					break;
				case FollowType.D3FollowD3:
					break;
				default: break;
			}
			yield return null;
		}
	}

	IEnumerator CFollowLocal()
	{
		if(target == null) {
			yield break;
		}
		yield return null;

		while(true) {
			switch(type) {
				case FollowType.D2FollowD2:
					mCachedTransform.localPosition = target.localPosition + offset;
					break;
				case FollowType.D2FollowD3:
					break;
				case FollowType.D3FollowD2:
					break;
				case FollowType.D3FollowD3:
					mCachedTransform.localPosition = target.localPosition + offset;
					break;
				default: break;
			}
			yield return null;
		}
	}

	public IEnumerator StartCoroutine(IEnumerator routine, IEnumerator instance)
	{
		if(instance != null) StopCoroutine(instance);
		instance = routine;
		StartCoroutine(instance);
		return instance;
	}

	public static AutoFollow Begin(Transform body,
									Transform target,
									bool world = false,
									FollowType type = FollowType.D2FollowD2,
									ToolsCamera cameraThis = null,
									ToolsCamera cameraTarget = null)
	{
		AutoFollow comp = body.GetComponent<AutoFollow>();

		if(comp == null) {
			comp = body.gameObject.AddComponent<AutoFollow>();
		}
		comp.target = target;
		comp.offset = Vector3.zero;
		comp.worldSpace = world;
		comp.type = type;
		comp.cameraThis = cameraThis;
		comp.cameraTarget = cameraTarget;
		comp.enabled = true;
		comp.StartFollow();
		return comp;
	}
}
