using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class PlayerCount : MonoBehaviour
{
    UILabel mUILabel;
    public float longTime = 5;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        mUILabel = GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        mUILabel.text = LibWGM.machine.PlayerCount.ToString();
        if (DealCommand.GetKey(BgKeyCode.Background))
        {
            timer += Time.deltaTime;
            if (timer >= longTime)
            {
                timer = 0;
                LibWGM.machine.PlayerCount++;
                if (LibWGM.machine.PlayerCount >= 3)
                    LibWGM.machine.PlayerCount = 1;
            }
        }
        else
            timer = 0;
    }
}
