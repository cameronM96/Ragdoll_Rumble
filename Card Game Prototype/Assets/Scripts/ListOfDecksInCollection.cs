using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfDecksInCollection : MonoBehaviour
{
    public Transform Content;
    public GameObject DeckInListPrefab;
    public GameObject NewDeckButtonPrefab;

    public void UpdateList()
    {
        foreach (Transform transform in Content)
        {
            if (transform!= Content)
            {
                Destroy(transform.gameObject);
            }
        }

        foreach (DeckInfo info in DecksStorage.Instance.allDecks)
        {
            GameObject g = Instantiate(DeckInListPrefab, Content);
            g.transform.localScale = Vector3.one;
            DeckInScrollList deckInScrollListComponent = g.GetComponent<DeckInScrollList>();
            deckInScrollListComponent.ApplyInfo(info);
        }

        if (DecksStorage.Instance.allDecks.Count<9)
        {
            GameObject g = Instantiate(NewDeckButtonPrefab, Content);
            g.transform.localScale = Vector3.one;
        }
    }
}
