using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DeckInScrollList : MonoBehaviour
{
    public Text NameText;
    public GameObject DeleteDeckButton;
    public DeckInfo savedDeckInfo;

    public void EditThisDeck()
    {
        DeckBuildingScreen.Instance.HideScreen();
        DeckBuildingScreen.Instance.builderScript.BuildADeck();
        DeckBuildingScreen.Instance.builderScript.deckName.text = savedDeckInfo.deckName;

        foreach (Card ca in savedDeckInfo.deckCards)
        {
            DeckBuildingScreen.Instance.builderScript.AddCard(ca);
        }
        DecksStorage.Instance.allDecks.Remove(savedDeckInfo);
        DeckBuildingScreen.Instance.collectionBrowserScript.ShowCollectionForDeckBuilding();

    }

    public void DeleteThisDeck()
    {
        DecksStorage.Instance.allDecks.Remove(savedDeckInfo);
        Destroy(gameObject);
    }

    public void ApplyInfo(DeckInfo deckInfo)
    {
        NameText.text = deckInfo.deckName;
        savedDeckInfo = deckInfo;
    }
}
