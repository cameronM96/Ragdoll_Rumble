using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapTexturesRaw : MonoBehaviour
{
    public GameObject Initimage;
    public Texture image1;
    public Texture image2;
    public Texture image3;
    public float WaitTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(transition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator transition()
    {
        while (true)
        {
            yield return new WaitForSeconds(WaitTime);
            Initimage.GetComponent<RawImage>().texture = image2;
            yield return new WaitForSeconds(WaitTime);
            Initimage.GetComponent<RawImage>().texture = image3;
            yield return new WaitForSeconds(WaitTime);
            Initimage.GetComponent<RawImage>().texture = image1;
        }
    }
}
