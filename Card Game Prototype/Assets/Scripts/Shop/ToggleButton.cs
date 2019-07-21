using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public SinglesShop shop;

    public GameObject[] buttons;

    public Color onColour;
    public Color offColour;
    public int onValue;
    public int offValue = 0;

    public bool toggle;

    private void Start()
    {
        offColour = GetComponent<Button>().colors.normalColor;
    }

    public void Toggle()
    {
        toggle = !toggle;

        ColorBlock cB = GetComponent<Button>().colors;

        if (toggle)
        {
            cB.normalColor = onColour;
            shop.cardType = onValue;
        }
        else
        {
            cB.normalColor = offColour;
            shop.cardType = offValue;
        }

        shop.LoadCards();

        SwitchOthersOff();
    }

    public void SwitchOthersOff()
    {
        foreach (GameObject b in buttons)
        {
            ColorBlock cB = b.GetComponent<Button>().colors;
            cB.normalColor = offColour;
            b.GetComponent<ToggleButton>().toggle = false;
        }
    }
}
