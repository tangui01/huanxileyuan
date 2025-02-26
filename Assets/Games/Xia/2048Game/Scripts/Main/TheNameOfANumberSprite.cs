using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//次脚本功能是通过代码控制图片的位置以及相关的参数设置
public class TheNameOfANumberSprite : MonoBehaviour {

    private  Image image;
    private void Awake()
    { 
        image = GetComponent<Image>(); 
	}
    /// <summary>
    /// 初始化数字精灵
    /// </summary>
    /// <param name="number">数字</param>
    /// <param name="position">世界坐标</param>
    /// <param name="size">大小</param>
    public void Init(int number, Vector3 position, float size)
    {
        SetImage(number); 

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);//设置图片大小

        this.transform.position = position; 
    }
    
    
    
    
    /// <summary>
    /// 设置方格显示的数字
    /// </summary>
    /// <param name="number">数字</param>
    public void SetImage(int number)
    {
        image.sprite = TheNameOfAResourceManager.GetImage(number);
    }
     
 
   public void MergeEffect(GameObject vfx = null)
   {  
       iTween.ScaleFrom(gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.4f);
       if (vfx != null)
       {
           Vector3 pos = gameObject.transform.position;
           pos.z = vfx.transform.position.z;
           vfx.transform.position = pos;
           vfx.GetComponent<ParticleSystem>().Play();
       }
   } 

   public void CreateEffect()
   { 
       iTween.ScaleFrom(gameObject, Vector3.zero, 0.3f);
   }
   /// <summary>
   /// 移动速度相关
   /// </summary>
   public void MoveEffect(Vector3 pos)
   {     
       iTween.MoveTo(gameObject, iTween.Hash(
           "position",pos,"speed",15,"easetype",iTween.EaseType.easeOutQuart
           ));
   }
}
