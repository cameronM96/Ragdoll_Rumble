using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingModeToggle : MonoBehaviour
{
    private Toggle t;

    private void Awake()
    {
        t = GetComponent<Toggle>();
    }

    public void ValueChanged()
    {
        DeckBuildingScreen.Instance.collectionBrowserScript.ShowingCardsPlayerDoesNotOwn = t.isOn;
    }

    public void SetValue(bool value)
    {
        if (t != null)
        {
            t.isOn = value;
        }
    }
}
