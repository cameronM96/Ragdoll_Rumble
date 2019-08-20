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
            Debug.LogWarning("Was unable to add this card to the library");
        else
            Debug.Log(newCard.cardName + " was successfully added to the Library!");

        return addSuccessful;
    }

    public bool RemoveCardFromLibrary(Card card)
    {
        bool removeSuccessful = false;

        removeSuccessful = cardLibrary.Remove(card);

        if (!removeSuccessful)
            Debug.LogWarning("Could not find this card in library");
        else
            Debug.Log(card.cardName + " was successfully removed from Library");

        return removeSuccessful;
    }

    public void ClearLibrary()
    {
        cardLibrary.Clear();

        Debug.Log("Library was Cleared!");
    }
}