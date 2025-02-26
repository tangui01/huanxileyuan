using UnityEngine;
using System.Collections;

public class ComicScene : MonoBehaviour {

	string clickedItem;
	string releasedItem;

	// Use this for initialization
	void Start () 
	{
		SoundManager.soundOn = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			clickedItem = RaycastFunction(Input.mousePosition);
		}
		if(Input.GetMouseButtonUp(0))
		{
			releasedItem = RaycastFunction(Input.mousePosition);

			if(clickedItem.Equals(releasedItem))
			{
				if(releasedItem.Equals("Main Camera"))
				{
					GetComponent<Collider>().enabled = false;
					transform.Find("SkipText").gameObject.SetActive(true);
				}
				else if(releasedItem.Equals("SkipText"))
				{

					StartCoroutine(OpenMainScene());
				}
			}
		}
	}

	IEnumerator OpenMainScene()
	{
		SoundManager_ComicScene.Instance.Stop_ComicMusic();
		transform.Find("SkipText").GetComponent<Collider>().enabled = false;
		transform.Find("BlackBackground").GetComponent<Animation>().Play();
		yield return new WaitForSeconds(1f);
		Application.LoadLevel("MainScene");
	}

	string RaycastFunction(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 35))
		{
			return hit.collider.name;
		}
		return System.String.Empty;
	}
}
