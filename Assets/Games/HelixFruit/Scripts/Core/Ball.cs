using System;
using System.Collections;
using UnityEngine;

namespace Sans.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] float _bouncePower;
        [SerializeField] float _splatDuration = 2f;
        [SerializeField] float _minVelocityPower = 1f;
        [SerializeField] Transform _splat;
        [SerializeField] ParticleSystem _splatEffect;
        [SerializeField] GameObject _trailFX;
        [SerializeField] TrailRenderer _trail;

        Color _ballColor;
        bool _isLevelComplete;
        bool _onPower;
        bool _preventCollidingTwice;
        Vector3 _originalScale;
        Rigidbody _rb;
        Renderer _renderer;
        Material _defaultMaterial;
        SoundController _soundController;

        public static event Action OnPlayerDeath;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _renderer = GetComponent<Renderer>();
            _defaultMaterial = GetComponent<Renderer>().material;
        }

        private void Start()
        {
            _soundController = SoundController.Instance;
            _originalScale = transform.localScale;
            _ballColor = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), .6f, .8f);
            SetColor();
        }

        private void Update()
        {
            if (_isLevelComplete) return;

            if (_rb.velocity.y <= -_minVelocityPower)
            {
                _rb.velocity = -Vector3.up * _minVelocityPower;

                if (_onPower) return;

                _onPower = true;
                _trailFX.SetActive(true);
                _trail.gameObject.SetActive(false);
            }

            BallSquash();
        }


        private void OnCollisionExit(Collision collision)
        {
            _preventCollidingTwice = false;
            _onPower = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isLevelComplete) return;

            if (collision.gameObject.CompareTag("Ground"))
            {
                if (Math.Abs(collision.GetContact(0).normal.x) >= .9f || collision.GetContact(0).normal.y < -0.1f)
                    return;

                if (_onPower) DestroyGroundSet(collision.collider);

                Jump();
                SpawnSplat(collision);
                _splatEffect.Play();

                if (!_preventCollidingTwice)
                {
                    _preventCollidingTwice = true;
                    AudioManager.Instance.playerEffect1(_soundController.GetClip(AudioType.BallBounce));
                };
            }
        }

        public void HitObstacle(Collider collider)
        {
            if (_onPower)
            {
                DestroyGroundSet(collider);
                Jump();
                return;
            }
            Death();
        }

        public void HitFinishLine()
        {
            _isLevelComplete = true;
            transform.localScale = _originalScale;
             AudioManager.Instance.playerEffect1(_soundController.GetClip(AudioType.Win));
        }

        public void Revive()
        {
            StartCoroutine(ReviveRoutine());
        }

        private IEnumerator ReviveRoutine()
        {
            _trail.gameObject.SetActive(false);

            Vector3 currentPos = transform.position;
            currentPos.y += 5f;
            transform.position = currentPos;

            _isLevelComplete = false;
            _renderer.enabled = true;

            yield return new WaitForSeconds(1f);
            _rb.isKinematic = false;

            _onPower = true;
            _trailFX.SetActive(true);
        }

        public void Death(bool isTimeDestroyed = false)
        {
            if (_isLevelComplete) return;
            _isLevelComplete = true;
            if (!isTimeDestroyed)
            {
                OnPlayerDeath?.Invoke();
            }
            _splatEffect.Play();
            _renderer.enabled = false;
            _rb.isKinematic = true;
            AudioManager.Instance.playerEffect1(_soundController.GetClip(AudioType.Die));
        }

        private void DestroyGroundSet(Collider collider)
        {
            GroundSet set = collider.transform.parent.GetComponent<GroundSet>();
            set?.DestroyPiece(_defaultMaterial);

            _trailFX.SetActive(false);
            _trail.gameObject.SetActive(true);
        }

        private void SpawnSplat(Collision collision)
        {
            Transform newSplat = Instantiate(_splat.gameObject, _splat.position, _splat.rotation, collision.transform).transform;
            Vector3 newPosition = collision.contacts[0].point;
            newPosition.y = collision.transform.position.y + .18f;
            newSplat.position = newPosition;
            Color newColor = newSplat.gameObject.GetComponent<SpriteRenderer>().color;
            newColor.a = 0;
            //LeanTween.color(newSplat.gameObject, newColor, _splatDuration);
            Destroy(newSplat.gameObject, _splatDuration);
        }

        private void Jump()
        {
            _rb.velocity = Vector3.up * _bouncePower;
        }

        private void BallSquash()
        {
            float velocity = _rb.velocity.y;

            Vector3 newScale = transform.localScale;
            newScale.y = .3f + Mathf.Abs(velocity / 40);
            transform.localScale = newScale;
        }

        private void SetColor()
        {
            GetComponent<Renderer>().material.color = _ballColor;
            _splat.GetComponent<SpriteRenderer>().color = _ballColor;
            _splatEffect.GetComponent<ParticleSystemRenderer>().materials[0].SetColor("_TintColor",_ballColor) ;
            _trailFX.GetComponent<ParticleSystemRenderer>().material.color = _ballColor;
            _trail.material.color = _ballColor;
        }
    }
}
