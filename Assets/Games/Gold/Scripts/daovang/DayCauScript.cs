using System;
using UnityEngine;
using System.Collections;

public class DayCauScript : MonoBehaviour {
    public static DayCauScript instance; 
	public Vector3 angles;
	private float speed = 3;
	public float angleMax = 70;
	public TypeAction typeAction = TypeAction.Nghi;
	private Vector3 initAngles;
    public float rotationDay;


    private LineRenderer line;
    // Use this for initialization
    private void Awake()
    {
	    if (instance==null)
	    {
		    instance = this;
	    }
	    else
	    {
		    Destroy(gameObject);
	    }
    }

    void Start() 
    {
        line = GetComponent<LineRenderer>();
        initAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
	// Update is called once per frame
	void Update () {
		line.SetPosition(0, transform.position);
		line.SetPosition(1, LuoiCauScript.instance.gameObject.transform.position);
	}

	void FixedUpdate() {
		if(speed > 0 && typeAction == TypeAction.Nghi&&GoldMinerGameManager.instance.gameState==EnumStateGame.Play)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * speed) * angleMax);
        }
        rotationDay = transform.rotation.z;	
	}
}
