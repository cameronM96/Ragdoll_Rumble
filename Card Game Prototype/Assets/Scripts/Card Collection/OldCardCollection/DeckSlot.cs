using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class DeckSlot : MonoBehaviour
{
    public Text deckNameText;
    public GameObject deckNotCompleteObject;

    private bool selected = false;

    public DeckInfo deckInformation { get; set; }

    private void ApplyInformationToSlot(DeckInfo info)
    {
       deckInformation = info;

        deckNotCompleteObject.SetActive(!info.IsComplete());
        deckNameText.text = info.deckName;
    }

    private void OnMouseDown()
    {
        if (!selected)
        {
            selected = true;
            if (deckInformation.IsComplete())
            {
                //changed based on if selected
            }
            DeckSelectionScreen.Instance.deckPicker.SelectDeck(this);
            DeckSlot[] deckSlots = GameObject.FindObjectsOfType<DeckSlot>();
            foreach ( DeckSlot ds in deckSlots)
            {
                if (ds != this)
                {
                    ds.Deselect();
                }
            }
        }
        else
        {
            Deselect();
            DeckSelectionScreen.Instance.deckPicker.SelectDeck(null);
        }
    }

    public void Deselect()
    {
        selected = false;
    }

    public void InstantDeselect()
    {
        selected = false;
    }
}
