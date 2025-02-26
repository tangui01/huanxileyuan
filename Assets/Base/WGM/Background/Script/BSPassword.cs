using UnityEngine;
using System.Collections;
using WGM;

public class BSPassword : MonoBehaviour
{

    UIInput mInput;
    UILabel mUITip;

    // Use this for initialization
    void Start()
    {
        mInput = transform.Find("InputPassword").GetComponent<UIInput>();
        mUITip = transform.Find("Tooltip").GetComponent<UILabel>();
        mUITip.text = " ";
        mInput.onSubmit.Add(new EventDelegate(OnSubmitPassword));
    }

    // Update is called once per frame
    void Update()
    {
        //if (DealCommand.GetKeyDown(0, AppKeyCode.Bet) ||
        //    DealCommand.GetKeyDown(BgKeyCode.Background) ||
        //   Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Debug.Log(Input.GetKeyDown(KeyCode.Escape));
        //    OnBtnExit();
        //}
    }

    void OnSubmitPassword()
    {
        mUITip.text = " ";

        if (mInput.value.Length != 8)
        {
            mUITip.text = Localization.Get("少于8位密码");
            gameObject.SetActive(false);
            BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opTest);
            BSRoot.Instance.gameObject.SetActive(true);
            return;
        }

        string psd = mInput.value;

        if (psd == LibWGM.machine.Password)
        {
            gameObject.SetActive(false);

            BSRoot.bsRunPara.gameObject.SetActive(true);
        }
        else
        {
            mUITip.text = Localization.Get("密码错误");
            gameObject.SetActive(false);
            BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opTest);
            BSRoot.Instance.gameObject.SetActive(true);
        }
    }

    void OnBtnExit()
    {
        BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opRunPara);
        gameObject.SetActive(false);
        BSRoot.Instance.gameObject.SetActive(true);
    }
}
