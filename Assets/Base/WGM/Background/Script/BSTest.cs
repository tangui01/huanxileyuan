using UnityEngine;
using System.Collections;
using WGM;

public class BSTest : MonoBehaviour
{
    public AppKeyCode[] start;
    public AppKeyCode[] Up;
    public AppKeyCode[] Dwon;
    public AppKeyCode[] Left;
    public AppKeyCode[] Right;

    public class TestData
    {
        public Transform transform;
        public UILabel mSwitch;
        public UILabel mStart;
        public UILabel mCoinIn;
        public UILabel mCoinOut;
        public UILabel mUp;
        public UILabel mDown;
        public UILabel mLeft;
        public UILabel mRight;

        public int mCoinInCount;
        public int mCoinOutCount;

        public TestData(Transform tran)
        {
            transform = tran;
            mSwitch = transform.Find("Switch/Value").GetComponent<UILabel>();
            mStart = transform.Find("Start/Value").GetComponent<UILabel>();
            mCoinIn = transform.Find("CoinIn/Value").GetComponent<UILabel>();
            mCoinOut = transform.Find("CoinOut/Value").GetComponent<UILabel>();
            mUp = transform.Find("Up/Value").GetComponent<UILabel>();
            mDown = transform.Find("Down/Value").GetComponent<UILabel>();
            mLeft = transform.Find("Left/Value").GetComponent<UILabel>();
            mRight = transform.Find("Right/Value").GetComponent<UILabel>();
        }
    }

    TestData[] data = new TestData[2];

    void Awake()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new TestData(transform.Find("Player" + i));
        }
    }

    void OnEnable()
    {
        if (LibWGM.machine.PlayerCount == 1)
        {
            data[0].transform.gameObject.SetActive(false);
            data[1].transform.localPosition = new Vector3(-180, 33.3f, 0);
        }

        if (LibWGM.machine.PlayerCount == 2)
        {
        }
    }


    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        if (DealCommand.GetKeyDown(BgKeyCode.Background))
        {
            ReturnToRoot(); //直接退出
        }

        for (int i = 0; i < data.Length; i++)
        {
            data[i].mStart.text = DealCommand.GetKey(1, start[i]) ? "1" : "0";
            data[i].mCoinInCount += DealCommand.GetKeyTrg(1, AppKeyCode.CoinIn) ? 1 : 0;
            data[i].mUp.text = DealCommand.GetKey(1, Up[i]) ? "1" : "0";
            data[i].mDown.text = DealCommand.GetKey(1, Dwon[i]) ? "1" : "0";
            data[i].mLeft.text = DealCommand.GetKey(1, Left[i]) ? "1" : "0";
            data[i].mRight.text = DealCommand.GetKey(1, Right[i]) ? "1" : "0";
            data[i].mCoinIn.text = data[i].mCoinInCount.ToString();
            data[i].mCoinOut.text = data[i].mCoinOutCount.ToString();
        }

        for (int i = 0; i < data.Length; i++)
        {
            if (DealCommand.GetKeyDown(i, AppKeyCode.Flight))
            {
                // LibWGM.Valve(i);
            }
        }

        if (DealCommand.GetKeyDown(BgKeyCode.Ok))
        {
            DriverManager.WinOnePrize(1, 10, (no, b) =>
            {
                data[0].mCoinOutCount++;
                data[1].mCoinOutCount++;
            });
        }
    }


    void ReturnToRoot()
    {
        BSRoot.Instance.SetMenuSelect(BSRoot.Instance.opTest);
        gameObject.SetActive(false);
        BSRoot.Instance.gameObject.SetActive(true);
    }
}