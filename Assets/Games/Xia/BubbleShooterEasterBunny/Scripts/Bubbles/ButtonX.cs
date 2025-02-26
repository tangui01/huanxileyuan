using UnityEngine;
using System.Collections;

public class ButtonX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnMouseDown()
    {
        if (name == "Change" && BubbleGamePlay.Instance.GameStatus == BubbleGameState.Playing)
        {
            mainscript.Instance.ChangeBoost();
        }

    }
	
	// Update is called once per frame
	void OnPress (bool press) {
        if (press) return;
 	}
}
