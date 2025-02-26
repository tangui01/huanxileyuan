using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class NewSuperCommando : MonoBehaviour
{
    [HideInInspector]
    public float horizontalInput = 0f;
    [HideInInspector]
    public float verticalInput = 0f;

    private Vector3 viewportPos;
    [HideInInspector]
    public bool isPaused = false;

    private float JumpDownTriTime = 0;
    SuperCommandoPlayer player;
    void Start()
    {
        player = GetComponent<SuperCommandoPlayer>();
    }
    
    void Update()
    {
        viewportPos = Camera.main.WorldToViewportPoint(transform.position) ;
        if (isPaused)
        {
            verticalInput = 0;
            horizontalInput = 0;
            return;
        }
        if (DealCommand.GetKey(1,(AppKeyCode)0))
        {
            verticalInput = 1f;
        }
        else
        if (DealCommand.GetKey(1,(AppKeyCode)2))
        {
            verticalInput = -1f;
        }
        else
        {
            verticalInput = 0f;
        }
        
        if (DealCommand.GetKey(1,(AppKeyCode)6))
        {
            horizontalInput = -1f;
        }
        else if (DealCommand.GetKey(1,(AppKeyCode)1))
        {
            horizontalInput = 1f;
        }
        else
        {
            horizontalInput = 0f;
        }
        if(JumpDownTriTime > 0)
            JumpDownTriTime -= Time.deltaTime;
        if (DealCommand.GetKeyDown(1, (AppKeyCode)2))
        {
            if(JumpDownTriTime <= 0)
                JumpDownTriTime = 0.5f;
            else
                player.TriJumpDown();
        }
            
    }
    public Vector2 Verify(Vector2 movePos)
    {
        if (viewportPos.x <= 0.05 && movePos.x <= 0) movePos.x = 0f;
        if (viewportPos.x >= 0.95 && movePos.x >= 0) movePos.x = 0f;
        return movePos;
    }
}
