using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualControlStartRomPos : MonoBehaviour
{
    public Vector2 RomX;
    void Start()
    {
        transform.position = new Vector3(Random.Range(RomX.x,RomX.y) , transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
