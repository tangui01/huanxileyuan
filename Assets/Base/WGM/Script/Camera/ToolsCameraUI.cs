using UnityEngine;
using System.Collections;

public class ToolsCameraUI : ToolsCamera {
	
	public float uiY = 25;

	void SetUICamera(float uiYPos)
	{
		Camera maicam = transform.parent.GetComponent<Camera>();
		if(maicam == null) {
			return;
		}

		float xita = maicam.fieldOfView / 2 * Mathf.Deg2Rad;
		float size = Mathf.Abs(uiYPos - transform.position.y) * Mathf.Tan(xita);
        
		mCamera.clearFlags = CameraClearFlags.Depth;    //UI Camera使用Depth only
        mCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
        mCamera.orthographic = true;                    //正交投影
        mCamera.orthographicSize = size;                //由深度得出UI Camera的大小
        mCamera.nearClipPlane = maicam.nearClipPlane;   //近平面与Main Camera一致
        mCamera.farClipPlane = maicam.farClipPlane;     //远平面与Main Camera一致
        //mCamera.depth = maicam.depth + 1;               //深度在Main Camera上面
        mCamera.rect = maicam.rect;   //设置UI Camera的Viewport Rect和Main Camera一致
	}

	protected override void OnDrawGizmos()
	{
		if(GetComponent<Camera>().aspect == 1) {  //防止Unity5.0启动的时候报错。5.0刚启动摄像机的aspect为1
			return;
		}
		base.OnDrawGizmos();

		Color col = new Color(0.3f, 0.3f, 0.3f, 0.3f);
		DrawAreaPlane(GetComponent<Camera>(), col, uiY, corners);

		SetUICamera(uiY);
	}
}