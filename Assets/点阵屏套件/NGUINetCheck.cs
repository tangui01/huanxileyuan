using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;
using System;

using UnityEngine.SceneManagement;
public enum NetState
{
    none,
    noNet,
    fourG,
    fourGConnectServer,
    wifi,
    wifiConnectServer,
   
}
public class UINetCheck: MonoBehaviour
{
    public bool isBox = true;
    public static NetState curNetState;
    public Color color = new Color(1, 1, 1, 1);
    public float checkDuration = 20f;
    [SerializeField]
    protected float timer;
    public static Action<bool> onStateChange;
    public static Dictionary<int, bool> nets = new Dictionary<int, bool>();
    public static bool running;

    protected virtual  NetState netState
    {
        get
        {

            return curNetState;
        }
        set
        {
            if (curNetState == value)
                return;
            curNetState = value;
        }
    }
   

    // Use this for initialization
    protected virtual void Start()
    {
        color = Color.red;
        curNetState = NetState.none;
        netState = NetState.fourGConnectServer;
    }
    protected virtual void OnEnable()
    {
        if (isBox)
        {
            LibWGM.onServerConnect += OnServerConnect;
        }
    }
    protected virtual void OnDisable()
    {
        if (isBox)
        {
            LibWGM.onServerConnect -= OnServerConnect;
        }
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (!running)
            return;

        if (LibWGM.machine.Language == 2)
        {
            LibWGM.netTimer = -1;
            return;
        }
        timer -= Time.deltaTime;
        if (timer > 0)
            return;
        timer = checkDuration;

        if (isBox)
        {
            LibWGM.RequestNetState(2).WrapErrors();
            DealCommand.Instance.SerialPortManager.RequestNetState(2).WrapErrors ();
            //DealCommand.Instance.SerialPortManager.SendGetLedScreencnf(2).WrapErrors();
            return;
        }
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                netState = NetState.noNet;
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                if (SignalRRunner.Instance != null)
                    if (SignalRRunner.Instance.IsConnected)
                        netState = NetState.fourGConnectServer;
                    else
                        netState = NetState.fourG;
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                if (SignalRRunner.Instance != null)
                    if (SignalRRunner.Instance.IsConnected)
                        netState = NetState.wifiConnectServer;
                    else
                        netState = NetState.wifi;
                break;

        }

    }
   
    protected virtual void OnServerConnect(int portHandle, bool result)
    {
        if (!running)
            return;
        nets[portHandle] = result;
        bool hasNet = false;
        for (int i = 0; i < DealCommand.handles.Length; i++)
        {
            if (DealCommand.handles[i] > 0 && nets[DealCommand.handles[i]])
            {
                print("有网");
                hasNet = true;
                break;
            }
        }
        netState = hasNet ? NetState.fourGConnectServer : NetState.noNet;
    }
}

public class NGUINetCheck : UINetCheck
{
  
    public UISprite NotReachable,FourG,Wifi,curNet;
   
    protected override NetState netState {
        get {

            return curNetState;
        }
        set {
            if (curNetState == value)
                return;
           
            curNetState = value;
            print("curNetState   " + curNetState);
            switch (curNetState)
            {
                case NetState.noNet:
                    color = Color.red;
                    curNet.color = color;
                    if (curNet != NotReachable)
                    {
                        curNet.gameObject.SetActive(false);
                        curNet = NotReachable;
                        curNet.gameObject.SetActive(true);                        
                    }
                    color = Color.red;
                    curNet.color = color;
                    onStateChange?.Invoke(false);
                    break;
                case NetState.fourG:
                   
                    if (curNet != FourG)
                    {
                        curNet.gameObject.SetActive(false);
                        curNet = FourG;
                        curNet.gameObject.SetActive(true);                        
                    }
                    color = Color.red;
                    curNet.color = color;
                    onStateChange?.Invoke(false);
                    break;
                case NetState.fourGConnectServer:
                  
                    if (curNet != FourG)
                    {
                        curNet.gameObject.SetActive(false);
                        curNet = FourG;
                        curNet.gameObject.SetActive(true);                        
                    }
                    color = Color.green;
                    curNet.color = color;
                    onStateChange?.Invoke(true);
                    break;
                case NetState.wifi:                                
                    curNet.gameObject.SetActive(false);
                    curNet = Wifi;
                    curNet.gameObject.SetActive(true);
                    color = Color.red;
                    curNet.color = color;
                    onStateChange?.Invoke(false);
                    break;
                case NetState.wifiConnectServer:                                   
                    curNet.gameObject.SetActive(false);
                    curNet = Wifi;
                    curNet.gameObject.SetActive(true);
                    color = Color.green;
                    curNet.color = color;
                    onStateChange?.Invoke(true);
                    break;
            }

        }
    }


    // Use this for initialization
    protected override void Start () {
        NotReachable.gameObject.SetActive(true);
        FourG.gameObject.SetActive(false);
        Wifi.gameObject.SetActive(false);
        color = Color.red;
        curNet = NotReachable;
        curNetState = NetState.none;
        netState = NetState.fourGConnectServer;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }
   
    protected override void OnServerConnect(int portHandle,bool result)
    {

        nets[portHandle] = result;
        bool hasNet=false;
        for (int i = 0; i < DealCommand.handles.Length; i++)
        {
            if (DealCommand.handles[i]>0&&nets[DealCommand.handles[i]])
            {
                print("有网");
                hasNet = true;
                break;
            }
        }
        netState = hasNet ? NetState.fourGConnectServer : NetState.noNet;
    }
}
