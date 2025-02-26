using UnityEngine;
using System.Collections;

public class UILifeBar : MonoBehaviour
{
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Camera targetCamera;
    [HideInInspector]
    public Camera guiCamera;

    UISlider mUISlider;

    void Awake()
    {
        mUISlider = transform.GetComponent<UISlider>();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        Vector3 pos = targetCamera.WorldToViewportPoint(target.position);
        pos.z = 0;
        pos = guiCamera.ViewportToWorldPoint(pos);
        transform.position = pos;
	}

    public static UILifeBar Create(Transform target, Camera targetCam, Camera guiCam)
    {
        GameObject go =(Resources.Load("Prefab/LifeBar") as GameObject);
        go = (GameObject)Instantiate(go);
        UILifeBar lb = go.GetComponent<UILifeBar>();
        lb.transform.parent = guiCam.transform;
        lb.transform.localScale = Vector3.one;
        lb.Init(target, targetCam, guiCam);
        return lb;
    }

    public void Init(Transform target, Camera targetCam, Camera guiCam)
    {
        this.target = target;
        this.targetCamera = targetCam;
        this.guiCamera = guiCam;
    }

    public void SetValue(float value)
    {
        mUISlider.value = value;
    }

    public float GetValue()
    {
        return mUISlider.value;
    }
}
