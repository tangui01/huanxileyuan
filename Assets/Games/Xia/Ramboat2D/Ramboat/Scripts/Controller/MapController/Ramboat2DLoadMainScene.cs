using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Ramboat2DLoadMainScene : MonoBehaviour {
	Animator animconvert;
	// Use this for initialization
	void Awake(){
		
	}
	void Start () {
		DontDestroyOnLoad (gameObject);
		animconvert = GetComponentInChildren<Animator> ();
		StartCoroutine (InLoadScene ());
	}
	
	// Update is called once per frame
	void Update () {
	}
	public IEnumerator InLoadScene(){
		yield return new WaitForSeconds (1.5f);
		animconvert.SetTrigger ("First");
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.openCloseDoor [1]);
		yield return new WaitForSeconds (1f);
		if(!SceneManager.GetActiveScene ().name.Equals("MainScene")) {
			SceneManager.LoadScene ("MainScene");
		}
		while (!SceneManager.GetActiveScene ().name.Equals("MainScene")) {
			yield return new WaitForFixedUpdate ();
		}
		animconvert.SetTrigger ("Out");
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.openCloseDoor [0]);

//		yield return new WaitForSeconds(1.5f);
//		animconvert.SetTrigger ("First");
//		yield return new WaitForSeconds (1f);
//		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
//		AsyncOperation async = Application.LoadLevelAsync("MainScene");
//
//		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
//		while (!async.isDone) {
//			yield return null;
//		}
//		animconvert.SetTrigger ("Out");
	}

}
