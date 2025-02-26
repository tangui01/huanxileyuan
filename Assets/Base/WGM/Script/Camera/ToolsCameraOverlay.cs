using UnityEngine;
using System.Collections;

public class ToolsCameraOverlay : ToolsCamera
{


    void SetOverlayCamera()
    {
        Camera maicam = transform.parent.GetComponent<Camera>();
        if(maicam == null) {
            return;
        }


        /*mCamera.clearFlags = CameraClearFlags.Depth;    //UI Camera使用Depth only
        mCamera.cullingMask = 1 << LayerMask.NameToLayer("Overlay");
        mCamera.orthographic = maicam.orthographic;     //正交投影
        mCamera.fieldOfView = maicam.fieldOfView;
        mCamera.nearClipPlane = maicam.nearClipPlane;   //近平面与Main Camera一致
        mCamera.farClipPlane = maicam.farClipPlane;     //远平面与Main Camera一致
        mCamera.rect = maicam.rect;   //设置UI Camera的Viewport Rect和Main Camera一致
        mCamera.depth = maicam.depth + 3;               //深度在Main Camera上面
        */

        float xita = maicam.fieldOfView / 2 * Mathf.Deg2Rad;
        float size = Mathf.Abs(0 - transform.position.y) * Mathf.Tan(xita);

        mCamera.clearFlags = CameraClearFlags.Depth;    //UI Camera使用Depth only
        //mCamera.cullingMask = 1 << LayerMask.NameToLayer("Overlay");
        mCamera.orthographic = true;     //正交投影
        mCamera.orthographicSize = size;
        mCamera.nearClipPlane = maicam.nearClipPlane;   //近平面与Main Camera一致
        mCamera.farClipPlane = maicam.farClipPlane;     //远平面与Main Camera一致
        mCamera.rect = maicam.rect;   //设置UI Camera的Viewport Rect和Main Camera一致
        //mCamera.depth = maicam.depth + 3;               //深度在Main Camera上面
    }

    protected override void OnDrawGizmos()
    {
		if(GetComponent<Camera>().aspect == 1) {  //防止Unity5.0启动的时候报错。5.0刚启动摄像机的aspect为1
			return;
		}
        base.OnDrawGizmos();

        SetOverlayCamera();
    }
}
