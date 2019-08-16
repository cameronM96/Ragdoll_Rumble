using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContinue : MonoBehaviour
{

    private static AudioContinue instance = null;

    public AudioSource audioSource;

    private void OnEnable()
    {
        SettingsSlider.saveSettings += UpdateVolume;
    }

    private void OnDisable()
    {
        SettingsSlider.saveSettings -= UpdateVolume;
    }

    public static AudioContinue Instance
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPreferances.Music * PlayerPreferances.Master;
    }

    public void UpdateVolume()
    {
        audioSource.volume = PlayerPreferances.Music * PlayerPreferances.Master;
    }
}
