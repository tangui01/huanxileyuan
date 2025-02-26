using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestAnimState : MonoBehaviour
{
	Animator anim;
	int numberClicked;
	GameObject canvasCard;
	GameObject canvasNumber;
	GameObject canvasAvatar;

	Image imgAvatar,imgCard,imgNumber;
	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		canvasCard = transform.GetChild (0).gameObject;
		canvasNumber = transform.GetChild (1).gameObject;
		canvasAvatar=transform.GetChild (2).gameObject;
		imgAvatar = canvasAvatar.GetComponent<Image> ();
		imgCard = canvasCard.GetComponent<Image> ();
		imgNumber = canvasNumber.GetComponent<Image> ();
	}
	public void SetCard (Card card)
	{
		if (card.numberCard == 9) {
			//j
			Color color=imgAvatar.color;
			color.a = 1;
			imgAvatar.color = color;
			imgAvatar.sprite=CanvasController.THIS.spriteAvatar[0];
			imgAvatar.SetNativeSize ();
		} else if (card.numberCard ==10 ) {
			//q
			Color color=imgAvatar.color;
			color.a = 1;
			imgAvatar.color = color;
			imgAvatar.sprite=CanvasController.THIS.spriteAvatar[1];
			imgAvatar.SetNativeSize ();
		} else if (card.numberCard == 11) {
			Color color=imgAvatar.color;
			color.a = 1;
			imgAvatar.color = color;
			imgAvatar.sprite=CanvasController.THIS.spriteAvatar[2];
			imgAvatar.SetNativeSize ();
			//k
		}else if(card.numberCard==12){
			Color color=imgAvatar.color;
			color.a = 1;
			imgAvatar.color = color;
			imgAvatar.sprite=CanvasController.THIS.spriteAvatar[3];
			imgAvatar.SetNativeSize ();
			//at
		}else{
			Color color=imgAvatar.color;
			color.a = 0;
			imgAvatar.color = color;
		}
		if (card.cardType == 0 || card.cardType == 1) {
			imgCard.sprite = CanvasController.THIS.spriteCards [card.cardType];
			imgNumber.sprite = CanvasController.THIS.spriteNumber [card.numberCard];
			imgNumber.SetNativeSize ();
			imgCard.color = Color.red;
			imgNumber.color = Color.red;
		} else {
			imgCard.sprite = CanvasController.THIS.spriteCards [card.cardType];
			imgNumber.sprite = CanvasController.THIS.spriteNumber [card.numberCard];
			imgNumber.SetNativeSize ();
			imgCard.color = Color.black;
			imgNumber.color = Color.black;
		}
		numberClicked += 1;

		if (numberClicked == 1)
			anim.SetTrigger ("One");
		else if (numberClicked == 2) {
			numberClicked = 0;
			anim.SetTrigger ("Two");
		}

	}

	public void SetImage ()
	{
		canvasCard.SetActive (true);
		canvasNumber.SetActive (true);
		canvasAvatar.SetActive (true);
	}

	public void SetFalseImage ()
	{
		canvasCard.SetActive (false);
		canvasNumber.SetActive (false);
		canvasAvatar.SetActive (false);
	}

}
