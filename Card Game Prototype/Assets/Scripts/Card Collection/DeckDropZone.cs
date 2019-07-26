using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DeckCard_Draggable d = eventData.pointerDrag.GetComponent<DeckCard_Draggable>();
        if (d != null)
        {
            d.returnParent = this.transform;
        }
    }
}
