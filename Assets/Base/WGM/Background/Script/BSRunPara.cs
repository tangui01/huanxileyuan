using UnityEngine;
using System.Collections;
using WGM;

public class BSRunPara : MonoBehaviour
{
    public RunPara mRun;
    public Information mInfo;
    public int mPassword;
    public UIInput mPasswordInput;
    public UILabel mPasswordLabel;
    public int mClearAccPatCnt;


    [System.Serializable]
    public class RunPara
    {
        /// <summary>
        /// 投币几币
        /// </summary>
        public UIOption cp_coin;
        /// <summary>
        /// 投币几分
        /// </summary>
        public UIOption cp_point;
        /// <summary>
        /// 退奖几币
        /// </summary>
        public UIOption cl_coin;
        /// <summary>
        /// 退奖几分
        /// </summary>
        public UIOption cl_prize;
        /// <summary>
        /// 奖品类型
        /// </summary>
        public UIOption prize_type;
        /// <summary>
        /// 退奖模式
        /// </summary>
        public UIOption return_mode;
        /// <summary> 难度调整 </summary>
        public UIOption difficulty;
        /// <summary> Item_演示音量 </summary>
        public UIOption volume;
        /// <summary>
        /// 游戏时长
        /// </summary>
        public UIOption gametime;
        public UIOption upSpeedtime;
       // public UIOption continueTime;
        public UIOption gameVolume;
        public UIOption language;
        public UIOption password;
        public UIOption shockTime;
        public UIOption clear_acc;
        public UIOption save_exit;
        public UIOption exit;

        public RunPara(Transform run)
        {
            cp_coin = run.Find("Item_投币几币").GetComponent<UIOption>();
            cp_point = run.Find("Item_投币几分").GetComponent<UIOption>();
            cl_coin = run.Find("Item_退奖几币").GetComponent<UIOption>();
            cl_prize = run.Find("Item_退奖几分").GetComponent<UIOption>();
            prize_type = run.Find("Item_奖品类型").GetComponent<UIOption>();
            return_mode = run.Find("Item_退奖模式").GetComponent<UIOption>();
            difficulty = run.Find("Item_难度调整").GetComponent<UIOption>();
            gameVolume= run.Find("Item_游戏音量").GetComponent<UIOption>();
            volume = run.Find("Item_演示音量").GetComponent<UIOption>();
            language = run.Find("Item_语言显示").GetComponent<UIOption>();
            password = run.Find("Item_密码设定").GetComponent<UIOption>();
            shockTime = run.Find("Item_震机时间").GetComponent<UIOption>();
            gametime = run.Find("Item_游戏时长").GetComponent<UIOption>();
            
            //continueTime = run.Find("Item_自动游戏").GetComponent<UIOption>();
            clear_acc = run.Find("Item_清零账目").GetComponent<UIOption>();
            save_exit = run.Find("Item_保存并退出").GetComponent<UIOption>();
            exit = run.Find("Item_不保存退出").GetComponent<UIOption>();
        }
    }

    public class Information
    {
        public UILabel In;
        public UILabel Out;
        public UILabel TotalIn;
        public UILabel TotalOut;
        public UILabel MachineNum;
        public UILabel Version;

        public Information(Transform go)
        {
            In = go.Find("In/Value").GetComponent<UILabel>();
            Out = go.Find("Out/Value").GetComponent<UILabel>();
            TotalIn = go.Find("TotalIn/Value").GetComponent<UILabel>();
            TotalOut = go.Find("TotalOut/Value").GetComponent<UILabel>();
            MachineNum = go.Find("MachineNum/Value").GetComponent<UILabel>();
            Version = go.Find("Version/Value").GetComponent<UILabel>();
        }
    }

    void Awake()
    {
        mRun = new RunPara(transform);
        mInfo = new Information(transform);

        mRun.language.actionChange += (index, value) =>
        {
            Localization.language = value;
            foreach (UIOption l in UIOption.list)
            {
                l.index = l.index;
            }
        };

        mRun.prize_type.actionChange += (index, value) =>
        {
            UpdatePrizePerCoin(index, true);
        };

        mRun.cp_coin.actionChange += (index, value) => OnLimit();
        mRun.cp_point.actionChange += (index, value) => OnLimit();
        mRun.cl_coin.actionChange += (index, value) => OnLimit();
        mRun.cl_prize.actionChange += (index, value) => OnLimit();
        mRun.prize_type.actionChange += (index, value) => OnLimit();

        mPasswordInput = mRun.password.transform.Find("Input").GetComponent<UIInput>();
        mPasswordLabel = mPasswordInput.transform.Find("Label").GetComponent<UILabel>();
        mPasswordInput.onSubmit.Add(new EventDelegate(OnSubmitSettingPassword));
        mRun.password.onClick.Add(new EventDelegate(OnClickSettingPassword));
        mRun.clear_acc.onClick.Add(new EventDelegate(OnClearAccount));
        mRun.save_exit.onClick.Add(new EventDelegate(OnBtnSaveAndExit));
        mRun.exit.onClick.Add(new EventDelegate(OnBtnExit));

        mRun.cp_coin.actionSelect += CheckPlayerAccountClear;
        mRun.cl_coin.actionSelect += CheckPlayerAccountClear;
        mRun.prize_type.actionSelect += CheckPlayerAccountClear;
        mRun.language.actionSelect += CheckDeveloperMode;
        
    }

    void OnEnable()
    {
        Invoke("Display", 0.01f);
        UpdatePrizePerCoin(LibWGM.machine.Prize_type, false);
        ClearDatasOnEnable();
    }

    /// <summary>
    /// 按下保存退出
    /// </summary>
    void OnBtnSaveAndExit()
    {
        SaveRunPara();//保存数据
        OnBtnExit();
    }

    /// <summary>
    /// 按下退出
    /// </summary>
    void OnBtnExit()
    {
        gameObject.SetActive(false);
        BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opRunPara);
        BSRoot.Instance.gameObject.SetActive(true);
    }

    /// <summary>
    /// 显示初始数据
    /// </summary>
    void Display()
    {
        mRun.cp_coin.value = LibWGM.machine.Cp_coin.ToString();
        //mRun.cp_coin.value = "11";
        Debug.Log("显示初始数据");

        mRun.cp_point.value = LibWGM.machine.Cp_point.ToString();
        mRun.cl_coin.value = LibWGM.machine.Cl_coin.ToString();
        mRun.cl_prize.value = LibWGM.machine.Cl_prize.ToString();
        mRun.prize_type.index = LibWGM.machine.Prize_type;
        mRun.return_mode.index = LibWGM.machine.Return_mode;
        mRun.difficulty.index = LibWGM.machine.Difficult;
        mRun.volume.index = (int)(LibWGM.machine.BgmVolume);
        mRun.gameVolume.index = (int)(LibWGM.machine.SeVolume);
        mRun.shockTime.value = LibWGM.machine.ValveAsyncTime.ToString();
        print(LibWGM.machine.GameTime);
        mRun.gametime.value = LibWGM.machine.GameTime.ToString();
       
        
       // mRun.continueTime.value = LibWGM.machine.AutoTime.ToString();
        mRun.language.index = LibWGM.machine.Language;
        mPasswordInput.value = "";

        int coin_in_amount = 0, coin_out_amount = 0;
        int total_coin_in_amount = 0, total_coin_out_amount = 0;
        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            coin_in_amount += LibWGM.playerData[i].CoinInCur ;
            coin_out_amount += LibWGM.playerData[i].CoinOutCur;
            total_coin_in_amount += LibWGM.playerData[i].CoinInTotal;
            total_coin_out_amount += LibWGM.playerData[i].CoinOutTotal;
        }
        mInfo.In.text = coin_in_amount.ToString();
        mInfo.Out.text = coin_out_amount.ToString();
        mInfo.TotalIn.text = total_coin_in_amount.ToString();
        mInfo.TotalOut.text = total_coin_out_amount.ToString();
        mInfo.MachineNum.text = LibWGM.machine.MachineId;
        mInfo.Version.text = "V" + Application.version+
            "B" + LibWGM.machine.BootCount;
    }

    void SetAsInitPara()
    {
        mRun.cp_coin.oldIndex = mRun.cp_coin.index;
        mRun.cp_point.oldIndex = mRun.cp_point.index;
        mRun.cl_coin.oldIndex = mRun.cl_coin.index;
        mRun.cl_prize.oldIndex = mRun.cl_prize.index;
        mRun.prize_type.oldIndex = mRun.prize_type.index;
        mRun.return_mode.oldIndex = mRun.return_mode.index;
        mRun.difficulty.oldIndex = mRun.difficulty.index;
        mRun.language.oldIndex = mRun.language.index;
        mRun.shockTime.oldIndex = mRun.shockTime.index;
        mRun.gametime.oldIndex = mRun.gametime.index;
        mRun.upSpeedtime.oldIndex = mRun.upSpeedtime.index;
        //mRun.continueTime.oldIndex = mRun.continueTime.index;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    void SaveRunPara()
    {
        LibWGM.machine.Cp_coin = int.Parse(mRun.cp_coin.value);
        LibWGM.machine.Cp_point = int.Parse(mRun.cp_point.value);
        LibWGM.machine.Cl_coin = int.Parse(mRun.cl_coin.value);
        LibWGM.machine.Cl_prize = int.Parse(mRun.cl_prize.value);
        LibWGM.machine.Prize_type = mRun.prize_type.index;
        LibWGM.machine.Return_mode = mRun.return_mode.index;
        LibWGM.machine.Difficult = mRun.difficulty.index;
        LibWGM.machine.BgmVolume  = mRun.volume.index;
        LibWGM.machine.SeVolume  = mRun.gameVolume.index;
        LibWGM.machine.Language = mRun.language.index;
        LibWGM.machine.ValveAsyncTime = float.Parse(mRun.shockTime.value);
        LibWGM.machine.GameTime = int.Parse(mRun.gametime.value);
        //LibWGM.machine.AddSpeed = float.Parse(mRun.upSpeedtime.value);
        //LibWGM.machine.AutoTime = int.Parse(mRun.continueTime.value);
        if (mPasswordInput.value.Length != 0) LibWGM.machine.Password = mPasswordInput.value;

    }

    public void OnClickSettingPassword()
    {
        UICamera.currentScheme = UICamera.ControlScheme.Controller;
        UICamera.selectedObject = mPasswordInput.gameObject;

        mPasswordLabel.color = Color.green;
    }

    public void OnSubmitSettingPassword()
    {
        if (mPasswordInput.value.Length != 8)
        {
            mRun.password.uil_tooltip.alpha = 1;
            mRun.password.uil_tooltip.GetComponent<UILocalize>().value = Localization.Get("少于8位密码");
            Invoke("HidePasswordTooltip", 2);
            return;
        }
        UICamera.currentScheme = UICamera.ControlScheme.Controller;
        UICamera.selectedObject = mRun.password.gameObject;

        mPasswordLabel.color = Color.white;
    }

    void HidePasswordTooltip()
    {
        mRun.password.uil_tooltip.text = "";
        mRun.password.uil_tooltip.alpha = 0;
    }

    public void OnClearAccount()
    {
        UILocalize lo = mRun.clear_acc.uil_tooltip.GetComponent<UILocalize>();
        switch (mClearAccPatCnt)
        {
            case 0:
                mRun.clear_acc.uil_tooltip.alpha = 1;
                lo.value = Localization.Get("确认清零，请再按一次");
                break;
            case 1:
                lo.value = Localization.Get("清零成功");
                ResetAccount();
                break;
            default:
                lo.value = Localization.Get("数据已经清零");
                break;
        }

        mClearAccPatCnt++;
    }

    void ClearDatasOnEnable()
    {
        mClearAccPatCnt = 0;
    }

    /// <summary>
    /// 检查玩家帐户清除
    /// </summary>
    /// <returns></returns>
    bool CheckPlayerAccountClear()
    {
        int coin_in_amount = 0;
        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            coin_in_amount += LibWGM.playerData[i].CoinInCur;
        }

        return coin_in_amount == 0;
    }

    /// <summary>
    /// 重置帐户 /重设帐户
    /// </summary>
    void ResetAccount()
    {
        LibWGM.ClearAllData();
        LibWGM.machine.ValveAsyncTime = 0.18f;
        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            LibWGM.playerData[i].coin_in = 0;
            LibWGM.playerData[i].coin_use = 0;
            LibWGM.playerData[i].Free_coin_in = 0;
            LibWGM.playerData[i].CoinInCur = 0;
            LibWGM.playerData[i].CoinOutCur = 0;
        }

        Display();
    }

    void UpdatePrizePerCoin(int index, bool reset)
    {
        Debug.Log("BG 退奖类型  " + index + "   LibWGM.data.prize_type= " + LibWGM.machine.Prize_type);
        switch (index)
        {
            case 1:
                mRun.cl_coin.uil_name.GetComponent<UILocalize>().SetKey("出票比例");
                mRun.cl_prize.transform.Find("Unit").GetComponent<UILocalize>().SetKey("票");
                if (reset)
                {
                    mRun.cl_coin.value = "1";
                    mRun.cl_prize.value = "20";
                }
                break;
            case 0:
                mRun.cl_coin.uil_name.GetComponent<UILocalize>().SetKey("礼品比例");
                mRun.cl_prize.transform.Find("Unit").GetComponent<UILocalize>().SetKey("个");
                if (reset)
                {
                    mRun.cl_coin.value = "2";
                    mRun.cl_prize.value = "1";
                }
                break;
            default: break;
        }
    }

    void OnLimit()
    {
        if (mRun.prize_type.index == 2)
        {
            mRun.cl_coin.firstLimit = 0;
            mRun.cl_coin.endLimit = int.MaxValue;      //N币退1个礼品

            //mRun.cl_prize.firstLimit = 0;
            //mRun.cl_prize.endLimit = 1;
        }
        else
        {
            //mRun.cl_coin.firstLimit = 0;
            //mRun.cl_coin.endLimit = 0;

            mRun.cl_prize.firstLimit = 0;
            mRun.cl_prize.endLimit = int.MaxValue;   //1币退N张彩票
        }
    }
    public bool CheckDeveloperMode()
    {
        return LibWGM.machine.DeveloperMode;

    }
}
