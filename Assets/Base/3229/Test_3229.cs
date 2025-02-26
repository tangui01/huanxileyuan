using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class Test_3229 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool value = false;
    // Update is called once per frame
    void Update()
    {
        for (int j = 0; j < 2; j++)
        for (int i = 0; i < 8; i++)
        {
            if (DealCommand.GetKeyDown(j, (AppKeyCode)i))
            {
                Debug.Log($"Íæ¼Ò{j} Down " + (AppKeyCode)i);
            }
        }
        if (DealCommand.GetKeyDown(0, AppKeyCode.ExtCh1))
        {
            value = !value;
            for (int j = 0; j < 2; j++)
                for (int i = 0; i < 4; i++)
                {
                    LibWGM.Rk3229SetGpio(j, i, value?1:0);

                }
        }
        for (int i = 0; i < 4; i++)
        {
            if(DealCommand.GetKeyDown((BgKeyCode)i))
                 Debug.Log($"ºóÌ¨{i} Down " + (BgKeyCode)i);
        }
       
    }
}
