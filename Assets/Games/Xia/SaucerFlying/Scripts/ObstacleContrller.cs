using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SaucerFlying
{
    public class ObstacleContrller : MonoBehaviour
    {
        [Header("Config")]
        [Header("Position")]
        [SerializeField]
        private bool isCenter;

        [Header("Scale")]
        [SerializeField]
        [Range(0, 1)]
        private float scaleFrequency = 0;

        [SerializeField]
        private Vector3 minScale = Vector3.one;

        [SerializeField]
        private Vector3 maxScale = Vector3.one * 2;

        [Header("Rotation")]
        [SerializeField]
        private bool randomRotationAtStart;

        [SerializeField]
        private Vector3 minRotation;

        [SerializeField]
        private Vector3 maxRotation;

        [SerializeField]
        [Range(0, 1f)]
        private float rotationFrequency = 0;

        [SerializeField]
        private Vector3 dirRotation;

        [SerializeField]
        private float minSpeedRotation = 3;

        [SerializeField]
        private float maxSpeedRotaion = 6;

        private float currentSpeedRotation;
        private bool isRotate;
        

        // Use this for initialization
        void Start()
        {
            if (Random.Range(0.001f, 0.999f) <= scaleFrequency)
                transform.localScale = new Vector3(Random.Range(minScale.x, maxScale.x),
                    Random.Range(minScale.y, maxScale.y), Random.Range(minScale.z, maxScale.z));
            
            if (randomRotationAtStart)
                transform.rotation = Quaternion.Euler(Random.Range(minRotation.x, maxRotation.x),
                    Random.Range(minRotation.y, maxRotation.y), Random.Range(minRotation.z, maxRotation.z));

            currentSpeedRotation = Random.Range(minSpeedRotation, (float)maxSpeedRotaion);
            if (Random.Range(0.001f, 0.999f) <= rotationFrequency)
                isRotate = true;
            else
                isRotate = false;
            StartCoroutine(CR_SetPosition());
        }
        

        IEnumerator CR_SetPosition()
        {
            yield return new WaitForSeconds(0.05f);
            if (isCenter)
                transform.localPosition = new Vector3(0, SaucerFlyingRoadManager.Instance.tunnelHeight / 2, transform.localPosition.z);
        }

        // Update is called once per frame
        void Update()
        {
            if (isRotate)
            {
                transform.Rotate(currentSpeedRotation * dirRotation * Time.deltaTime);
            }
        }
    }
}
