using UnityEngine;
using System.Collections;

public class StarDestroyer : MonoBehaviour {

	Transform mainCameraPosition;
	public bool dragging = false;
	Transform parentPosition;

	void Start()
	{
		mainCameraPosition = Camera.main.transform;
		parentPosition = transform.parent;
	}

	void OnBecameInvisible()
	{
		if(transform.parent.parent.gameObject != null && mainCameraPosition != null)
		{
			if(transform.position.y < mainCameraPosition.position.y - 12f)
				Destroy(transform.parent.parent.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag.Equals("Magnet"))
		{
			dragging = true;
			StartCoroutine(MagnetDrag());
		}
	}

	public IEnumerator MagnetDrag()
	{
		while(dragging)
		{
			Vector3 target = new Vector3(PandaPlane.Instance.transform.position.x, PandaPlane.Instance.transform.position.y, parentPosition.position.z);
			parentPosition.position = Vector3.MoveTowards(parentPosition.position,target,25*Time.deltaTime);
			yield return null;
		}
	}
}
