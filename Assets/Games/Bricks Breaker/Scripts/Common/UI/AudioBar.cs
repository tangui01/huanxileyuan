using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioBar : MonoBehaviour, IDragHandler, IPointerUpHandler
{


    public enum SliderType
    {
        Effect,
        Music
    }

    public SliderType sliderType;
    Slider slider;

    public Color colorMute;
    public Color colorOn;
    public Image audioIcon;
    public Image iconMude;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = Value;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        BricksBreakerSoundManager.Instance.PlayEffect(SoundList.sound_common_btn_in);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slider.value < 0)
        {
            slider.value = 0;
        }
        else if (slider.value > 1)
        {
            slider.value = 1;
        }

        Value = slider.value;
    }
    float Value
    {
        get
        {
            if (sliderType == SliderType.Music)
            {
                return Data.VolumeMusic;
            }
            else
            {
                return Data.VolumeEffect;
            }
        }
        set
        {
            if (sliderType == SliderType.Music)
            {
                Data.VolumeMusic = value;
            }
            else
            {
                Data.VolumeEffect = value;
            }
        }
    }
}
