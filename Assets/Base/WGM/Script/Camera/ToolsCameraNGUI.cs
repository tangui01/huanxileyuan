using UnityEngine;
using System.Collections;

public class ToolsCameraNGUI : ToolsCamera {
	
	void SetUICamera()
	{

		Camera maicam =  Camera.main.GetComponent<Camera>();
		if(maicam == null) {
			return;
		}

		GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;    //UI Camera使用Depth only
		GetComponent<Camera>().orthographic = true;                    //正交投影
		GetComponent<Camera>().depth = maicam.depth + 2;               //深度在Main Camera和UI Camera上面
		//GetComponent<Camera>().rect = maicam.rect;   //设置UI Camera的Viewport Rect和Main Camera一致
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		DrawCameraView(GetComponent<Camera>(), Color.cyan, GetComponent<Camera>().aspect, 1);

		SetUICamera();
	}
}