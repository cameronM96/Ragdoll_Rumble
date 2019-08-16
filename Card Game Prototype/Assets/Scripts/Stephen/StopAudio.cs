using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioContinue.Instance.gameObject.GetComponent<AudioSource>().Pause();
        AudioContinue.Instance.gameObject.GetComponent<AudioListener>().enabled = false;
    }
}
