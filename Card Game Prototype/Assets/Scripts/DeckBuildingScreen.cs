using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class DeckBuildingScreen : MonoBehaviour
{
    public GameObject screenContent;
    public GameObject readyDecksList;
    public GameObject cardsInDecklist;
    public DeckBuilder builderScript;
    public ListOfDecksInCollection listOfReadyMadeDecksScript;
    public CollectionBrowser collectionBrowserScript;
    public TypeSelectionTab TabsScript;
    public bool showReducedQuantitiesInDeckBuilding = true;

    public static DeckBuildingScreen Instance;

    private void Awake()
    {
        Instance = this;
        HideScreen();
    }

    public void ShowScreenForCollectionBrowsing()
    {
        screenContent.SetActive(true);
        readyDecksList.SetActive(true);
        cardsInDecklist.SetActive(true);
        builderScript.InDeckBuildingMode = false;
        listOfReadyMadeDecksScript.UpdateList();

        //check if needed as we dont restric based on type
        collectionBrowserScript.allTypeTabs.gameObject.SetActive(true);
        collectionBrowserScript.oneTypeTabs.gameObject.SetActive(false);
        Canvas.ForceUpdateCanvases();

        collectionBrowserScript.ShowCollectionForBrowsing();
    }

    public void ShowScreenForDeckBuilding()
    {
        screenContent.SetActive(true);
        readyDecksList.SetActive(false);
        cardsInDecklist.SetActive(true);

        collectionBrowserScript.allTypeTabs.gameObject.SetActive(false);
        collectionBrowserScript.oneTypeTabs.gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();
    }

    public void BuildADeck()
    {
        ShowScreenForDeckBuilding();
    }

    public void HideScreen()
    {
        screenContent.SetActive(false);
        collectionBrowserScript.ClearCreatedCards();
    }
}
