using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldChooseUI : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float smooth = 1000;

    void Awake()
    {
    }

    private IEnumerator Start()
    {
        yield return null;
        if (SuperCommandoGlobalValue.Instance.currentHighestLevelObj)
        {
            var playerPosX = SuperCommandoGlobalValue.Instance.currentHighestLevelObj.position.x;
            var limitPosX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 0)).x;
            //Debug.LogError(playerPosX + "-" + limitPosX);
            while (playerPosX > limitPosX)
            {
                //Debug.LogError(playerPosX);
                scrollbar.value += Time.deltaTime;
                playerPosX = SuperCommandoGlobalValue.Instance.currentHighestLevelObj.position.x;
                limitPosX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 0)).x;
                //yield return null;
            }
        }
        //if (levelSeletected != null && levelSeletected.GetComponent<MainMenu_Level>())
        //{
        //    var screenPos = Camera.main.ScreenToViewportPoint(levelSeletected.transform.position);
        //    var delta = screenPos.x - 0.5f;
        //    if (Mathf.Abs(delta) > 0.05f)
        //    {
        //        scrollbar.value += delta / smooth;
        //        scrollbar.value = Mathf.Clamp01(scrollbar.value);
        //    }
        //}
    }

    //private void Update()
    //{
    //    //var levelSeletected = EventSystem.current.currentSelectedGameObject;
    //    var levelSeletected = GlobalValue.currentHighestLevelObj;

    //    if (levelSeletected !=null && levelSeletected.GetComponent<MainMenu_Level>())
    //    {
    //        var screenPos = Camera.main.ScreenToViewportPoint(levelSeletected.transform.position);
    //        var delta = screenPos.x - 0.5f;
    //        if(Mathf.Abs( delta) > 0.05f)
    //        {
    //            scrollbar.value += delta / smooth;
    //            scrollbar.value = Mathf.Clamp01(scrollbar.value);
    //        }
    //    }
    //}

    public void Back_performed()
    {
        MainMenuHomeScene.Instance.OpenStartMenu();
        
    }

    private void OnEnable()
    {
        int numberOfLevels = FindObjectsOfType<MainMenu_Level>().Length;
        SuperCommandoGlobalValue.Instance.totalLevel = numberOfLevels;
    }
}