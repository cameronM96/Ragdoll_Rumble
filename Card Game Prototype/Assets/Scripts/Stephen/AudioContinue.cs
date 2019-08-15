using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContinue : MonoBehaviour
{

    private static AudioContinue instance = null;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
