using UnityEngine;
using System.Collections;

public class LifeOfTime : MonoBehaviour {
    public float time=2;
    WaitForSeconds t;

	// Use this for initialization
	void OnEnable () {
        if(t==null)
	       t = new WaitForSeconds(time);
        StartCoroutine(UnActiveThis());
	}
    IEnumerator UnActiveThis()
    {
        yield return t;
        gameObject.SetActive(false);
    
    }
	// Update is called once per frame
	
}
