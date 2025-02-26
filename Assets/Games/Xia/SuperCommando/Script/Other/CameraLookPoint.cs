using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookPoint : MonoBehaviour
{
    [Header("Follow Order")]
    [ReadOnly] public bool lookTarget = true;
    public bool setCameraLimitMin = true;
    public bool setCameraLimitMax = true;
    public bool movePlayerToPoint = true;
    public bool activeGate = true;
    public bool changeGameMusic = true;
    [Space]
    public AudioClip soundShowUp;
    Vector3 mainCameraStartPoint;
    
    bool moveCameraToTarget = true;
    [Header("MOVE TO TARGET")]
    public float cameraMoveToSpeed = 1;
    public float cameraMoveBackSpeed = 2;
    public Transform targetCameraLook;
    [Header("SET CAMERA MIN MAX")]
    public Transform limitMinPos, limitMaxPos;
    [Header("MOVE PLAYER")]
    public float movePlayerSpeed = 1.5f;
    public Transform movePlayerToPointPos;
    [Header("ACTIVE GATE")]
    public float delayGate = 1f;
    public GameObject theGate;
    [Header("GAME MUSIC")]
    public AudioClip newMusic;

    SuperCommandoCameraFollow mainCamera;
    bool isWorked = false;

    void Start()
    {
        mainCamera = SuperCommandoCameraFollow.Instance;
    }
    
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (isWorked)
            yield break;

        if (other.gameObject != SuperCommandoGameManager.Instance.Player.gameObject)
            yield break;

        isWorked = true;
        if (setCameraLimitMin)
            mainCamera._min.x = limitMinPos.position.x;
        if(setCameraLimitMax)
            mainCamera._max.x = limitMaxPos.position.x;

        SuperCommandoGameManager.Instance.Player.Frozen(true);
        SuperCommandoMenuManager.Instance.TurnController(false);
        SuperCommandoSoundManager.Instance.PlaySfx(soundShowUp);

        mainCamera.isFollowing = false;
        mainCameraStartPoint = mainCamera.transform.position;

        SuperCommandoSoundManager.Instance.PauseMusic(true);
        GroupEnemySystemUI.Instance.ShowWarning(true);

        Vector3 targetPos = targetCameraLook.position;
        targetPos.z = mainCameraStartPoint.z;

        float percent = 0;


       

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * cameraMoveToSpeed;
            percent = Mathf.Clamp01(percent);
            mainCamera.transform.position = Vector3.Lerp(mainCameraStartPoint, targetPos, percent);
            yield return null;
        }

        percent = 0;
        if (setCameraLimitMin)
        {
            var targetBack = new Vector3(mainCamera._min.x + mainCamera.CameraHalfWidth, mainCameraStartPoint.y, mainCameraStartPoint.z);
            while (percent < 1)
            {
                percent += Time.deltaTime * cameraMoveBackSpeed;
                percent = Mathf.Clamp01(percent);
                mainCamera.transform.position = Vector3.Lerp(targetPos, targetBack, percent);
                yield return null;
            }
        }
        else
        {
            while (percent < 1)
            {
                percent += Time.deltaTime * cameraMoveBackSpeed;
                percent = Mathf.Clamp01(percent);
                mainCamera.transform.position = Vector3.Lerp(targetPos, mainCameraStartPoint, percent);
                yield return null;
            }
        }

        percent = 0;
        if (movePlayerToPoint)
        {
            Vector3 playerStartPos = SuperCommandoGameManager.Instance.Player.transform.position;
            Vector3 playerEndPos = movePlayerToPointPos.position;
            playerEndPos.y = playerStartPos.y;
            while (percent < 1)
            {
                percent += Time.deltaTime * movePlayerSpeed;
                percent = Mathf.Clamp01(percent);
                SuperCommandoGameManager.Instance.Player.transform.position = Vector3.Lerp(playerStartPos, playerEndPos, percent);
                yield return null;
            }
        }

        if (activeGate)
        {
            theGate.SetActive(true);
            yield return new WaitForSeconds(delayGate);
        }

        if (changeGameMusic)
            SuperCommandoSoundManager.Instance.PlayMusic(newMusic, 1);


        mainCamera.isFollowing = true;
        GroupEnemySystemUI.Instance.ShowWarning(false);
        SuperCommandoSoundManager.Instance.PauseMusic(false);
        SuperCommandoGameManager.Instance.Player.Frozen(false);
        SuperCommandoMenuManager.Instance.TurnController(true);
       
    }

    private void OnDrawGizmos()
    {
        if (targetCameraLook)
        {
            Gizmos.DrawLine(transform.position, targetCameraLook.position);
            Gizmos.DrawWireCube(targetCameraLook.position, new Vector2(Camera.main.orthographicSize * ((float)Screen.width / Screen.height) * 2, Camera.main.orthographicSize*2));
        }
    }
}
