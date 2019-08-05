using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pack_Draggable : GenericDrag, IBeginDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int myIndex = transform.GetSiblingIndex();
        Transform targetParent = transform.parent;
        transform.SetParent(null);
        transform.SetParent(targetParent);
        transform.SetSiblingIndex(myIndex);
        CollectionManager cM = transform.root.GetComponent<CollectionManager>();
        transform.localPosition = new Vector3(cM.packOffset.x, cM.packOffset.y, 0);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
