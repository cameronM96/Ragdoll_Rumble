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
                slider.value = PlayerPreferances.Master;
                break;
            case SliderTypes.Vocals:
                slider.value = PlayerPreferances.Vocals;
                break;
            case SliderTypes.Sfx:
                slider.value = PlayerPreferances.SFX;
                break;
            case SliderTypes.Music:
                slider.value = PlayerPreferances.Music;
                break;
            default:
                break;
        }
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
