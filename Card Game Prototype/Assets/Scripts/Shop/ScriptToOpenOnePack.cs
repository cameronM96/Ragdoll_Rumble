using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScriptToOpenOnePack : MonoBehaviour
{
    private bool allowedToOpen = false;
    private Collider col;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    public void AllowToOpenThisPack()
    {
        allowedToOpen = true;
        ShopManager.instance.OpeningArea.AllowedToDragAPack = false;
        // Disable back button so that player can not exit the pack opening screen while he has not opened a pack
        ShopManager.instance.OpeningArea.backButton.interactable = false;
    }

    private bool CursorOverPack()
    {
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        return passedThroughTableCollider;
    }

    private void OnMouseDown()
    {
        OpenPack();
    }

    void OpenPack()
    {
        Debug.Log("OPENING PACK");
        if (allowedToOpen)
        {
            // 0) prevent from opening again
            allowedToOpen = false;
            // Start a dotween sequence
            Sequence s = DOTween.Sequence();
            // 1) raise the pack to opening position
            s.Append(transform.DOLocalMoveZ(-2f, 0.5f));
            s.Append(transform.DOShakeRotation(1f, 20f, 20));

            s.OnComplete(() =>
            {
                // 2) add glow, particle system

                // 3): 
                ShopManager.instance.OpeningArea.ShowPackOpening(transform.position);
                if (ShopManager.instance.packsCreated > 0)
                    ShopManager.instance.packsCreated--;
                // 4) destroy this pack in the end of the sequence
                Destroy(gameObject);
            });
        }
    }
}
