﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUITrigger : MonoBehaviour
{
    public bool isFinishLevel = false;
    public bool disableWhenDone = false;
   [HideInInspector] public bool canTalkAgain = false;
    [HideInInspector] public bool hideSmallFaceIcon;
    [HideInInspector] public bool givePlayerAKey = false;
    public AudioClip soundDetectPlayer;
    
    public Dialogs[] dialogs;
    public Dialogs[] talkAgainDialogs;

    [Header("BOSS ON START BEHAVIOR")]
    public bool hideBossOnStart = false;
    public GameObject showBossEffect;
    public AudioClip bossVisibleSound;

    [Header("Show Boss Option")]
    public bool activeBoss = false;
    public BossManager bossObject;

    [Header("CHANGE GAME MUSIC")]
    public bool changeMusic = true;
    public AudioClip music;
    public float musicVolume = 0.8f;

    [Header("SET CAMERA MIN MAX")]
    public float limitLeftPos = -3;
    public float limitRightPos = 7;
    public bool setCameraLimitMin = true;
    public bool setCameraLimitMax = true;
    [Space]
    public KeyItem keyItem;

    [HideInInspector]
    public bool isTalk = false;
    [HideInInspector]
    public bool isTalking = false;
    [HideInInspector]
    public bool isTakingFinish = false;

    bool isGaveAKey = false;

    bool isFirstTalk = true;
    void Start()
    {
        if (hideBossOnStart)
        {
            bossObject.gameObject.SetActive(false);
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (isTalk && !canTalkAgain)
            yield break;
        
        if (other.GetComponent<SuperCommandoPlayer>())
        {
            SuperCommandoSoundManager.Instance.PlaySfx(soundDetectPlayer, 0.8f);
            if (setCameraLimitMin)
                SuperCommandoCameraFollow.Instance._min.x = transform.position.x - limitLeftPos;
            if (setCameraLimitMax)
                SuperCommandoCameraFollow.Instance._max.x = transform.position.x + limitRightPos;


            SuperCommandoGameManager.Instance.Player.velocity.x = 0;


            SuperCommandoSoundManager.Instance.PauseMusic(true);
            SuperCommandoGameManager.Instance.Player.StopMove();
            SuperCommandoMenuManager.Instance.TurnController(false);
            SuperCommandoMenuManager.Instance.TurnGUI(false);

            if (setCameraLimitMin)
            {
                Vector3 targetPos = SuperCommandoCameraFollow.Instance.transform.position;
                targetPos.z = SuperCommandoCameraFollow.Instance.transform.position.z;

                SuperCommandoCameraFollow.Instance.isFollowing = false;
                Vector3 mainCameraStartPoint = SuperCommandoCameraFollow.Instance.transform.position;

                float percent = 0;
                var targetBack = new Vector3(SuperCommandoCameraFollow.Instance._min.x + SuperCommandoCameraFollow.Instance.CameraHalfWidth, mainCameraStartPoint.y, mainCameraStartPoint.z);
                while (percent < 1)
                {
                    percent += Time.deltaTime * 1;
                    percent = Mathf.Clamp01(percent);
                    SuperCommandoCameraFollow.Instance.transform.position = Vector3.Lerp(targetPos, targetBack, percent);
                    yield return null;
                }
                SuperCommandoCameraFollow.Instance.isFollowing = true;
            }

            if (hideBossOnStart)
            {
                bossObject.gameObject.SetActive(true);
                if (showBossEffect)
                    Instantiate(showBossEffect, bossObject.gameObject.transform.position, Quaternion.identity);
                SuperCommandoSoundManager.Instance.PlaySfx(bossVisibleSound);
                yield return new WaitForSeconds(2);
            }
            
            isTalk = true;
            isTalking = true;
            DialogManager.Instance.StartDialog(isFirstTalk ? dialogs : talkAgainDialogs, gameObject, disableWhenDone, isFinishLevel, hideSmallFaceIcon, this);
            isFirstTalk = false;
        }
    }

    //called from DialogManager
    public void FinishDialog()
    {
        StartCoroutine(FinishDialogueCo());
    }


    IEnumerator FinishDialogueCo()
    {
        if (givePlayerAKey && !isGaveAKey)
        {
            isGaveAKey = true;
            Instantiate(keyItem, SuperCommandoGameManager.Instance.Player.transform.position, Quaternion.identity);
        }
        
        if (activeBoss)
        {
            yield return new WaitForSeconds(1);
            bossObject.Play();
        }

        SuperCommandoSoundManager.Instance.PauseMusic(false);
        if (changeMusic)
            SuperCommandoSoundManager.Instance.PlayMusic(music, musicVolume);

        SuperCommandoControllerInput.Instance.StopMove();
        SuperCommandoMenuManager.Instance.TurnController(true);
        SuperCommandoMenuManager.Instance.TurnGUI(true);

        isTakingFinish = true;
    }

    private void OnDrawGizmos()
    {
        if (activeBoss && bossObject)
            Gizmos.DrawLine(transform.position, bossObject.transform.position);

        if (setCameraLimitMin)
        {
            Gizmos.DrawLine(transform.position + Vector3.left * limitLeftPos, transform.position);
            Gizmos.DrawSphere(transform.position + Vector3.left * limitLeftPos, 0.2f);
        }

        if (setCameraLimitMax)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * limitRightPos);
            Gizmos.DrawSphere(transform.position + Vector3.right * limitRightPos, 0.2f);
        }
    }
}
