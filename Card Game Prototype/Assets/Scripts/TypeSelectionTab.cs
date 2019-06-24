using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class TypeSelectionTab : MonoBehaviour
{
    public List<TypeFilterTab> Tabs = new List<TypeFilterTab>();
    public TypeFilterTab typeTab;
    public TypeFilterTab NeutralTabWhenCollectionBrowsing;

    private int currentIndex = 0;

    public void SelectTab(TypeFilterTab tab, bool instant)
    {
        int newIndex = Tabs.IndexOf(tab);

        if (newIndex == currentIndex)
        {
            return;
        }

        currentIndex = newIndex;

        foreach (TypeFilterTab t in Tabs)
        {
            if (t != tab)
            {
                t.Deselect(instant);
            }
        }

        tab.Select(instant);
        DeckBuildingScreen.Instance.collectionBrowserScript.includeAllTypes = tab.showAllTypes;
    }

    public void SetTypeOnTypeTab(CardType type)
    {
        typeTab.cardType = type;
        typeTab.GetComponentInChildren<Text>().text = type.ToString();
    }
}
