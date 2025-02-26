using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private Animator _animtor;
	private CanvasGroup _cancvasGroup;

	public bool IsOpen
	{
		get
		{
			return _animtor.GetBool("IsOpen");
		}
		set
		{
			_animtor.SetBool("IsOpen", value);
		}
	}

	// Use this for initialization
	public void Awake () 
	{
		_animtor = GetComponent<Animator> ();
		_cancvasGroup = GetComponent<CanvasGroup> ();

		var rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}



	public void ResetObject()
	{
		gameObject.SetActive (false);
	}

	public void LoadScene(string levelName)
	{
		Application.LoadLevel (levelName);
	}

	public void LoadSceneAsync(string levelName)
	{
		Application.LoadLevelAsync (levelName);
	}

	
	public void DisableObject(string gameObjectName)
	{
		GameObject gameObject= GameObject.Find (gameObjectName);
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}

	
	// Update is called once per frame
	//void Update () 
	//{
//		if (!_animtor.GetCurrentAnimatorStateInfo (0).IsName ("Open")) 
//		{
//			_cancvasGroup.blocksRaycasts = _cancvasGroup.interactable = false;
//		} 
//		else 
//		{
//			_cancvasGroup.blocksRaycasts = _cancvasGroup.interactable = true;
//		}
	//}

}
