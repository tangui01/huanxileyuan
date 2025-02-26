using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPos : MonoBehaviour
{
    public Vector3 diretion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position +diretion);
    }
    
}
