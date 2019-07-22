using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackShop : MonoBehaviour
{
    public ConfirmationWindow confirmWindow;
    public GameObject shopPackPanel;
    public GameObject shopPackTemplate;
    public Scrollbar scrollBar;
    public float startYPos;
    public float colLength;

    public List<PackOfCards> purchasablePacks;

    public void LoadWindow()
    {
        // Clear Shop
        foreach (Transform child in shopPackPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Add packs to shop
        foreach (PackOfCards pack in purchasablePacks)
        {
            GameObject newPack = CreatePack(pack);
            newPack.transform.SetParent(shopPackPanel.transform);
        }
    }

    public void ScrollWindow(Scrollbar bar)
    {
        Debug.Log(Mathf.CeilToInt(purchasablePacks.Count / 6f));
        float top = startYPos + (colLength * (Mathf.CeilToInt(purchasablePacks.Count / 6f)));
        if (top < startYPos)
            top = startYPos;

        Vector3 newPos = shopPackPanel.transform.localPosition;

        newPos.y = Map(bar.value, 0, 1, startYPos, top, true);

        shopPackPanel.transform.localPosition = newPos;
    }

    public GameObject CreatePack(PackOfCards newPack)
    {
        if (shopPackTemplate == null)
            return null;

        GameObject newCard = Instantiate(shopPackTemplate);

        newCard.GetComponent<PackHolder>().Initialise(newPack);

        return newCard;
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Mathf.Max(in_min, Mathf.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}