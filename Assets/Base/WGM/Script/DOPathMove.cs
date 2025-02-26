using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DOPathMove : MonoBehaviour {
    public CommonPath mCommonpath;
    public Ease ease;
    public float  time;
	// Use this for initialization
	void OnEnable () {
        transform.DOPath(mCommonpath.GetPoint(), time).SetEase(ease);
	}
	
	
}
