using System.Collections;
using UnityEngine;

namespace SaucerFlying
{
    public class SaucerFlyingCameraController : MonoBehaviour
    {
        static public SaucerFlyingCameraController Instance;
        public Transform playerTransform;
        private Vector3 velocity = Vector3.zero;
        private Vector3 originalDistance;
        private float timeLast;
        //private float distanceTime = 0.2f;

        [Header("Camera Follow Smooth-Time")]
        public float smoothTime = 0.01f;

        [Header("Shaking Effect")]
        // How long the camera shaking.
        public float shakeDuration = 0.1f;
        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.2f;
        public float decreaseFactor = 0.3f;
        [HideInInspector]
        public Vector3 originalPos;

        private float currentShakeDuration;
        private float currentDistance;

        //[Header("Camera compared to Player")]
        //Elevation compared to Player
        //private float elvationCTP;

        //Distance compared to Player
        //private float distanceCTP;
        void OnEnable()
        {
            CharacterScroller.ChangeCharacter += ChangeCharacter;
        }
        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }
        void OnDisable()
        {
            CharacterScroller.ChangeCharacter -= ChangeCharacter;
        }

        void Start()
        {
            StartCoroutine(WaitingPlayerController());
        }

        private void LateUpdate()
        {
            if (SaucerFlyingGameManager.Instance.GameState == GameState.Playing && playerTransform != null)
            {
                Vector3 pos = playerTransform.position + originalDistance;
                Vector3 posHor = new Vector3(transform.position.x, transform.position.y, pos.z);
                GameObject obj = GameObject.Find("RoadManager");
                if (obj != null)
                {
                    transform.position = posHor;
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Mathf.Clamp(pos.x, (-SaucerFlyingRoadManager.Instance.tunnelWidth / 2) + 0.5f, (SaucerFlyingRoadManager.Instance.tunnelWidth / 2) - 0.5f),
                        Mathf.Clamp(pos.y, -SaucerFlyingRoadManager.Instance.tunnelHeight / 4, SaucerFlyingRoadManager.Instance.tunnelHeight / 4), pos.z), ref velocity, smoothTime);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothTime);
                }
            }
        }

        public void FixPosition()
        {
            transform.position = playerTransform.position + originalDistance;
        }

        public void ShakeCamera()
        {
            StartCoroutine(Shake());
        }

        IEnumerator Shake()
        {
            originalPos = transform.position;
            currentShakeDuration = shakeDuration;
            while (currentShakeDuration > 0)
            {
                transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
                currentShakeDuration -= Time.deltaTime * decreaseFactor;
                yield return null;
            }
            transform.position = originalPos;
            yield return new WaitForSeconds(0.5f);
            CommonUI.instance.BackMainPanel_OPen();
            yield return new WaitForSeconds(2f);
            FindObjectOfType<SaucerFlyingUIManager>().RestartGame();
        }

        void ChangeCharacter(int cur)
        {
            StartCoroutine(WaitingPlayerController());
        }

        IEnumerator WaitingPlayerController()
        {
            yield return new WaitForEndOfFrame();
            while (SaucerFlyingGameManager.Instance.playerController == null)
            {
                yield return null;
            }

            playerTransform = SaucerFlyingGameManager.Instance.playerController.transform;
            originalDistance = transform.position - playerTransform.transform.position;
        }

    }
}