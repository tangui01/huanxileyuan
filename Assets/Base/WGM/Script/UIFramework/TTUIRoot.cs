using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Init The UI Root
/// 
/// View1
/// -UI
/// --Camera
/// --Global
/// ---FixedRoot
/// ---NormalRoot
/// ---PopupRoot
/// </summary>
public class TTUIRoot : MonoBehaviour {
    private static TTUIRoot m_Instance = null;
    public static TTUIRoot Instance {
        get {
            if (Application.isPlaying)
            {
              
                if (m_Instance == null)
                {
                    InitRoot();
                }
                return m_Instance;
            }
            return null;
        }
    }

	public static Action onUpdate;
    public static Action onLateUpdate;
    public static Action onDestroy;

	public static int normalRootDepth = 1;
	public static int fixedRootDepth = 250;
	public static int popupRootDepth = 500;

    public Transform root;
    public Transform fixedRoot;
    public Transform normalRoot;
    public Transform popupRoot;
    public Camera uiCamera;

	static void InitRoot() {

        GameObject go = GameObject.Find("View1/UI/Global");
        m_Instance = go.AddComponent<TTUIRoot>();
        m_Instance.root = go.transform;

		m_Instance.uiCamera = GameObject.Find("View1/UI/Camera").GetComponent<Camera>();

		m_Instance.normalRoot = new GameObject("NormalRoot").transform;
		TTUITool.AddChildToTarget(m_Instance.root, m_Instance.normalRoot);

		m_Instance.fixedRoot = new GameObject("FixedRoot").transform;
		TTUITool.AddChildToTarget(m_Instance.root, m_Instance.fixedRoot);

		m_Instance.popupRoot = new GameObject("PopupRoot").transform;
		TTUITool.AddChildToTarget(m_Instance.root, m_Instance.popupRoot);
    }

	void Update()
	{
		if(onUpdate != null) onUpdate();
	}

    void LateUpdate()
    {
        if(onLateUpdate != null) onLateUpdate();
    }

    void OnDestroy() {
        if(onDestroy != null) onDestroy();
        m_Instance = null;
		onUpdate = null;
        onLateUpdate = null;
        onDestroy = null;
		TTUIPage.OnPageDestroy(); 
    }
}