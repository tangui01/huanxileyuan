using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonPath : MonoBehaviour
{

    public string pathName = "";
    public Color pathColor = Color.red;
    public List<Vector3> nodes = new List<Vector3>() { Vector3.zero, Vector3.zero };
    public bool initialized = false;
    public string initialName = "";
    [HideInInspector]
    public Vector3[] pathPoint;
 
    void OnDrawGizmosSelected()
    {
		if(enabled && !Application.isPlaying)
        {
            if (nodes.Count > 1)
            {
                GetPoint();
                iTween.DrawPath(pathPoint, pathColor);
            }
        }
    }

    public Vector3[] GetPoint()
    {
        pathPoint = nodes.ToArray();
        for (int i = 0; i < pathPoint.Length; i++)
            pathPoint[i] = transform.TransformPoint(pathPoint[i]);

        return pathPoint;
    }
}
