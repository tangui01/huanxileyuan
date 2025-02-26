using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class NewTank : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 TankInput(int PlayerID)
    {
        Vector3 mousePos = Vector3.zero;
        if (DealCommand.GetKey(PlayerID, (AppKeyCode)6))
            mousePos.x = -1;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.D))
            mousePos.x = 1;
        if (Input.GetKey(KeyCode.D) && DealCommand.GetKey(PlayerID, (AppKeyCode)6))
            mousePos.x = 0;
#else
        if (DealCommand.GetKey(PlayerID, (AppKeyCode)1))
            mousePos.x = 1;
        if (DealCommand.GetKey(PlayerID, (AppKeyCode)1) && DealCommand.GetKey(PlayerID, (AppKeyCode)6))
            mousePos.x = 0;
#endif        
        if (DealCommand.GetKey(PlayerID, (AppKeyCode)0))
            mousePos.y = 1;
        if (DealCommand.GetKey(PlayerID, (AppKeyCode)2))
            mousePos.y = -1;
        if (DealCommand.GetKey(PlayerID, (AppKeyCode)0) && DealCommand.GetKey(PlayerID, (AppKeyCode)2))
            mousePos.y = 0;
        return mousePos;
    }

    public Vector3 Verify(Vector3 movePos)
    {
        
        return movePos;
    }
}
