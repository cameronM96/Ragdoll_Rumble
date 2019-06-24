using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class CollectionBrowser : MonoBehaviour
{
    public Transform[] slots;
    public GameObject CardMenuPrefab;
    public GameObject oneTypeTabs;
    public GameObject allTypeTabs;

    public KeywordInputField keywordInputFieldScript;
    public CraftingModeToggle craftingModeToggleScript;

    private CardType cardType;

    private List<GameObject> CreatedCards = new List<GameObject>();

    #region PROPERTIES
    private bool _showingCardsPlayerDoesNotOwn = false;
    public bool ShowingCardsPlayerDoesNotOwn
    {
        get
        {
            return _showingCardsPlayerDoesNotOwn;
        }
        set
        {
            _showingCardsPlayerDoesNotOwn = value;
            UpdatePage();
        }
    }

    private int _pageIndex = 0;
    public int pageIndex
    {
        get
        {
            return _pageIndex;
        }
        set
        {
            _pageIndex = value;
            UpdatePage();
        }
    }

    private bool _includeAllRarities = true;
    public bool includeAllRarities
    {
        get
        {
            return _includeAllTypes;
        }
        set
        {
            _includeAllRarities = value;
            UpdatePage();
        }
    }

    private bool _includeAllTypes = true;
    public bool includeAllTypes
    {
        get
        {
            return _includeAllTypes;
        }
        set
        {

        }
    }

    private Rarity _rarity = Rarity.None;
    public Rarity rarity
    {
        get
        {
            return _rarity;
        }
        set
        {
            _rarity = value;
            UpdatePage();
        }
    }

    private string _keyword = "";
    public string keyword
    {
        get
        {
            return _keyword;
        }
        set
        {
            _keyword = value;
            UpdatePage();
        }
    }

    #endregion

    public void ShowCollectionForBrowsing()
    {
        keywordInputFieldScript.Clear();
        craftingModeToggleScript.SetValue(false);

        ShowCards(false, 0, true, true, Rarity.None, "");
        DeckBuildingScreen.Instance.TabsScript.NeutralTabWhenCollectionBrowsing.Select(instant: true);
        DeckBuildingScreen.Instance.TabsScript.SelectTab(DeckBuildingScreen.Instance.TabsScript.NeutralTabWhenCollectionBrowsing, instant: true);
    }

    public void ShowCollectionForDeckBuilding()
    {
        keywordInputFieldScript.Clear();
        craftingModeToggleScript.SetValue(false);

        ShowCards(false, 0, true, false, Rarity.None, "");
        DeckBuildingScreen.Instance.TabsScript.NeutralTabWhenCollectionBrowsing.Select(instant: true);
        DeckBuildingScreen.Instance.TabsScript.SelectTab(DeckBuildingScreen.Instance.TabsScript.NeutralTabWhenCollectionBrowsing, instant: true);
    }

    public void ClearCreatedCards()
    {
        while (CreatedCards.Count > 0)
        {
            GameObject g = CreatedCards[0];
            CreatedCards.RemoveAt(0);
            Destroy(g);
        }
    }

    public void UpdateQuantitiesOnPage()
    {
        foreach (GameObject card in CreatedCards)
        {
            AddCardToDeck addCardComponent = card.GetComponent<AddCardToDeck>();
            addCardComponent.UpdateQuantity();
        }
    }
    public void UpdatePage()
    {
        ShowCards(_showingCardsPlayerDoesNotOwn,
            _pageIndex,
            _includeAllRarities,
            _includeAllTypes,
            _rarity,
            _keyword);
    }
    private void ShowCards(bool showingCardsPlayerDoesNotOwn = false,
            int pageIndex = 0,
            bool includeAllRarities = true,
            bool includeAllTypes = true,
            Rarity rarity = Rarity.None,
            string keyword = "")
    {
        _showingCardsPlayerDoesNotOwn = showingCardsPlayerDoesNotOwn;
        _pageIndex = pageIndex;
        _includeAllRarities = includeAllRarities;
        _includeAllTypes = includeAllTypes;
        _rarity = rarity;
        _keyword = keyword;

        List<Card> CardsOnThisPage = PageSelection(showingCardsPlayerDoesNotOwn, pageIndex, includeAllRarities, includeAllTypes, rarity, keyword);

        ClearCreatedCards();

        if (CardsOnThisPage.Count == 0)
        {
            return;
        }

        for (int i = 0; i < CardsOnThisPage.Count; i++)
        {
            GameObject newMenuCard;
            newMenuCard = Instantiate(CardMenuPrefab, slots[i].position, Quaternion.identity) as GameObject;

            newMenuCard.transform.SetParent(this.transform);
            CreatedCards.Add(newMenuCard);

            Card manager = newMenuCard.GetComponent<Card>();
            manager = CardsOnThisPage[i];

            AddCardToDeck addCardComponent = newMenuCard.GetComponent<AddCardToDeck>();
            addCardComponent.SetGameCard(CardsOnThisPage[i]);
            addCardComponent.UpdateQuantity();

        }
    }

    public void Next()
    {
        if (PageSelection(_showingCardsPlayerDoesNotOwn, _pageIndex + 1, _includeAllRarities, _includeAllTypes, _rarity, _keyword).Count == 0)
        {
            return;
        }

        ShowCards(_showingCardsPlayerDoesNotOwn, _pageIndex + 1, _includeAllRarities, _includeAllTypes, _rarity, _keyword);
    }

    public void Previous()
    {
        if (_pageIndex == 0)
        {
            return;
        }
        ShowCards(_showingCardsPlayerDoesNotOwn, _pageIndex - 1, _includeAllRarities, _includeAllTypes, _rarity, _keyword);
    }

    private List<Card> PageSelection(bool showingCardsPlayerDoesNotOwn, int pageIndex = 0, bool includeAllRarities = true, bool includeAllTypes = true, Rarity rarity = Rarity.None, string keyword = "")
    {
        List<Card> returnList = new List<Card>();
        List<Card> cardsToChooseFrom = CardCollection.Instance.GetCards(showingCardsPlayerDoesNotOwn, includeAllTypes, includeAllRarities, keyword,CardType.None, rarity);

        if (cardsToChooseFrom.Count > pageIndex * slots.Length)
        {
            for (int i = 0; i < cardsToChooseFrom.Count - pageIndex * slots.Length && i < slots.Length; i++)
            {
                returnList.Add(cardsToChooseFrom[pageIndex * slots.Length + i]);
            }
        }
        return returnList;
    }
}

