using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackHolder : MonoBehaviour
{
    [HideInInspector] public PackOfCards pack;

    public Text nameText;
    public Image packImage;

    public GameObject costValues;
    public Text coinText;
    public Text gemText;

    public void Initialise(PackOfCards newPack)
    {
        pack = newPack;
        nameText.text = pack.packName;
        packImage.sprite = pack.packImage;
        coinText.text = "" + pack.coinCost;
        gemText.text = "" + pack.gemCost;
    }

    public void PackSelected()
    {
        PackShop shop = transform.parent.GetComponentInParent<PackShop>();

        if (shop != null && pack != null)
            shop.confirmWindow.OpenWindow(pack);
        else
            Debug.Log("Something went wrong and I couldn't find everything I needed");
    }
}