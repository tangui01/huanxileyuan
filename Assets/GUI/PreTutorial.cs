using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreTutorial : MonoBehaviour {
    public Sprite[] pictures;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = pictures[(int)LevelData.mode]; 
        AudioManager.Instance.playerEffect1(SoundBase.Instance.swish[0] );

	}
	
	// Update is called once per frame
	public void  Stop() {
        AudioManager.Instance.playerEffect1(SoundBase.Instance.swish[1]);
        BubbleGamePlay.Instance.GameStatus = BubbleGameState.Tutorial;
        // FindObjectOfType<TimeUIController>().isStopTime = false;
        gameObject.SetActive( false );
	}
	
}
