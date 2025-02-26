using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WGM;
using Random = UnityEngine.Random;

namespace PhantasmalTrack
{
    public class PhantasmalTrackPlayerController : MonoBehaviour
    {
        public Transform rayDown, rayLeft, rayRight;
        private LayerMask platformLayer, obstacleLayer;

        /// <summary>
        /// 是否向左移动，反之向右
        /// </summary>
        private bool isMoveLeft = false;

        /// <summary>
        /// 是否正在跳跃
        /// </summary>
        private bool isJumping = false;

        private Vector3 nextPlatformLeft, nextPlatformRight;
        private ManagerVars vars;
        private Rigidbody2D my_Body;
        private SpriteRenderer spriteRenderer;
        private bool isMove = false;
        private AudioSource m_AudioSource;
        public List<GameObject> buffPrefabs;
        private ParticleSystem particleSystem;
        private void Awake()
        {
            EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
            EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
            vars = FindObjectOfType<ManagerVars>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            my_Body = GetComponent<Rigidbody2D>();
            platformLayer.value = 256;
            obstacleLayer.value = 512;
            
            var obj = Instantiate(buffPrefabs[Random.Range(0, buffPrefabs.Count)],new Vector3(0, -200, 0), transform.Find("Buff").rotation);
            obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            obj.transform.position = transform.Find("Buff").position;
            obj.transform.parent = transform.Find("Buff");
            particleSystem= obj.GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            ChangeSkin(Random.Range(0, 4));
            effect = new GameObject[CFXPrefab.Count];
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
            EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        }
        
        public  List<GameObject> CFXPrefab = new List<GameObject>();
        private GameObject[] effect;
        private int effectIndex = 0;
        private Transform CFXend;
        public AudioClip CFXendSound;
        private Image ScoreImg;
        public void AddScoreVFX()
        {
            int ran = Random.Range(0, CFXPrefab.Count);
            if (effect[ran] == null)
            {
                CFXend = GameObject.Find("CFXend").transform;
                effect[ran] = Instantiate(CFXPrefab[ran],transform.Find("CFX"));
                effect[ran].transform.parent = GameObject.Find("Bg").transform;
            }
            else
            {
                effect[ran].transform.position = transform.Find("CFX").position;
                effect[ran].SetActive(true);
            }
            AudioManager.Instance.SetEff(CFXendSound,1,Random.Range(0,3));
            StartCoroutine(CFXMove(ran));
        }
        IEnumerator CFXMove(int ran)
        {
            Vector3 pos = effect[ran].transform.position;
            float moveTime = Random.Range(1.5f, 2f);
            float lerpIndex = 0;
            float lerpSpeed = 1f;
            if(ScoreImg == null)
                ScoreImg = GameObject.Find("Image_Score").GetComponent<Image>();
            bool isTri = true;
            while (lerpIndex < 1f)
            {
                lerpIndex += Time.deltaTime * lerpSpeed / moveTime;
                lerpSpeed += 0.1f;
                pos.x = Mathf.Lerp(pos.x, CFXend.position.x, Mathf.Clamp01(lerpIndex));
                pos.y = Mathf.Lerp(pos.y, CFXend.position.y, Mathf.Clamp01(lerpIndex));
                effect[ran].transform.position = pos;
                if (lerpIndex > 0.55f && isTri)
                {
                    isTri = false;
                    ScoreImg.GetComponent<Animator>()?.SetTrigger("Play");
                }
                yield return null;
            }
            effect[ran].SetActive(false);

        }
        /// <summary>
        /// 音效是否开启
        /// </summary>
        /// <param name="value"></param>
        private void IsMusicOn(bool value)
        {
            // m_AudioSource.mute = !value;
        }

        /// <summary>
        /// 更换皮肤的调用
        /// </summary>
        /// <param name="skinIndex"></param>
        private void ChangeSkin(int skinIndex)
        {
            spriteRenderer.sprite = vars.characterSkinSpriteList[skinIndex];
        }

        private int count;

        private bool IsPointerOverGameObject(Vector2 mousePosition)
        {
            //创建一个点击事件
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击的UI
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }
        [HideInInspector]
        public bool isContinueJump = false;
        private float JumpTime = 0.1f;
        private void Update()
        {
            Debug.DrawRay(rayDown.position, Vector2.down * 1, Color.red);
            Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
            Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);

            //if (Application.platform == RuntimePlatform.Android ||
            //    Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    int fingerId = Input.GetTouch(0).fingerId;
            //    if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;
            //}
            //else
            //{
            //    if (EventSystem.current.IsPointerOverGameObject()) return;
            //}
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.J))
                isContinueJump = !isContinueJump;
                
            
            //点击的是左边屏幕 mousePos.x <= Screen.width / 2
            if (Input.GetKey(KeyCode.A))
            {
                isMoveLeft = true;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            //点击的右边屏幕 mousePos.x > Screen.width / 2
            else if (Input.GetKey(KeyCode.D))
            {
                isMoveLeft = false;
                transform.localScale = Vector3.one;
            }
#else
            if(DealCommand.GetKeyDown(1,(AppKeyCode)3))
                isContinueJump = !isContinueJump;
            if (DealCommand.GetKey(1,(AppKeyCode)6))
            {
                    isMoveLeft = true;
                    transform.localScale = new Vector3(-1, 1, 1);
            }
            //点击的右边屏幕 mousePos.x > Screen.width / 2
            else if (DealCommand.GetKey(1,(AppKeyCode)1))
            {
                    isMoveLeft = false;
                    transform.localScale = Vector3.one;
            }
#endif
            //游戏结束了
            if (my_Body.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
            {
                AudioManager.Instance.playerEffect1(vars.fallClip);
                spriteRenderer.sortingLayerName = "Default";
                GetComponent<BoxCollider2D>().enabled = false;
                GameManager.Instance.IsGameOver = true;
                StartCoroutine(DealyShowGameOverPanel());
            }

            if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
            {
                AudioManager.Instance.playerEffect1(vars.hitClip);
                GameObject go = ObjectPool.Instance.GetDeathEffect();
                go.SetActive(true);
                go.transform.position = transform.position;
                GameManager.Instance.IsGameOver = true;
                spriteRenderer.enabled = false;
                StartCoroutine(DealyShowGameOverPanel());
            }

            if (transform.position.y - Camera.main.transform.position.y < -5 &&
                GameManager.Instance.IsGameOver == false)
            {
                AudioManager.Instance.playerEffect1(vars.fallClip);
                GameManager.Instance.IsGameOver = true;
                StartCoroutine(DealyShowGameOverPanel());
            }
        }
        IEnumerator DealyShowGameOverPanel()
        {
            yield return new WaitForSeconds(1f);
            //调用结束面板
            EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
            ChangeSkin(Random.Range(0, 4));
        }

        private GameObject lastHitGo = null;

        /// <summary>
        /// 是否检测到平台
        /// </summary>
        /// <returns></returns>
        private bool IsRayPlatform()
        {
            RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Platform")
                {
                    if (lastHitGo != hit.collider.gameObject)
                    {
                        if (lastHitGo == null)
                        {
                            lastHitGo = hit.collider.gameObject;
                            return true;
                        }

                        EventCenter.Broadcast(EventDefine.AddScore);
                        lastHitGo = hit.collider.gameObject;
                    }

                    return true;
                }
            }

            return false;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver
                                                            || GameManager.Instance.IsPause)
                return;
            JumpTime += 0.02f;
            if (isContinueJump && isJumping == false && nextPlatformLeft != Vector3.zero && JumpTime>0.15f)
            {
                JumpTime = 0;
                if (isMove == false)
                {
                    EventCenter.Broadcast(EventDefine.PlayerMove);
                    isMove = true;
                }
                
                AudioManager.Instance.playerEffect1(vars.jumpClip);
                EventCenter.Broadcast(EventDefine.DecidePath);
                isJumping = true;
                Jump();
            }
        }

        /// <summary>
        /// 是否检测到障碍物
        /// </summary>
        /// <returns></returns>
        private bool IsRayObstacle()
        {
            RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
            RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

            if (leftHit.collider != null)
            {
                if (leftHit.collider.tag == "Obstacle")
                {
                    return true;
                }
            }

            if (rightHit.collider != null)
            {
                if (rightHit.collider.tag == "Obstacle")
                {
                    return true;
                }
            }

            return false;
        }

        private void Jump()
        {
            if (isJumping)
            {
                if (isMoveLeft)
                {
                    transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                    transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
                }
                else
                {
                    transform.DOMoveX(nextPlatformRight.x, 0.2f);
                    transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f); 
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Platform")
            {
                isJumping = false;
                Vector3 currentPlatformPos = collision.gameObject.transform.position;
                nextPlatformLeft = new Vector3(currentPlatformPos.x -
                                               vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
                nextPlatformRight = new Vector3(currentPlatformPos.x +
                                                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Pickup")
            {
                AudioManager.Instance.playerEffect1(vars.diamondClip);
                EventCenter.Broadcast(EventDefine.AddDiamond);
                //吃到钻石了
                collision.gameObject.SetActive(false);
                particleSystem.Play();
            }
        }
    }
}
