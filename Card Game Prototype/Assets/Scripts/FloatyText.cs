using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyText : MonoBehaviour
{
    public float speed = 1;
    public float lifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = null;
        this.transform.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
