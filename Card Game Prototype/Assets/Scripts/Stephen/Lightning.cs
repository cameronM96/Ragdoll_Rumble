using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float minTime;
    public float threshhold;
    public float LastTime;
    public GameObject lightning1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - LastTime) > minTime)
        {
            if(Random.value > threshhold)
            {
                lightning1.SetActive (true);
            }
            else
            {
                lightning1.SetActive(false);
                LastTime = Time.time;
            }
            
        }
    }
}
