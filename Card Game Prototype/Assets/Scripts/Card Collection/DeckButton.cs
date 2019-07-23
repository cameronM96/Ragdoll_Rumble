using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    public CollectionManager collectionManager;
    public Text deckNameText;

    public string deckName;

    // Start is called before the first frame update
    void Start()
    {
        collectionManager = transform.root.GetComponent<CollectionManager>();
        deckNameText.text = deckName;
    }

    private void Update()
    {
        deckNameText.text = deckName;
    }

    public void OpenDeck()
    {
        bool loaded = collectionManager.LoadDeck(deckName, this);
        if (loaded)
            collectionManager.ToggleDeckCreation();
    }
}
