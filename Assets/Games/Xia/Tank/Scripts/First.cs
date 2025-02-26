using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First : MonoBehaviour {
    public float speed;
    public float jump;

    private Transform camTrans;
    private Vector3 camAng;
    private float camHeight = 5f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        camTrans = Camera.main.transform;
        Vector3 startpos = transform.position;
        startpos.y += camHeight;
        camTrans.position = startpos;
        camTrans.rotation = transform.rotation;
        camAng = camTrans.eulerAngles;
    }
    void FixedUpdate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Rot_move();
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * -speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * -speed);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * jump);
        }

    }
    void Rot_move()
    {
        float y = Input.GetAxis("Mouse X");
        float x = Input.GetAxis("Mouse Y");
        camAng.x -= x;
        camAng.y += y;
        
        camTrans.eulerAngles = camAng;
        camTrans.position = new Vector3(this.transform.position.x, camTrans.position.y, this.transform.position.z);
        float camy = camAng.y;

        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, camy, this.transform.eulerAngles.z);

        Vector3 startPos = transform.position;

        startPos.y += camHeight;

        camTrans.position = startPos;
    }
        
}
