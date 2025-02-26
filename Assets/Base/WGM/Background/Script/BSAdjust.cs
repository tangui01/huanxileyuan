using UnityEngine;
using System.Collections;
using WGM;

public class BSAdjust : MonoBehaviour
{

    public class Player
    {
        public UILabel real;
        public UILabel ld;
        public UILabel ru;
        public UILabel lastLD;
        public UILabel lastRU;
        public Transform aim;
        public int status;
        public Transform instruction;
        public UIWidget[] instructionStep;

        public int up;
        public int down;
        public int left;
        public int right;
        public Transform tran;
        public UILabel aimLabel;
        public Player(Transform tran)
        {
            this.tran = tran;
            real = tran.Find("Real").GetComponent<UILabel>();
            ld = tran.Find("LeftDown").GetComponent<UILabel>();
            ru = tran.Find("RightUp").GetComponent<UILabel>();
            lastLD = tran.Find("LastLeftDown").GetComponent<UILabel>();
            lastRU = tran.Find("LastRightUp").GetComponent<UILabel>();
            aim = tran.Find("Aim");
            aimLabel = aim.GetComponentInChildren<UILabel>();
            status = 0;
            instruction = tran.Find("Instruction");
            instructionStep = new UIWidget[instruction.childCount];
            for (int i = 0; i < instruction.childCount; i++)
            {
                instructionStep[i] = instruction.Find(i.ToString()).GetComponent<UIWidget>();
            }
        }
    }

    Player[] mPlayer = new Player[Machine.PlayerMax];
    Camera mCamera;



    void Awake()
    {
        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            mPlayer[i] = new Player(transform.Find("Player" + i));
        }
        mCamera = transform.root.Find("Camera").GetComponent<Camera>();
    }

    void OnEnable()
    {
        if (LibWGM.machine.PlayerCount == 1)
        {
            mPlayer[0].tran.gameObject.SetActive(false);
            mPlayer[1].tran.localPosition = new Vector3(0, 0, 0);
           mPlayer[1].aimLabel.text = "1";
        }

        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            mPlayer[i].ld.text = "0,0";
            mPlayer[i].ru.text = "0,0";
           // mPlayer[i].lastLD.text = LibWGM.GetPlayer(i, GetPly.adc_left) + "," + LibWGM.GetPlayer(i, GetPly.adc_down);
           // mPlayer[i].lastRU.text = LibWGM.GetPlayer(i, GetPly.adc_right) + "," + LibWGM.GetPlayer(i, GetPly.adc_up);
            mPlayer[i].status = 0;
            //mPlayer[i].aim.gameObject.SetActive(false);
            SetStep(i, 0);
        }
        DriverManager.RegisterKeyEvent(0, AppKeyCode.Flight, OnFireKey);
        DriverManager.RegisterKeyEvent(0, AppKeyCode.UpScore, OnFireKey);
        DriverManager.RegisterKeyEvent(1, AppKeyCode.Flight, OnFireKey);
        DriverManager.RegisterKeyEvent(1, AppKeyCode.UpScore, OnFireKey);
    }
    private void OnDisable()
    {
        DriverManager.UnRegisterKeyEvent(0, AppKeyCode.Flight, OnFireKey);
        DriverManager.UnRegisterKeyEvent(0, AppKeyCode.UpScore, OnFireKey);
        DriverManager.UnRegisterKeyEvent(1, AppKeyCode.Flight, OnFireKey);
        DriverManager.UnRegisterKeyEvent(1, AppKeyCode.UpScore, OnFireKey);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Machine.PlayerMax; i++)
        {
            
            mPlayer[i].real.text = LibWGM.GetAdcValue(i, 0) + "," + LibWGM.GetAdcValue(i, 1);
            mPlayer[i].aim.position = mCamera.ViewportToWorldPoint(DealCommand.aimPosition[i]);

            switch (mPlayer[i].status)
            {
                case 0:
                    mPlayer[i].ld.text = LibWGM.GetAdcValue(i, 0) + "," + LibWGM.GetAdcValue(i, 1);
                    break;
                case 1:
                    mPlayer[i].ru.text = LibWGM.GetAdcValue(i, 0) + "," + LibWGM.GetAdcValue(i, 1);
                    break;
                default: break;
            }

           
        }

        if (
            DealCommand.GetKeyDown(BgKeyCode.Background) || DealCommand.GetKeyDown(BgKeyCode.Ok) ||
           Input.GetKeyDown(KeyCode.Escape))
        {

            OnBtnExit();
        }
    }


    void OnFireKey(AppKeyState appKeyState,  Driver3229.KeyPara.KeyState keyState)
    {
        if (mPlayer[keyState.no].status == 0)
        {
            mPlayer[keyState.no].left = LibWGM.GetAdcValue(keyState.no, 0);
            mPlayer[keyState.no].down = LibWGM.GetAdcValue(keyState.no, 1);
            SetStep(keyState.no, 1);
        }
        else if (mPlayer[keyState.no].status == 1)
        {
            mPlayer[keyState.no].right = LibWGM.GetAdcValue(keyState.no, 0);
            mPlayer[keyState.no].up = LibWGM.GetAdcValue(keyState.no, 1);
            mPlayer[keyState.no].aim.gameObject.SetActive(true);
            LibWGM.playerData[keyState.no].AdcDown = mPlayer[keyState.no].down;
            LibWGM.playerData[keyState.no].AdcUp = mPlayer[keyState.no].up;
            LibWGM.playerData[keyState.no].AdcLeft = mPlayer[keyState.no].left;
            LibWGM.playerData[keyState.no].AdcRight = mPlayer[keyState.no].right;
            Debug.Log("BSAdjust " + keyState.no);
            DealCommand.ResetAllPosition();
            SetStep(keyState.no, 2);
        }
        mPlayer[keyState.no].status++;
    }

    void OnExit()
    {
        OnBtnExit();
    }

    void SetStep(int no, int step)
    {
        for (int i = 0; i < mPlayer[no].instruction.childCount; i++)
        {
            mPlayer[no].instructionStep[i].color = Color.white;
            mPlayer[no].instructionStep[i].GetComponent<TweenScale>().enabled = false;
            mPlayer[no].instructionStep[i].transform.localScale = Vector3.one;
        }
        mPlayer[no].ld.color = Color.white;
        mPlayer[no].ru.color = Color.white;
        mPlayer[no].ld.GetComponent<TweenScale>().enabled = false;
        mPlayer[no].ru.GetComponent<TweenScale>().enabled = false;
        mPlayer[no].ld.transform.localScale = Vector3.one;
        mPlayer[no].ru.transform.localScale = Vector3.one;


        mPlayer[no].instructionStep[step].color = Color.green;
        mPlayer[no].instructionStep[step].GetComponent<TweenScale>().enabled = true;

        switch (step)
        {
            case 0: mPlayer[no].ld.color = Color.green; mPlayer[no].ld.GetComponent<TweenScale>().enabled = true; break;
            case 1: mPlayer[no].ru.color = Color.green; mPlayer[no].ru.GetComponent<TweenScale>().enabled = true; break;
            default: break;
        }
    }

    void OnBtnExit()
    {
        BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opAdjust);
        gameObject.SetActive(false);
        BSRoot.Instance.gameObject.SetActive(true);
    }
}
