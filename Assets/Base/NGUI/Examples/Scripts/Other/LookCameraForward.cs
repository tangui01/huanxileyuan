using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraForward : MonoBehaviour
{
    Transform camTran;
    // Start is called before the first frame update
    void Start()
    {
        camTran = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = transform.eulerAngles;
        pos.z = 0;
;        transform.eulerAngles = pos;
        transform.forward = camTran.forward;
        
    }
}
