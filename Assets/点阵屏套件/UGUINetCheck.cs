using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UGUINetCheck : UINetCheck  {
    public RawImage NotReachable, FourG, Wifi, curNet;
    protected override NetState netState
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
    protected override void Start()
    {
        NotReachable.gameObject.SetActive(true);
        FourG.gameObject.SetActive(false);
        Wifi.gameObject.SetActive(false);
        color = Color.red;
        curNet = NotReachable;
        curNetState = NetState.none;
        netState = NetState.fourGConnectServer;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnServerConnect(int portHandle, bool result)
    {

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
