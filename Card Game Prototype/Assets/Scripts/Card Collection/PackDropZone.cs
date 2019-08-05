using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PackDropZone : MonoBehaviour, IDropHandler
{
    public CardCollection cardCollection;

    public void Start()
    {
        cardCollection = transform.root.GetComponent<CardCollection>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Pack_Draggable d = eventData.pointerDrag.GetComponent<Pack_Draggable>();
        if (d != null)
        {
            d.transform.SetParent(transform);
            CollectionPack o = eventData.pointerDrag.GetComponent<CollectionPack>();
            if (o != null)
            {
                foreach(Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                o.OpenPack(cardCollection);
            }
        }
    }
}
