using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QanminTuoPaoTip : MonoBehaviour
{
    public GameObject demo, game;
    int cnt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cnt++;
        cnt %= 10;
        if (cnt != 0)
            return;
        if (!FreeGameController.isFreeGame)
        {
            demo.SetActive(false);
            game.SetActive(false);
            return;
        }

        if (SceneManager.GetActiveScene().name == "Demo")
        {
            demo.SetActive(false);
            game.SetActive(true);
        }
        else {
            demo.SetActive(false);
            game.SetActive(true);

        }
    }
}
