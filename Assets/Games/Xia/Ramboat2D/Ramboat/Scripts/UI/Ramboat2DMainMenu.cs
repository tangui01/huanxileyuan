using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Ramboat2DMainMenu : MonoBehaviour {
	public Text score;
	public Text coinCollected;
	public Text yourCashCoin;
	public Image avatarPlayer;
	public Image avatarBoat;
	public Image[] garget;
	public Image[] guns;
	public Image[] missionImage;
	public Text[] missionText;
	public Button[] skip;
	// Use this for initialization
	void Start () {
		coinCollected.text = Ramboat2DLevelManager.THIS.coinCollected.ToString ();
		avatarPlayer.sprite = Ramboat2DLevelManager.THIS.avatarPlayer [PlayerPrefs.GetInt("ChoosePlayer")];
		avatarBoat.sprite = Ramboat2DLevelManager.THIS.avatarPlayer [PlayerPrefs.GetInt("ChooseBoat")];
		yourCashCoin.text = Ramboat2DPlayerController.Intance.textCoinCollect.text; //PlayerPrefs.GetFloat ("CoinCollected").ToString ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
