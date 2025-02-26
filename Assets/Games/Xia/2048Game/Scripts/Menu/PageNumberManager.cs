using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PageNumberManager : MonoBehaviour
{
    public Color normalColor = Color.blue;
    public Color highlightedColor=Color.red;
    private SilderPanel silderPanel;
    private Image[] imageArray;
    private int prevNumber;
    private void Start()
    { 
        silderPanel =FindObjectOfType<SilderPanel>();
        imageArray = new Image[silderPanel.panelCount]; 
        for (int i = 0; i < imageArray.Length; i++)
        {
            imageArray[i] = CreatePageNumber(i);
        }
        prevNumber = silderPanel.currentIndex;
        imageArray[prevNumber].color = highlightedColor; 
	}

    private Image CreatePageNumber(int number)
    {
        GameObject go = new GameObject(number.ToString());
        go.transform.SetParent(this.transform, false);
        Image image =  go.AddComponent<Image>();
        image.color = normalColor;
        return image;
    }

    public void ChangePageNumber()
    {
        imageArray[prevNumber].color = normalColor;
        imageArray[silderPanel.currentIndex].color = highlightedColor;
        prevNumber = silderPanel.currentIndex;
    }
}
