using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnOverCardFromPack : MonoBehaviour
{
    public Image glow;

    private float initialScale;
    private float scaleFactor = 1.1f;
    private bool turnedOver = false;
    private Card manager;

    private void Awake()
    {
        initialScale = transform.localScale.x;
        manager = GetComponent<Card>();
    }

    private void OnMouseDown()
    {
        if (turnedOver)
        {
            return;
        }
        turnedOver = true;
        transform.DORotate(Vector3.zero, 0.5f);
        ShopManager.instance.OpeningArea.numberOfCardsOpenedFromPack++;
    }
}
