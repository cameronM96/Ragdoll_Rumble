using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pack_Draggable : GenericDrag, IEndDragHandler
{
    public void OnEndDrag(PointerEventData eventData)
    {
        int myIndex = transform.GetSiblingIndex();
        transform.SetSiblingIndex(myIndex);
    }
}
