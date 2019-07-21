using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Card Library")]
public class CardLibrary : ScriptableObject
{
    public List<Card> cardLibrary;

    [HideInInspector] public string storedPath;
    [HideInInspector] public bool newLibrary;

    public Card[] GetCardLibrary()
    {
        return cardLibrary.ToArray();
    }

    public bool AddCardToLibrary(Card newCard)
    {
        if (cardLibrary.Contains(newCard))
        {
            Debug.Log("Card already exists in library.");
            return false;
        }

        cardLibrary.Add(newCard);

        bool addSuccessful = false;

        addSuccessful = cardLibrary.Contains(newCard);

        if (!addSuccessful)
            Debug.Log("Was unable to add this card to the library");

        return addSuccessful;
    }

    public bool RemoveCardFromLibrary(Card card)
    {
        bool removeSuccessful = false;

        removeSuccessful = cardLibrary.Remove(card);

        if (!removeSuccessful)
            Debug.Log("Could not find this card in library");

        return removeSuccessful;
    }

    public void ClearLibrary()
    {
        cardLibrary.Clear();
    }
}