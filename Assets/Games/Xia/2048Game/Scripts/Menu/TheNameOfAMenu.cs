using UnityEngine;
using System.Collections;


//菜单界面的显示和逻辑
public class TheNameOfAMenu : MonoBehaviour
{
    private GameObject menuPanel;
    private Transform canvasTF;
    private SilderPanel silderPanel;
    private CanvasGroup canvasGroup;
    private Vector3 menuOldPosition;
    private TheNameOfAGameController gameController;
    public float moveSpeed = 300;
    private void Start()
    {
        menuOldPosition = this.transform.position;
        menuPanel = this.transform.gameObject;
        canvasTF = this.transform.parent;
        silderPanel = FindObjectOfType<SilderPanel>();
        canvasGroup = silderPanel.GetComponent<CanvasGroup>();
        SetPageRaycasState(false);
        gameController = GameObject.FindObjectOfType<TheNameOfAGameController>();
    }
    /// <summary>
    /// 隐藏菜单
    /// </summary>
    public void HideMenu()
    {
        iTween.MoveTo(menuPanel, iTween.Hash(
                "position", menuOldPosition, "easetype", iTween.EaseType.easeOutExpo,
                "speed", moveSpeed
         ));
        //设置滑动页面页码
        silderPanel.SetIndex(0);
        SetPageRaycasState(false);
    }
    /// <summary>
    /// 显示菜单
    /// </summary>
    public void DisplayMenu()
    {
        iTween.MoveTo(menuPanel, iTween.Hash(
             "position", canvasTF.position, "easetype", iTween.EaseType.easeOutExpo,
             "speed", moveSpeed, "oncomplete", "SetPageRaycasState", "oncompleteparams", true
       ));
    }
    private void SetPageRaycasState(bool state)
    {
        canvasGroup.blocksRaycasts = state;
    }
    /// <summary>
    /// 游戏重新开始
    /// </summary>
    public void Restart()
    {
        HideMenu();
        gameController.Restart();
    }
}
