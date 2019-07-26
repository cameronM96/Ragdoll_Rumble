using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckCard_Draggable : GenericDrag, IEndDragHandler, IBeginDragHandler
{
    public Collection_Card collectionCard;
    public Transform returnParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        returnParent = null;
        //this.transform.SetParent(null);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (returnParent == null)
        {
            if (collectionCard != null)
                collectionCard.UpdateCardCount(++collectionCard.currentCardCount);

            Destroy(this.gameObject);
        }
        else
        {
            this.transform.SetParent(null);
            this.transform.SetParent(returnParent);
            this.transform.SetAsFirstSibling();
        }
    }
}
