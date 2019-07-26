using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCardButton : MonoBehaviour
{
    public CollectionManager collectionManager;
    // Start is called before the first frame update
    void Start()
    {
        collectionManager = transform.root.GetComponent<CollectionManager>();
    }

    public void OpenCard()
    {
        collectionManager.OpenCardWindow(transform.GetComponentInParent<CardDisplay>().card);
    }
}
