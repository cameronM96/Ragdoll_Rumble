using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    public CollectionManager collectionManager;
    public Text deckNameText;

    public string deckName;

    public void Initialise(CollectionManager cM, string newName)
    {
        collectionManager = cM;
        deckName = newName;
        deckNameText.text = deckName;
    }

    public void OpenDeck()
    {
        bool loaded = collectionManager.LoadDeck(deckName);
        if (loaded)
        {
            collectionManager.ToggleDeckCreation();
            collectionManager.LoadCards(collectionManager.creatingDeck);
        }
    }
}
