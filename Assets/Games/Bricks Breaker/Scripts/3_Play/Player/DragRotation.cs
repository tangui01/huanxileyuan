using System;
using UnityEngine;
using UnityEngine.EventSystems;
using WGM;

public class DragRotation : MonoBehaviour {
    
    public Transform playerCenter;
    private float rotationz = 0f;
    [SerializeField] private float RotateSpeed=10;
    private void Update()
    {
        if(CtrGame.instance.IsLock||Time.timeScale==0) return;
        if (DealCommand.GetKey(1,AppKeyCode.TicketOut))
        {
            rotationz+=Time.deltaTime*RotateSpeed;
        }
        else if (DealCommand.GetKey(1,AppKeyCode.Flight))
        {
            rotationz-=Time.deltaTime*RotateSpeed;
        }
        playerCenter.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp((rotationz), -83, 83));
        if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore))
        {
            Player.instance.ShotBall();
        }
    }
}





