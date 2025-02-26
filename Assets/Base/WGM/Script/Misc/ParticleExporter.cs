//作者：吴高明  QQ:394743576
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ParticleExporter : MonoBehaviour
{
    //导出PNG序列帧到该文件夹下
    public string folder = "PNG_Animations";

    //导出序列帧子文件夹
    public string subFolder = "01";

    //序列帧采集帧率,unity3d设置Time.captureFramerate会忽略真实时间，直接使用此帧率
    public int frameRate = 25;

    //最多采集多少帧，超过此帧数，停止采集
    public float frameCount = 100;

    //采集图片的像素宽度，为0则默认采集相机像素
    public int widthPixels = 0;

    //采集图片的像素高度，为0则默认采集相机像素
    public int heightPixels = 0;

    //采集时移动的速度，由于有些效果是拖尾的，需要移动才会展现拖尾效果
    public Vector3 moveSpeed = Vector3.up;

    //开始采集
    public bool enableCapture = true;

    private string realFolder = "";
    private float originaltimescaleTime;
    private bool over = false;
    private int currentIndex = 0;
    private Camera exportCamera;    //导出特效的摄像机，使用RenderTexture

    public void Start()
    {
        realFolder = Path.Combine(folder, subFolder);
        if(!Directory.Exists(realFolder)) {
            Directory.CreateDirectory(realFolder);
        }

        Time.captureFramerate = frameRate;
        originaltimescaleTime = Time.timeScale;

        GameObject go = Instantiate(Camera.main.gameObject) as GameObject;
        go.transform.parent = Camera.main.transform;
        exportCamera = go.GetComponent<Camera>();
        GameObject.Destroy(go.GetComponent<AudioListener>());
    }

    void Update()
    {
        if(!over && currentIndex >= frameCount) {
            over = true;
            Cleanup();
            Debug.Log("Finish");
            return;
        }

        Camera.main.transform.Translate(moveSpeed * Time.deltaTime);

        // 每帧截屏
        if(enableCapture) {
            StartCoroutine(CaptureFrame());
        }
    }

    void Cleanup()
    {
        Destroy(exportCamera.gameObject);
        DestroyImmediate(gameObject);
    }

    IEnumerator CaptureFrame()
    {
        //游戏时间停止
        Time.timeScale = 0;
        print("--------------");
        //必须等待这帧相机全部渲染后再获取图片，否则获取图片不完整
        yield return new WaitForEndOfFrame();

        //图片文件位置
        string filename = String.Format("{0}\\{1:D03}.png", realFolder, currentIndex++);
        Debug.Log(filename);

        int width = Screen.width;
        int height = Screen.height;

        //初始化render textures
        RenderTexture blackCamRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        RenderTexture whiteCamRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

        //先选择黑色作为相机渲染背景
        exportCamera.targetTexture = blackCamRenderTexture;
        exportCamera.backgroundColor = Color.black;
        exportCamera.Render();
        RenderTexture.active = blackCamRenderTexture;
        Texture2D texb = GetTex2D();

        //再选择白色作为相机渲染背景
        exportCamera.targetTexture = whiteCamRenderTexture;
        exportCamera.backgroundColor = Color.white;
        exportCamera.Render();
        RenderTexture.active = whiteCamRenderTexture;
        Texture2D texw = GetTex2D();

        /*
         * R' = (1-a)*Rbackground + a*Rorigin    叠加公式（红色），其它颜色也一样
         * Rb = a*Rorigin                      1 黑色作为背景时，叠加后的颜色和原始色关系
         * Rw = 1-a + a*Rorigin                2 白色作为背景时，叠加后的颜色和原始色关系
         *                                       根据公式1和2可求出a，再由求出的a和合成的颜色得出图片真实像素
         */
        if(texw && texb) {
            Rect r = GetCaptureRect();
            Texture2D outputtex = new Texture2D((int)r.width, (int)r.height, TextureFormat.ARGB32, false);
            for(int y = 0; y < outputtex.height; ++y) {    //行
                for(int x = 0; x < outputtex.width; ++x) { //列
                    float alpha;
					float alphaR = texw.GetPixel(x + (int)r.x, y + (int)r.y).r - texb.GetPixel(x + (int)r.x, y + (int)r.y).r;
					float alphaG = texw.GetPixel(x + (int)r.x, y + (int)r.y).g - texb.GetPixel(x + (int)r.x, y + (int)r.y).g;
					float alphaB = texw.GetPixel(x + (int)r.x, y + (int)r.y).b - texb.GetPixel(x + (int)r.x, y + (int)r.y).b;
					alpha = Mathf.Min(alphaR, alphaG, alphaB);
                    /*alpha = (texw.GetPixel(x+(int)r.x, y+(int)r.y).r - texb.GetPixel(x+(int)r.x, y+(int)r.y).r +
							texw.GetPixel(x+(int)r.x, y+(int)r.y).g - texb.GetPixel(x+(int)r.x, y+(int)r.y).g +
							texw.GetPixel(x+(int)r.x, y+(int)r.y).b - texb.GetPixel(x+(int)r.x, y+(int)r.y).b) / 3;*/
                    alpha = 1.0f - alpha;
                    Color color;
                    if(alpha == 0) {
                        color = Color.clear;
                    } else {
                        color = texb.GetPixel(x + (int)r.x, y + (int)r.y);
                    }
					
                    color.a = alpha;
                    color.r /= alpha;
                    color.g /= alpha;
                    color.b /= alpha;
                    outputtex.SetPixel(x, y, color);
                }
            }


            //把Texture解码成PNG字节序列，用来写文件
            byte[] pngShot = outputtex.EncodeToPNG();
            File.WriteAllBytes(filename, pngShot);

            //清除垃圾，图片处理需要大量内存，需要及时释放
            pngShot = null;
            RenderTexture.active = null;
            DestroyImmediate(outputtex);
            outputtex = null;
            DestroyImmediate(blackCamRenderTexture);
            blackCamRenderTexture = null;
            //DestroyImmediate(whiteCamRenderTexture);
            whiteCamRenderTexture = null;
            DestroyImmediate(texb);
            texb = null;
            DestroyImmediate(texw);
            texb = null;

            System.GC.Collect();

            //timeScale重新打开，游戏时间继续
            Time.timeScale = originaltimescaleTime;
        }

        
    }

    //获取Game视图中显示的内容
    private Texture2D GetTex2D()
    {
        int width = Screen.width;
        int height = Screen.height;

        //创建一幅ARGB32的Texture，存储Game视图
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);

        //读取Game视图中的内容到Texture中
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        return tex;
    }

    Rect GetCaptureRect()
    {
        Rect r = new Rect();

        int width = (int)Camera.main.pixelWidth;
        int height = (int)Camera.main.pixelHeight;

        if(widthPixels > width) {
            widthPixels = width;
        }
        if(heightPixels > height) {
            heightPixels = height;
        }

        r.width = (widthPixels > 0 && widthPixels <= Screen.width) ? widthPixels : width;
        r.height = (heightPixels > 0 && heightPixels <= Screen.height) ? heightPixels : height;
        r.x = (width - r.width) / 2;
        r.y = (height - r.height) / 2;

        return r;
    }

    void OnDrawGizmos()
    {
        Rect r = GetCaptureRect();

        Vector3 cor0 = Camera.main.ViewportToWorldPoint(new Vector3(r.x / Screen.width, r.y / Screen.height, 1));
        Vector3 cor1 = Camera.main.ViewportToWorldPoint(new Vector3(1-(r.x / Screen.width), 1-(r.y / Screen.height), 1));
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        Gizmos.DrawWireCube(center, (cor1 - cor0)+new Vector3(0.001f,0.001f,0));
    }
}