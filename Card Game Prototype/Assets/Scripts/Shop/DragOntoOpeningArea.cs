using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DragOntoOpeningArea : MonoBehaviour
{
    //private void OnCollisionEnter(Collision collision)
    //{
    //    transform.DOMove(ShopManager.instance.OpeningArea.transform.position, 0.5f).OnComplete(() =>
    //    {
    //        GetComponent<ScriptToOpenOnePack>().AllowToOpenThisPack();
    //    });
    //}

    private void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            GetComponent<ScriptToOpenOnePack>().AllowToOpenThisPack();
        }
    }
}
