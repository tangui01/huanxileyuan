using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public enum AppKeyState
{
    Down,
    LongDown,
    Up
}

public class DriverManager : MonoBehaviour
{
    static Driver3229 driver3229;
    static DriverManager ins;

    static List<List<Action<AppKeyState, Driver3229.KeyPara.KeyState>>> actions =
        new List<List<Action<AppKeyState, Driver3229.KeyPara.KeyState>>>();

    static List<List<KeyCode>> SystemKeys = new List<List<KeyCode>>();
    public static List<List<Driver3229.KeyPara.KeyState>> keyStates = new List<List<Driver3229.KeyPara.KeyState>>();

    public struct BGState //??????????
    {
        public int last;
        public int state;
        public int down; //????
        public int up; //????
        public int longDown; //????
        public float LongMultiInterval;
        public KeyCode keyCode;
    }

    public static BGState[] bgstate = new BGState[4]
    {
        new BGState() { LongMultiInterval = 0.1f }, new BGState() { LongMultiInterval = 0.1f },
        new BGState() { LongMultiInterval = 0.1f }, new BGState() { LongMultiInterval = 0.1f },
    };

    // Start is called before the first frame update
    void Awake()
    {
        if (ins != null)
            return;
        ins = this;
        int[] playerProts = new int[] { 0, 1 };
        driver3229 = new Driver3229(playerProts);

        for (int i = 0; i < 2; i++)
        {
            actions.Add(new List<Action<AppKeyState, Driver3229.KeyPara.KeyState>>());
            SystemKeys.Add(new List<KeyCode>());
            keyStates.Add(new List<Driver3229.KeyPara.KeyState>());
            for (int m = 0; m < 8; m++)
            {
                actions[i].Add((A, b) => { });
                actions[i][m] += Log;
                SystemKeys[i].Add(new KeyCode());
                keyStates[i].Add(new Driver3229.KeyPara.KeyState());
            }
        }
    }

    private void OnDestroy()
    {
        if (ins == this)
            driver3229.Abort();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    WinOnePrize(5);
        //}
    }

    string log;
    float timer;

    void LateUpdate()
    {
        for (int i = 0; i < SystemKeys.Count; i++)
        {
            for (int j = 0; j < SystemKeys[i].Count; j++)
            {
                var ks = keyStates[i][j];
                ks.longDown = 0;
                ks.down = 0;
                ks.up = 0;
                keyStates[i][j] = ks;
            }
        }

        LibWGM.UpdateGlobalKey();
        for (int i = 0; i < bgstate.Length; i++)
        {
            bgstate[i].down = 0;
            bgstate[i].up = 0;
            bgstate[i].longDown = 0;
        }

        for (int i = 0; i < bgstate.Length; i++)
        {
            UpdateBGKey(i);
        }

        for (int i = 0; i < SystemKeys.Count; i++)
        {
            for (int j = 0; j < SystemKeys[i].Count; j++)
            {
                if (LibWGM.machine.DeveloperMode) //
                    log = $"KEY {SystemKeys[i][j]} ";
                if (Input.GetKeyDown(SystemKeys[i][j]))
                {
                    timer = 0;
                    actions[i][j]?.Invoke(AppKeyState.Down, new Driver3229.KeyPara.KeyState() { no = i, type = j });
                    var k = keyStates[i][j];
                    k.down = 1;
                    keyStates[i][j] = k;
                }

                if (Input.GetKeyUp(SystemKeys[i][j]))
                {
                    actions[i][j]?.Invoke(AppKeyState.Up, new Driver3229.KeyPara.KeyState() { no = i, type = j });
                    var k = keyStates[i][j];
                    k.up = 1;
                    keyStates[i][j] = k;
                }

                if (Input.GetKey(SystemKeys[i][j]))
                {
                    var k = keyStates[i][j];
                    timer += Time.deltaTime;
                    if (timer >= 0.1f)
                    {
                        timer -= 0.1f;
                        actions[i][j]?.Invoke(AppKeyState.LongDown,
                            new Driver3229.KeyPara.KeyState() { no = i, type = j });


                        k.longDown = 1;
                    }

                    k.state = 1;
                    keyStates[i][j] = k;
                }
                else
                {
                    var k = keyStates[i][j];
                    k.state = 0;
                    keyStates[i][j] = k;
                }
                //var ks = keyStates[i][j];
                //ks.longDown = 1;
                //keyStates[i][j] = ks;
            }
        }

        while (Driver3229.keyStates.Count > 0)
        {
            var key = Driver3229.keyStates.Dequeue();
            if (LibWGM.machine.DeveloperMode)
                log = $"KEY {key.no} {key.type} ";
            if (key.keyCodeState == 1)
            {
                actions[key.no][key.type]?.Invoke(AppKeyState.Down, key);
                var ks = keyStates[key.no][key.type];
                ks.down = 1;
                keyStates[key.no][key.type] = ks;
            }

            if (key.keyCodeState == 3)
            {
                actions[key.no][key.type]?.Invoke(AppKeyState.Up, key);
                var ks = keyStates[key.no][key.type];
                ks.up = 1;
                keyStates[key.no][key.type] = ks;
            }

            if (key.keyCodeState == 2)
            {
                actions[key.no][key.type]?.Invoke(AppKeyState.LongDown, key);
                var ks = keyStates[key.no][key.type];
                ks.longDown = 1;
                keyStates[key.no][key.type] = ks;
            }
        }
    }

    public static bool GetKey(int n, int t)
    {
        return Driver3229.keyParas[n][t].keyState.state != Driver3229.keyParas[n][t].keyState.baseVR ||
               keyStates[n][t].state == 1;
    }

    public void UpdateBGKey(int key) //????????key
    {
        bgstate[key].state = 0; //??

        int state = LibWGM.GetGlobalKey(key) || Input.GetKey(bgstate[key].keyCode) ? 1 : 0;
        bgstate[key].state = state;
        if (state > 0)
        {
            if (bgstate[key].last != state)
            {
                bgstate[key].down = state;
                bgstate[key].last = state;
            }

            if (bgstate[key].down != 0)
            {
                timer = bgstate[key].LongMultiInterval + Time.time;
            }

            //up = false;
        }
        else
        {
            bgstate[(int)key].state = 0;
            bgstate[(int)key].last = 0;
        }
    }

    public static BgKeyCode GetRK3299Key(KeyCode key) //???????key????????????
    {
        for (int i = 0; i < bgstate.Length; i++)
        {
            if (bgstate[i].keyCode == key)
            {
                return (BgKeyCode)i;
            }
        }

        return BgKeyCode.None;
    }

    /// <summary>
    /// count ????????  timeOut ??????  onOut ??????????????  onResult<bool,int> ??? true ??? false ??? int ??δ???????
    /// </summary>
    /// <param name="count"></param>
    /// <param name="timeOut"></param>
    /// <param name="onOut"></param>
    /// <param name="onResult"></param>
    public static async void WinOnePrize(int count = 1, float timeOut = 10, Action<int, int> onOut = null,
        Action<bool, int> onResult = null)
    {
        Driver3229.prizeCount = count;
        print("?????????");
        RegisterKeyEvent(0, AppKeyCode.CoinOut, OnOutPrize); //??????
        RegisterKeyEvent(1, AppKeyCode.CoinOut, OnOutPrize); //??????
        LibWGM.Rk3229SetGpio(0, 0, 1); // ???? ?? ?????? ?????????0?????????1????????
        LibWGM.Rk3229SetGpio(1, 0, 1);
        bool finish = false;

        await new WaitUntil(() =>
        {
            timeOut -= Time.unscaledDeltaTime;
            return finish || timeOut < 0;
        }); //
        Driver3229.prizeCount = 0;
        if (!finish && timeOut <= 0)
        {
            Debug.LogError("WinOnePrize TimeOut" + count);
        }

        onResult?.Invoke(finish, count);
        UnRegisterKeyEvent(0, AppKeyCode.CoinOut, OnOutPrize); //??????????
        UnRegisterKeyEvent(1, AppKeyCode.CoinOut, OnOutPrize); //??????????

        void OnOutPrize(AppKeyState appKeyState, Driver3229.KeyPara.KeyState keyState) //?????? 
        {
            if (appKeyState == AppKeyState.Up) //up???
            {
                print("??????????");
                count--; //??????????
                onOut?.Invoke(keyState.no, count);
                if (count <= 0)
                {
                    LibWGM.Rk3229SetGpio(0, 0, 0); //???
                    LibWGM.Rk3229SetGpio(1, 0, 0);
                    finish = true;
                }
            }
        }
    }

    public void OnOut(int no, int count)
    {
    }

    public void OnResult(bool finsh, int count)
    {
        if (count <= 0)
        {
            finsh = true;
        }
        else
        {
            finsh = false;
        }
    }

    public static void BindinAppKeyCode(int no, AppKeyCode appKeyCode, KeyCode key)
    {
        SystemKeys[no][(int)appKeyCode] = key;
    }

    public static void BindinBgKeyCode(BgKeyCode bgKeyCode, KeyCode key)
    {
        bgstate[(int)bgKeyCode].keyCode = key;
    }

    public static void RegisterKeyEvent(int no, AppKeyCode appKeyCode,
        Action<AppKeyState, Driver3229.KeyPara.KeyState> action) //??????
    {
        actions[no][(int)appKeyCode] += action;
    }

    public static void UnRegisterKeyEvent(int no, AppKeyCode appKeyCode,
        Action<AppKeyState, Driver3229.KeyPara.KeyState> action) //????????
    {
        actions[no][(int)appKeyCode] -= action;
    }

    public void Log(AppKeyState appKeyState, Driver3229.KeyPara.KeyState keyState)
    {
        if (LibWGM.machine.DeveloperMode)
        {
            // print(log + appKeyState.ToString());
        }
    }
}