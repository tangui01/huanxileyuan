using UnityEngine;
using System;
using System.Collections;

public class BezierTest : MonoBehaviour {


	public Transform center;

	float radio;
	float angle;
	float initAngle;


	//贝塞尔曲线算法类
	public Bezier myBezier;

	//曲线的对象
	public GameObject Yellowline;

	public Transform startPoint;

	public Transform endPoint;

	public Transform p1;

	public Transform p2;

	//曲线对象的曲线组件
	private LineRenderer YellowlineRenderer;

	//拖动条用来控制贝塞尔曲线的两个点
	public float p1x;
	public float p1y;
	public float p2x;
	public float p2y;

	Vector3 ctr1 { get { return p1.localPosition; } }
	Vector3 ctr2 { get { return p2.localPosition; } }

	// Use this for initialization
	void Start () {
		//得到曲线组件
		YellowlineRenderer = Yellowline.GetComponent<LineRenderer>();
		//为了让曲线更加美观，设置曲线由100个点来组成
		YellowlineRenderer.SetVertexCount(100);

		//MoveBezier(center, endPoint.position, ctr1, ctr2, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.B)) {
			//MoveCircle(transform, center.position, 360, 5);
			//MoveLine(transform, transform.forward, 3, 3);
		}

		//在这里来计算贝塞尔曲线
		//四个参数 表示当前贝塞尔曲线上的4个点 第一个点和第四个点
		//我们是不需要移动的，中间的两个点是由拖动条来控制的。
		//myBezier = new Bezier(new Vector3(-5f, 0f, 0f), new Vector3(hSliderValue1, 0, hSliderValue0), new Vector3(hSliderValue1, 0, hSliderValue0), new Vector3(5f, 0f, 0f));
		myBezier = new Bezier(startPoint.position, ctr1, ctr2, endPoint.position);


		//循环100遍来绘制贝塞尔曲线每个线段
		for(int i = 1; i <= 100; i++) {
			//参数的取值范围 0 - 1 返回曲线没一点的位置
			//为了精确这里使用i * 0.01 得到当前点的坐标
			Vector3 vec = myBezier.GetPointAtTime((float)(i * 0.01));
			//把每条线段绘制出来 完成白塞尔曲线的绘制
			YellowlineRenderer.SetPosition(i - 1, vec);
		}
	}

	void OnGUI()
	{
		//拖动条得出 -5.0 - 5.0之间的一个数值
		p1y = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), p1y, -5.0F, 5.0F);
		p1x = GUI.HorizontalSlider(new Rect(25, 70, 100, 30), p1x, -5.0F, 5.0F);
	}


	void OnTriggerEnter(Collider other)
	{
		print("OnTriggerEnter");
	}

	static void MoveCircle(Transform target, Vector3 center, float amount, float duration)
	{
		/*if(GetComponent<iTween>()) {
			return;
		}*/

		Vector3 dirVec = target.position - center;
		float startAngle = Vector3.Angle(dirVec, Vector3.forward);
		//模型到中心点向量与z轴正方向夹角
		startAngle = Vector3.Cross(dirVec, Vector3.forward).y > 0 ? -startAngle : startAngle;

		float radio = Vector3.Distance(center, target.position);
		//模型本身的y轴旋转欧拉角
		float localAngle = target.eulerAngles.y;


		iTween.ValueTo(target.gameObject, iTween.Hash(
						"time", duration,
						"from", startAngle,
						"to", amount + startAngle,
						"onupdate", (Action<float>)((value)=>{
							target.position = new Vector3(Mathf.Sin(value * Mathf.Deg2Rad) * radio, target.position.y, Mathf.Cos(value * Mathf.Deg2Rad) * radio) + center;
							target.eulerAngles = new Vector3(0, value + localAngle - startAngle, 0);
						}),
						"oncomplete", (Action)(()=>print("complete"))));
	}

	void MoveLine(Transform target, Vector3 direction, float amount, float duration)
	{
		iTween.ValueTo(target.gameObject, iTween.Hash(
						"time", duration,
						"from", target.position,
						"to", target.position + direction * amount,
						"onupdate", (Action<Vector3>)((value) => {
							target.position = value;
						}),
						"oncomplete", (Action)(() => print("MoveLine"))));
	}

	void MoveBezier(Transform target, Vector3 destination, Vector3 ctrl1, Vector3 ctrl2, float duration)
	{
		Bezier line = new Bezier(target.position, ctrl1, ctrl2, destination);
		iTween.ValueTo(target.gameObject, iTween.Hash(
						"time", duration,
						"from", 0,
						"to", 1,
						"onupdate", (Action<float>)((value) => {
							target.position = line.GetPointAtTime(value);
						})));
	}
	
}
