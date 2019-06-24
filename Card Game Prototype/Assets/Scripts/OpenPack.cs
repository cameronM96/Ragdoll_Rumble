using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenPack : MonoBehaviour
{
    public Image glowImage;
    public Color32 glowColour;
    private bool allowedToOpen = false;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    public void AllowToOpenThisPack()
    {
        allowedToOpen = true;
        ShopManager.instance.OpeningArea.AllowedToDragAPack = false;
        ShopManager.instance.OpeningArea.backButton.interactable = false;
    }

    private bool CursorOverPack()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach(RaycastHit h in hits)
        {
            if (h.collider == col)
            {
                passedThroughTableCollider = true;
            }
        }
        return passedThroughTableCollider;
    }

    private void OnMouseDown()
    {
        if (allowedToOpen)
        {
            allowedToOpen = false;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOLocalMoveZ(-2f, 0.5f));
            s.Append(transform.DOShakeRotation(1f, 20f, 20));

            s.OnComplete(() =>
            {
                ShopManager.instance.OpeningArea.ShowPackOpening(transform.position);
                if (ShopManager.instance.packsCreated > 0)
                {
                    ShopManager.instance.packsCreated--;
                }
                Destroy(gameObject);
            });
        }
    }
}   
