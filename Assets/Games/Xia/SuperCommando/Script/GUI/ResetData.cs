using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetData : MonoBehaviour
{
    SuperCommandoSoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SuperCommandoSoundManager>();
        CharacterHolder.Instance.GetPickedCharacter();
    }

    public void Reset()
    {
        bool _removeAd = SuperCommandoGlobalValue.Instance.RemoveAds;
        PlayerPrefs.DeleteAll();
        SuperCommandoGlobalValue.Instance.RemoveAds = _removeAd;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        SuperCommandoSoundManager.Instance.PlaySfx(soundManager.soundClick);
    }

    public void UnlockAll()
    {
        PlayerPrefs.SetInt(SuperCommandoGlobalValue.Instance.WorldReached, int.MaxValue);
        for (int i = 1; i < 100; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 10000);        //world i, unlock 10000 levels
        }

        SuperCommandoGlobalValue.Instance.LevelHighest = 9999;

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        SuperCommandoSoundManager.Instance.PlaySfx(soundManager.soundClick);
    }
}
