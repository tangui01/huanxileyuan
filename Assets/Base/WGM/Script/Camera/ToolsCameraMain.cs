using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ToolsCameraMain : ToolsCamera {

	public bool showArea = true;
	public float nearY = 10;
	public float midY = 0;
	public float farY = -10;

	protected override void OnDrawGizmos()
	{
		if(GetComponent<Camera>().aspect == 1) {  //防止Unity5.0启动的时候报错。5.0刚启动摄像机的aspect为1
			return;
		}
		base.OnDrawGizmos();

		DrawCameraView(GetComponent<Camera>(), Color.cyan, GetComponent<Camera>().aspect, 1);

		if (showArea) {
			Color col = new Color(195 / 255f, 99 / 255f, 194 / 255f, 0.3f);
			//DrawAreaPlane(GetComponent<Camera>(), col, nearY, corners);
			DrawAreaPlane(GetComponent<Camera>(), col, midY, corners);
			//DrawAreaPlane(GetComponent<Camera>(), col, farZ, corners);
		}
        //col = new Color(197f/255, 255f/255, 86f/255, 0.3f);
        //DrawAreaPlane(GetComponent<Camera>(), col, nearY, corners, 4);
	}
    public void shakeCamMain()
    {
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.DOShakePosition(1, 0.5f, 60).OnComplete(() => { Camera.main.transform.localPosition = Vector3.zero; });
    }

   

}