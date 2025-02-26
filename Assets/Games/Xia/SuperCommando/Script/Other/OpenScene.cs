using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScene : MonoBehaviour
{
    public string sceneName = "scene name";

   public void Open()
    {
        SuperCommandoGlobalValue.Instance.levelPlaying = -1;
        MainMenuHomeScene.Instance.LoadScene(sceneName);
    }
}
