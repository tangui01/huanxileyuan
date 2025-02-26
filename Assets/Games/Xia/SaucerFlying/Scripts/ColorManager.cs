using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaucerFlying
{
    public enum ColorMode
    { BaseOnFog, UseDefault, Random}

    [System.Serializable]
    public class ObjectColorData
    {
        public Material material;
        public Color defaultColor;
        public ColorMode colorMode;
    }
    public class ColorManager : MonoBehaviour
    {
        static public ColorManager Instance { get; private set; }

        [SerializeField]
        private List<ObjectColorData> ColorDataList;
        [Header("List color of end line")]
        public Color[] listColor;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }
        private void Start()
        {
            int indexColor = Random.Range(0, ColorManager.Instance.listColor.Length);
            RenderSettings.fogColor = listColor[indexColor];

            for(int i = 0; i < ColorDataList.Count;i++)
            {
                switch(ColorDataList[i].colorMode)
                {
                    case ColorMode.BaseOnFog:
                        ColorDataList[i].material.color = listColor[indexColor];
                        break;
                    case ColorMode.UseDefault:
                        ColorDataList[i].material.color = ColorDataList[i].defaultColor;
                        break;
                    case ColorMode.Random:
                        ColorDataList[i].material.color = listColor[Random.Range(0, ColorManager.Instance.listColor.Length)];
                        break;
                }
            }
        }
    }
}
