//using UnityEngine;
//using System.Collections;

//public class BSHype : MonoBehaviour {

//	private CodePage code_page;

//	public class CodePage {
//		public UILabel total_in;
//		public UILabel mac_num;
//		public UILabel code_times;
//		public UILabel check_code;
//        public UILabel id;
//        public UILabel score;
//        public UILabel coin;
//		public UIInput input;
//		public UILabel tooltip;
//        public UILabel version;
		
//		public CodePage(Transform code) {
//			total_in  = code.Find("In/Value").GetComponent<UILabel>();
//			mac_num     = code.Find("MacNum/Value").GetComponent<UILabel>();
//			code_times  = code.Find("CodeTimes/Value").GetComponent<UILabel>();
//			check_code  = code.Find("CheckCode/Value").GetComponent<UILabel>();
//            id = code.Find("Id/Value").GetComponent<UILabel>();
//            score = code.Find("Score/Value").GetComponent<UILabel>();
//            coin = code.Find("Coin/Value").GetComponent<UILabel>();
//			input       = code.Find("InputCode").GetComponent<UIInput>();
//			tooltip     = code.Find("Tooltip").GetComponent<UILabel>();
//            version = code.Find("Version/Value").GetComponent<UILabel>();
//		}
//	}

//	void Awake() {
//		code_page = new CodePage(transform);

//        code_page.input.onSubmit.Add(new EventDelegate(OnBtnSubmit));

//        LibWGM.GetAllData();
//	}

//	void OnEnable() {
//		Display();
//	}

//	// Use this for initialization
//	void Start () {
//	}
	
//	// Update is called once per frame
//	void Update () {
		
//	}

//	void OnBtnSubmit() {
//        int ret;

//        if (!int.TryParse(code_page.input.value, out ret) || code_page.input.value.Length < 8)
//        {
//            code_page.tooltip.text = Localization.Get("少于8位密码");
//            return;
//        }

//        ret = LibWGM.Code(ret);

//        switch(ret) {
//            case -1:
//				code_page.tooltip.text = Localization.Get("密码错误");
//                break;
//            case 0:
//                code_page.tooltip.text = Localization.Get("难度码生效");
//                break;
//			case 1:
//				code_page.tooltip.text = Localization.Get("密码清除");
//				break;
//            case 2:
//                code_page.tooltip.text = Localization.Get("通关码生效");
//                Invoke("GoToNext", 2f);
//                break;
//            case 3:
//				code_page.tooltip.text = Localization.Get("清零码生效");
//                break;
//            case 4:
//				code_page.tooltip.text = Localization.Get("激活码生效");
//                DealCommand.g_backstage_ret = 1;
//                break;
//            default:
//				code_page.tooltip.text = Localization.Get("未知码");
//                break;
//        }

//        LibWGM.GetAllData();
//        Display(false);
//	}

//	void Display(bool init = true)
//	{
//        int coin_in_amount = 0, coin_out_amount = 0;
//        for (int i = 0; i < GameData.PlayerMax; i++)
//        {
//            coin_in_amount += LibWGM.player[i].coin_in;
//            coin_out_amount += LibWGM.player[i].coin_out;
//        }
//        code_page.total_in.text = coin_in_amount.ToString();
//		code_page.mac_num.text = LibWGM.data.mac_id.ToString();
//		code_page.code_times.text = LibWGM.data.code_times.ToString();
//		code_page.check_code.text = LibWGM.GetCheckCode().ToString();
//        code_page.id.text = LibWGM.record[0].id.ToString();
//        code_page.score.text = LibWGM.record[0].score.ToString();
//        code_page.coin.text = LibWGM.record[0].coin.ToString();

//        if (!init) return;
//		code_page.input.value = "0";
//		code_page.tooltip.text = "";

//        code_page.version.text = "G" + CurrentBundleVersion.version +
//            "H" + LibWGM.data.hardware +
//            "D" + LibWGM.data.difficult;
//	}

//    void GoToNext()
//    {
//        Application.LoadLevel(0);
//    }
//}
