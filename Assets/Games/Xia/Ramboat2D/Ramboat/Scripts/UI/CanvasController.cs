using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public enum TypeBonus
{
	NULL,
	ONEPAIR,
	TWOPAIR,
	THREEOFAKIND,
	STRAIGHT,
	FLUSH,
	FULLHOUSE,
	FLUSHFOUROFAKIND,
}

public class CanvasController : MonoBehaviour
{
	public static CanvasController THIS;
	public Sprite[] spriteCards;
	public Sprite[] spriteNumber;
	public Sprite[] spriteAvatar;
	public GameObject button;
	private TypeBonus typeBonus;
	[SerializeField]
	private Sprite[] formCardImage;
	[SerializeField]
	private string[] formCard;
	[SerializeField]
	private string[] formBonus;
	private Hashtable cardRandoms;
	private Hashtable chooseCard;
	private int numberCouple;
	List<int> chooseKeyHaveSameCard;
	public GameObject bonusCanvas;
	public Text numberPocker;
	public Text yourCoin;
	public GameObject buttonDraw;
	public GameObject buttonClose;
	public GameObject buttonBack;
	public Button buttonCash;
	public GameObject cashAnimationObj;
	//bonus
	public Image imgForm;
	public Text formText;
	public Text bonusText;

	bool loadScene;
	int gunType;
	[SerializeField]
	private GameObject canvasPrizesTutorial;
	int[] m_card;
	int[] sub = { };

	void Awake ()
	{
		THIS = this;

	}

	void OnEnable ()
	{
		Ramboat2DFXSound.THIS.fxSound.clip = Ramboat2DFXSound.THIS.none;
		Ramboat2DFXSound.THIS.fxSound.loop = false;
		Ramboat2DFXSound.THIS.fxSound.Stop ();
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.scrollClick);
		numberPocker.text = "x" + PlayerPrefs.GetInt ("Pocker").ToString ();
		yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		buttonDraw.SetActive (true);
		buttonClose.SetActive (false);
		gunType = gunType = Random.Range (1, 5);;
		if (PlayerPrefs.GetInt ("Pocker") > 0) {
			buttonCash.gameObject.SetActive (true);
		}

	}

	IEnumerator RandomCard ()
	{
		float m_numberPocker = PlayerPrefs.GetInt ("Pocker");
		if (m_numberPocker > 0) {
			PlayerPrefs.SetInt ("Pocker", PlayerPrefs.GetInt ("Pocker") - 1);
			PlayerPrefs.Save ();
			numberPocker.text = "x" + PlayerPrefs.GetInt ("Pocker").ToString ();
			for (int i = 0; i < 5; i++) {
				GameObject canvas = transform.GetChild (i).gameObject;
				canvas.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (canvas.GetComponent<RectTransform> ().anchoredPosition.x, 0, 0);
			}
			cardRandoms = new Hashtable ();
			cardRandoms.Clear ();
			chooseCard = new Hashtable ();
			chooseCard.Clear ();
			m_card = new int[5];
			chooseKeyHaveSameCard = new List<int> ();
			IEnumerable listCardDifferent;
			typeBonus = TypeBonus.NULL;
			numberCouple = 0;
			for (int i = 0; i < 5; i++) {
				//yield return new WaitForSeconds (0.1f);
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.flipCard[Random.Range(0,3)]);
				int cardType = Random.Range (0, 4);
				int cardNumber = Random.Range (0, 13);
				string choose = string.Concat (cardType.ToString (), cardNumber.ToString ());
				Card card = new Card (cardType, cardNumber);
			
				while (chooseCard.ContainsValue (choose)) {
					cardType = Random.Range (0, 4);
					cardNumber = Random.Range (0, 13);
					card = new Card (cardType, cardNumber);
					choose = string.Concat (cardType.ToString (), cardNumber.ToString ());
				}
				cardRandoms.Add (i, card);
				chooseCard.Add (i, choose);
				m_card [i] = card.numberCard;
			
			}
			listCardDifferent = m_card.Union (sub);
			ICollection key = cardRandoms.Keys;

			foreach (int b in listCardDifferent) {
				int j = 0;
				List<int> addNumber = new List<int> ();
				foreach (int k in key) {
					Card a = (Card)cardRandoms [k];
						
					if (a.numberCard == b) {
						j++;
						addNumber.Add (k);
					}
				}
				if (j > 1) {
					j = 0;
					numberCouple++;
					chooseKeyHaveSameCard.AddRange (addNumber);
				}
			}
			yield return new WaitForSeconds (0.8f);
			if (chooseKeyHaveSameCard != null) {
				if (numberCouple == 1 && chooseKeyHaveSameCard.Count == 2) {
					typeBonus = TypeBonus.ONEPAIR;
				}
				if (numberCouple == 1 && chooseKeyHaveSameCard.Count == 3) {
					typeBonus = TypeBonus.THREEOFAKIND;
				}
				if (numberCouple == 2 && chooseKeyHaveSameCard.Count == 4) {
					typeBonus = TypeBonus.TWOPAIR;
				}
				if (numberCouple == 2 && chooseKeyHaveSameCard.Count == 5) {
					typeBonus = TypeBonus.STRAIGHT;
				}
				if (numberCouple == 1 && chooseKeyHaveSameCard.Count == 4) {
					typeBonus = TypeBonus.FLUSHFOUROFAKIND;
				}

				foreach (int k in chooseKeyHaveSameCard) {
					GameObject canvas = transform.GetChild (k).gameObject;
					canvas.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (canvas.GetComponent<RectTransform> ().anchoredPosition.x, 30, 0);
				}
			} else {
				bool list02468 = true;
				bool list12345 = true;
				List<int> addNumber = new List<int> ();
				foreach (int b in listCardDifferent) {
					addNumber.Add (b);
				}
				addNumber.Sort ();
				for (int i = 0; i < 4; i++) {
					if ((addNumber [i + 1] - addNumber [i]) != 1) {
						list12345 = false;
						break;
					}
				}
				for (int j = 0; j < 4; j++) {
					if ((addNumber [j + 1] - addNumber [j]) != 2) {
						list12345 = false;
						break;
					}
				}

				if (list02468) {
					typeBonus = TypeBonus.STRAIGHT;
				}
				if (list12345) {
					typeBonus = TypeBonus.FLUSH;
				}
				if (!list02468 && !list12345) {
					typeBonus = TypeBonus.NULL;
				}
			}

			SetBonus (typeBonus);
		} 
		if (PlayerPrefs.GetInt ("Pocker") == 0) {
			buttonDraw.SetActive (false);
			buttonClose.SetActive (true);
			buttonCash.gameObject.SetActive (false);
		}

	}

	public void ButtonClicked ()
	{
		if (!loadScene) {
			gunType = Random.Range (1, 5);
			StartCoroutine (RandomCard ());
			StartCoroutine (ButtonClickDelay ());
		}
	}

	IEnumerator ButtonClickDelay ()
	{
		if(!loadScene){
		button.GetComponent<Button> ().interactable = false;
		int i = 0;
		foreach (DictionaryEntry card in cardRandoms) {
			TestAnimState cardChildren = transform.GetChild (i).GetComponent<TestAnimState> ();
			Card a = (Card)card.Value;
			cardChildren.SetCard (a);
			i++;
//			yield return WaitForSeconds (0.1f);
		}


		yield return new WaitForSeconds (1f);
		button.GetComponent<Button> ().interactable = true;

	}
}

	void SetBonus (TypeBonus type)
	{
		
		if (type == TypeBonus.ONEPAIR) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			GameObject obj = bonusCanvas.transform.GetChild (1).gameObject;
			obj.transform.GetChild (1).GetComponent<Image> ().sprite = Ramboat2DLevelManager.THIS.gunTypeSprite [gunType];
			obj.SetActive (true);
		} else if (type == TypeBonus.TWOPAIR) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			bonusCanvas.transform.GetChild (2).gameObject.SetActive (true);
		} else if (type == TypeBonus.THREEOFAKIND) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			bonusCanvas.transform.GetChild (2).gameObject.SetActive (true);
			
		} else if (type == TypeBonus.STRAIGHT) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			bonusCanvas.transform.GetChild (2).gameObject.SetActive (true);
			
		} else if (type == TypeBonus.FLUSH) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			bonusCanvas.transform.GetChild (2).gameObject.SetActive (true);
			
		} else if (type == TypeBonus.FULLHOUSE) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			bonusCanvas.transform.GetChild (2).gameObject.SetActive (true);
			
		} else if (type == TypeBonus.FLUSHFOUROFAKIND) {
			Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.bonus);
			bonusCanvas.SetActive (true);
			imgForm.sprite = formCardImage [(int)type - 1];
			imgForm.SetNativeSize ();
			formText.text = formCard [(int)type - 1];
			bonusText.text = formBonus [(int)type - 1];
			bonusCanvas.transform.GetChild (2).gameObject.SetActive (true);
			
		}

	}

	public void GetIt ()
	{
		if (typeBonus == TypeBonus.NULL) {
		} else if (typeBonus == TypeBonus.ONEPAIR) {
			
			if (gunType == 1) {
				PlayerPrefs.SetInt ("Free1", PlayerPrefs.GetInt ("Free1") + 1);
			} else if (gunType == 2) {
				PlayerPrefs.SetInt ("Free2", PlayerPrefs.GetInt ("Free2") + 1);
			} else if (gunType == 3) {
				PlayerPrefs.SetInt ("Free3", PlayerPrefs.GetInt ("Free3") + 1);
			} else if (gunType == 4) {
				PlayerPrefs.SetInt ("Free4", PlayerPrefs.GetInt ("Free4") + 1);
			}
		} else if (typeBonus == TypeBonus.TWOPAIR) {
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 200);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		} else if (typeBonus == TypeBonus.THREEOFAKIND) {
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 300);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		} else if (typeBonus == TypeBonus.STRAIGHT) {
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 400);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		} else if (typeBonus == TypeBonus.FLUSH) {
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 500);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		} else if (typeBonus == TypeBonus.FULLHOUSE) {
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 600);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		} else if (typeBonus == TypeBonus.FLUSHFOUROFAKIND) {
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 700);
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
		}
		PlayerPrefs.Save ();
		bonusCanvas.transform.GetChild (1).gameObject.SetActive (false);
		bonusCanvas.transform.GetChild (2).gameObject.SetActive (false);
		bonusCanvas.SetActive (false);
	}

	public void CloseMenu ()
	{
		ReadWriteTextMission.THIS.CheckMission (3);
		if (!loadScene) {
			
			if (ReadWriteTextMission.THIS.numberMissionComplete.Count > 0) {
				// neu co thanh tuu thi goi menu thanh tuu o day
				GameObject.Find ("Canvas").transform.GetChild (8).gameObject.SetActive (true);
				Time.timeScale = 0;
				transform.parent.gameObject.SetActive (false);
			} else {
				if (PlayerPrefs.GetInt ("PokerClick") == 0) {
					loadScene = true;
					PlayerPrefs.SetInt ("PokerClick", 0);
					PlayerPrefs.Save ();
					StartCoroutine (Ramboat2DPlayerController.Intance.LoadScene ());
				} else {
					loadScene = false;
					PlayerPrefs.SetInt ("PokerClick", 0);
					PlayerPrefs.Save ();
					GameObject.Find ("Canvas").transform.GetChild (5).gameObject.SetActive (false);
				}
			}
		}
	}

	public void PrizesClickEnable ()
	{
		canvasPrizesTutorial.SetActive (true);
	}

	public void prizesClickDisable ()
	{
		canvasPrizesTutorial.SetActive (false);
	}

	public void CashClick ()
	{
		if (!loadScene) {
			StartCoroutine (CanClickCash ());
		}
	}

	IEnumerator CanClickCash ()
	{
		if (PlayerPrefs.GetInt ("Pocker") > 0) {
			PlayerPrefs.SetInt ("Pocker", PlayerPrefs.GetInt ("Pocker") - 1);
			PlayerPrefs.SetFloat ("CoinCollected", PlayerPrefs.GetFloat ("CoinCollected") + 100);
			numberPocker.text = "x  " + PlayerPrefs.GetInt ("Pocker").ToString ();
			yourCoin.text = PlayerPrefs.GetFloat ("CoinCollected").ToString ();
			PlayerPrefs.Save ();
			cashAnimationObj.SetActive (true);
			buttonCash.interactable = false;
	
			yield return new WaitForSeconds (0.5f);
			cashAnimationObj.SetActive (false);
			buttonCash.interactable = true;
		}
		if (PlayerPrefs.GetInt ("Pocker") == 0) {
			buttonCash.gameObject.SetActive (false);
			buttonDraw.SetActive (false);
			buttonClose.SetActive (true);
		}
	}
}