using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_StartScreen : MonoBehaviour {
    public Text worldTxt;

    void Start()
    {
        if (SuperCommandoGlobalValue.Instance.levelPlaying == -1)
        {
            worldTxt.text = "TEST GAMEPLAY";
        }
        else
        {
            worldTxt.text = "LEVEL: " + SuperCommandoGlobalValue.Instance.levelPlaying;
        }
    }
}
