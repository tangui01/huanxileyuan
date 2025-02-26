using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MainMenu_Level : MonoBehaviour
{
    public int levelNumber = 0;
    public GameObject imgLock, imgOpen, imgPass;
    public Text TextLevel;

    void Start()
    {
        imgLock.SetActive(false);
        imgOpen.SetActive(false);
        imgPass.SetActive(false);

        if (GetComponent<Animator>())
            GetComponent<Animator>().enabled = levelNumber == SuperCommandoGlobalValue.Instance.LevelHighest;
        GetComponent<Button>().interactable = levelNumber <= SuperCommandoGlobalValue.Instance.LevelHighest;

        if (levelNumber == SuperCommandoGlobalValue.Instance.LevelHighest)
            SuperCommandoGlobalValue.Instance.currentHighestLevelObj = transform;

        if (levelNumber <= SuperCommandoGlobalValue.Instance.LevelHighest)
        {
            TextLevel.gameObject.SetActive(true);
            TextLevel.text = levelNumber.ToString();

            imgOpen.SetActive(levelNumber == SuperCommandoGlobalValue.Instance.LevelHighest);
            imgPass.SetActive(levelNumber < SuperCommandoGlobalValue.Instance.LevelHighest);
        }
        else
        {
            TextLevel.gameObject.SetActive(false);
            imgLock.SetActive(true);
        }
        
        
    }

    public void LoadScene()
    {
        SuperCommandoGlobalValue.Instance.levelPlaying = levelNumber;
        MainMenuHomeScene.Instance.LoadScene("Level " + SuperCommandoGlobalValue.Instance.levelPlaying);
    }
}