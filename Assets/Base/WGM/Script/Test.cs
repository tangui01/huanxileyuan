using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

public class Test3229 : MonoBehaviour
{
	public TextAsset protoCaffe;
	WebCamTexture webCamTex;

	[DllImport("opencv")]
	public static extern int QRCodeDetecterInit(string detect_proto, string detect_caffe, string sr_proto, string sr_caffe);

	[DllImport("opencv")]
	public static extern IntPtr QRCodeDetectAndDecode(IntPtr inputData, int width, int height);

	[DllImport("opencv")]
	public static extern void QRCodeFree(IntPtr obj2free);

	// Start is called before the first frame update
	void Start()
    {
		RunAsync().WrapErrors();
    }

    // Update is called once per frame
    void Update()
    {
		if(webCamTex != null) {
			if(webCamTex.didUpdateThisFrame) {

				GCHandle handle = GCHandle.Alloc(webCamTex.GetPixels32(), GCHandleType.Pinned);
				IntPtr pixelPtr = handle.AddrOfPinnedObject();

				var strPtr = QRCodeDetectAndDecode(pixelPtr, webCamTex.width, webCamTex.height);
				string str = Marshal.PtrToStringAnsi(strPtr);
				QRCodeFree(strPtr);

				handle.Free();

				//Debug.Log($"decode str = " + str);

				var decodes = JsonUtility.FromJson<QRCodeDecode[]>(str); 
				//JsonConvert.DeserializeObject<QRCodeDecode[]>(str);
				foreach(var de in decodes) {
					Debug.Log("de = " + de.str);
				}
			}
		}
    }

	async Task RunAsync()
	{
		await Application.RequestUserAuthorization(UserAuthorization.WebCam);

		if(!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
			Debug.Log("no permission for webcam");
			return;
		}

		await new WaitForSeconds(0.5f);

		WebCamDevice[] devices = WebCamTexture.devices;
		Debug.Log("devices count = " + devices.Length);

		if(devices.Length <= 0) {
			return;
		}

		string path = $"{Application.persistentDataPath}/Caffemodel";
		LoadCaffemodelDatas(path);
		QRCodeDetecterInit($"{path}/detect.prototxt", $"{path}/detect.caffemodel", $"{path}/sr.prototxt", $"{path}/sr.caffemodel");

		webCamTex = new WebCamTexture(devices[0].name, 640, 480, 30);

		GetComponent<Renderer>().material.mainTexture = webCamTex;

		webCamTex.Play();
		
		Debug.Log("play");
	}

	void LoadCaffemodelDatas(string path)
	{
		if(!Directory.Exists(path) && protoCaffe != null) {
			Directory.CreateDirectory(path);
			FileIO.UnZipMemory(protoCaffe.bytes, path);
		}
	}

	public class QRCodeDecode
	{
		public string str;
		public Point[] pts;

		public class Point
		{
			public int x;
			public int y;
		}
	}
}
