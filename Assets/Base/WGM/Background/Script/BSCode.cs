using UnityEngine;
using System.Collections;
using WGM;

public class BSCode : MonoBehaviour
{

    private CodePage code_page;

    public class CodePage
    {
        public UILabel total_in;
        public UILabel total_out;
        public UILabel mac_num;
        public UILabel code_times;
        public UILabel check_code;
        public UIInput input;
        public UILabel tooltip;

        public CodePage(Transform code)
        {
            total_in = code.Find("In/Value").GetComponent<UILabel>();
            total_out = code.Find("Out/Value").GetComponent<UILabel>();
            mac_num = code.Find("MacNum/Value").GetComponent<UILabel>();
            code_times = code.Find("CodeTimes/Value").GetComponent<UILabel>();
            check_code = code.Find("CheckCode/Value").GetComponent<UILabel>();
            input = code.Find("InputCode").GetComponent<UIInput>();
            tooltip = code.Find("Tooltip").GetComponent<UILabel>();
        }
    }

    void Awake()
    {
        code_page = new CodePage(transform);

        code_page.input.onSubmit.Add(new EventDelegate(OnBtnSubmit));
    }

    void OnEnable()
    {
        Display();
       
    }
    private void OnDisable()
    {
        
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DealCommand.GetKeyDown(BgKeyCode.Background) ||
           Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToRoot();                                                     //直接退出
        }
    }

    void OnBtnSubmit()
    {
        int ret;

        if (!int.TryParse(code_page.input.value, out ret) || code_page.input.value.Length < 8)
        {
            code_page.tooltip.text = Localization.Get("少于8位密码");
            return;
        }

        // ret = LibWGM.Code(ret);

        switch (ret)
        {
            case -1:
                code_page.tooltip.text = Localization.Get("密码错误");
                break;
            case 0:
                code_page.tooltip.text = Localization.Get("难度码生效");
                Invoke("ReturnToRoot", 2f);
                break;
            case 1:
                code_page.tooltip.text = Localization.Get("密码清除");
                Invoke("ReturnToRoot", 2f);
                break;
            case 2:
                code_page.tooltip.text = Localization.Get("通关码生效");
                Invoke("ReturnToRoot", 2f);
                break;
            case 3:
                code_page.tooltip.text = Localization.Get("清零码生效");
                Invoke("ReturnToRoot", 2f);
                break;
            case 4:
                code_page.tooltip.text = Localization.Get("激活码生效");
                // DealCommand.g_backstage_ret = 1;
                int hide = PlayerPrefs.GetInt("HideLevel");
                hide++;
                if (hide >= 5)
                    hide = 0;
                PlayerPrefs.SetInt("HideLevel", hide);
                Invoke("ReturnToRoot", 2f);
                break;
            default:
                code_page.tooltip.text = Localization.Get("未知码");
                break;
        }
    }

    void Display()
    {
        int coin_in_amount = 0, coin_out_amount = 0;
        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            coin_in_amount += LibWGM.playerData[i].coin_in;
            coin_out_amount += LibWGM.playerData[i].CoinOutTotal;
        }
        code_page.total_in.text = coin_in_amount.ToString();
        code_page.total_out.text = coin_out_amount.ToString();
        code_page.mac_num.text = LibWGM.machine.MachineId.ToString();
        //code_page.code_times.text = LibWGM.machine.code_times.ToString();
       // code_page.check_code.text = LibWGM.GetCheckCode().ToString();
        code_page.input.value = "0";
        code_page.tooltip.text = "";
    }

    void ReturnToRoot()
    {
        BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opCode);
        gameObject.SetActive(false);
        BSRoot.Instance.gameObject.SetActive(true);
    }
}
