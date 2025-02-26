using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using WGM;

public class BSRoot : MonoBehaviour
{

    public int SelIndex;
    /// <summary> 参数设置 </summary>
    public UIOption opRunPara { get; private set; }
    /// <summary> 光标校准 </summary>
    public UIOption opAdjust { get; private set; }
    /// <summary> 查账 </summary>
    public UIOption opAccount { get; private set; }
    /// <summary> 报帐 </summary>
    public UIOption opCode { get; private set; }
    /// <summary> 出厂测试 </summary>
    public UIOption opTest { get; private set; }
    /// <summary> 退出 </summary>
    public UIOption opExit { get; private set; }

    /// <summary> 参数设置子类tf </summary>
    public static Transform bsRunPara;
    /// <summary> 光标较准子类tf </summary>
    public static Transform bsAdjust;

    public static Transform bsAccount;

    /// <summary> 报账子类tf </summary>
    public static Transform bsCode;
    /// <summary> 出厂测试 </summary>
    public static Transform bsTest;

    /// <summary> 输入密码子类tf </summary>
    public static Transform bsPassword;

    public static string[] languages = { "简体中文", "繁體中文", "English" };

    public static BSRoot Instance;
    void Awake()
    {
        Instance = this;
       
        DealCommand.isBackGround = true;
    }

    void OnEnable()
    {
        Localization.language = languages[LibWGM.machine.Language];
      
    }

    // Use this for initialization
    void Start()
    {
        bsRunPara = transform.root.Find("BSRunPara");
        bsAdjust = transform.root.Find("BSAdjust");
        bsAccount = transform.root.Find("BSAccount");
        bsCode = transform.root.Find("BSCode");
        bsTest = transform.root.Find("BSTest");
        bsPassword = transform.root.Find("BSPassword");
        opRunPara = transform.Find("Menu 参数设置").GetComponent<UIOption>();
        opAdjust = transform.Find("Menu 光标校准").GetComponent<UIOption>();
        opAccount = transform.Find("Menu 查账").GetComponent<UIOption>();
        opCode = transform.Find("Menu 报码").GetComponent<UIOption>();
        opTest = transform.Find("Menu 出厂测试").GetComponent<UIOption>();
        opExit = transform.Find("Menu 退出").GetComponent<UIOption>();


        opRunPara.onClick.Add(new EventDelegate(OnRunPara));
        opAdjust.onClick.Add(new EventDelegate(OnAdjust));
        opAccount.onClick.Add(new EventDelegate(OnAccount));
        opCode.onClick.Add(new EventDelegate(OnCode));
        opTest.onClick.Add(new EventDelegate(OnTest));
        opExit.onClick.Add(new EventDelegate(OnExit));
       
    }

    /// <summary>
    /// 输入密码子类tf显示
    /// </summary>
    void OnRunPara()
    {
        gameObject.SetActive(false);
        bsRunPara.gameObject.SetActive(true);
        //BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opRunPara);
        // BSRoot.Instance.gameObject.SetActive(true);
    }

    /// <summary>
    /// 光标较准子类tf显示
    /// </summary>
    void OnAdjust()
    {
        gameObject.SetActive(false);
        bsAdjust.gameObject.SetActive(true);

    }

    /// <summary>
    /// 没有这个选项
    /// </summary>
    void OnAccount()
    {

        gameObject.SetActive(false);
        bsAccount.gameObject.SetActive(true);
    }

    /// <summary>
    /// 报账子类tf显示
    /// </summary>
    void OnCode()
    {
        gameObject.SetActive(false);
        bsCode.gameObject.SetActive(true);
    }

    /// <summary>
    /// 出厂测试子类显示
    /// </summary>
    void OnTest()
    {
        gameObject.SetActive(false);
        bsTest.gameObject.SetActive(true);

    }

    /// <summary>
    /// 退出后台场景 进入主游戏界面
    /// </summary>
    void OnExit()
    {
        DealCommand.isBackGround = false;
        /*退出后台加载主游戏界面*/ 
        LoadABManger.Instance.LoadAB(MainConstant.MainSceneName);
        SceneLoadManager.instance.ExitSceneACtion = () =>
        {
            GameTimeManager.instance.StopColdDown();
            GameTimeManager.instance.SetPauseGame(false);
            GameStateManager.Instance.SwitchState(GameState.Idle);
            CommonUI.instance.ExitBG();
            //更新背景音乐和音效
            AudioManager.Instance?.SetBGmVolume(LibWGM.machine.BgmVolume/10);
            AudioManager.Instance?.SetEfVolume(LibWGM.machine.SeVolume/10);
        };
    }

    public void SetMenuSelect(UIOption op)
    {
        opRunPara.startsSelected = false;
        opAdjust.startsSelected = false;
        opAccount.startsSelected = false;
        opCode.startsSelected = false;
        opTest.startsSelected = false;
        opExit.startsSelected = false;
        op.startsSelected = true;
        Debug.Log(op.gameObject.name);
    }
}
