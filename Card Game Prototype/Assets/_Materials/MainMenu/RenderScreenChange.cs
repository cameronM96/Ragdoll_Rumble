using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderScreenChange : MonoBehaviour{

    public GameObject RenderDisplay;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator fade()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            RenderDisplay.GetComponent<Animation>().Play("Crossfade1");
            yield return new WaitForSeconds(5f);
            RenderDisplay.GetComponent<Animation>().Play("Crossfade2");
        }
    }
}
