using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    public delegate void SaveSettings();
    public static event SaveSettings saveSettings;

    public Slider slider;
    public enum SliderTypes {None, Master, Vocals, Sfx, Music };
    public SliderTypes sliderType = SliderTypes.None;

    private void OnEnable()
    {
        PlayerPreferances.LoadPrefs();
    }

    private void OnDisable()
    {
        PlayerPreferances.SavePrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        if (slider == null)
            return;

        PlayerPreferances.LoadPrefs();
        switch (sliderType)
        {
            case SliderTypes.None:
                break;
            case SliderTypes.Master:
                if (PlayerPreferances.Master != -1)
                    slider.value = PlayerPreferances.Master;
                else
                    PlayerPreferances.Master = slider.value;
                break;
            case SliderTypes.Vocals:
                if (PlayerPreferances.Vocals != -1)
                    slider.value = PlayerPreferances.Vocals;
                else
                    PlayerPreferances.Vocals = slider.value;
                break;
            case SliderTypes.Sfx:
                if (PlayerPreferances.SFX != -1)
                    slider.value = PlayerPreferances.SFX;
                else
                    PlayerPreferances.SFX = slider.value;
                break;
            case SliderTypes.Music:
                if (PlayerPreferances.Music != -1)
                    slider.value = PlayerPreferances.Music;
                else
                    PlayerPreferances.Music = slider.value;
                break;
            default:
                break;
        }
        PlayerPreferances.SavePrefs();
    }

    public void UpdateValue()
    {
        switch (sliderType)
        {
            case SliderTypes.None:
                break;
            case SliderTypes.Master:
                PlayerPreferances.Master = slider.value;
                break;
            case SliderTypes.Vocals:
                PlayerPreferances.Vocals = slider.value; ;
                break;
            case SliderTypes.Sfx:
                PlayerPreferances.SFX = slider.value; ;
                break;
            case SliderTypes.Music:
                PlayerPreferances.Music = slider.value; ;
                break;
            default:
                break;
        }
    }

    public void SavePrefs()
    {
        PlayerPreferances.SavePrefs();
        saveSettings?.Invoke();
    }
}
