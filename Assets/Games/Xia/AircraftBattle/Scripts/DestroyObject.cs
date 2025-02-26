using UnityEngine;
using WGM;

public class DestroyObject : MonoBehaviour {

	public float time = 3f;
	void Start () 
	{
		var audsou = GetComponent<AudioSource>();
		if (audsou != null)
			audsou.volume  *= LibWGM.machine.SeVolume /10f;
		GameObject.Destroy(this.gameObject,time);
	}
	

}
