using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WGM;
/****************************************************
    文件：CrazyKitchenPlayer.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：疯狂厨房玩家
*****************************************************/
namespace Crzaykitchen
{
    public enum CrazyKitchenPlayerState
    {
        Idle = 0,
        Walking = 1,
        
    }

    public class CrazyKitchenPlayer : MonoBehaviour,IKitchenObjectParent
{
    private int MoveX = 0;
    private int MoveY = 0;
    private Vector2 inputVector2;
    private Vector3 Dir;
    private Rigidbody Rb;
    private Animator Ani;
    [SerializeField]private CrazyKitchenPlayerState state;


    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;
    
    private BaseCounter currentSelestCounter;
    private KitchenObject kitchenObject;
    private float stepSoundRate = .13f;
    private float stepSoundTimer = 0;
    private void Awake()
    {
        Rb=GetComponent<Rigidbody>();
        Ani = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (CrzayKitchenGameManager.instance.GameOver||Time.timeScale.Equals(0))
        {
            return;
        }
        HandleInteraction();
        InteractInput();
    }
    private void FixedUpdate()
    {
        if (CrzayKitchenGameManager.instance.GameOver)
        {
            return;
        }
        switch (state)
        {
            case CrazyKitchenPlayerState.Idle :
                Idle();
                MoverInput();
                break;
            case CrazyKitchenPlayerState.Walking :
                MoverInput();
                Move();
                MoveRotate();
                break;
        }
    }
    private void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo, 2f,counterLayerMask))
        {
            if (hitinfo.collider.TryGetComponent<BaseCounter>(out BaseCounter clearCounter))
            {
                SetSelectedCounter(clearCounter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    public void SetSelectedCounter(BaseCounter counter)
    {
        if (counter != currentSelestCounter)
        {
            currentSelestCounter?.Deselect();
            currentSelestCounter = counter;
            currentSelestCounter?.Select();
        }
    }
    public void MoverInput()
    {
        if (DealCommand.GetKey(1,AppKeyCode.Bet))
        {
            MoveY = 1;
        }
        else if (DealCommand.GetKey(1,AppKeyCode.ExtCh0))
        {
            MoveY = -1;
        }
        else
        {
            MoveY = 0;
        }
        if (DealCommand.GetKey(1,AppKeyCode.TicketOut))
        {
            MoveX = -1;
        }
        else if (DealCommand.GetKey(1,AppKeyCode.Flight))
        {
            MoveX = 1;
        }
        else
        {
            MoveX = 0;
        }
        inputVector2=new Vector2(MoveX,MoveY);
        if (inputVector2 != Vector2.zero)
        {
            state = CrazyKitchenPlayerState.Walking;
        }
        else
        {
            state = CrazyKitchenPlayerState.Idle;
        }
    }
    public void Move()
    {
        Ani.SetBool(CrzaykitchenConst.IsWalking,true);
        Dir=new Vector3(inputVector2.x,0,inputVector2.y);
        stepSoundTimer += Time.deltaTime;
        if (!AudioManager.Instance.Effect1ISPalyer())
        {
            SoundManager.Instance.PlayStepSound();
        }
        Rb.velocity = Dir * (CrzaykitchenConst.moveSpeed * Time.deltaTime);
    }
    public void MoveRotate()
    {
        if (Dir!=Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward,Dir,Time.deltaTime*CrzaykitchenConst.RotateSpeed);
        }
    }
    public void Idle()
    {
        Rb.velocity = Vector3.zero;
        AudioManager.Instance.StopEffect1Player();
        Ani.SetBool(CrzaykitchenConst.IsWalking,false);
    }
    public void InteractInput()
    {
        if (DealCommand.GetKeyDown(1, AppKeyCode.UpScore))
        {
            currentSelestCounter?.Interact(this);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+Dir.normalized*2);
    }

    public Transform GetKitchenObjectFollow()
    {
        return KitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollow(KitchenObjectSO ko)
    {
        return KitchenObjectHoldPoint;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
}

